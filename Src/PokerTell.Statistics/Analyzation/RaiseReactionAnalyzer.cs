namespace PokerTell.Statistics.Analyzation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using log4net;

    using PokerTell.Infrastructure.Enumerations.PokerHand;
    using PokerTell.Infrastructure.Interfaces.PokerHand;
    using PokerTell.Statistics.Interfaces;

    using Tools;

    /// <summary>
    ///   Uses the information of the given analyzation Preparer to further inspect the Sequence in order
    ///   to determine if and how much opponent raised, how the hero reacted and if the situation is standard
    ///   or even valid reactions were found.
    /// </summary>
    public class RaiseReactionAnalyzer : IRaiseReactionAnalyzer
    {
        static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        IReactionAnalyzationPreparer _analyzationPreparer;

        IConvertedPokerActionWithId _heroReaction;

        bool _considerOpponentsRaiseSize;

        public IAnalyzablePokerPlayer AnalyzablePokerPlayer { get; set; }

        /// <summary>
        ///   Indicates how hero reacted to a raise by the opponent (e.g. fold)
        /// </summary>
        public ActionTypes HeroReactionType
        {
            get { return _heroReaction.What; }
        }

        /// <summary>
        ///   Situation is considered standard if no more raises occurred after the original raise of the opponent or
        ///   the hero's reaction occurred before these additional raises and thus didn't influence his decision.
        /// </summary>
        public bool IsStandardSituation { get; protected set; }

        /// <summary>
        ///   Is only true if an opponents raise after the action of the hero and a reaction to this 
        ///   raise by the hero were found.
        /// </summary>
        public bool IsValidResult { get; protected set; }

        /// <summary>
        ///   Indicates the ratio of the opponents raise to which the hero reacted
        /// </summary>
        public int ConsideredRaiseSize { get; private set; }

        public double[] RaiseSizeKeys { get; protected set; }

        public override string ToString()
        {
            return string.Format(
                "ConsideredRaiseSize: {0}, IsStandardSituation: {1}, IsValidResult: {2}, HeroReactionType: {3}", 
                ConsideredRaiseSize, 
                IsStandardSituation, 
                IsValidResult, 
                _heroReaction);
        }

        /// <summary>
        ///   Analyzes the data given by the analyzation preparer.
        /// </summary>
        /// <param name="analyzablePokerPlayer">
        ///   Player whose data is examined
        /// </param>
        /// <param name="analyzationPreparer">
        ///   Provides StartingIndex (Hero's original action) and Sequence
        /// </param>
        /// <param name="considerOpponentsRaiseSize">If true, it will determine the opponents re(raise) size and set the Considered raise size to it.
        /// So far this only is done like this for PostFlopHero acts. In all other cases set it to false and the raise size of the hero is used.</param>
        /// <param name="raiseSizeKeys">
        ///   Raise sizes to which the Opponent Raise size should be normalized to
        /// </param>
        public IRaiseReactionAnalyzer AnalyzeUsingDataFrom(
            IAnalyzablePokerPlayer analyzablePokerPlayer, 
            IReactionAnalyzationPreparer analyzationPreparer, 
            bool considerOpponentsRaiseSize, 
            double[] raiseSizeKeys)
        {
            _considerOpponentsRaiseSize = considerOpponentsRaiseSize;
            AnalyzablePokerPlayer = analyzablePokerPlayer;
            _analyzationPreparer = analyzationPreparer;
            RaiseSizeKeys = raiseSizeKeys;

            try
            {
                IsValidResult = AnalyzeReaction();
            }
            catch (Exception excep)
            {
                Log.Error(ToString(), excep);

                IsValidResult = false;
            }

            return this;
        }

        bool AdditionalRaisesHappenedAfterHerosReactionToFirstRaise(
            IList<IConvertedPokerActionWithId> remainingActions, IEnumerable<IConvertedPokerActionWithId> foundRaises)
        {
            int herosReactionIndex = remainingActions.IndexOf(_heroReaction);
            int count = 0;

            foreach (
                IConvertedPokerActionWithId _ in foundRaises.Where(raise => remainingActions.IndexOf(raise) < herosReactionIndex))
            {
                count++;
                if (count > 1)
                {
                    return false;
                }
            }

            return true;
        }

        bool AnalyzeReaction()
        {
            IEnumerable<IConvertedPokerActionWithId> foundRaises = SetConsideredRaiseSize();

            if (foundRaises.Count() == 0)
            {
                return false;
            }

            List<IConvertedPokerActionWithId> actionsAfterHeroRaise = GetActionsAfterHeroRaise();

            SetHerosReaction(actionsAfterHeroRaise);

            if (_heroReaction == null || !ActionTypesUtility.Reactions.Contains(HeroReactionType))
            {
                return false;
            }

            DetermineIfSituationIsStandard(actionsAfterHeroRaise, foundRaises);

            return true;
        }

        void DetermineIfSituationIsStandard(
            IList<IConvertedPokerActionWithId> actionsAfterHeroRaise, IEnumerable<IConvertedPokerActionWithId> foundRaises)
        {
            IsStandardSituation = foundRaises.Count() == 1 ||
                                  AdditionalRaisesHappenedAfterHerosReactionToFirstRaise(
                                      actionsAfterHeroRaise, foundRaises);
        }

        List<IConvertedPokerActionWithId> GetActionsAfterHeroRaise()
        {
            var actionsAfterHeroRaise = new List<IConvertedPokerActionWithId>();

            for (int i = _analyzationPreparer.StartingActionIndex + 1; i < _analyzationPreparer.Sequence.Count; i++)
            {
                actionsAfterHeroRaise.Add((IConvertedPokerActionWithId)_analyzationPreparer.Sequence[i]);
            }

            return actionsAfterHeroRaise;
        }

        void SetHerosReaction(IEnumerable<IConvertedPokerActionWithId> actionsAfterHeroRaise)
        {
            IEnumerable<IConvertedPokerActionWithId> herosReaction =
                from IConvertedPokerActionWithId action in actionsAfterHeroRaise
                where action.Id.Equals(_analyzationPreparer.HeroPosition)
                select action;

            _heroReaction = herosReaction.Count() > 0
                                ? herosReaction.First()
                                : null;
        }

        IEnumerable<IConvertedPokerActionWithId> SetConsideredRaiseSize()
        {
            const ActionTypes actionToLookFor = ActionTypes.R;

            var actionsAfterHerosInitialAction =
                _analyzationPreparer.Sequence.Actions.Skip(_analyzationPreparer.StartingActionIndex);

            IEnumerable<IConvertedPokerActionWithId> opponentsRaises =
                from IConvertedPokerActionWithId action in actionsAfterHerosInitialAction
                where action.What.Equals(actionToLookFor) && !action.Id.Equals(_analyzationPreparer.HeroPosition)
                select action;

            if (_considerOpponentsRaiseSize && opponentsRaises.Count() > 0)
            {
                ConsideredRaiseSize =
                    (int)Normalizer.NormalizeToKeyValues(RaiseSizeKeys, opponentsRaises.First().Ratio);
            }
            else if (!_considerOpponentsRaiseSize)
            {
                IEnumerable<IConvertedPokerActionWithId> herosRaises =
                    from IConvertedPokerActionWithId action in actionsAfterHerosInitialAction
                    where action.What.Equals(actionToLookFor) && action.Id.Equals(_analyzationPreparer.HeroPosition)
                    select action;
                if (herosRaises.Count() > 0)
                {
                    ConsideredRaiseSize = (int)Normalizer.NormalizeToKeyValues(RaiseSizeKeys, herosRaises.First().Ratio);
                }
                else
                {
                    // Returning empty list will let caller know that the appropriate reaction was not found
                    return herosRaises;
                }
            }

            return opponentsRaises;
        }
    }
}