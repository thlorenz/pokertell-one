namespace PokerTell.PokerHandParsers.PokerStars
{
    public class TableNameParser : Base.TableNameParser
    {
        internal const string PokerStarsTableNamePattern = @".*Table ['](?<TableName>.*[^'])[']";

        protected override string TableNamePattern
        {
            get { return PokerStarsTableNamePattern; }
        }
    }
}