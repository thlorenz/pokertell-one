namespace PokerTell.Infrastructure.Interfaces.DatabaseSetup
{
    public interface IDatabaseConnector
    {
        IDatabaseConnector ConnectToServer();

        void TryToConnectToServerUsing(string serverConnectString, bool showSuccessMessage);

        IDatabaseConnector InitializeWith(IDataProviderInfo dataProviderInfo);

        IDatabaseConnector InitializeFromSettings();

        IDatabaseManager CreateDatabaseManager();

        IDataProvider DataProvider { get; }

        void TryToConnectToDatabaseUsing(string connectionString);
    }
}