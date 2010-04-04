namespace PokerTell.Statistics.Tests
{
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.Practices.Composite.Events;

    using Moq;

    using NUnit.Framework;

    using PokerTell.Infrastructure.Enumerations.PokerHand;
    using PokerTell.Infrastructure.Events;
    using PokerTell.Infrastructure.Interfaces.DatabaseSetup;
    using PokerTell.Infrastructure.Interfaces.PokerHand;
    using PokerTell.Infrastructure.Interfaces.Repository;
    using PokerTell.Infrastructure.Interfaces.Statistics;
    using PokerTell.Statistics.Tests.Fakes;
    using PokerTell.UnitTests.Tools;

    [TestFixture]
    public class PlayerStatisticsTests
    {
        const int Id = 1;

        const string Name = "somePlayer";

        const string Site = "someSite";

        IEventAggregator _eventAggregator;

        IPlayerIdentity _playerIdentityStub;

        Mock<IRepository> _repositoryMock;

        StubBuilder _stub;

        PlayerStatisticsSut _sut;

        [SetUp]
        public void _Init()
        {
            _stub = new StubBuilder();
            _eventAggregator = new EventAggregator();
            _repositoryMock = new Mock<IRepository>();

            _playerIdentityStub = _stub.Setup<IPlayerIdentity>()
                .Get(pi => pi.Id).Returns(Id)
                .Out;

            _sut = new PlayerStatisticsSut(_eventAggregator, _repositoryMock.Object);
            _sut.InitializePlayer(Name, Site);
        }

        [Test]
        public void DatabaseInUseChangedEventWasRaised_AnalyzablePlayersNotEmpty_ReinitializesAnalyzablePlayers()
        {
            _sut.AnalyzablePlayers.Add(_stub.Out<IAnalyzablePokerPlayer>());

            _eventAggregator.GetEvent<DatabaseInUseChangedEvent>()
                .Publish(_stub.Out<IDataProvider>());

            _sut.AnalyzablePlayers.ShouldBeEmpty();
        }

        [Test]
        public void DatabaseInUseChangedEventWasRaised_LastQueriedIdIsNotZero_SetsLastQueriedIdToZero()
        {
            _sut.LastQueriedId = 1;

            _eventAggregator.GetEvent<DatabaseInUseChangedEvent>()
                .Publish(_stub.Out<IDataProvider>());

            _sut.LastQueriedId.ShouldBeEqualTo(0);
        }

        [Test]
        public void DatabaseInUseChangedEventWasRaised_PlayerIdentityNotNull_ReinitializesItToNull()
        {
            _sut.PlayerIdentitySet = _stub.Out<IPlayerIdentity>();

            _eventAggregator.GetEvent<DatabaseInUseChangedEvent>()
                .Publish(_stub.Out<IDataProvider>());

            _sut.PlayerIdentity.ShouldBeNull();
        }

        [Test]
        public void GetFilteredAnalyzablePlayers_FilterNotSet_ReturnsAllAnalyzablePlayers()
        {
            _sut.AnalyzablePlayers = new List<IAnalyzablePokerPlayer> { _stub.Out<IAnalyzablePokerPlayer>() };

            IEnumerable<IAnalyzablePokerPlayer> filteredPlayers = _sut.GetFilteredAnalyzablePlayersInvoke();

            filteredPlayers.ShouldHaveCount(1);
        }

        [Test]
        public void GetFilteredAnalyzablePlayers_FilterSet_ReturnsPlayersPassedBackByFilterMethodOfFilter()
        {
            IAnalyzablePokerPlayer analyzablePlayerStub1 = _stub.Setup<IAnalyzablePokerPlayer>()
                .Get(ap => ap.Id).Returns(1)
                .Get(ap => ap.ActionSequences).Returns(new[] { ActionSequences.PreFlopFrontRaise })
                .Out;
            IAnalyzablePokerPlayer analyzablePlayerStub2 = _stub.Setup<IAnalyzablePokerPlayer>()
                .Get(ap => ap.Id).Returns(2)
                .Get(ap => ap.ActionSequences).Returns(new[] { ActionSequences.PreFlopFrontRaise })
                .Out;

            _sut.AnalyzablePlayers = new List<IAnalyzablePokerPlayer> { analyzablePlayerStub1, analyzablePlayerStub2 };

            var filterStub = new Mock<IAnalyzablePokerPlayersFilter>();
            filterStub
                .Setup(f => f.Filter(_sut.AnalyzablePlayers))
                .Returns(new List<IAnalyzablePokerPlayer> { analyzablePlayerStub1 });

            _sut.Filter = filterStub.Object;

            IEnumerable<IAnalyzablePokerPlayer> filteredPlayers = _sut.GetFilteredAnalyzablePlayersInvoke();

            filteredPlayers.ShouldContain(analyzablePlayerStub1);
            filteredPlayers.ShouldNotContain(analyzablePlayerStub2);
        }

        [Test]
        public void UpdateStatistics_FilterSetAndPlayerIdentityNotNull_FiltersPlayers()
        {
            IAnalyzablePokerPlayer analyzablePlayerStub = _stub.Setup<IAnalyzablePokerPlayer>()
                .Get(ap => ap.Id).Returns(1)
                .Get(ap => ap.ActionSequences).Returns(new[] { ActionSequences.PreFlopFrontRaise })
                .Out;

            _sut.AnalyzablePlayers = new List<IAnalyzablePokerPlayer> { analyzablePlayerStub };

            _sut.Filter = _stub.Out<IAnalyzablePokerPlayersFilter>();
            _sut.PlayerIdentitySet = _playerIdentityStub;

            _sut.UpdateStatistics();

            _sut.MatchingPlayersWereFiltered.ShouldBeTrue();
        }

        [Test]
        public void UpdateStatistics_PlayerIdentityIsNotNull_AddsAnalyzablePlayersReturnedFromRepository()
        {
            IAnalyzablePokerPlayer analyzablePlayerStub = _stub.Setup<IAnalyzablePokerPlayer>()
                .Get(ap => ap.Id).Returns(1)
                .Get(ap => ap.ActionSequences).Returns(new[] { ActionSequences.PreFlopFrontRaise })
                .Out;

            _repositoryMock
                .Setup(rp => rp.FindAnalyzablePlayersWith(_playerIdentityStub.Id, _sut.LastQueriedId))
                .Returns(new List<IAnalyzablePokerPlayer> { analyzablePlayerStub });

            _sut.PlayerIdentitySet = _playerIdentityStub;

            _sut
                .InitializePlayer(Name, Site)
                .UpdateStatistics();

            _sut.AnalyzablePlayers.Last().ShouldBeEqualTo(analyzablePlayerStub);
        }

        [Test]
        public void UpdateStatistics_PlayerIdentityIsNotNull_DoesntCallRepositoryFindPlayerIdentityForNameAndSite()
        {
            _sut.PlayerIdentitySet = _playerIdentityStub;
            _sut
                .InitializePlayer(Name, Site)
                .UpdateStatistics();

            _repositoryMock.Verify(rp => rp.FindPlayerIdentityFor(Name, Site), Times.Never());
        }

        [Test]
        public void UpdateStatistics_PlayerIdentityIsNotNull_UpdatesStatistics()
        {
            _sut.PlayerIdentitySet = _playerIdentityStub;

            _sut
                .InitializePlayer(Name, Site)
                .UpdateStatistics();

            _sut.StatisticsGotUpdated.ShouldBeTrue();
        }

        [Test]
        public void UpdateStatistics_PlayerIdentityIsNull_AssignsPlayerIdentityReturnedFromRepository()
        {
            Mock<IRepository> repositoryStub = _repositoryMock;
            repositoryStub
                .Setup(rp => rp.FindPlayerIdentityFor(Name, Site))
                .Returns(_playerIdentityStub);

            _sut
                .InitializePlayer(Name, Site)
                .UpdateStatistics();

            _sut.PlayerIdentity.Id.ShouldBeEqualTo(Id);
        }

        [Test]
        public void UpdateStatistics_PlayerIdentityIsNull_CallsRepositoryFindPlayerIdentityForNameAndSite()
        {
            _sut
                .InitializePlayer(Name, Site)
                .UpdateStatistics();

            _repositoryMock.Verify(rp => rp.FindPlayerIdentityFor(Name, Site));
        }

        [Test]
        public void
            UpdateStatistics_PlayerIdentityIsNullRepositoryReturnsPlayerIdentity_CallsRepositoryFindAnalayzablePlayersForIdAndLastQueriedId
            ()
        {
            _repositoryMock
                .Setup(rp => rp.FindPlayerIdentityFor(Name, Site))
                .Returns(_playerIdentityStub);

            _sut
                .InitializePlayer(Name, Site)
                .UpdateStatistics();

            _repositoryMock.Verify(rp => rp.FindAnalyzablePlayersWith(_playerIdentityStub.Id, _sut.LastQueriedId));
        }

        [Test]
        public void UpdateStatistics_RepositoryReturnsEmptyList_LastQueriedIdIsUnchanged()
        {
            const long originalLastQueriedId = 1;

            _repositoryMock
                .Setup(rp => rp.FindAnalyzablePlayersWith(_playerIdentityStub.Id, _sut.LastQueriedId))
                .Returns(new List<IAnalyzablePokerPlayer>());

            _sut.PlayerIdentitySet = _playerIdentityStub;
            _sut.LastQueriedId = originalLastQueriedId;

            _sut
                .InitializePlayer(Name, Site)
                .UpdateStatistics();

            _sut.LastQueriedId.ShouldBeEqualTo(originalLastQueriedId);
        }

        [Test]
        public void UpdateStatistics_RepositoryReturnsTwoAnalyzablePlayers_SetsLastQueriedIdToMaxIdOfReturnedPlayers()
        {
            IAnalyzablePokerPlayer analyzablePlayerStub1 = _stub.Setup<IAnalyzablePokerPlayer>()
                .Get(ap => ap.Id).Returns(1)
                .Get(ap => ap.ActionSequences).Returns(new[] { ActionSequences.PreFlopFrontRaise })
                .Out;
            IAnalyzablePokerPlayer analyzablePlayerStub2 = _stub.Setup<IAnalyzablePokerPlayer>()
                .Get(ap => ap.Id).Returns(2)
                .Get(ap => ap.ActionSequences).Returns(new[] { ActionSequences.PreFlopFrontRaise })
                .Out;

            _repositoryMock
                .Setup(rp => rp.FindAnalyzablePlayersWith(_playerIdentityStub.Id, _sut.LastQueriedId))
                .Returns(new List<IAnalyzablePokerPlayer> { analyzablePlayerStub1, analyzablePlayerStub2 });

            _sut.PlayerIdentitySet = _playerIdentityStub;

            _sut
                .InitializePlayer(Name, Site)
                .UpdateStatistics();

            _sut.LastQueriedId.ShouldBeEqualTo(analyzablePlayerStub2.Id);
        }
    }
}