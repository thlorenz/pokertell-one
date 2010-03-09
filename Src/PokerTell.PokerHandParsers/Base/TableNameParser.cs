namespace PokerTell.PokerHandParsers.Base
{
    using System.Text.RegularExpressions;

    public abstract class TableNameParser
    {
        public bool IsValid { get; protected set; }

        public string TableName { get; protected set; }

        protected abstract string TableNamePattern { get; }

        public TableNameParser Parse(string handHistory)
        {
            Match table = MatchTableName(handHistory);
            IsValid = table.Success;

            if (IsValid)
            {
                ExtractTableName(table);
            }
            return this;
        }

        Match MatchTableName(string handHistory)
        {
            return Regex.Match(handHistory, TableNamePattern, RegexOptions.IgnoreCase);
        }

        void ExtractTableName(Match table)
        {
            TableName = table.Groups["TableName"].Value;
        }
    }
}