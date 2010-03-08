namespace PokerTell.PokerHandParsers.FullTiltPoker
{
    public class TableNameParser : Base.TableNameParser
    {
        #region Constants and Fields

        internal const string FullTiltTableNamePattern = @".*Table (?<TableName>.*[^ ]) ";

        #endregion

        #region Properties

        protected override string TableNamePattern
        {
            get { return FullTiltTableNamePattern; }
        }

        #endregion
    }
}