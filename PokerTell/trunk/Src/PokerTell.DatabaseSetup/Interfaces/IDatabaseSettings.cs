using System.Collections.Generic;

using PokerTell.Infrastructure.Interfaces.DatabaseSetup;

namespace PokerTell.DatabaseSetup.Interfaces
{
    public interface IDatabaseSettings
    {
        IEnumerable<IDataProviderInfo> GetAvailableProviders();

        /// <summary>
        /// Obtains connection string for current provider from the settings
        /// </summary>
        /// <returns>connection string</returns>
        string GetConnectionStringFor(IDataProviderInfo dataProviderInfo);

        /// <summary>
        /// Only applies to MySql and Postgres
        /// </summary>
        /// <returns>
        /// Gets the connection string  that is necesary to connect
        /// to the server from the settings
        /// </returns>
        string GetServerConnectStringFor(IDataProviderInfo dataProviderInfo);

        IDatabaseSettings SetServerConnectStringFor(IDataProviderInfo dataProviderInfo, string serverConnectString);

        bool ProviderIsAvailable(IDataProviderInfo dataProviderInfo);

        bool ConnectionStringExistsFor(IDataProviderInfo dataProviderInfo);

        IDatabaseSettings SetCurrentDataProviderTo(IDataProviderInfo dataProviderInfo);

        IDataProviderInfo GetCurrentDataProvider();

        IDatabaseSettings SetConnectionStringFor(IDataProviderInfo dataProviderInfo, string connectionString);
    }
}