namespace PokerTell.Repository.Tests
{
    using System;
    using System.Collections.Generic;

    using Microsoft.Practices.Composite.Events;

    using Moq;

    using global::NHibernate;

    using NUnit.Framework;

    using PokerTell.Infrastructure.Interfaces.DatabaseSetup;
    using PokerTell.Infrastructure.Interfaces.PokerHand;
    using PokerTell.Infrastructure.Interfaces.Repository;
    using PokerTell.Repository.Interfaces;
    using PokerTell.UnitTests.Tools;

    public class RepositoryTests
    {
        #region Constants and Fields

        IEventAggregator _eventAggregator;

        Mock<IConvertedPokerHandDao> _pokerHandDaoMock;

        Mock<IConvertedPokerHandDaoFactory> _pokerHandDaoMakeStub;

        StubBuilder _stub;

        Mock<ITransactionManagerFactory> _transactionManagerMakeStub;

        Mock<ITransactionManager> _transactionManagerMock;

        #endregion

        #region Public Methods

        [SetUp]
        public void _Init()
        {
            _stub = new StubBuilder();
            _eventAggregator = new EventAggregator();

            _pokerHandDaoMock = new Mock<IConvertedPokerHandDao>();
            _pokerHandDaoMakeStub = new Mock<IConvertedPokerHandDaoFactory>();
            _pokerHandDaoMakeStub
                .Setup(phdm => phdm.New(It.IsAny<ISession>()))
                .Returns(_pokerHandDaoMock.Object);

            _transactionManagerMock = new Mock<ITransactionManager>();
           
            _transactionManagerMock
                .Setup(tm => tm.ExecuteWithoutCommitting(It.IsAny<Action>()))
                .Returns(_transactionManagerMock.Object);

            _transactionManagerMakeStub = new Mock<ITransactionManagerFactory>();
            _transactionManagerMakeStub
                .Setup(tm => tm.New(It.IsAny<ITransaction>()))
                .Returns(_transactionManagerMock.Object);
        }

        [Test]
        public void InsertHand_DatabaseIsConnected_ExecutesTransaction()
        {
            IRepository sut = new Repository(_eventAggregator,
                                              _pokerHandDaoMakeStub.Object, 
                                             _transactionManagerMakeStub.Object, 
                                             _stub.Out<IRepositoryParser>())
                .Use(GetConnectedDataProviderStub());

            sut.InsertHand(_stub.Out<IConvertedPokerHand>());

            _transactionManagerMock.Verify(tx => tx.Execute(It.IsAny<Action>()));
        }

        [Test]
        public void InsertHand_DatabaseIsConnected_CreatesPokerHandDaoWithNewSession()
        {
            var pokerHandDaoMakeMock = _pokerHandDaoMakeStub;
            IRepository sut = new Repository(_eventAggregator,
                                             pokerHandDaoMakeMock.Object, 
                                             _transactionManagerMakeStub.Object, 
                                             _stub.Out<IRepositoryParser>())
                .Use(GetConnectedDataProviderStub());

            var handMock = new Mock<IConvertedPokerHand>();

            sut.InsertHand(handMock.Object);

            pokerHandDaoMakeMock.Verify(phdm => phdm.New(It.IsAny<ISession>()));
        }

        [Test]
        public void InsertHand_DatabaseIsNotConnected_DoesNotExecuteTransaction()
        {
            IRepository sut = new Repository(_eventAggregator,
                                             _stub.Out<IConvertedPokerHandDaoFactory>(), 
                                             _transactionManagerMakeStub.Object, 
                                             _stub.Out<IRepositoryParser>())
                .Use(GetUnconnectedDataProviderStub());

            sut.InsertHand(_stub.Out<IConvertedPokerHand>());

            _transactionManagerMock.Verify(tx => tx.Execute(It.IsAny<Action>()), Times.Never());
        }

        [Test]
        public void InsertHands_TwoHands_CommitsTransactionOnlyOnce()
        {
            IRepository sut = new Repository(_eventAggregator,
                                             _pokerHandDaoMakeStub.Object, 
                                             _transactionManagerMakeStub.Object, 
                                             _stub.Out<IRepositoryParser>())
                .Use(GetConnectedDataProviderStub());

            IList<IConvertedPokerHand> handsToInsert = new[]
                {
                    _stub.Out<IConvertedPokerHand>(), _stub.Out<IConvertedPokerHand>()
                };

            sut.InsertHands(handsToInsert);

            _transactionManagerMock.Verify(tm => tm.Commit(), Times.Once());
        }

        [Test]
        public void InsertHands_TwoHands_ExecutesTwoTransactionsWithoutCommitting()
        {
            IRepository sut = new Repository(_eventAggregator,
                                             _pokerHandDaoMakeStub.Object, 
                                             _transactionManagerMakeStub.Object, 
                                             _stub.Out<IRepositoryParser>())
                .Use(GetConnectedDataProviderStub());

            IList<IConvertedPokerHand> handsToInsert = new[]
                {
                    _stub.Out<IConvertedPokerHand>(), _stub.Out<IConvertedPokerHand>()
                };

            sut.InsertHands(handsToInsert);

            _transactionManagerMock.Verify(tm => tm.ExecuteWithoutCommitting(It.IsAny<Action>()), Times.Exactly(2));
        }

