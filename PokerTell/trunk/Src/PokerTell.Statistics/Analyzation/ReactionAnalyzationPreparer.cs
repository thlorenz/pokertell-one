namespace PokerTell.Statistics.Analyzation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Detailed;

    using Infrastructure.Enumerations.PokerHand;
    using Infrastructure.Interfaces.PokerHand;
    using Infrastructure.Interfaces.Statistics;

    using Interfaces;

    using log4net;

    public class ReactionAnalyzationPreparer : IReactionAnalyzationPreparer
    {
        #region Constants and Fields

        static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        #endregion

        #region Constructors and Destructors

        public ReactionAnalyzationPreparer(
            IConvertedPokerHand convertedHand, Streets street, string heroName, ActionSequences actionSequence)
        {
            InitializeProperties(convertedHand, street, heroName, actionSequence);

            PrepareAnalyzation();
        }

        #endregion

        #region Properties

        public ActionSequences ActionSequence { get; set; }

        public IConvertedPokerHand ConvertedHand { get; private set; }

        public IActionSequenceStatisticsSet ActionSequenceStatisticsSet { get; set; }

        public int HeroIndex { get; private set; }

        public string HeroName { get; set; }

        public IConvertedPokerRound Sequence { get; private set; }

        public int StartingActionIndex { get; private set; }

        public Streets Street { get; set; }

        public bool WasSuccessful { get; private set; }

        #endregion

        #region Implemented Interfaces

        #region IReactionAnalyzationPreparer

        public override string ToString()
        {
            return
                string.Format(
                    "HeroIndex: {0}, Sequence: {1}, StartingActionIndex: {2}, HeroName: {3}, Street: {4}, ActionSequence: {5}, ConvertedHand: {6}, WasSuccessful: {7}",
                    HeroIndex,
                    Sequence,
                    StartingActionIndex,
                    HeroName,
                    Street,
                    ActionSequence,
                    ConvertedHand,
                    WasSuccessful);
        }

        #endregion

        #endregion

        #region Methods

        protected void InitializeProperties(
            IConvertedPokerHand convertedHand, Streets street, string heroName, ActionSequences actionSequence)
        {
            ConvertedHand = convertedHand;
            Street = street;
            HeroName = heroName;
            ActionSequence = actionSequence;
        }

        void PrepareAnalyzation()
        {
            try
            {
                SetSequence();

                SetHeroIndex();

                SetStartingActionIndex();

                WasSuccessful = true;
            }
            catch (Exception excep)
            {
                Log.Error(excep);
                WasSuccessful = false;
            }
        }

        void SetHeroIndex()
        {
            IEnumerable<int> foundIndex = from IConvertedPokerPlayer player in ConvertedHand
                                          where player.Name.Equals(HeroName)
                                          select player.Position;
            if (foundIndex.Count() > 0)
            {
                HeroIndex = foundIndex.First();
            }
            else
            {
                throw new ArgumentException("Hero not found in hand");
            }
        }

        void SetSequence()
        {
            if (ConvertedHand.Sequences != null)
            {
                Sequence = ConvertedHand.Sequences[(int)Street];
            }
            else
            {
                throw new NullReferenceException("ConvertedHand.Sequences[" + Street + "]");
            }
        }

        void SetStartingActionIndex()
        {
            ActionTypes actionToLookFor = ActionSequencesUtility.GetLastActionIn(ActionSequence);

            // Debug.WriteLine(string.Format("Find {0} in {1} for HeroIndex {2}", actionToLookFor, Sequence, HeroIndex));
            IEnumerable<IConvertedPokerActionWithId> foundAction
                = from IConvertedPokerActionWithId action in Sequence
                  where
                      action.What.Equals(actionToLookFor) &&
                      action.Id.Equals(HeroIndex)
                  select action;

            if (foundAction.Count() > 0)
            {
                StartingActionIndex = Sequence.Actions.IndexOf(foundAction.First());
            }
            else
            {
                throw new ArgumentException("Couldn't find action: " + actionToLookFor);
            }
        }

        #endregion
    }
}