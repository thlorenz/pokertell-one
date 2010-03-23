namespace PokerTell.DatabaseSetup
{
    using System;

    using Microsoft.Practices.Composite.Events;

    using PokerTell.DatabaseSetup.Properties;
    using PokerTell.Infrastructure.Events;
    using PokerTell.Infrastructure.Interfaces.DatabaseSetup;

    public class DatabaseConnector : IDatabaseConnector
    {
        readonly IDatabaseSettings _databaseSettings;

        readonly IEventAggregator _eventAggregator;

        IDataProviderInfo _dataProviderInfo;

        public DatabaseConnector(
            IEventAggregator eventAggregator, IDatabaseSettings databaseSettings, IDataProvider dataProvider)
        {
            _eventAggregator = eventAggregator;
            _databaseSettings = databaseSettings;
            DataProvider = dataProvider;
        }

        public IDataProvider DataProvider { get; private set; }

        public IDatabaseConnector ConnectToServer()
        {
            if (_dataProviderInfo == null)
            {
                PublishWarningMessage(Resources.Warning_NoDataProviderDefinedInSettings);
                return this;
            }

            if (ProviderIsNotAvailable())
            {
                return this;
            }

            if (! _dataProviderInfo.IsEmbedded)
            {
                ConnectToExternalServer();
            }

            return this;
        }

        public IDatabaseConnector ConnectToDatabase()
        {
            if (_dataProviderInfo == null)
            {
                PublishWarningMessage(Resources.Warning_NoDataProviderDefinedInSettings);
                return this;
            }

            string connectionString = _databaseSettings.GetConnectionStringFor(_dataProviderInfo);
            if (connectionString == null)
            {
                string message = string.Format(
                    Resources.Warning_NoDatabaseHasBeenChosenForCurrentProvider, _dataProviderInfo.NiceName);
                PublishWarningMessage(message);
            }

            TryToConnectToDatabaseUsing(connectionString);

            return this;
        }

        void ConnectToExternalServer()
        {
            string serverConnectString = _databaseSettings.GetServerConnectStringFor(_dataProviderInfo);
            if (!ServerConnectStringIsInvalid(serverConnectString))
            {
                TryToConnectToServerUsing(serverConnectString, false);
            }
        }

        public IDatabaseManager CreateDatabaseManager()
        {
            if (_dataProviderInfo != null)
            {
                IManagedDatabase managedDatabase;

                if (_dataProviderInfo.IsEmbedded)
                {
                    managedDatabase = new EmbeddedManagedDatabase(DataProvider, _dataProviderInfo);
                    return new DatabaseManager(managedDatabase, _databaseSettings);
                }

                if (DataProvider.IsConnectedToServer)
                {
                    managedDatabase = new ExternalManagedDatabase(DataProvider, _dataProviderInfo);
                    return new DatabaseManager(managedDatabase, _databaseSettings);
                }
            }

            return null;
        }

        public IDatabaseConnector InitializeFromSettings()
        {
            IDataProviderInfo dataProviderInfo = _databaseSettings.GetCurrentDataProvider();

            return InitializeWith(dataProviderInfo);
        }

        public IDatabaseConnector InitializeWith(IDataProviderInfo dataProviderInfo)
        {
            _dataProviderInfo = dataProviderInfo;
            return this;
        }

        public void TryToConnectToDatabaseUsing(string connectionString)
        {
            try
            {
                DataProvider.Connect(connectionString, _dataProviderInfo);

                if (_dataProviderInfo.IsEmbedded)
                {
                    DataProvider.DatabaseName = new DatabaseConnectionInfo(connectionString).Database;
                }

                DataProvider.ParameterPlaceHolder = _dataProviderInfo.ParameterPlaceHolder;
            }
            catch (Exception excep)
            {
                string message = string.Format(Resources.Error_UnableToConnectToDatabaseSpecifiedInTheSettings);
                if (! _dataProviderInfo.IsEmbedded)
                {
                    message += "\n" + string.Format(Resources.Hint_EnsureExternalServerIsRunning, _dataProviderInfo.NiceName);
                }

                PublishErrorMessage(message, excep);

                return;
            }

            PublishDatabaseChangedEvent();
        }

        void PublishDatabaseChangedEvent()
        {
            _eventAggregator
                .GetEvent<DatabaseInUseChangedEvent>()
                .Publish(DataProvider);
        }

        public void TryToConnectToServerUsing(string serverConnectString, bool showSuccessMessage)
        {
            if (!_dataProviderInfo.IsEmbedded)
            {
                try
                {
                    DataProvider.Connect(serverConnectString, _dataProviderInfo);
                }
                catch (Exception excep)
                {
                    PublishErrorMessage(
                        string.Format(Resources.Error_UnableToConnectToServer, _dataProviderInfo.NiceName), excep);
                    return;
                }
            }

            DataProvider.ParameterPlaceHolder = _dataProviderInfo.ParameterPlaceHolder;

            if (showSuccessMessage)
            {
                PublishSuccessMessage();
            }
        }

        bool ProviderIsNotAvailable()
        {
            if (_databaseSettings.ProviderIsAvailable(_dataProviderInfo))
            {
                return false;
            }

            PublishErrorMessage(
                string.Format(Resources.Error_ProviderNotFoundInSettings, _dataProviderInfo.NiceName));

            return true;
        }

        void PublishErrorMessage(string message)
        {
            var userMessage = new UserMessageEventArgs(UserMessageTypes.Error, message);
            _eventAggregator.GetEvent<UserMessageEvent>().Publish(userMessage);
        }

        void PublishErrorMessage(string message, Exception excep)
        {
            var userMessage = new UserMessageEventArgs(UserMessageTypes.Error, message, excep);
            _eventAggregator.GetEvent<UserMessageEvent>().Publish(userMessage);
        }

        void PublishSuccessMessage()
        {
            string msg =
                string.Format(
                    Resources.Info_SuccessfullyConnectedToServer, 
                    _dataProviderInfo.NiceName);
            var userMessage = new UserMessageEventArgs(UserMessageTypes.Info, msg);

            _eventAggregator.GetEvent<UserMessageEvent>().Publish(userMessage);
        }

        void PublishWarningMessage(string message)
        {
            var userMessage = new UserMessageEventArgs(UserMessageTypes.Warning, message);
            _eventAggregator.GetEvent<UserMessageEvent>().Publish(userMessage);
        }

        bool ServerConnectStringIsInvalid(string serverConnectString)
        {
            var databaseConnectionInfo = new DatabaseConnectionInfo(serverConnectString);

            if (databaseConnectionInfo.IsValidForServerOnlyConnection())
            {
                return false;
            }

            PublishErrorMessage(
                string.Format(Resources.Error_SettingsContainInvalidServerConnectString, _dataProviderInfo.NiceName));

            return true;
        }
    }
}