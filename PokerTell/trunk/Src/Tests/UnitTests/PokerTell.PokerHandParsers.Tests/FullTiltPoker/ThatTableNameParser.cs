using System;

namespace PokerTell.PokerHandParsers.Tests.FullTiltPoker
{
    using Base;

    public class ThatTableNameParser : Tests.ThatTableNameParser
    {
        protected override string ValidTableName(string tableName)
        {
            // Table Bicycle 
            return string.Format("Table {0} ", tableName);
        }

        protected override TableNameParser GetTableNameParser()
        {
            return new PokerHandParsers.FullTiltPoker.TableNameParser();
        }
    }
}