        [Test]
        public void InsertHands_TwoHands_CreatesPokerHandDaoWithNewSessionOnlyOnce()
        {
            var pokerHandDaoMakeMock = _pokerHandDaoMakeStub;
            IRepository sut = new Repository(_eventAggregator,
                                             pokerHandDaoMakeMock.Object,
                                             _transactionManagerMakeStub.Object,
                                             _stub.Out<IRepositoryParser>())
                .Use(GetConnectedDataProviderStub());

            IList<IConvertedPokerHand> handsToInsert = new[]
                {
                    _stub.Out<IConvertedPokerHand>(), _stub.Out<IConvertedPokerHand>()
                };

            sut.InsertHands(handsToInsert);

           pokerHandDaoMakeMock.Verify(phdm => phdm.New(It.IsAny<ISession>()), Times.Once());
        }

        [Test]
        public void InsertHands_TwoHands_CreatesTransactionOnlyOnce()
        {
            var transactionManagerMakeMock = _transactionManagerMakeStub;

            IRepository sut = new Repository(_eventAggregator,
                                              _pokerHandDaoMakeStub.Object,
                                             transactionManagerMakeMock.Object, 
                                             _stub.Out<IRepositoryParser>())
                .Use(GetConnectedDataProviderStub());

            IList<IConvertedPokerHand> handsToInsert = new[]
                {
                    _stub.Out<IConvertedPokerHand>(), _stub.Out<IConvertedPokerHand>()
                };

            sut.InsertHands(handsToInsert);

            transactionManagerMakeMock.Verify(tmm => tmm.New(It.IsAny<ITransaction>()), Times.Once());
        }

        [Test]
        public void RetrieveConvertedHand_DatabaseIsConnected_DoesGetHandWithGivenIdFromPokerHandDao()
        {
            IRepository sut = new Repository(_eventAggregator,
                                             _pokerHandDaoMakeStub.Object, 
                                             _transactionManagerMakeStub.Object, 
                                             _stub.Out<IRepositoryParser>())
                .Use(GetConnectedDataProviderStub());

            const int handId = 1;
            sut.RetrieveConvertedHand(handId);

            _pokerHandDaoMock.Verify(phd => phd.Get(handId));
        }

        [Test]
        public void RetrieveConvertedHand_DatabaseIsConnected_ReturnsResultOfGetHandWithGivenIdFromPokerHandDao()
        {
            const int handId = 1;
            const ulong gameId = 1;
            var returnedHandStub = new Mock<IConvertedPokerHand>();
            returnedHandStub
                .SetupGet(hand => hand.GameId)
                .Returns(gameId);

            _pokerHandDaoMock
                .Setup(phd => phd.Get(handId))
                .Returns(returnedHandStub.Object);

            IRepository sut = new Repository(_eventAggregator,
                                             _pokerHandDaoMakeStub.Object, 
                                             _transactionManagerMakeStub.Object, 
                                             _stub.Out<IRepositoryParser>())
                .Use(GetConnectedDataProviderStub());

            IConvertedPokerHand retrievedHand = sut.RetrieveConvertedHand(handId);

            retrievedHand.GameId.IsEqualTo(gameId);
        }

        [Test]
        public void RetrieveConvertedHand_DatabaseIsNotConnected_DoesNotGetHandFromPokerHandDao()
        {
            IRepository sut = new Repository(_eventAggregator,
                                             _pokerHandDaoMakeStub.Object, 
                                             _transactionManagerMakeStub.Object, 
                                             _stub.Out<IRepositoryParser>())
                .Use(GetUnconnectedDataProviderStub());

            sut.RetrieveConvertedHand(_stub.Some(1));

            _pokerHandDaoMock.Verify(phd => phd.Get(It.IsAny<int>()), Times.Never());
        }

        [Test]
        public void RetrieveConvertedHand_DatabaseIsNotConnected_ReturnsNull()
        {
            IRepository sut = new Repository(_eventAggregator,
                                             _pokerHandDaoMakeStub.Object, 
                                             _transactionManagerMakeStub.Object, 
                                             _stub.Out<IRepositoryParser>())
                .Use(GetUnconnectedDataProviderStub());

            IConvertedPokerHand retrievedHand = sut.RetrieveConvertedHand(_stub.Some(1));

            retrievedHand.IsNull();
        }

        [Test]
        public void RetrieveConvertedHandWith_DatabaseIsNotConnected_DoesNotGetHandFromPokerHandDao()
        {
            IRepository sut = new Repository(_eventAggregator,
                                             _pokerHandDaoMakeStub.Object,
                                             _transactionManagerMakeStub.Object,
                                             _stub.Out<IRepositoryParser>())
                .Use(GetUnconnectedDataProviderStub());

            sut.RetrieveConvertedHandWith(_stub.Some<ulong>(1), "someSite");

            _pokerHandDaoMock.Verify(phd => phd.GetHandWith(It.IsAny<ulong>(), It.IsAny<string>()), Times.Never());
        }

