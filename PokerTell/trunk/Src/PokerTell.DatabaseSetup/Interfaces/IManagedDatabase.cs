namespace PokerTell.DatabaseSetup.Interfaces
{
    using System.Collections.Generic;

    public interface IManagedDatabase
    {
        #region Properties

        string ConnectionString { get; }

        IDataProviderInfo DataProviderInfo { get; }

        #endregion

        #region Public Methods

        IManagedDatabase ChooseDatabase(string databaseName);

        IManagedDatabase CreateDatabase(string databaseName);

        IManagedDatabase CreateTables();

        bool DatabaseExists(string databaseName);

        IManagedDatabase DeleteDatabase(string databaseName);

        IEnumerable<string> GetAllPokerTellDatabaseNames();

        #endregion
    }
}