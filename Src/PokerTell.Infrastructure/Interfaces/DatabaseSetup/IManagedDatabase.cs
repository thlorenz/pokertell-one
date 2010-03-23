namespace PokerTell.Infrastructure.Interfaces.DatabaseSetup
{
    using System.Collections.Generic;

    public interface IManagedDatabase
    {
        string ConnectionString { get; }

        IDataProviderInfo DataProviderInfo { get; }

        IManagedDatabase ChooseDatabase(string databaseName);

        IManagedDatabase CreateDatabase(string databaseName);

        IManagedDatabase CreateTables();

        bool DatabaseExists(string databaseName);

        IManagedDatabase DeleteDatabase(string databaseName);

        IEnumerable<string> GetAllPokerTellDatabaseNames();

        string GetNameFor(string databaseInUse);
    }
}