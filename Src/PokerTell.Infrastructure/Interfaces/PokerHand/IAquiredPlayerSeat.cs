namespace PokerTell.Infrastructure.Interfaces.PokerHand
{
    public interface IAquiredPlayerSeat
    {
        /// <summary>Name of the Player in the seat</summary>
        string PlayerName { get; }

        /// <summary>Number of the seat</summary>
        int SeatNumber { get; }

        void InitializeWith(string playerName, int seatNumber);
    }
}