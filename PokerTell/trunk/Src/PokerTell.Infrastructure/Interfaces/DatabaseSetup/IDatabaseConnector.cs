namespace PokerTell.Infrastructure.Interfaces.DatabaseSetup
{
    public interface IDatabaseConnector
    {
        IDatabaseConnector ConnectToServer();

        void TryToConnectToTheServerUsing(string serverConnectString, bool showSuccessMessage);

        IDatabaseConnector InitializeWith(IDataProviderInfo dataProviderInfo);

        IDatabaseConnector InitializeFromSettings();

        IDatabaseManager CreateDatabaseManager();

        IDataProvider DataProvider { get; }
    }
}