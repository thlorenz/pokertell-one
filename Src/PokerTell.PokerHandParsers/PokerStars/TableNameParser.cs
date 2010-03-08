namespace PokerTell.PokerHandParsers.PokerStars
{
    public class TableNameParser : Base.TableNameParser
    {
        #region Constants and Fields

        internal const string PokerStarsTableNamePattern = @".*Table ['](?<TableName>.*[^'])[']";

        #endregion

        #region Properties

        protected override string TableNamePattern
        {
            get { return PokerStarsTableNamePattern; }
        }

        #endregion
    }
}