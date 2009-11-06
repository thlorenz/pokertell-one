namespace PokerTell.DatabaseSetup.ViewModels
{
    using System;
    using System.Windows.Input;

    using Infrastructure.Events;
    using Infrastructure.Interfaces.DatabaseSetup;

    using Interfaces;

    using Microsoft.Practices.Composite.Events;

    using Properties;

    using Tools.WPF;
    using Tools.WPF.ViewModels;

    public abstract class AddDataProviderViewModel : NotifyPropertyChanged
    {
        #region Constants and Fields

        protected string _serverConnectString;

        readonly IDatabaseSettings _databaseSettings;

        readonly IDataProvider _dataProvider;

        bool _connectionIsValid;

        string _password;

        ICommand _saveCommand;

        string _serverName;

        ICommand _testConnectionCommand;

        string _userName;

        protected readonly IEventAggregator _eventAggregator;

        #endregion

        #region Constructors and Destructors

        protected AddDataProviderViewModel(IEventAggregator eventAggregator, IDatabaseSettings databaseSettings, IDataProvider dataProvider)
        {
            _eventAggregator = eventAggregator;
            _dataProvider = dataProvider;
            _databaseSettings = databaseSettings;
        }

        protected AddDataProviderViewModel InititializeWith(string serverName, string userName, string password)
        {
            _serverName = serverName;
            _userName = userName;
            _password = Password;
            return this;
        }

        #endregion

        #region Properties

        ICommand _SetDefaultsCommand;

        public ICommand SetDefaultsCommand
        {
            get
            {
                return _SetDefaultsCommand ?? (_SetDefaultsCommand = new SimpleCommand
                    {
                        ExecuteDelegate = arg => InititializeWith("localhost", "root", string.Empty),
                    });
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                RaisePropertyChanged(() => Password);
            }
        }

        public ICommand SaveCommand
        {
            get
            {
                return _saveCommand ?? (_saveCommand = new SimpleCommand
                    {
                        ExecuteDelegate =
                            arg => _databaseSettings.SetServerConnectStringFor(DataProviderInfo, _serverConnectString),
                        CanExecuteDelegate = arg => _connectionIsValid
                    });
            }
        }

        public string ServerName
        {
            get { return _serverName; }
            set
            {
                _serverName = value;
                RaisePropertyChanged(() => ServerName);
            }
        }

        public ICommand TestConnectionCommand
        {
            get
            {
                return _testConnectionCommand ?? (_testConnectionCommand = new SimpleCommand
                    {
                        ExecuteDelegate = arg => TestConnection(),
                        CanExecuteDelegate =
                            arg => ! (string.IsNullOrEmpty(ServerName) || string.IsNullOrEmpty(UserName) || Password == null)
                    });
            }
        }

        public string UserName
        {
            get { return _userName; }
            set
            {
                _userName = value;
                RaisePropertyChanged(() => UserName);
            }
        }

        protected abstract IDataProviderInfo DataProviderInfo { get; }

        #endregion

        #region Methods

        void TestConnection()
        {
            _connectionIsValid = false;
            try
            {
                _serverConnectString = new DatabaseConnectionInfo(ServerName, UserName, Password).ServerConnectString;

                _dataProvider.Connect(_serverConnectString, DataProviderInfo.FullName);

                _connectionIsValid = true;

                PublishSuccessUserMessage();
            }
            catch (Exception excep)
            {
                PublishErrorUserMessage(excep);
            }
        }

        void PublishErrorUserMessage(Exception excep)
        {
            string msg = Resources.Error_UnableToConnectToServer;
            var userMessage = new UserMessageEventArgs(UserMessageTypes.Error, msg, excep);
            _eventAggregator.GetEvent<UserMessageEvent>().Publish(userMessage);
        }

        void PublishSuccessUserMessage()
        {
            string msg =
                string.Format(
                    Resources.Info_SuccessfullyConnectedToServer,
                    DataProviderInfo.NiceName);
            var userMessage = new UserMessageEventArgs(UserMessageTypes.Info, msg);

            _eventAggregator.GetEvent<UserMessageEvent>().Publish(userMessage);
        }

        #endregion
    }
}