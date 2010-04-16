namespace PokerTell.PokerHandParsers.PokerStars
{
    using Interfaces.Parsers;

    public class TableNameParser : Base.TableNameParser, IPokerStarsTableNameParser 
    {
        internal const string PokerStarsTableNamePattern = @".*Table ['](?<TableName>.*[^'])[']";

        protected override string TableNamePattern
        {
            get { return PokerStarsTableNamePattern; }
        }
    }
}