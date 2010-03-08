namespace PokerTell.PokerHandParsers.Tests.PokerStars
{
    using System;

    using Base;

    public class TableNameParserTests : Tests.TableNameParserTests
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