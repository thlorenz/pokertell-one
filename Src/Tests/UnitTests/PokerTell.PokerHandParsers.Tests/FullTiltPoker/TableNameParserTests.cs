using System;

namespace PokerTell.PokerHandParsers.Tests.FullTiltPoker
{
    using Base;

    public class TableNameParserTests : Tests.TableNameParserTests
    {
        protected override string ValidTableName(string tableName)
        {
            // Table Bicycle 
            return string.Format("Table {0} ", tableName);
        }

        protected override TableNameParser GetTableNameParser()
        {
            return new PokerTell.PokerHandParsers.FullTiltPoker.TableNameParser();
        }
    }
}