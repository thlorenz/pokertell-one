namespace PokerTell.PokerHandParsers
{
    public abstract class HoleCardsParser
    {
        protected string _handHistory;

        protected string _playerName;

        public string Holecards { get; protected set; }

        public abstract HoleCardsParser Parse(string handHistory, string playerName);
    }
}