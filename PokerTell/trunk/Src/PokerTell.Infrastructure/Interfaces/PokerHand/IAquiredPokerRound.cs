namespace PokerTell.Infrastructure.Interfaces.PokerHand
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public interface IAquiredPokerRound : IEnumerable
    {
        double ChipsGained { get; }

        IList<IAquiredPokerAction> Actions { get; }

        IAquiredPokerRound AddAction(IAquiredPokerAction theAction);

        /// <summary>
        /// Removes an action at a certain index
        /// Currently only used by PokerHandConverter to remove posting actions
        /// </summary>
        /// <param name="index">Index at which action will be removed</param>
        void RemoveAction(int index);

        /// <summary>
        /// Removes a given action
        /// </summary>
        /// <param name="theAction">Action to remove</param>
        /// <returns>true if it was removed</returns>
        bool RemoveAction(IAquiredPokerAction theAction);

        /// <summary>
        /// The add action.
        /// </summary>
        /// <param name="action">
        /// The the action.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// </exception>
        IAquiredPokerRound Add(IAquiredPokerAction action);

        IAquiredPokerAction this[int index]
        {
            get;
        }
    }
}