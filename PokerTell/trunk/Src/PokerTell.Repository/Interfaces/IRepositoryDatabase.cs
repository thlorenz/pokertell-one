namespace PokerTell.Repository.Interfaces
{
    using System.Collections.Generic;
    using System.Data;

    using Infrastructure.Interfaces.DatabaseSetup;
    using Infrastructure.Interfaces.PokerHand;

    public interface IRepositoryDatabase
    {
        bool IsConnected { get; }

        int ExecuteNonQuery(string nonQuery);

        IDataReader ExecuteQuery(string query);

        IList<T> ExecuteQueryGetColumn<T>(string query, int column);

        IList<T> ExecuteQueryGetFirstColumn<T>(string query);

        /// <summary>
        /// Tries to obtain an integer value from the query
        /// </summary>
        /// <param name="query">Query like Count, sum or max</param>
        /// <returns>Result or null if not possible to convert the result to int</returns>
        int? ExecuteScalar(string query);

        DataTable GetDataTableFor(string query);

        /// <summary>
        /// Inserts Hand into database and returns the Id that was assigned to it
        /// </summary>
        /// <param name="convertedHand"></param>
        /// <returns>Id used in the identity column of the gamehhd table of the database</returns>
        int? InsertHandAndReturnHandId(IConvertedPokerHand convertedHand);

        IConvertedPokerHand RetrieveConvertedHand(int handId);

        IRepositoryDatabase Use(IDataProvider dataProvider);

        IRepositoryDatabase InsertHandsAndSetTheirHandIds(IEnumerable<IConvertedPokerHand> handsToInsert);
    }
}