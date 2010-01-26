namespace PokerTell.Statistics.Analyzation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Infrastructure;
    using Infrastructure.Enumerations.PokerHand;
    using Infrastructure.Interfaces.PokerHand;

    using Interfaces;

    using log4net;

    using Tools;

    public class RaiseReactionAnalyzer
    {
        #region Constants and Fields

        static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        readonly IReactionAnalyzationPreparer _analyzationPreparer;

        IConvertedPokerActionWithId _heroReaction;

        #endregion

        #region Constructors and Destructors

        public RaiseReactionAnalyzer(IReactionAnalyzationPreparer analyzationPreparer)
        {
            _analyzationPreparer = analyzationPreparer;

            try
            {
                IsValidResult = AnalyzeReaction();
            }
            catch (Exception excep)
            {
                Log.Error(ToString(), excep);

                IsValidResult = false;
            }
        }

        #endregion

        #region Properties

        public ActionTypes HeroReactionType
        {
            get { return _heroReaction.What; }
        }

        public bool IsStandardSituation { get; protected set; }

        public bool IsValidResult { get; protected set; }

        public int OpponentRaiseSize { get; private set; }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return string.Format(
                "OpponentRaiseSize: {0}, IsStandardSituation: {1}, IsValidResult: {2}, HeroReactionType: {3}",
                OpponentRaiseSize,
                IsStandardSituation,
                IsValidResult,
                _heroReaction);
        }

        #endregion

        #region Methods

        bool AdditionalRaisesHappenedAfterHerosReactionToFirstRaise(
            IList<IConvertedPokerActionWithId> remainingActions, IEnumerable<IConvertedPokerActionWithId> foundRaises)
        {
            int herosReactionIndex = remainingActions.IndexOf(_heroReaction);
            int count = 0;
            foreach (IConvertedPokerActionWithId raise in foundRaises)
            {
                if (remainingActions.IndexOf(raise) < herosReactionIndex)
                {
                    count++;
                    if (count > 1)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        bool AnalyzeReaction()
        {
            IEnumerable<IConvertedPokerActionWithId> foundRaises = SetRaiseSize();

            if (foundRaises.Count() == 0)
            {
                return false;
            }

            List<IConvertedPokerActionWithId> actionsAfterHeroRaise = GetActionsAfterHeroRaise();

            SetHerosReaction(actionsAfterHeroRaise);

            if (_heroReaction == null || !PokerActionsUtility.Reactions.Contains(HeroReactionType))
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

        IEnumerable<IConvertedPokerActionWithId> SetRaiseSize()
        {
            const ActionTypes actionToLookFor = ActionTypes.R;
            IEnumerable<IConvertedPokerActionWithId> foundRaises =
                from IConvertedPokerActionWithId action in _analyzationPreparer.Sequence
                where action.What.Equals(actionToLookFor) && !action.Id.Equals(_analyzationPreparer.HeroPosition)
                select action;

            if (foundRaises.Count() > 0)
            {
                OpponentRaiseSize =
                    (int)Normalizer.NormalizeToKeyValues(ApplicationProperties.RaiseSizeKeys, foundRaises.First().Ratio);
            }

            return foundRaises;
        }

        #endregion
    }
}