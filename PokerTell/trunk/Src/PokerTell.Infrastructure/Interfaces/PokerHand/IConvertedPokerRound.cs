namespace PokerTell.Infrastructure.Interfaces.PokerHand
{
    using System;
    using System.Collections;

    public interface IConvertedPokerRound : IPokerRound<IConvertedPokerAction>
    {
        /// <summary>
        /// Number of actions in this round
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Gives a string representation of the round
        /// </summary>
        /// <returns>String representation of the round, including info about all actions</returns>
        string ToString();

        /// <summary>
        /// The add action.
        /// </summary>
        /// <param name="action">
        /// The the action.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// </exception>
        IConvertedPokerRound Add(IConvertedPokerAction action);
    }
}