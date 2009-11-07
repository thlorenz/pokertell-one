namespace PokerTell.DatabaseSetup.Tests
{
    using System;

    using Infrastructure.Events;
    using Infrastructure.Interfaces.DatabaseSetup;

    using Interfaces;

    using Microsoft.Practices.Composite.Events;

    using Moq;

    using NUnit.Framework;

    using UnitTests.Tools;

    using ViewModels;

    public class ConfigureDataProviderViewModelTests
    {
        #region Constants and Fields

        IEventAggregator _eventAggregator;

        StubBuilder _stub;

        #endregion

        #region Public Methods

        [SetUp]
        public void _Init()
        {
            _stub = new StubBuilder();
            _eventAggregator = new EventAggregator();
        }

        [Test]
        public void Initialize_SettingsDontContainServerConnectStringForDataProvider_InitializesToDefault()
        {
            var settingsMock = new Mock<IDatabaseSettings>();
            settingsMock
                .Setup(ds => ds.ProviderIsAvailable(It.IsAny<IDataProviderInfo>()))
                .Returns(false);

            var sut =
                new ConfigureDataProviderViewModelMock(
                    _eventAggregator, settingsMock.Object, _stub.Out<IDataProvider>())
                    .Initialize();

            bool initializedToDefault =
                sut.ServerName.Equals("localhost") && 
                sut.UserName.Equals("root") &&
                sut.Password.Equals(string.Empty);
            
            Assert.That(initializedToDefault, Is.True);
        }

        [Test]
        public void Initialize_SettingsContainServerConnectStringForDataProvider_InitializesAccordingToServerConnectString()
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

            var sut =
                new ConfigureDataProviderViewModelMock(
                    _eventAggregator, settingsMock.Object, _stub.Out<IDataProvider>())
                    { DataProviderInfoMock = dataProviderInfo }
                    .Initialize();

            var actualServerConnectString =
                new DatabaseConnectionInfo(sut.ServerName, sut.UserName, sut.Password).ServerConnectString;

            Assert.That(actualServerConnectString, Is.EqualTo(serverConnectStringFoundInSettings));
        }

        [Test]
        public void SaveCommandExecute_ValidSetupValues_SavesServerConnectStringToDatabaseSettings()
        {
            var settingsMock = new Mock<IDatabaseSettings>();

            ConfigureDataProviderViewModelMock sut =
                new ConfigureDataProviderViewModelMock(
                    _eventAggregator, settingsMock.Object, _stub.Out<IDataProvider>())
                    .InitializeServerNameUserNameAndPasswordToValidValues();

            sut.SaveCommand.Execute(null);

            settingsMock.Verify(ds => ds.SetServerConnectStringFor(sut.DataProviderInfoMock, It.IsAny<string>()));
        }

        [Test]
        public void TestConnectionCommandCanExecute_PasswordEmpty_ReturnsTrue()
        {
            ConfigureDataProviderViewModelMock sut =
                new ConfigureDataProviderViewModelMock(
                    _eventAggregator, _stub.Out<IDatabaseSettings>(), _stub.Out<IDataProvider>())
                    .InitializeServerNameUserNameAndPasswordToValidValues();
            sut.Password = string.Empty;

            Assert.That(sut.TestConnectionCommand.CanExecute(null), Is.True);
        }

        [Test]
        public void TestConnectionCommandCanExecute_PasswordNotSet_ReturnsFalse()
        {
            ConfigureDataProviderViewModelMock sut =
                new ConfigureDataProviderViewModelMock(
                    _eventAggregator, _stub.Out<IDatabaseSettings>(), _stub.Out<IDataProvider>())
                    .InitializeServerNameUserNameAndPasswordToValidValues();
            sut.Password = null;

            Assert.That(sut.TestConnectionCommand.CanExecute(null), Is.False);
        }

        [Test]
        public void TestConnectionCommandCanExecute_ServerNameEmpty_ReturnsFalse()
        {
            ConfigureDataProviderViewModelMock sut =
                new ConfigureDataProviderViewModelMock(
                    _eventAggregator, _stub.Out<IDatabaseSettings>(), _stub.Out<IDataProvider>())
                    .InitializeServerNameUserNameAndPasswordToValidValues();
            sut.ServerName = string.Empty;

            Assert.That(sut.TestConnectionCommand.CanExecute(null), Is.False);
        }

        [Test]
        public void TestConnectionCommandCanExecute_ServerNameNotSet_ReturnsFalse()
        {
            ConfigureDataProviderViewModelMock sut =
                new ConfigureDataProviderViewModelMock(
                    _eventAggregator, _stub.Out<IDatabaseSettings>(), _stub.Out<IDataProvider>())
                    .InitializeServerNameUserNameAndPasswordToValidValues();
            sut.ServerName = null;

            Assert.That(sut.TestConnectionCommand.CanExecute(null), Is.False);
        }

        [Test]
        public void TestConnectionCommandCanExecute_ServerNameUserNameAndPasswordValid_ReturnsTrue()
        {
            ConfigureDataProviderViewModelMock sut =
                new ConfigureDataProviderViewModelMock(
                    _eventAggregator, _stub.Out<IDatabaseSettings>(), _stub.Out<IDataProvider>())
                    .InitializeServerNameUserNameAndPasswordToValidValues();

            Assert.That(sut.TestConnectionCommand.CanExecute(null), Is.True);
        }

        [Test]
        public void TestConnectionCommandCanExecute_UserNameEmpty_ReturnsFalse()
        {
            ConfigureDataProviderViewModelMock sut =
                new ConfigureDataProviderViewModelMock(
                    _eventAggregator, _stub.Out<IDatabaseSettings>(), _stub.Out<IDataProvider>())
                    .InitializeServerNameUserNameAndPasswordToValidValues();
            sut.UserName = string.Empty;

            Assert.That(sut.TestConnectionCommand.CanExecute(null), Is.False);
        }

        [Test]
        public void TestConnectionCommandCanExecute_UserNameNotSet_ReturnsFalse()
        {
            ConfigureDataProviderViewModelMock sut =
                new ConfigureDataProviderViewModelMock(
                    _eventAggregator, _stub.Out<IDatabaseSettings>(), _stub.Out<IDataProvider>())
                    .InitializeServerNameUserNameAndPasswordToValidValues();
            sut.UserName = null;

            Assert.That(sut.TestConnectionCommand.CanExecute(null), Is.False);
        }

        [Test]
        public void TestConnectionCommandExecute_DataProviderDoesNotThrowError_ObtainsServerConnectString()
        {
            ConfigureDataProviderViewModelMock sut =
                new ConfigureDataProviderViewModelMock(
                    _eventAggregator, _stub.Out<IDatabaseSettings>(), _stub.Out<IDataProvider>())
                    .InitializeServerNameUserNameAndPasswordToValidValues();

            sut.TestConnectionCommand.Execute(null);

            Assert.That(sut.ServerConnectString, Is.Not.EqualTo(string.Empty));
        }

        [Test]
        public void TestConnectionCommandExecute_DataProviderDoesNotThrowError_PublishesUserInfoMessage()
        {
            ConfigureDataProviderViewModelMock sut =
                new ConfigureDataProviderViewModelMock(
                    _eventAggregator, _stub.Out<IDatabaseSettings>(), _stub.Out<IDataProvider>())
                    .InitializeServerNameUserNameAndPasswordToValidValues();

            bool userInfoMessageRequested = false;
            _eventAggregator
                .GetEvent<UserMessageEvent>()
                .Subscribe(arg => userInfoMessageRequested = arg.MessageType == UserMessageTypes.Info);

            sut.TestConnectionCommand.Execute(null);

            Assert.That(userInfoMessageRequested, Is.True);
        }

        [Test]
        public void TestConnectionCommandExecute_DataProviderDoesNotThrowError_SaveCommandCanExecute()
        {
            ConfigureDataProviderViewModelMock sut =
                new ConfigureDataProviderViewModelMock(
                    _eventAggregator, _stub.Out<IDatabaseSettings>(), _stub.Out<IDataProvider>())
                    .InitializeServerNameUserNameAndPasswordToValidValues();

            sut.TestConnectionCommand.Execute(null);

            Assert.That(sut.SaveCommand.CanExecute(null), Is.True);
        }

        [Test]
        public void TestConnectionCommandExecute_DataProviderThrowsError_PublishesUserErrorMessage()
        {
            var dataProviderStub = new Mock<IDataProvider>();
            dataProviderStub
                .Setup(dp => dp.Connect(It.IsAny<string>(), It.IsAny<string>()))
                .Throws(new Exception());

            ConfigureDataProviderViewModelMock sut =
                new ConfigureDataProviderViewModelMock(
                    _eventAggregator, _stub.Out<IDatabaseSettings>(), dataProviderStub.Object)
                    .InitializeServerNameUserNameAndPasswordToValidValues();

            bool userErrorMessageRequested = false;
            _eventAggregator
                .GetEvent<UserMessageEvent>()
                .Subscribe(arg => userErrorMessageRequested = arg.MessageType == UserMessageTypes.Error);

            sut.TestConnectionCommand.Execute(null);

            Assert.That(userErrorMessageRequested, Is.True);
        }

        [Test]
        public void TestConnectionCommandExecute_DataProviderThrowsError_SaveCommandCannotExecute()
        {
            var dataProviderStub = new Mock<IDataProvider>();
            dataProviderStub
                .Setup(dp => dp.Connect(It.IsAny<string>(), It.IsAny<string>()))
                .Throws(new Exception());

            ConfigureDataProviderViewModelMock sut =
                new ConfigureDataProviderViewModelMock(
                    _eventAggregator, _stub.Out<IDatabaseSettings>(), dataProviderStub.Object)
                    .InitializeServerNameUserNameAndPasswordToValidValues();

            sut.TestConnectionCommand.Execute(null);

            Assert.That(sut.SaveCommand.CanExecute(null), Is.False);
        }

        #endregion
    }

    internal class ConfigureDataProviderViewModelMock : ConfigureDataProviderViewModel
    {
        #region Constructors and Destructors

        public ConfigureDataProviderViewModelMock(
            IEventAggregator eventAggregator, IDatabaseSettings databaseSettings, IDataProvider dataProvider)
            : base(eventAggregator, databaseSettings, dataProvider)
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