namespace PokerTell.Infrastructure.Interfaces.DatabaseSetup
{
    using System.Collections.Generic;

    public interface IDatabaseManager
    {
        #region Public Methods

        IDatabaseManager ChooseDatabase(string databaseName);

        IDatabaseManager ClearDatabase(string databaseName);

        IDatabaseManager CreateDatabase(string databaseName);

        IDatabaseManager DeleteDatabase(string databaseName);

        IEnumerable<string> GetAllPokerTellDatabases();

        #endregion

        /// <summary>
        /// Finds databse used for the Provider
        /// </summary>
        /// <returns>Database in use or empty if none is found</returns>
        string GetDatabaseInUse();
    }
}