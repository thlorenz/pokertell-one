namespace PokerTell.PokerHandParsers.PokerStars
{
    using System.Text.RegularExpressions;

    public class TableNameParser : PokerHandParsers.TableNameParser
    {
        #region Constants and Fields

        internal const string TableNamePattern = @".*Table ['](?<TableName>.*[^'])[']";

        #endregion

        #region Public Methods

        public override void Parse(string handHistory)
        {
            Match table = MatchTableName(handHistory);
            IsValid = table.Success;
           
            if (IsValid)
            {
                ExtractTableName(table);
            }
        }

        #endregion

        #region Methods

        static Match MatchTableName(string handHistory)
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