namespace PokerTell.DatabaseSetup.Tests
{
    using System;

    using Base;

    using Microsoft.Practices.Composite.Events;
    using Microsoft.Practices.Unity;

    using Moq;

    using NUnit.Framework;

    using PokerTell.Infrastructure.Interfaces.DatabaseSetup;

    public class ConfigureDataProviderViewModelTests
    {
        #region Constants and Fields

        IUnityContainer _container;

        StubBuilder _stub;

        #endregion

        #region Public Methods

        [SetUp]
        public void _Init()
        {
            _stub = new StubBuilder();
            _container = new UnityContainer()
                .RegisterType<IEventAggregator, EventAggregator>()
                .RegisterInstance(_stub.Out<IDatabaseSettings>())
                .RegisterInstance(_stub.Out<IDataProvider>())
                .RegisterInstance(_stub.Out<IDatabaseConnector>());
        }

        [Test]
        public void
            Initialize_SettingsContainServerConnectStringForDataProvider_InitializesAccordingToServerConnectString()
        {
            var dataProviderInfo = new MySqlInfo();

            const string serverConnectStringFoundInSettings =
                "data source = servername; user id = username; password = pass;";

            var settingsMock = new Mock<IDatabaseSettings>();
            settingsMock
                .Setup(ds => ds.ProviderIsAvailable(dataProviderInfo))
                .Returns(true);
            settingsMock
                .Setup(ds => ds.GetServerConnectStringFor(dataProviderInfo))
                .Returns(serverConnectStringFoundInSettings);

            _container
                .RegisterInstance(settingsMock.Object);

            var sut = _container
                .Resolve<ConfigureDataProviderViewModelMock>();
            sut.DataProviderInfoMock = dataProviderInfo;
            sut.Initialize();

            string actualServerConnectString =
                new DatabaseConnectionInfo(sut.ServerName, sut.UserName, sut.Password).ServerConnectString;

            Assert.That(actualServerConnectString, Is.EqualTo(serverConnectStringFoundInSettings));
        }

        [Test]
        public void Initialize_SettingsDontContainServerConnectStringForDataProvider_InitializesToDefault()
        {
            var settingsMock = new Mock<IDatabaseSettings>();
            settingsMock
                .Setup(ds => ds.ProviderIsAvailable(It.IsAny<IDataProviderInfo>()))
                .Returns(false);

            _container
                .RegisterInstance(settingsMock.Object);

            ConfigureDataProviderViewModelBase sut = _container
                .Resolve<ConfigureDataProviderViewModelMock>()
                .Initialize();

            bool initializedToDefault =
                sut.ServerName.Equals("localhost") &&
                sut.UserName.Equals("root") &&
                sut.Password.Equals(string.Empty);

            Assert.That(initializedToDefault, Is.True);
        }

        [Test]
        public void SaveCommandExecute_ValidSetupValues_SavesServerConnectStringToDatabaseSettings()
        {
            var settingsMock = new Mock<IDatabaseSettings>();

            _container
                .RegisterInstance(settingsMock.Object);

            ConfigureDataProviderViewModelMock sut = _container
                .Resolve<ConfigureDataProviderViewModelMock>()
                .InitializeServerNameUserNameAndPasswordToValidValues();

            sut.SaveCommand.Execute(null);

            settingsMock.Verify(ds => ds.SetServerConnectStringFor(sut.DataProviderInfoMock, It.IsAny<string>()));
        }

        [Test]
        public void TestConnectionCommandCanExecute_PasswordEmpty_ReturnsTrue()
        {
            ConfigureDataProviderViewModelMock sut = _container
                .Resolve<ConfigureDataProviderViewModelMock>()
                .InitializeServerNameUserNameAndPasswordToValidValues();

            sut.Password = string.Empty;

            Assert.That(sut.TestConnectionCommand.CanExecute(null), Is.True);
        }

        [Test]
        public void TestConnectionCommandCanExecute_ServerNameEmpty_ReturnsFalse()
        {
            ConfigureDataProviderViewModelMock sut = _container
                .Resolve<ConfigureDataProviderViewModelMock>()
                .InitializeServerNameUserNameAndPasswordToValidValues();

            sut.ServerName = string.Empty;

            Assert.That(sut.TestConnectionCommand.CanExecute(null), Is.False);
        }

        [Test]
        public void TestConnectionCommandCanExecute_ServerNameNotSet_ReturnsFalse()
        {
            ConfigureDataProviderViewModelMock sut = _container
                .Resolve<ConfigureDataProviderViewModelMock>()
                .InitializeServerNameUserNameAndPasswordToValidValues();

            sut.ServerName = null;

            Assert.That(sut.TestConnectionCommand.CanExecute(null), Is.False);
        }

