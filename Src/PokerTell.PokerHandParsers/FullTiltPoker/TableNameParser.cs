namespace PokerTell.PokerHandParsers.FullTiltPoker
{
    public class TableNameParser : Base.TableNameParser
    {
        internal const string FullTiltTableNamePattern = @".*Table (?<TableName>[^-]+) ";

        protected override string TableNamePattern
        {
            get { return FullTiltTableNamePattern; }
        }
    }
}