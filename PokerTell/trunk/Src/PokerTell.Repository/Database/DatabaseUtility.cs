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
    }
}