namespace PokerTell.Statistics.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Fakes;

    using Infrastructure.Events;
    using Infrastructure.Interfaces.DatabaseSetup;
    using Infrastructure.Interfaces.PokerHand;
    using Infrastructure.Interfaces.Repository;
    using Infrastructure.Interfaces.Statistics;

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
            
            _sut = new PlayerStatisticsMock(_eventAggregator, _repositoryMock.Object);
        }

        [Test]
        public void DatabaseInUseChangedEventWasRaised_LastQueriedIdIsNotZero_SetsLastQueriedIdToZero()
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
        public void DatabaseInUseChangedEventWasRaised_PlayerIdentityNotNull_ReinitializesItToNull()
        {
            _sut.PlayerIdentitySet = _stub.Out<IPlayerIdentity>();

            _eventAggregator.GetEvent<DatabaseInUseChangedEvent>()
                .Publish(_stub.Out<IDataProvider>());

            _sut.PlayerIdentity.IsNull();
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
        public void UpdateStatistics_PlayerIdentityIsNotNull_DoesntCallRepositoryFindPlayerIdentityForNameAndSite()
        {
            _sut.PlayerIdentitySet = _playerIdentityStub;
            _sut
                .InitializePlayer(Name, Site)
                .UpdateStatistics();

            _repositoryMock.Verify(rp => rp.FindPlayerIdentityFor(Name, Site), Times.Never());
        }

        [Test]
        public void UpdateStatistics_PlayerIdentityIsNull_AssignsPlayerIdentityReturnedFromRepository()
        {
            var repositoryStub = _repositoryMock;
            repositoryStub
                .Setup(rp => rp.FindPlayerIdentityFor(Name, Site))
                .Returns(_playerIdentityStub);

            _sut
                .InitializePlayer(Name, Site)
                .UpdateStatistics();

            _sut.PlayerIdentity.Id.IsEqualTo(Id);
        }

        [Test]
        public void UpdateStatistics_PlayerIdentityIsNullRepositoryReturnsPlayerIdentity_CallsRepositoryFindAnalayzablePlayersForIdAndLastQueriedId()
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
        public void UpdateStatistics_PlayerIdentityIsNotNull_AddsAnalyzablePlayersReturnedFromRepository()
        {
            var analyzablePlayerStub = _stub.Setup<IAnalyzablePokerPlayer>()
                .Get(ap => ap.Id).Returns(1).Out;

            _repositoryMock
                .Setup(rp => rp.FindAnalyzablePlayersWith(_playerIdentityStub.Id, _sut.LastQueriedId))
                .Returns(new List<IAnalyzablePokerPlayer> { analyzablePlayerStub });
            
            _sut.PlayerIdentitySet = _playerIdentityStub;
            
            _sut
                .InitializePlayer(Name, Site)
                .UpdateStatistics();

            _sut.AnalyzablePlayers.Last().IsEqualTo(analyzablePlayerStub);
        }

        [Test]
        public void UpdateStatistics_PlayerIdentityIsNotNull_UpdatesStatistics()
        {
            _sut.PlayerIdentitySet = _playerIdentityStub;

            _sut
                .InitializePlayer(Name, Site)
                .UpdateStatistics();

            _sut.StatisticsWereUpdated.IsTrue();
        }

        [Test]
        public void UpdateStatistics_RepositoryReturnsTwoAnalyzablePlayers_SetsLastQueriedIdToMaxIdOfReturnedPlayers()
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
                .UpdateStatistics();

            _sut.LastQueriedId.IsEqualTo(analyzablePlayerStub2.Id);
        }

        [Test]
        public void UpdateStatistics_RepositoryReturnsEmptyList_LastQueriedIdIsUnchanged()
        {
            const long originalLastQueriedId = 1;
           
            _repositoryMock
                .Setup(rp => rp.FindAnalyzablePlayersWith(_playerIdentityStub.Id, _sut.LastQueriedId))
                .Returns(new List<IAnalyzablePokerPlayer> { });

            _sut.PlayerIdentitySet = _playerIdentityStub;
            _sut.LastQueriedId = originalLastQueriedId;

            _sut
                .InitializePlayer(Name, Site)
                .UpdateStatistics();

            _sut.LastQueriedId.IsEqualTo(originalLastQueriedId);
        }

        [Test]
        public void GetFilteredAnalyzablePlayers_FilterNotSet_ReturnsAllAnalyzablePlayers()
        {
            _sut.AnalyzablePlayers = new List<IAnalyzablePokerPlayer> { _stub.Out<IAnalyzablePokerPlayer>() };

            var filteredPlayers = _sut.GetFilteredAnalyzablePlayersInvoke();

            filteredPlayers.HasCount(1);
        }

        [Test]
        public void GetFilteredAnalyzablePlayers_FilterSet_ReturnsPlayersPassedBackByFilterMethodOfFilter()
        {
            var analyzablePlayerStub1 = _stub.Setup<IAnalyzablePokerPlayer>()
                .Get(ap => ap.Id).Returns(1).Out;
            var analyzablePlayerStub2 = _stub.Setup<IAnalyzablePokerPlayer>()
                .Get(ap => ap.Id).Returns(2).Out;

            _sut.AnalyzablePlayers = new List<IAnalyzablePokerPlayer> { analyzablePlayerStub1, analyzablePlayerStub2 };

            var filterStub = new Mock<IAnalyzablePokerPlayersFilter>();
            filterStub
                .Setup(f => f.Filter(_sut.AnalyzablePlayers))
                .Returns(new List<IAnalyzablePokerPlayer> { analyzablePlayerStub1 });

            _sut.SetFilter(filterStub.Object);

            var filteredPlayers = _sut.GetFilteredAnalyzablePlayersInvoke();

            filteredPlayers.DoesContain(analyzablePlayerStub1);
            filteredPlayers.DoesNotContain(analyzablePlayerStub2);
        }

        [Test]
        public void UpdateStatistics_FilterSetAndPlayerIdentityNotNull_FiltersPlayers()
        {
            _sut.AnalyzablePlayers = new List<IAnalyzablePokerPlayer> { _stub.Out<IAnalyzablePokerPlayer>() };

            _sut.SetFilter(_stub.Out<IAnalyzablePokerPlayersFilter>());
            _sut.PlayerIdentitySet = _playerIdentityStub;

            _sut.UpdateStatistics();

            _sut.MatchingPlayersWereFiltered.IsTrue();
        }


    }
}