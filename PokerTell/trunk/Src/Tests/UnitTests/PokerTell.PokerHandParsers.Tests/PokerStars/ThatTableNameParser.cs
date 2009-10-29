namespace PokerTell.PokerHandParsers.Tests.PokerStars
{
    using System;

    public class ThatTableNameParser : Tests.ThatTableNameParser
    {
        protected override string ValidTableName(string tableName)
        {
            // Table 'Marceline V' 
            return string.Format("Table '{0}'", tableName);
        }

        protected override TableNameParser GetTableNameParser()
        {
            return new PokerHandParsers.PokerStars.TableNameParser();
        }
    }
}