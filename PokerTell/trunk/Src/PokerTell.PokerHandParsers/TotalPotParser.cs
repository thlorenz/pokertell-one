namespace PokerTell.PokerHandParsers
{
    public abstract class TotalPotParser
    {
        public bool IsValid { get; protected set; }

        public double TotalPot { get; protected set; }

        public abstract void Parse(string handHistory);
    }
}