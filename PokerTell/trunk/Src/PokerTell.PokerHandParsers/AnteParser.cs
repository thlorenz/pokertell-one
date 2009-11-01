namespace PokerTell.PokerHandParsers
{
    public abstract class AnteParser
    {
        public bool IsValid { get; protected set; }

        public double Ante { get; protected set; }

        public abstract AnteParser Parse(string handHistory);
    }
}