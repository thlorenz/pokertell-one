namespace PokerTell.DatabaseSetup
{
    using System;

    using Microsoft.Practices.Composite.Events;

    using PokerTell.DatabaseSetup.Properties;
    using PokerTell.Infrastructure.Events;
    using PokerTell.Infrastructure.Interfaces.DatabaseSetup;

    public class DatabaseConnector : IDatabaseConnector
    {
        #region Constants and Fields

        readonly IDatabaseSettings _databaseSettings;

        readonly IEventAggregator _eventAggregator;

        IDataProviderInfo _dataProviderInfo;

        #endregion

        #region Constructors and Destructors

        public DatabaseConnector(
            IEventAggregator eventAggregator, IDatabaseSettings databaseSettings, IDataProvider dataProvider)
        {
            _eventAggregator = eventAggregator;
            _databaseSettings = databaseSettings;
            DataProvider = dataProvider;
        }

        #endregion

        #region Properties

        public IDataProvider DataProvider { get; private set; }

        #endregion

        #region Implemented Interfaces

        #region IDatabaseConnector

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
                DataProvider.Connect(connectionString, _dataProviderInfo.FullName);
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

            PublishStatusUpdate();
        }

        void PublishStatusUpdate()
        {
            var statusMessage = string.Format(
                Resources.Status_ConnectedTo, DataProvider.DatabaseName, _dataProviderInfo.NiceName);
            
            var statusUpdate = new StatusUpdateEventArgs(StatusTypes.DatabaseConnection, statusMessage);
           
            _eventAggregator
                .GetEvent<StatusUpdateEvent>()
                .Publish(statusUpdate);
        }

        public void TryToConnectToServerUsing(string serverConnectString, bool showSuccessMessage)
        {
            if (!_dataProviderInfo.IsEmbedded)
            {
                try
                {
                    DataProvider.Connect(serverConnectString, _dataProviderInfo.FullName);
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

        #endregion

        #endregion

        #region Methods

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

        #endregion
    }
}