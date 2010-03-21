namespace PokerTell.PokerHandParsers.Tests.PokerStars
{
    using PokerTell.PokerHandParsers.Base;

    public class TableNameParserTests : Base.TableNameParserTests
    {
        protected override string ValidTableName(string tableName)
        {
            // Table 'Marceline V' 
            return string.Format("Table '{0}'", tableName);
        }

        protected override TableNameParser GetTableNameParser()
        {
            return new PokerTell.PokerHandParsers.PokerStars.TableNameParser();
        }
    }
}