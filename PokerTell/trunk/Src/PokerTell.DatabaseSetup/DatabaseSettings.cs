namespace PokerTell.DatabaseSetup
{
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;

    using Enumerations;

    using Infrastructure.Interfaces;

    using Interfaces;

    public class DatabaseSettings : IDatabaseSettings
    {
        #region Constants and Fields

        readonly IDataProviderInfos _dataProviderInfos;

        readonly ISettings _settings;

        #endregion

        #region Constructors and Destructors

        public DatabaseSettings(ISettings settings, IDataProviderInfos dataProviderInfos)
        {
            _settings = settings;
            _dataProviderInfos = dataProviderInfos;
        }

        #endregion

        #region Implemented Interfaces

        #region IDatabaseSettings

        public bool ConnectionStringExistsFor(IDataProviderInfo dataProviderInfo)
        {
            return _settings.ConnectionStrings[dataProviderInfo.FullName] != null;
        }

        public IEnumerable<IDataProviderInfo> GetAvailableProviders()
        {
            return from dataProviderInfo in _dataProviderInfos.Supported
                   let hasSavedServerConnectString =
                       !string.IsNullOrEmpty(GetServerConnectStringFor(dataProviderInfo))
                   where dataProviderInfo.IsEmbedded || hasSavedServerConnectString
                   select dataProviderInfo;
        }

        /// <summary>
        /// Obtains connection string for current provider from the settings
        /// </summary>
        /// <returns>connection string</returns>
        public string GetConnectionStringFor(IDataProviderInfo dataProviderInfo)
        {
            if (ConnectionStringExistsFor(dataProviderInfo))
            {
                return
                    _settings.ConnectionStrings[dataProviderInfo.FullName].ConnectionString.TrimEnd(';');
            }
            return string.Empty;
        }

        public IDataProviderInfo GetCurrentDataProvider()
        {
            return (from dataProviderInfo in _dataProviderInfos.Supported
                    let providerName = _settings.RetrieveString(ServerSettings.ProviderName.ToString())
                    where dataProviderInfo.FullName.Equals(providerName)
                    select dataProviderInfo).FirstOrDefault();
        }

        /// <summary>
        /// Only applies to MySql and Postgres
        /// </summary>
        /// <returns>
        /// Gets the connection string  that is necesary to connect
        /// to the server from the settings
        /// </returns>
        public string GetServerConnectStringFor(IDataProviderInfo dataProviderInfo)
        {
            string key = ServerSettingsUtility.GetServerConnectKeyFor(dataProviderInfo);

            return _settings.RetrieveString(key);
        }

        public bool ProviderIsAvailable(IDataProviderInfo dataProviderInfo)
        {
            return GetAvailableProviders().Any(dp => dp.FullName.Equals(dataProviderInfo.FullName));
        }

        public IDatabaseSettings SetConnectionStringFor(IDataProviderInfo dataProviderInfo, string connectionString)
        {
            if (!ConnectionStringExistsFor(dataProviderInfo))
            {
                _settings.ConnectionStrings.Add(
                    new ConnectionStringSettings(dataProviderInfo.FullName, connectionString));
            }
            else
            {
                _settings.ConnectionStrings[dataProviderInfo.FullName].ConnectionString = connectionString;
            }

            return this;
        }

        public IDatabaseSettings SetCurrentDataProviderTo(IDataProviderInfo dataProviderInfo)
        {
            _settings.Set(ServerSettings.ProviderName.ToString(), dataProviderInfo.FullName);
            return this;
        }

        public IDatabaseSettings SetServerConnectStringFor(
            IDataProviderInfo dataProviderInfo, string serverConnectString)
        {
            string key = ServerSettingsUtility.GetServerConnectKeyFor(dataProviderInfo);
            _settings.Set(key, serverConnectString);
            return this;
        }

        #endregion

        #endregion
    }
}