namespace PokerTell.DatabaseSetup.Tests
{
    using System;

    using Infrastructure.Events;
    using Infrastructure.Interfaces.DatabaseSetup;

    using Interfaces;

    using Microsoft.Practices.Composite.Events;

    using Moq;

    using NUnit.Framework;

    using ViewModels;

    public class AddDataProviderViewModelTests
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
        public void SaveCommandExecute_ValidSetupValues_SavesServerConnectStringToDatabaseSettings()
        {
            var settingsMock = new Mock<IDatabaseSettings>();

            AddDataProviderViewModelMock sut =
                new AddDataProviderViewModelMock(_eventAggregator, settingsMock.Object, _stub.Out<IDataProvider>())
                    .InitializeServerNameUserNameAndPasswordToValidValues();

            sut.SaveCommand.Execute(null);

            settingsMock.Verify(ds => ds.SetServerConnectStringFor(sut.DataProviderInfoMock, It.IsAny<string>()));
        }

        [Test]
        public void TestConnectionCommandCanExecute_PasswordEmpty_ReturnsTrue()
        {
            AddDataProviderViewModelMock sut =
                new AddDataProviderViewModelMock(
                    _eventAggregator, _stub.Out<IDatabaseSettings>(), _stub.Out<IDataProvider>())
                    .InitializeServerNameUserNameAndPasswordToValidValues();
            sut.Password = string.Empty;

            Assert.That(sut.TestConnectionCommand.CanExecute(null), Is.True);
        }

        [Test]
        public void TestConnectionCommandCanExecute_PasswordNotSet_ReturnsFalse()
        {
            AddDataProviderViewModelMock sut =
                new AddDataProviderViewModelMock(
                    _eventAggregator, _stub.Out<IDatabaseSettings>(), _stub.Out<IDataProvider>())
                    .InitializeServerNameUserNameAndPasswordToValidValues();
            sut.Password = null;

            Assert.That(sut.TestConnectionCommand.CanExecute(null), Is.False);
        }

        [Test]
        public void TestConnectionCommandCanExecute_ServerNameEmpty_ReturnsFalse()
        {
            AddDataProviderViewModelMock sut =
                new AddDataProviderViewModelMock(
                    _eventAggregator, _stub.Out<IDatabaseSettings>(), _stub.Out<IDataProvider>())
                    .InitializeServerNameUserNameAndPasswordToValidValues();
            sut.ServerName = string.Empty;

            Assert.That(sut.TestConnectionCommand.CanExecute(null), Is.False);
        }

        [Test]
        public void TestConnectionCommandCanExecute_ServerNameNotSet_ReturnsFalse()
        {
            AddDataProviderViewModelMock sut =
                new AddDataProviderViewModelMock(
                    _eventAggregator, _stub.Out<IDatabaseSettings>(), _stub.Out<IDataProvider>())
                    .InitializeServerNameUserNameAndPasswordToValidValues();
            sut.ServerName = null;

            Assert.That(sut.TestConnectionCommand.CanExecute(null), Is.False);
        }

        [Test]
        public void TestConnectionCommandCanExecute_ServerNameUserNameAndPasswordValid_ReturnsTrue()
        {
            AddDataProviderViewModelMock sut =
                new AddDataProviderViewModelMock(
                    _eventAggregator, _stub.Out<IDatabaseSettings>(), _stub.Out<IDataProvider>())
                    .InitializeServerNameUserNameAndPasswordToValidValues();

            Assert.That(sut.TestConnectionCommand.CanExecute(null), Is.True);
        }

        [Test]
        public void TestConnectionCommandCanExecute_UserNameEmpty_ReturnsFalse()
        {
            AddDataProviderViewModelMock sut =
                new AddDataProviderViewModelMock(
                    _eventAggregator, _stub.Out<IDatabaseSettings>(), _stub.Out<IDataProvider>())
                    .InitializeServerNameUserNameAndPasswordToValidValues();
            sut.UserName = string.Empty;

            Assert.That(sut.TestConnectionCommand.CanExecute(null), Is.False);
        }

