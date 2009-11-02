namespace PokerTell.PokerHandParsers
{
    public abstract class SmallBlindPlayerNameParser
    {
        public bool IsValid { get; protected set; }

        public string SmallBlindPlayerName { get; protected set; }

        public abstract SmallBlindPlayerNameParser Parse(string handHistory);
    }
}