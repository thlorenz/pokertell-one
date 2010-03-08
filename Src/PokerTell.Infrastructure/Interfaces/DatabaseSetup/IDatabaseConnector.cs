namespace PokerTell.Infrastructure.Interfaces.DatabaseSetup
{
    using NHibernate;

    public interface IDatabaseConnector
    {
        IDatabaseConnector ConnectToServer();

        void TryToConnectToServerUsing(string serverConnectString, bool showSuccessMessage);

        IDatabaseConnector InitializeWith(IDataProviderInfo dataProviderInfo);

        IDatabaseConnector InitializeFromSettings();

        IDatabaseManager CreateDatabaseManager();

        IDataProvider DataProvider { get; }

        void TryToConnectToDatabaseUsing(string connectionString);

        IDatabaseConnector ConnectToDatabase();
    }
}