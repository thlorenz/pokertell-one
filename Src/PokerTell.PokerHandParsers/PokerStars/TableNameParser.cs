namespace PokerTell.PokerHandParsers.PokerStars
{
    using Interfaces.Parsers;

    public class TableNameParser : Base.TableNameParser, IPokerStarsTableNameParser 
    {
        const string PokerStarsTableNamePattern = "((" + PokerStarsLegitTableNamePattern + ")|(" + PokerStarsPokerOfficeDatabaseBlobTableNamePattern + "))";
        internal const string PokerStarsLegitTableNamePattern = @".*Table ['](?<TableName>.*[^'])[']";
       
        // e.g. Table Messina III 9-max Seat #1
        const string PokerStarsPokerOfficeDatabaseBlobTableNamePattern = @"\nTable (?<TableName>.+ )\d{1,2}-max ";

        protected override string TableNamePattern
        {
            get { return PokerStarsTableNamePattern; }
        }
    }
}