namespace PokerTell.PokerHandParsers
{
    public abstract class TotalSeatsParser
    {
        public bool IsValid { get; protected set; }

        public int TotalSeats { get; protected set; }

        public abstract void Parse(string handHistory);
    }
}