        [Test]
        public void TestConnectionCommandCanExecute_UserNameNotSet_ReturnsFalse()
        {
            AddDataProviderViewModelMock sut =
                new AddDataProviderViewModelMock(
                    _eventAggregator, _stub.Out<IDatabaseSettings>(), _stub.Out<IDataProvider>())
                    .InitializeServerNameUserNameAndPasswordToValidValues();
            sut.UserName = null;

            Assert.That(sut.TestConnectionCommand.CanExecute(null), Is.False);
        }

        [Test]
        public void TestConnectionCommandExecute_DataProviderDoesNotThrowError_ObtainsServerConnectString()
        {
            AddDataProviderViewModelMock sut =
                new AddDataProviderViewModelMock(
                    _eventAggregator, _stub.Out<IDatabaseSettings>(), _stub.Out<IDataProvider>())
                    .InitializeServerNameUserNameAndPasswordToValidValues();

            sut.TestConnectionCommand.Execute(null);

            Assert.That(sut.ServerConnectString, Is.Not.EqualTo(string.Empty));
        }

        [Test]
        public void TestConnectionCommandExecute_DataProviderDoesNotThrowError_SaveCommandCanExecute()
        {
            AddDataProviderViewModelMock sut =
                new AddDataProviderViewModelMock(
                    _eventAggregator, _stub.Out<IDatabaseSettings>(), _stub.Out<IDataProvider>())
                    .InitializeServerNameUserNameAndPasswordToValidValues();

            sut.TestConnectionCommand.Execute(null);

            Assert.That(sut.SaveCommand.CanExecute(null), Is.True);
        }

        [Test]
        public void TestConnectionCommandExecute_DataProviderDoesNotThrowError_PublishesUserInfoMessage()
        {
            AddDataProviderViewModelMock sut =
                new AddDataProviderViewModelMock(
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
        public void TestConnectionCommandExecute_DataProviderThrowsError_SaveCommandCannotExecute()
        {
            var dataProviderStub = new Mock<IDataProvider>();
            dataProviderStub
                .Setup(dp => dp.Connect(It.IsAny<string>(), It.IsAny<string>()))
                .Throws(new Exception());

            AddDataProviderViewModelMock sut =
                new AddDataProviderViewModelMock(
                    _eventAggregator, _stub.Out<IDatabaseSettings>(), dataProviderStub.Object)
                    .InitializeServerNameUserNameAndPasswordToValidValues();

            sut.TestConnectionCommand.Execute(null);

            Assert.That(sut.SaveCommand.CanExecute(null), Is.False);
        }

        [Test]
        public void TestConnectionCommandExecute_DataProviderThrowsError_PublishesUserErrorMessage()
        {
            var dataProviderStub = new Mock<IDataProvider>();
            dataProviderStub
                .Setup(dp => dp.Connect(It.IsAny<string>(), It.IsAny<string>()))
                .Throws(new Exception());

            AddDataProviderViewModelMock sut =
                new AddDataProviderViewModelMock(
                    _eventAggregator, _stub.Out<IDatabaseSettings>(), dataProviderStub.Object)
                    .InitializeServerNameUserNameAndPasswordToValidValues();

            bool userErrorMessageRequested = false;
            _eventAggregator
                .GetEvent<UserMessageEvent>()
                .Subscribe(arg => userErrorMessageRequested = arg.MessageType == UserMessageTypes.Error);

            sut.TestConnectionCommand.Execute(null);

            Assert.That(userErrorMessageRequested, Is.True);
        }

        #endregion
    }

    internal class AddDataProviderViewModelMock : AddDataProviderViewModel
    {
        #region Constructors and Destructors

        public AddDataProviderViewModelMock(
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

        public AddDataProviderViewModelMock InitializeServerNameUserNameAndPasswordToValidValues()
        {
            ServerName = "someServer";
            UserName = "someUser";
            Password = "somePassword";

            return this;
        }

        #endregion
    }
}