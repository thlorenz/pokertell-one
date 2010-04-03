namespace PokerTell.PokerHandParsers.Tests.FullTiltPoker
{
    using PokerTell.PokerHandParsers.Base;

    public class TableNameParserTests : Base.TableNameParserTests
    {
        protected override string ValidTableName(string tableName)
        {
            // Table Bicycle - 300/600
            return string.Format("Table {0} - 300/600", tableName);
        }

        protected override TableNameParser GetTableNameParser()
        {
            return new PokerTell.PokerHandParsers.FullTiltPoker.TableNameParser();
        }
    }
}