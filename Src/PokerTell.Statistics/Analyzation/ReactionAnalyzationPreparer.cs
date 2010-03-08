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

    /// <summary>
    /// For a given Sequence it finds the last action of a hero (identified by is position) of a given ActionSequence
    /// </summary>
    public class ReactionAnalyzationPreparer : IReactionAnalyzationPreparer
    {
        #region Constants and Fields

        static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        #endregion

        #region Constructors and Destructors
       
        /// <summary>
        /// Prepares the analyzation, needs to be called before it's data becomes useful
        /// </summary>
        /// <param name="sequence"><see cref="Sequence"/></param>
        /// <param name="playerPosition"><see cref="HeroPosition"/></param>
        /// <param name="actionSequence">The ActionSequence whose last action's index will be idendified</param>
        public IReactionAnalyzationPreparer PrepareAnalyzationFor(IConvertedPokerRound sequence, int playerPosition, ActionSequences actionSequence)
        {
            InitializeProperties(sequence, playerPosition);

            PrepareAnalyzation(actionSequence);

            return this;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The Position of the hero during the given sequence. 
        /// This is needed to identify which actions belong to the hero and which belong to his opponents.
        /// </summary>
        public int HeroPosition { get; private set; }

        /// <summary>
        /// The Sequence of a PokerRound, containing the actions of the hero and his opponents.
        /// This is what is being examined.
        /// </summary>
        public IConvertedPokerRound Sequence { get; private set; }

        /// <summary>
        /// Index of the action of the hero that is to be examined.
        /// E.g. For HeroB it is the bet action.
        /// This is used to later find the opponents reaction to it (e.g. a raise) and how the hero reacted to that
        /// </summary>
        public int StartingActionIndex { get; private set; }

        /// <summary>
        /// Is true if the action to be examined was found and the StartingPosition was set
        /// </summary>
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