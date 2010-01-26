namespace PokerTell.Statistics.Analyzation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Infrastructure.Enumerations.PokerHand;
    using Infrastructure.Interfaces.PokerHand;

    using Interfaces;

    using log4net;

    public class ReactionAnalyzationPreparer : IReactionAnalyzationPreparer
    {
        #region Constants and Fields

        static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        #endregion

        #region Constructors and Destructors

        public ReactionAnalyzationPreparer(IConvertedPokerRound sequence, int playerPosition, ActionSequences actionSequence)
        {
            InitializeProperties(sequence, playerPosition);

            PrepareAnalyzation(actionSequence);
        }

        #endregion

        #region Properties

        public int HeroPosition { get; private set; }

        public IConvertedPokerRound Sequence { get; private set; }

        public int StartingActionIndex { get; private set; }

        public bool WasSuccessful { get; private set; }

        #endregion

        #region Implemented Interfaces

        #region IReactionAnalyzationPreparer

        public override string ToString()
        {
            return string.Format(
                "HeroPosition: {0}, Sequence: {1}, StartingActionIndex: {2}, WasSuccessful: {3}",
                HeroPosition,
                Sequence,
                StartingActionIndex,
                WasSuccessful);
        }

        #endregion

        #endregion

        #region Methods

        protected void InitializeProperties(IConvertedPokerRound sequence, int playerPosition)
        {
            Sequence = sequence;
            HeroPosition = playerPosition;
        }

        void PrepareAnalyzation(ActionSequences actionSequence)
        {
            try
            {
                SetStartingActionIndex(actionSequence);

                WasSuccessful = true;
            }
            catch (Exception excep)
            {
                Log.Error(excep);
                WasSuccessful = false;
            }
        }

        void SetStartingActionIndex(ActionSequences actionSequence)
        {
            ActionTypes actionToLookFor = ActionSequencesUtility.GetLastActionIn(actionSequence);

            IEnumerable<IConvertedPokerActionWithId> foundAction
                = from IConvertedPokerActionWithId action in Sequence
                  where
                      action.What.Equals(actionToLookFor) &&
                      action.Id.Equals(HeroPosition)
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