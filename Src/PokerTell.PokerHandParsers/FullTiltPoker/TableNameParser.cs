namespace PokerTell.PokerHandParsers.FullTiltPoker
{
    using Interfaces.Parsers;

    public class TableNameParser : Base.TableNameParser, IFullTiltPokerTableNameParser 
    {
        // First part matches Tourneys, Sit'n Go s etc., decond part matches CashTables
        internal const string FullTiltTableNamePattern = @"((?<TableName>\(\d+\), Table \d+ )|(Table (?<TableName>[^-]+) ))";

        protected override string TableNamePattern
        {
            get { return FullTiltTableNamePattern; }
        }
    }
}