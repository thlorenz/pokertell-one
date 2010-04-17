namespace PokerTell.Infrastructure.Interfaces.DatabaseSetup
{
    using System.Collections.Generic;

    public interface IDatabaseSettings
    {
        bool ConnectionStringExistsFor(IDataProviderInfo dataProviderInfo);

        IEnumerable<IDataProviderInfo> GetAvailableProviders();

        /// <summary>
        /// Obtains connection string for current provider from the settings
        /// </summary>
        /// <returns>connection string</returns>
        string GetConnectionStringFor(IDataProviderInfo dataProviderInfo);

        IDataProviderInfo GetCurrentDataProvider();

        /// <summary>
        /// Only applies to MySql and Postgres
        /// </summary>
        /// <returns>
        /// Gets the connection string  that is necesary to connect
        /// to the server from the settings
        /// </returns>
        string GetServerConnectStringFor(IDataProviderInfo dataProviderInfo);

        bool ProviderIsAvailable(IDataProviderInfo dataProviderInfo);

        IDatabaseSettings SetConnectionStringFor(IDataProviderInfo dataProviderInfo, string connectionString);

        IDatabaseSettings SetCurrentDataProviderTo(IDataProviderInfo dataProviderInfo);

        IDatabaseSettings SetServerConnectStringFor(IDataProviderInfo dataProviderInfo, string serverConnectString);
    }
}