namespace PokerTell.Repository.Tests
{
    using Fakes;

    using Infrastructure.Events;
    using Infrastructure.Interfaces.DatabaseSetup;

    using Microsoft.Practices.Composite.Events;

    using Moq;

    using NUnit.Framework;

    using PokerTell.Infrastructure.Interfaces.PokerHand;
    using PokerTell.Repository.Interfaces;

    public class RepositoryTests
    {
        #region Constants and Fields

        Mock<IRepositoryDatabase> _connectedDatabaseStub;

        StubBuilder _stub;

        IEventAggregator _eventAggregator;

        #endregion

        #region Public Methods

        [SetUp]
        public void _Init()
        {
            _stub = new StubBuilder();
            _connectedDatabaseStub = new Mock<IRepositoryDatabase>();
            _connectedDatabaseStub
                .SetupGet(db => db.IsConnected)
                .Returns(true);
            _eventAggregator = new EventAggregator();
        }

        [Test]
        public void InsertHand_DatabaseIsConnected_InsertsHandIntoDatabase()
        {
            Mock<IRepositoryDatabase> databaseMock = _connectedDatabaseStub;

            var sut = new Repository(_eventAggregator, databaseMock.Object, _stub.Out<IRepositoryParser>());

            var handMock = new Mock<IConvertedPokerHand>();

            sut.InsertHand(handMock.Object);

            databaseMock.Verify(db => db.InsertHandAndReturnHandId(handMock.Object));
        }

        [Test]
        public void InsertHand_DatabaseIsNotConnected_DoesNotInsertHandIntoDatabase()
        {
            Mock<IRepositoryDatabase> databaseMock = _connectedDatabaseStub;

            databaseMock
                .SetupGet(db => db.IsConnected)
                .Returns(false);

            var sut = new Repository(_eventAggregator, databaseMock.Object, _stub.Out<IRepositoryParser>());

            var handMock = new Mock<IConvertedPokerHand>();

            sut.InsertHand(handMock.Object);

            databaseMock.Verify(db => db.InsertHandAndReturnHandId(handMock.Object), Times.Never());
        }

        [Test]
        public void InsertHand_CachedBefore_HandIdReturnedByDatabaseInsertIsNotCachedAgain()
        {
            const int handId = 1;

            _connectedDatabaseStub
                .Setup(db => db.InsertHandAndReturnHandId(It.IsAny<IConvertedPokerHand>()))
                .Returns(handId);

            var sut = new Repository(_eventAggregator, _connectedDatabaseStub.Object, _stub.Out<IRepositoryParser>());
            sut.CachedHands.Add(handId, _stub.Out<IConvertedPokerHand>());

            sut.InsertHand(_stub.Out<IConvertedPokerHand>());

            Assert.That(sut.CachedHands.Count, Is.EqualTo(1));
        }

        [Test]
        public void InsertHand_NotInsertedBefore_HandIdReturnedByDatabaseInsertIsCached()
        {
            const int handId = 1;

            _connectedDatabaseStub
                .Setup(db => db.InsertHandAndReturnHandId(It.IsAny<IConvertedPokerHand>()))
                .Returns(handId);

            var sut = new Repository(_eventAggregator, _connectedDatabaseStub.Object, _stub.Out<IRepositoryParser>());

            sut.InsertHand(_stub.Out<IConvertedPokerHand>());

            Assert.That(sut.CachedHands.ContainsKey(handId), Is.True);
        }

        [Test]
        public void RetrieveConvertedHand_DatabaseIsNotConnected_DoesNotCallDatabaseRetrieveHand()
        {
            var databaseMock = _connectedDatabaseStub;

            databaseMock
                .SetupGet(db => db.IsConnected)
                .Returns(false);

            var sut = new Repository(_eventAggregator, _connectedDatabaseStub.Object, _stub.Out<IRepositoryParser>());

            sut.RetrieveConvertedHand(_stub.Some<int>());

            _connectedDatabaseStub.Verify(db => db.RetrieveConvertedHand(It.IsAny<int>()), Times.Never());
        }

