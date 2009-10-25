namespace PokerTell.Infrastructure.Interfaces.PokerHand
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public interface IConvertedPokerRound : IEnumerable
    {
        /// <summary>
        /// The this.
        /// </summary>
        /// <param name="index">
        /// The index.
        /// </param>
        IConvertedPokerAction this[int index]
        {
            get;
        }

        /// <summary>
        /// Number of actions in this round
        /// </summary>
        int Count { get; }

        IList<IConvertedPokerAction> Actions { get; }

        /// <summary>
        /// The add action.
        /// </summary>
        /// <param name="theAction">
        /// The the action.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// </exception>
        IConvertedPokerRound Add(IConvertedPokerAction theAction);

        /// <summary>
        /// Gives a string representation of the round
        /// </summary>
        /// <returns>String representation of the round, including info about all actions</returns>
        string ToString();
    }
}