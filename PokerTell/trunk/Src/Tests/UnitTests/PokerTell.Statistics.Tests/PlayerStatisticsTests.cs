namespace PokerTell.Statistics.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Infrastructure.Events;
    using Infrastructure.Interfaces.DatabaseSetup;
    using Infrastructure.Interfaces.PokerHand;
    using Infrastructure.Interfaces.Repository;

    using Interfaces;

    using Microsoft.Practices.Composite.Events;

    using Moq;

    using NUnit.Framework;

    using UnitTests.Tools;

    [TestFixture]
    public class PlayerStatisticsTests
    {
        IEventAggregator _eventAggregator;

        PlayerStatisticsMock _sut;

        StubBuilder _stub;

        Mock<IRepository> _repositoryMock;

        const string Name = "somePlayer";
        
        const string Site = "someSite";

        const int Id = 1;

        IPlayerIdentity _playerIdentityStub;


        [SetUp]
        public void _Init()
        {
            _stub = new StubBuilder();
            _eventAggregator = new EventAggregator();
            _repositoryMock = new Mock<IRepository>();
           
            _playerIdentityStub = _stub.Setup<IPlayerIdentity>()
                .Get(pi => pi.Id).Returns(Id)
                .Out;
            
            _sut = new PlayerStatisticsMock(_eventAggregator);
        }

        [Test]
        public void DatabaseInUseChangedEventWasRaised_LastQueriedIdIsNotZero_SetsLastQueriedIdIsToZero()
        {
            _sut.LastQueriedId = 1;

            _eventAggregator.GetEvent<DatabaseInUseChangedEvent>()
                .Publish(_stub.Out<IDataProvider>());

            _sut.LastQueriedId.IsEqualTo(0);
        }

        [Test]
        public void DatabaseInUseChangedEventWasRaised_AnalyzablePlayersNotEmpty_ReinitializesAnalyzablePlayers()
        {
            _sut.AnalyzablePlayers.Add(_stub.Out<IAnalyzablePokerPlayer>());

            _eventAggregator.GetEvent<DatabaseInUseChangedEvent>()
                .Publish(_stub.Out<IDataProvider>());

            _sut.AnalyzablePlayers.IsEmpty();
        }

        [Test]
        public void UpdateFrom_PlayerIdentityIsNull_CallsRepositoryFindPlayerIdentityForNameAndSite()
        {
            _sut
                .InitializePlayer(Name, Site)
                .UpdateFrom(_repositoryMock.Object);

            _repositoryMock.Verify(rp => rp.FindPlayerIdentityFor(Name, Site));
        }

        [Test]
        public void UpdateFrom_PlayerIdentityIsNotNull_DoesntCallRepositoryFindPlayerIdentityForNameAndSite()
        {
            _sut.PlayerIdentitySet = _playerIdentityStub;
            _sut
                .InitializePlayer(Name, Site)
                .UpdateFrom(_repositoryMock.Object);

            _repositoryMock.Verify(rp => rp.FindPlayerIdentityFor(Name, Site), Times.Never());
        }

        [Test]
        public void UpdateFrom_PlayerIdentityIsNull_AssignsPlayerIdentityReturnedFromRepository()
        {
            var repositoryStub = _repositoryMock;
            repositoryStub
                .Setup(rp => rp.FindPlayerIdentityFor(Name, Site))
                .Returns(_playerIdentityStub);

            _sut
                .InitializePlayer(Name, Site)
                .UpdateFrom(repositoryStub.Object);

            _sut.PlayerIdentity.Id.IsEqualTo(Id);
        }

        [Test]
        public void UpdateFrom_PlayerIdentityIsNullRepositoryReturnsPlayerIdentity_CallsRepositoryFindAnalayzablePlayersForIdAndLastQueriedId()
        {
            _repositoryMock
                .Setup(rp => rp.FindPlayerIdentityFor(Name, Site))
                .Returns(_playerIdentityStub);

            _sut
                .InitializePlayer(Name, Site)
                .UpdateFrom(_repositoryMock.Object);

            _repositoryMock.Verify(rp => rp.FindAnalyzablePlayersWith(_playerIdentityStub.Id, _sut.LastQueriedId));
        }

        [Test]
        public void UpdateFrom_PlayerIdentityIsNotNull_AddsAnalyzablePlayersReturnedFromRepository()
        {
            var analyzablePlayerStub = _stub.Setup<IAnalyzablePokerPlayer>()
                .Get(ap => ap.Id).Returns(1).Out;

            _repositoryMock
                .Setup(rp => rp.FindAnalyzablePlayersWith(_playerIdentityStub.Id, _sut.LastQueriedId))
                .Returns(new List<IAnalyzablePokerPlayer> { analyzablePlayerStub });
            
            _sut.PlayerIdentitySet = _playerIdentityStub;
            
            _sut
                .InitializePlayer(Name, Site)
                .UpdateFrom(_repositoryMock.Object);

            _sut.AnalyzablePlayers.Last().IsEqualTo(analyzablePlayerStub);
        }

        [Test]
        public void UpdateFrom_PlayerIdentityIsNotNull_UpdatesStatistics()
        {
            _sut.PlayerIdentitySet = _playerIdentityStub;

            _sut
                .InitializePlayer(Name, Site)
                .UpdateFrom(_repositoryMock.Object);

            _sut.StatisticsWereUpdated.IsTrue();
        }

        [Test]
        public void UpdateFrom_RepositoryReturnsTwoAnalyzablePlayers_SetsLastQueriedIdToLargestIdOfReturnedPlayers()
        {
            var analyzablePlayerStub1 = _stub.Setup<IAnalyzablePokerPlayer>()
                .Get(ap => ap.Id).Returns(1).Out;
            var analyzablePlayerStub2 = _stub.Setup<IAnalyzablePokerPlayer>()
                .Get(ap => ap.Id).Returns(2).Out;

            _repositoryMock
                .Setup(rp => rp.FindAnalyzablePlayersWith(_playerIdentityStub.Id, _sut.LastQueriedId))
                .Returns(new List<IAnalyzablePokerPlayer> { analyzablePlayerStub1, analyzablePlayerStub2 });

            _sut.PlayerIdentitySet = _playerIdentityStub;

            _sut
                .InitializePlayer(Name, Site)
                .UpdateFrom(_repositoryMock.Object);

            _sut.LastQueriedId.IsEqualTo(analyzablePlayerStub2.Id);
        }

        [Test]
        public void UpdateFrom_RepositoryReturnsEmptyList_LastQueriedIdIsUnchanged()
        {
            const long originalLastQueriedId = 1;
           
            _repositoryMock
                .Setup(rp => rp.FindAnalyzablePlayersWith(_playerIdentityStub.Id, _sut.LastQueriedId))
                .Returns(new List<IAnalyzablePokerPlayer> { });

            _sut.PlayerIdentitySet = _playerIdentityStub;
            _sut.LastQueriedId = originalLastQueriedId;

            _sut
                .InitializePlayer(Name, Site)
                .UpdateFrom(_repositoryMock.Object);

            _sut.LastQueriedId.IsEqualTo(originalLastQueriedId);
        }
    }

    class PlayerStatisticsMock : PlayerStatistics
    {
        public PlayerStatisticsMock(IEventAggregator eventAggregator)
            : base(eventAggregator)
        {
            StatisticsWereUpdated = false;
        }

        public long LastQueriedId
        {
            get { return _lastQueriedId; }
            set { _lastQueriedId = value; }
        }

        public IList<IAnalyzablePokerPlayer> AnalyzablePlayers
        {
            get { return _allAnalyzablePlayers; }
            set { _allAnalyzablePlayers = value; }
        }

        public IPlayerIdentity PlayerIdentitySet
        {
            set { PlayerIdentity = value; }
        }

        protected override IActionSequenceSetStatistics NewActionSequenceSetStatistics(IEnumerable<IActionSequenceStatistic> statistics, IPercentagesCalculator percentagesCalculator)
        {
            return new StubBuilder().Out<IActionSequenceSetStatistics>();
        }

        protected override void UpdateStatisticsWith(IEnumerable<IAnalyzablePokerPlayer> filteredAnalyzablePlayers)
        {
            base.UpdateStatisticsWith(filteredAnalyzablePlayers);
            StatisticsWereUpdated = true;
        }

        internal bool StatisticsWereUpdated { get; private set; }
    }
}