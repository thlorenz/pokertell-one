namespace PokerTell.DatabaseSetup
{
    using System;

    using Infrastructure.Events;
    using Infrastructure.Interfaces.DatabaseSetup;

    using Microsoft.Practices.Composite.Events;

    using Properties;

    public class DatabaseConnector : IDatabaseConnector
    {
        readonly IDatabaseSettings _databaseSettings;

        public IDataProvider DataProvider { get; private set; }

        IDataProviderInfo _dataProviderInfo;

        readonly IEventAggregator _eventAggregator;

        public DatabaseConnector(IEventAggregator eventAggregator, IDatabaseSettings databaseSettings, IDataProvider dataProvider)
        {
            _eventAggregator = eventAggregator;
            _databaseSettings = databaseSettings;
            DataProvider = dataProvider;
        }

        public IDatabaseConnector InitializeWith(IDataProviderInfo dataProviderInfo)
        {
            _dataProviderInfo = dataProviderInfo;
            return this;
        }

        public IDatabaseConnector InitializeFromSettings()
        {
            var dataProviderInfo = _databaseSettings.GetCurrentDataProvider();
            
            return InitializeWith(dataProviderInfo);
        }

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

            string serverConnectString = _databaseSettings.GetServerConnectStringFor(_dataProviderInfo);
            if (ServerConnectStringIsInvalid(serverConnectString))
            {
                return this;
            }

            TryToConnectToTheServerUsing(serverConnectString, false);

            return this;
        }

        public void TryToConnectToTheServerUsing(string serverConnectString, bool showSuccessMessage)
        {
            try
            {
                if (! _dataProviderInfo.IsEmbedded)
                {
                    DataProvider.Connect(serverConnectString, _dataProviderInfo.FullName);
                }

                DataProvider.ParameterPlaceHolder = _dataProviderInfo.ParameterPlaceHolder;

                if (showSuccessMessage)
                {
                    PublishSuccessMessage();
                }
            }
            catch (Exception excep)
            {
                PublishErrorMessage(
                    string.Format(Resources.Error_UnableToConnectToServer, _dataProviderInfo.NiceName), excep);
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

        void PublishWarningMessage(string message)
        {
            var userMessage = new UserMessageEventArgs(UserMessageTypes.Warning, message);
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
    }
}