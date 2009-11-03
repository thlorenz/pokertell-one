using System.Text.RegularExpressions;

namespace PokerTell.PokerHandParsers.Base
{
    public abstract class TableNameParser
    {
        public bool IsValid { get; protected set; }

        public string TableName { get; protected set; }

        protected abstract string TableNamePattern { get; }

        #region Public Methods

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

        #endregion

        #region Methods

        Match MatchTableName(string handHistory)
        {
            return Regex.Match(handHistory, TableNamePattern, RegexOptions.IgnoreCase);
        }

        void ExtractTableName(Match table)
        {
            TableName = table.Groups["TableName"].Value;
        }

        #endregion
    }
}