        [Test]
        public void RetrieveConvertedHand_DatabaseIsNotConnected_ReturnsNull()
        {
            _connectedDatabaseStub
                .SetupGet(db => db.IsConnected)
                .Returns(false);

            var sut = new Repository(_eventAggregator, _connectedDatabaseStub.Object, _stub.Out<IRepositoryParser>());

            var result = sut.RetrieveConvertedHand(_stub.Some<int>());

            Assert.That(result, Is.Null);
        }

        [Test]
        public void RetrieveConvertedHand_NotRetrievedBefore_CallsDatabaseRetrieveHand()
        {
            var databaseMock = _connectedDatabaseStub;

            const int handId = 1;
            var sut = new Repository(_eventAggregator, _connectedDatabaseStub.Object, _stub.Out<IRepositoryParser>());

            sut.RetrieveConvertedHand(handId);

            databaseMock.Verify(db => db.RetrieveConvertedHand(handId));
        }

        [Test]
        public void RetrieveConvertedHand_NotRetrievedBefore_AddsRetrievedHandToCachedHands()
        {
            const int handId = 1;
            var sut = new Repository(_eventAggregator, _connectedDatabaseStub.Object, _stub.Out<IRepositoryParser>());

            sut.RetrieveConvertedHand(handId);

            Assert.That(sut.CachedHands.ContainsKey(handId), Is.True);
        }

        [Test]
        public void RetrieveConvertedHand_AlreadyCached_DoesNotAddRetrievedHandToCachedHands()
        {
            const int handId = 1;
            var sut = new Repository(_eventAggregator, _connectedDatabaseStub.Object, _stub.Out<IRepositoryParser>());
            sut.CachedHands.Add(handId, _stub.Out<IConvertedPokerHand>());

            sut.RetrieveConvertedHand(handId);

            Assert.That(sut.CachedHands.Count, Is.EqualTo(1));
        }

        [Test]
        public void RetrieveConvertedHand_DatabaseIsConnected_ReturnsHandReturnedByDatabase()
        {
            const int handId = 1;
            var retrievedHandStub = new ConvertedPokerHandStub { HandId = handId };
           
            _connectedDatabaseStub
                .Setup(db => db.RetrieveConvertedHand(It.IsAny<int>()))
                .Returns(retrievedHandStub);

            var sut = new Repository(_eventAggregator, _connectedDatabaseStub.Object, _stub.Out<IRepositoryParser>());

            var result = sut.RetrieveConvertedHand(_stub.Some<int>());

            Assert.That(result.HandId, Is.EqualTo(handId));
        }

        [Test]
        public void DatabaseInUseChangedEvent_Always_CachedHandsAreCleared()
        {
            var sut = new Repository(_eventAggregator, _stub.Out<IRepositoryDatabase>(), _stub.Out<IRepositoryParser>());
            sut.CachedHands.Add(_stub.Some(1), _stub.Out<IConvertedPokerHand>());

            _eventAggregator
                .GetEvent<DatabaseInUseChangedEvent>()
                .Publish(_stub.Out<IDataProvider>());

            Assert.That(sut.CachedHands.Count, Is.EqualTo(0));
        }

        [Test]
        public void DatabaseInUseChangedEvent_Always_DatabaseUseIsCalledWithNewDataProvider()
        {
            var databaseMock = _connectedDatabaseStub;
            var sut = new Repository(_eventAggregator, databaseMock.Object, _stub.Out<IRepositoryParser>());
            sut.CachedHands.Add(_stub.Some(1), _stub.Out<IConvertedPokerHand>());

            var dataProviderStub = new Mock<IDataProvider>();
            _eventAggregator
                .GetEvent<DatabaseInUseChangedEvent>()
                .Publish(dataProviderStub.Object);

            databaseMock.Verify(db => db.Use(dataProviderStub.Object));
        }

        #endregion
    }
}