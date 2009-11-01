namespace PokerTell.PokerHandParsers
{
    public abstract class SmallBlindSeatNumberParser
    {
        public bool IsValid { get; protected set; }

        public int SmallBlindSeatNumber { get; protected set; }

        public abstract SmallBlindSeatNumberParser Parse(string handHistory);
    }
}