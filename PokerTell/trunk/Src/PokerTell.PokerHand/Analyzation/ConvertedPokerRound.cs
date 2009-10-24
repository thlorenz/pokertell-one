namespace PokerTell.PokerHand.Analyzation
{
    using System;
    using System.Collections.Generic;

    using Infrastructure.Interfaces.PokerHand;

    /// <summary>
    /// PokerRound used for analysing a Poker Hand
    /// </summary>
    [Serializable]
    public class ConvertedPokerRound : PokerRound, IConvertedPokerRound
    {
        #region Indexers

        /// <summary>
        /// The this.
        /// </summary>
        /// <param name="index">
        /// The index.
        /// </param>
        public IConvertedPokerAction this[int index]
        {
            get { return (IConvertedPokerAction)Actions[index]; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// The add action.
        /// </summary>
        /// <param name="theAction">
        /// The the action.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// </exception>
        public IConvertedPokerRound Add(IConvertedPokerAction theAction)
        {
            if (theAction != null)
            {
                Actions.Add(theAction);
            }
            else
            {
                throw new ArgumentNullException("theAction");
            }

            return this;
        }

        #endregion

    }
}