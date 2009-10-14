namespace PokerTell.Infrastructure.Interfaces.PokerHand
{
    public interface IAquiredPokerRound : IPokerRound
    {
        double ChipsGained { get; }

        IAquiredPokerAction this[int index]
        {
            get;
        }

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
        bool RemoveAction(IPokerAction theAction);
    }
}