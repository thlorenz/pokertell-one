namespace PokerTell.Infrastructure.Interfaces.DatabaseSetup
{
    using System.Collections.Generic;

    using DatabaseSetup;
    using DatabaseSetup;

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

        IManagedDatabase VersionDatabase(string databaseName);

        /// <summary>
        /// Initializes a new instance of the <see cref="IManagedDatabase"/> class. 
        /// Creates a database that will be managed by the Database Manager.
        /// Make sure to set up the dependencies as explained for each parameter.
        /// </summary>
        /// <param name="dataProvider">
        /// Needs to be connected to a server 
        /// ParameterPlaceHolder needs to be set
        /// </param>
        /// <param name="dataProviderInfo">
        /// External: MySql, Postgres etc.
        /// </param>
        IManagedDatabase InitializeWith(IDataProvider dataProvider, IDataProviderInfo dataProviderInfo);
    }

    public interface IExternalManagedDatabase : IManagedDatabase
    {
        IEnumerable<string> GetAllPokerOfficeDatabaseNames();

        IEnumerable<string> GetAllPokerTrackerDatabaseNames();
    }

    public interface IEmbeddedManagedDatabase : IManagedDatabase
    {
    }
}