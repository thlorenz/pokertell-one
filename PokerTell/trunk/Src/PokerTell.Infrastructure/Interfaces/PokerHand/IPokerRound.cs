namespace PokerTell.Infrastructure.Interfaces.PokerHand
{
    using System.Collections;
    using System.Collections.ObjectModel;

    public interface IPokerRound : IEnumerable
    {
        ReadOnlyCollection<IPokerAction> Actions { get; }

        /// <summary>
        /// Number of actions in this round
        /// </summary>
        int Count { get; }

        IPokerAction GetPokerActionAtIndex(int index);

        /// <summary>
        /// Gives a string representation of the round
        /// </summary>
        /// <returns>String representation of the round, including info about all actions</returns>
        string ToString();
    }
}