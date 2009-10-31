namespace PokerTell.PokerHandParsers
{
    public abstract class BoardParser
    {
        public bool IsValid { get; protected set; }

        public string Board { get; protected set; }

        public abstract void Parse(string handHistory);
    }
}