        [Test]
        public void RetrieveConvertedHandWith_DatabaseIsNotConnected_ReturnsNull()
        {
            IRepository sut = new Repository(_eventAggregator,
                                             _pokerHandDaoMakeStub.Object,
                                             _transactionManagerMakeStub.Object,
                                             _stub.Out<IRepositoryParser>())
                .Use(GetUnconnectedDataProviderStub());

            IConvertedPokerHand retrievedHand = sut.RetrieveConvertedHandWith(_stub.Some<ulong>(1), "someSite");

            retrievedHand.IsNull();
        }

        [Test]
        public void RetrieveConvertedHandWith_DatabaseIsConnected_ReturnsResultOfGetHandWithGivenGameIdAndSiteFromPokerHandDao()
        {
            const ulong gameId = 1;
            const string somesite = "someSite";
            var returnedHandStub = new Mock<IConvertedPokerHand>();
            returnedHandStub
                .SetupGet(hand => hand.GameId)
                .Returns(gameId);
            
            _pokerHandDaoMock
                .Setup(phd => phd.GetHandWith(gameId, somesite))
                .Returns(returnedHandStub.Object);

            IRepository sut = new Repository(_eventAggregator,
                                             _pokerHandDaoMakeStub.Object,
                                             _transactionManagerMakeStub.Object,
                                             _stub.Out<IRepositoryParser>())
                .Use(GetConnectedDataProviderStub());

            IConvertedPokerHand retrievedHand = sut.RetrieveConvertedHandWith(gameId, somesite);

            retrievedHand.GameId.IsEqualTo(gameId);
        }

        #endregion

        #region Methods

        IDataProvider GetConnectedDataProviderStub()
        {
            var sessionFactoryStub = new Mock<ISessionFactory>();
            sessionFactoryStub
                .Setup(sf => sf.OpenSession())
                .Returns(_stub.Out<ISession>);

            return _stub.Setup<IDataProvider>()
                .Get(dp => dp.NewSessionFactory)
                .Returns(sessionFactoryStub.Object)
                .Out;
        }

        IDataProvider GetConnectedDataProviderStub(ISessionFactory sessionFactoryStub)
        {
            return _stub.Setup<IDataProvider>()
                .Get(dp => dp.NewSessionFactory)
                .Returns(sessionFactoryStub)
                .Out;
        }

        IDataProvider GetUnconnectedDataProviderStub()
        {
            return _stub.Setup<IDataProvider>()
                .Get(dp => dp.NewSessionFactory)
                .Returns(null)
                .Out;
        }

        #endregion
    }
}

/* Old Repository Tests need to be reimplemented after NHibernate is integrated
   
  Mock<IRepositoryDatabase> _connectedDatabaseStub;
 _connectedDatabaseStub = new Mock<IRepositoryDatabase>();
         _connectedDatabaseStub
             .SetupGet(db => db.IsConnected)
             .Returns(true);

public class RepositoryTests
{


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
 */

/* No caching done anymore -> Tests redundant
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

 *  [Test]
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
 * 
 *  [Test]
    public void DatabaseInUseChangedEvent_Always_CachedHandsAreCleared()
    {
        var sut = new Repository(_eventAggregator, _stub.Out<IRepositoryDatabase>(), _stub.Out<IRepositoryParser>());
        sut.CachedHands.Add(_stub.Some(1), _stub.Out<IConvertedPokerHand>());

        _eventAggregator
            .GetEvent<DatabaseInUseChangedEvent>()
            .Publish(_stub.Out<IDataProvider>());

        Assert.That(sut.CachedHands.Count, Is.EqualTo(0));
    }
 * 
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
     
 * 

    [Test]
    public void InsertHands_TwoHandsOneCachedBefore_CachesUncachedHand()
    {
        Mock<IRepositoryDatabase> databaseMock = _connectedDatabaseStub;

        var sut = new Repository(_eventAggregator, databaseMock.Object, _stub.Out<IRepositoryParser>());

        var hand1Mock = new Mock<IConvertedPokerHand>();
        hand1Mock
            .SetupGet(hand => hand.Id)
            .Returns(1);
        var hand2Mock = new Mock<IConvertedPokerHand>();
       hand2Mock
           .SetupGet(hand => hand.Id)
           .Returns(2);

        IList<IConvertedPokerHand> handsToInsert = new[] { hand1Mock.Object, hand2Mock.Object };

        databaseMock
            .Setup(db => db.InsertHandsAndSetTheirHandIds(handsToInsert))
            .Returns(databaseMock.Object);

        sut.CachedHands.Add(1, hand1Mock.Object);
           
        sut.InsertHands(handsToInsert);

       Assert.That(sut.CachedHands.Count, Is.EqualTo(2));
    }

 */