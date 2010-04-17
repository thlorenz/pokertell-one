namespace PokerTell.PokerHandParsers.FullTiltPoker
{
    using Interfaces.Parsers;

    public class TableNameParser : Base.TableNameParser, IFullTiltPokerTableNameParser 
    {
        internal const string FullTiltCashTableNamePattern = @"(Table (?<TableName>[^-]+) )";

        internal const string FullTiltTournamentTableNamePattern = @"(?<TableName>\(\d+\), Table \d+ )";

        const string FullTiltTableNamePattern = "(" + FullTiltCashTableNamePattern + "|" + FullTiltTournamentTableNamePattern + ")";

        protected override string TableNamePattern
        {
            get { return FullTiltTableNamePattern; }
        }
    }
}