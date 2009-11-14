namespace PokerTell.Repository.Database
{
    using System.Data;

    using Infrastructure.Enumerations.DatabaseSetup;
    using Infrastructure.Interfaces.DatabaseSetup;

    using Interfaces;

    public class DatabaseUtility : IDatabaseUtility
    {
        IDataProvider _dataProvider;

        public IDatabaseUtility Use(IDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
            return this;
        }
        
        public int GetIdentityOfLastInsertedHand()
        {
            string query = string.Format("SELECT max({0}) FROM {1};", GameTable.identity, Tables.gamehhd);

            using (IDataReader dr = _dataProvider.ExecuteQuery(query))
            {
                int handId = dr.Read() ? dr.GetInt32(0) : 0;

                return handId;
            }
        }

        public int? GetHandIdForHandWith(ulong gameId, string site)
        {
            string query = string.Format(
                "SELECT gameid FROM {0} WHERE gameid = {1} " + "AND site = \"{2}\";",
                Tables.gamehhd,
                gameId,
                site);

            using (IDataReader dr = _dataProvider.ExecuteQuery(query))
            {
                if (dr.Read())
                {
                    var handId = (int) dr.GetInt64((int)GameTable.identity);
                    return handId;
                }

                return null;
            }
        }
    }
}