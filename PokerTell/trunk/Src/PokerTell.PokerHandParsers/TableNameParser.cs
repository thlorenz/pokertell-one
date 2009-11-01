namespace PokerTell.PokerHandParsers
{
    public abstract class TableNameParser
    {
        public bool IsValid { get; protected set; }

        public string TableName { get; protected set; }

        public abstract TableNameParser Parse(string handHistory);
    }
}