        [Test]
        public void TestConnectionCommandCanExecute_ServerNameUserNameAndPasswordValid_ReturnsTrue()
        {
            ConfigureDataProviderViewModelMock sut = _container
                .Resolve<ConfigureDataProviderViewModelMock>()
                .InitializeServerNameUserNameAndPasswordToValidValues();

            Assert.That(sut.TestConnectionCommand.CanExecute(null), Is.True);
        }

        [Test]
        public void TestConnectionCommandCanExecute_UserNameEmpty_ReturnsFalse()
        {
            ConfigureDataProviderViewModelMock sut = _container
                .Resolve<ConfigureDataProviderViewModelMock>()
                .InitializeServerNameUserNameAndPasswordToValidValues();

            sut.UserName = string.Empty;

            Assert.That(sut.TestConnectionCommand.CanExecute(null), Is.False);
        }

        [Test]
        public void TestConnectionCommandCanExecute_UserNameNotSet_ReturnsFalse()
        {
            ConfigureDataProviderViewModelMock sut = _container
                .Resolve<ConfigureDataProviderViewModelMock>()
                .InitializeServerNameUserNameAndPasswordToValidValues();

            sut.UserName = null;

            Assert.That(sut.TestConnectionCommand.CanExecute(null), Is.False);
        }

        [Test]
        public void TestConnectionCommandExecute_DataProviderDoesNotThrowError_ObtainsServerConnectString()
        {
            ConfigureDataProviderViewModelMock sut = _container
                .Resolve<ConfigureDataProviderViewModelMock>()
                .InitializeServerNameUserNameAndPasswordToValidValues();

            sut.TestConnectionCommand.Execute(null);

            Assert.That(sut.ServerConnectString, Is.Not.EqualTo(string.Empty));
        }

        [Test]
        public void TestConnectionCommandExecute_DataProviderSuccessfullyOpensConnection_SaveCommandCanExecute()
        {
            var dataProviderStub = new Mock<IDataProvider>();
            dataProviderStub
                .SetupGet(dp => dp.IsConnectedToServer)
                .Returns(true);
          
            var databaseConnectorStub = new Mock<IDatabaseConnector>();
            databaseConnectorStub
                .SetupGet(dc => dc.DataProvider)
                .Returns(dataProviderStub.Object);

            _container
                .RegisterInstance(dataProviderStub.Object)
                .RegisterInstance(databaseConnectorStub.Object);

            ConfigureDataProviderViewModelMock sut = _container
                .Resolve<ConfigureDataProviderViewModelMock>()
                .InitializeServerNameUserNameAndPasswordToValidValues();

            sut.TestConnectionCommand.Execute(null);

            Assert.That(sut.SaveCommand.CanExecute(null), Is.True);
        }

        [Test]
        public void TestConnectionCommandExecute_DataProviderCannotOpenConnection_SaveCommandCannotExecute()
        {
            var dataProviderStub = new Mock<IDataProvider>();
            dataProviderStub
                .SetupGet(dp => dp.IsConnectedToServer)
                .Returns(false);

            _container
                .RegisterInstance(dataProviderStub.Object);

            ConfigureDataProviderViewModelMock sut = _container
                .Resolve<ConfigureDataProviderViewModelMock>()
                .InitializeServerNameUserNameAndPasswordToValidValues();

            sut.TestConnectionCommand.Execute(null);

            Assert.That(sut.SaveCommand.CanExecute(null), Is.False);
        }

        #endregion
    }

    internal class ConfigureDataProviderViewModelMock : ConfigureDataProviderViewModelBase
    {
        #region Constructors and Destructors

        public ConfigureDataProviderViewModelMock(
            IEventAggregator eventAggregator, 
            IDatabaseSettings databaseSettings, 
            IDatabaseConnector databaseConnector)
            : base(eventAggregator, databaseSettings, databaseConnector)
        {
            DataProviderInfoMock = new SqLiteInfo();
        }

        #endregion

        #region Properties

        public IDataProviderInfo DataProviderInfoMock { get; set; }

        public string ServerConnectString
        {
            get { return _serverConnectString; }
        }

        protected override IDataProviderInfo DataProviderInfo
        {
            get { return DataProviderInfoMock; }
        }

        #endregion

        #region Public Methods

        public ConfigureDataProviderViewModelMock InitializeServerNameUserNameAndPasswordToValidValues()
        {
            ServerName = "someServer";
            UserName = "someUser";
            Password = "somePassword";

            return this;
        }

        #endregion
    }
}