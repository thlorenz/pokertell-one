namespace PokerTell.DatabaseSetup.Tests
{
    using System;

    using Microsoft.Practices.Composite.Events;

    using Moq;

    using NUnit.Framework;

    using PokerTell.Infrastructure.Events;
    using PokerTell.Infrastructure.Interfaces.DatabaseSetup;

    public class DatabaseConnectorTests
    {
        #region Constants and Fields

        const string ValidServerConnectString = "data Source = someSource; user id = someUser;";

        IEventAggregator _aggregator;

        Mock<IDataProvider> _dataProviderMock;

        StubBuilder _stub;

        #endregion

        #region Public Methods

        [SetUp]
        public void _Init()
        {
            _stub = new StubBuilder();
            _dataProviderMock = new Mock<IDataProvider>();
            _aggregator = new EventAggregator();
        }

        [Test]
        public void ConnectToServer_InvalidServerConnectStringFoundInSettings_DoesNotConnectDataProvider()
        {
            var settingsStub = new Mock<IDatabaseSettings>();
            settingsStub
                .Setup(ds => ds.ProviderIsAvailable(It.IsAny<IDataProviderInfo>()))
                .Returns(true);
            settingsStub
                .Setup(ds => ds.GetServerConnectStringFor(It.IsAny<IDataProviderInfo>()))
                .Returns("invalid serverConnectString");

            var sut = new DatabaseConnector(_aggregator, settingsStub.Object, _dataProviderMock.Object)
               .InitializeWith(_stub.Out<IDataProviderInfo>())
               .ConnectToServer();

            _dataProviderMock.Verify(dp => dp.Connect(It.IsAny<string>(), It.IsAny<string>()), Times.Never());
        }

        [Test]
        public void ConnectToServer_InvalidServerConnectStringFoundInSettings_PublishesError()
        {
            var settingsStub = new Mock<IDatabaseSettings>();
            settingsStub
                .Setup(ds => ds.ProviderIsAvailable(It.IsAny<IDataProviderInfo>()))
                .Returns(true);
            settingsStub
                .Setup(ds => ds.GetServerConnectStringFor(It.IsAny<IDataProviderInfo>()))
                .Returns("invalid serverConnectString");

            bool errorWasPublished = false;
            _aggregator.GetEvent<UserMessageEvent>()
                .Subscribe(arg => errorWasPublished = arg.MessageType.Equals(UserMessageTypes.Error));

            var sut = new DatabaseConnector(_aggregator, settingsStub.Object, _dataProviderMock.Object)
                .InitializeWith(_stub.Out<IDataProviderInfo>())
                .ConnectToServer();

            Assert.That(errorWasPublished, Is.True);
        }

        [Test]
        public void ConnectToServer_ServerConnectStringNotFoundInSettings_DoesNotConnectDataProvider()
        {
            var settingsStub = new Mock<IDatabaseSettings>();
            settingsStub
                .Setup(ds => ds.ProviderIsAvailable(It.IsAny<IDataProviderInfo>()))
                .Returns(false);

            var sut = new DatabaseConnector(_aggregator, settingsStub.Object, _dataProviderMock.Object)
                .InitializeWith(_stub.Out<IDataProviderInfo>())
                .ConnectToServer();

            _dataProviderMock.Verify(dp => dp.Connect(It.IsAny<string>(), It.IsAny<string>()), Times.Never());
        }

        [Test]
        public void ConnectToServer_ServerConnectStringNotFoundInSettings_PublishesError()
        {
            var settingsStub = new Mock<IDatabaseSettings>();
            settingsStub
                .Setup(ds => ds.ProviderIsAvailable(It.IsAny<IDataProviderInfo>()))
                .Returns(false);

            bool errorWasPublished = false;
            _aggregator.GetEvent<UserMessageEvent>()
                .Subscribe(arg => errorWasPublished = arg.MessageType.Equals(UserMessageTypes.Error));

            var sut = new DatabaseConnector(_aggregator, settingsStub.Object, _dataProviderMock.Object)
               .InitializeWith(_stub.Out<IDataProviderInfo>())
               .ConnectToServer();

            Assert.That(errorWasPublished, Is.True);
        }

        [Test]
        public void
            ConnectToServer_ValidServerConnectStringAndConnectionSuccessful_DataProviderParameterPlaceHolderIsSetToAccordingToTheDataProviderInfo()
        {
            var settingsStub = new Mock<IDatabaseSettings>();
            settingsStub
                .Setup(ds => ds.ProviderIsAvailable(It.IsAny<IDataProviderInfo>()))
                .Returns(true);
            settingsStub
                .Setup(ds => ds.GetServerConnectStringFor(It.IsAny<IDataProviderInfo>()))
                .Returns(ValidServerConnectString);

            var dataProviderInfoStub = new Mock<IDataProviderInfo>();
            const string placeholder = "placeHolder";
            dataProviderInfoStub
                .SetupGet(dpi => dpi.ParameterPlaceHolder)
                .Returns(placeholder);
            
            _dataProviderMock
                .SetupProperty(dp => dp.ParameterPlaceHolder);

            var sut = new DatabaseConnector(_aggregator, settingsStub.Object, _dataProviderMock.Object)
               .InitializeWith(dataProviderInfoStub.Object)
               .ConnectToServer();

            Assert.That(_dataProviderMock.Object.ParameterPlaceHolder, Is.EqualTo(placeholder));
        }

        [Test]
        public void ConnectToServer_ValidServerConnectStringButConnectFails_PublishesError()
        {
            var settingsStub = new Mock<IDatabaseSettings>();
            settingsStub
                .Setup(ds => ds.ProviderIsAvailable(It.IsAny<IDataProviderInfo>()))
                .Returns(true);
            settingsStub
                .Setup(ds => ds.GetServerConnectStringFor(It.IsAny<IDataProviderInfo>()))
                .Returns(ValidServerConnectString);
            _dataProviderMock
                .Setup(dp => dp.Connect(It.IsAny<string>(), It.IsAny<string>()))
                .Throws(new Exception());

            bool errorWasPublished = false;
            _aggregator.GetEvent<UserMessageEvent>()
                .Subscribe(arg => errorWasPublished = arg.MessageType.Equals(UserMessageTypes.Error));

            var sut = new DatabaseConnector(_aggregator, settingsStub.Object, _dataProviderMock.Object)
               .InitializeWith(_stub.Out<IDataProviderInfo>())
               .ConnectToServer();

            Assert.That(errorWasPublished, Is.True);
        }

        [Test]
        public void ConnectToServer_ValidServerConnectStringFoundInSettings_ConnectsDataProvider()
        {
            var settingsStub = new Mock<IDatabaseSettings>();
            settingsStub
                .Setup(ds => ds.ProviderIsAvailable(It.IsAny<IDataProviderInfo>()))
                .Returns(true);
            settingsStub
                .Setup(ds => ds.GetServerConnectStringFor(It.IsAny<IDataProviderInfo>()))
                .Returns(ValidServerConnectString);

            var sut = new DatabaseConnector(_aggregator, settingsStub.Object, _dataProviderMock.Object)
               .InitializeWith(_stub.Out<IDataProviderInfo>())
               .ConnectToServer();

            _dataProviderMock.Verify(dp => dp.Connect(ValidServerConnectString, It.IsAny<string>()));
        }

        [Test]
        public void ConnectToServer_DataProvideIsNullr_PublishesWarning()
        {
            bool errorWasPublished = false;
            _aggregator.GetEvent<UserMessageEvent>()
                .Subscribe(arg => errorWasPublished = arg.MessageType.Equals(UserMessageTypes.Warning));

            var sut = new DatabaseConnector(_aggregator, _stub.Out<IDatabaseSettings>(), _dataProviderMock.Object)
                .InitializeWith(null)
                .ConnectToServer();

            Assert.That(errorWasPublished, Is.True);
        }

        [Test]
        public void ConnectToServerUsing_ExternalDatabase_CallsDataProviderConnectWithServerConnectString()
        {
            var dataProviderInfoStub = new Mock<IDataProviderInfo>(); 

            dataProviderInfoStub
                .SetupGet(dpi => dpi.IsEmbedded)
                .Returns(false);

            var sut = new DatabaseConnector(_aggregator, _stub.Out<IDatabaseSettings>(), _dataProviderMock.Object)
                .InitializeWith(dataProviderInfoStub.Object);

            const string serverConnectString = "someString";
            sut.TryToConnectToTheServerUsing(serverConnectString, _stub.Some(false));

            _dataProviderMock.Verify(dp => dp.Connect(serverConnectString, It.IsAny<string>()));
        }

        [Test]
        public void ConnectToServerUsing_EmbeddedDatabase_DoesNotCallDataProviderConnect()
        {
            var dataProviderInfoStub = new Mock<IDataProviderInfo>();

            dataProviderInfoStub
                .SetupGet(dpi => dpi.IsEmbedded)
                .Returns(true);

            var sut = new DatabaseConnector(_aggregator, _stub.Out<IDatabaseSettings>(), _dataProviderMock.Object)
                .InitializeWith(dataProviderInfoStub.Object);

            sut.TryToConnectToTheServerUsing("someConnectString", _stub.Some(false));

            _dataProviderMock.Verify(dp => dp.Connect(It.IsAny<string>(), It.IsAny<string>()), Times.Never());
        }

        [Test]
        public void CreateDatabaseManager_EmbeddedDatabase_ReturnsNewDatabaseManager()
        {
            var dataProviderInfoStub = new Mock<IDataProviderInfo>();

            dataProviderInfoStub
                .SetupGet(dpi => dpi.IsEmbedded)
                .Returns(true);

            var sut = new DatabaseConnector(_aggregator, _stub.Out<IDatabaseSettings>(), _dataProviderMock.Object)
                .InitializeWith(dataProviderInfoStub.Object);

            var dataBaseManager = sut.CreateDatabaseManager();

            Assert.That(dataBaseManager, Is.Not.Null);
        }

        [Test]
        public void CreateDatabaseManager_ExternalUnconnectedDatabase_ReturnsNull()
        {
            var dataProviderInfoStub = new Mock<IDataProviderInfo>();

            dataProviderInfoStub
                .SetupGet(dpi => dpi.IsEmbedded)
                .Returns(false);

            _dataProviderMock
                .SetupGet(dp => dp.IsConnectedToServer)
                .Returns(false);

            var sut = new DatabaseConnector(_aggregator, _stub.Out<IDatabaseSettings>(), _dataProviderMock.Object)
                .InitializeWith(dataProviderInfoStub.Object);

            var dataBaseManager = sut.CreateDatabaseManager();

            Assert.That(dataBaseManager, Is.Null);
        }

        [Test]
        public void CreateDatabaseManager_DataProviderInfoIsNull_ReturnsNull()
        {
            _dataProviderMock
                .SetupGet(dp => dp.IsConnectedToServer)
                .Returns(false);

            var sut = new DatabaseConnector(_aggregator, _stub.Out<IDatabaseSettings>(), _dataProviderMock.Object)
                .InitializeWith(null);

            var dataBaseManager = sut.CreateDatabaseManager();

            Assert.That(dataBaseManager, Is.Null);
        }

        [Test]
        public void CreateDatabaseManager_ExternalConnectedDatabase_ReturnsNewDatabaseManager()
        {
            var dataProviderInfoStub = new Mock<IDataProviderInfo>();

            dataProviderInfoStub
                .SetupGet(dpi => dpi.IsEmbedded)
                .Returns(false);

            _dataProviderMock
                .SetupGet(dp => dp.IsConnectedToServer)
                .Returns(true);

            var sut = new DatabaseConnector(_aggregator, _stub.Out<IDatabaseSettings>(), _dataProviderMock.Object)
                .InitializeWith(dataProviderInfoStub.Object);

            var dataBaseManager = sut.CreateDatabaseManager();

            Assert.That(dataBaseManager, Is.Not.Null);
        }
        #endregion
    }
}