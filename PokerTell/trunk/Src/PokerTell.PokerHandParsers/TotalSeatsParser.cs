namespace PokerTell.PokerHandParsers
{
    public abstract class TotalSeatsParser
    {
        public bool IsValid { get; protected set; }

        public int TotalSeats { get; protected set; }

        public abstract TotalSeatsParser Parse(string handHistory);
    }
}