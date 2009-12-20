namespace PokerTell.LiveTracker.Tests
{
    using System;
    using System.Collections.Generic;

    using Moq;

    using NUnit.Framework;

    using PokerTell.Infrastructure.Interfaces;
    using PokerTell.Infrastructure.Interfaces.PokerHand;
    using PokerTell.Infrastructure.Interfaces.Statistics;
    using PokerTell.LiveTracker.ViewModels;
    using PokerTell.UnitTests.Tools;

    public class TableStatisticsViewModelTests
    {
        #region Constants and Fields

        Mock<IConstructor<IPlayerStatisticsViewModel>> _playerStatisticsViewModelMakeStub;

        StubBuilder _stub;

        TableStatisticsViewModel _sut;

        #endregion

        #region Public Methods

        [SetUp]
        public void _Init()
        {
            _stub = new StubBuilder();
            _playerStatisticsViewModelMakeStub = new Mock<IConstructor<IPlayerStatisticsViewModel>>();
            _playerStatisticsViewModelMakeStub
                .SetupGet(make => make.New)
                .Returns(_stub.Out<IPlayerStatisticsViewModel>);

            _sut = new TableStatisticsViewModel(_playerStatisticsViewModelMakeStub.Object);
        }

        [Test]
        public void Constructor_Always_InitializesPlayers()
        {
            _sut.Players.HasCount(0);
        }

        [Test]
        public void UpdateWith_SelectedPlayerNotInStatistics_SelectedPlayerIsSetToFirstPlayer()
        {
            const string player1 = "player1";
            const string player2 = "player2";

            var player1Mock = AddPlayerMock(player1);
            var player2Mock = AddPlayerMock(player2);

            _sut.SelectedPlayer = player2Mock.Object;

            _sut.UpdateWith(new[] { PlayerStatisticsStubFor(player1) });

            _sut.SelectedPlayer.IsEqualTo(player1Mock.Object);
        }

        [Test]
        public void UpdateWith_DifferentPlayerThanIsInPlayers_AddsDifferentPlayer()
        {
            const string player1 = "player1";
            const string differentPlayer = "differentPlayer";

            AddPlayerMock(player1);

            var differentPlayerStatistics = PlayerStatisticsStubFor(differentPlayer);
            var differentPlayerMock = PlayerMockFor(differentPlayer);
            
            _playerStatisticsViewModelMakeStub
                .SetupGet(make => make.New)
                .Returns(differentPlayerMock.Object);

            _sut.UpdateWith(new[] { differentPlayerStatistics });

            _sut.Players.DoesContain(differentPlayerMock.Object);
        }

        [Test]
        public void UpdateWith_DifferentPlayerThanIsInPlayers_UpdatesDifferentPlayer()
        {
            const string player1 = "player1";
            const string differentPlayer = "differentPlayer";

            AddPlayerMock(player1);

            var differentPlayerStatistics = PlayerStatisticsStubFor(differentPlayer);
            var differentPlayerMock = PlayerMockFor(differentPlayer);

            _playerStatisticsViewModelMakeStub
                .SetupGet(make => make.New)
                .Returns(differentPlayerMock.Object);

            _sut.UpdateWith(new[] { differentPlayerStatistics });

           differentPlayerMock.Verify(p => p.UpdateWith(differentPlayerStatistics));
        }

        [Test]
        public void UpdateWith_DifferentPlayerThanIsInPlayers_DoesNotUpdateOriginalPlayer()
        {
            const string player1 = "player1";
            const string differentPlayer = "differentPlayer";

            var player1Mock = AddPlayerMock(player1);

            var differentPlayerStatistics = PlayerStatisticsStubFor(differentPlayer);
            _sut.UpdateWith(new[] { differentPlayerStatistics });

            player1Mock.Verify(p => p.UpdateWith(differentPlayerStatistics), Times.Never());
        }

        [Test]
        public void UpdateWith_DifferentPlayerThanIsInPlayers_RemovesOriginalPlayer()
        {
            const string player1 = "player1";
            const string differentPlayer = "differentPlayer";

            var player1Mock = AddPlayerMock(player1);

            var differentPlayerStatistics = PlayerStatisticsStubFor(differentPlayer);
            _sut.UpdateWith(new[] { differentPlayerStatistics });

            _sut.Players.DoesNotContain(player1Mock.Object);
        }

        [Test]
        public void UpdateWith_EmptyList_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => _sut.UpdateWith(new List<IPlayerStatistics>()));
        }

        [Test]
        public void UpdateWith_SamePlayerAsIsInPlayers_UpdatesThatPlayer()
        {
            const string player1 = "player1";
            var player1Mock = AddPlayerMock(player1);

            var player1Statistics = PlayerStatisticsStubFor(player1);
            _sut.UpdateWith(new[] { player1Statistics });

            player1Mock.Verify(p => p.UpdateWith(player1Statistics));
        }

        #endregion

        #region Methods

        static Mock<IPlayerStatisticsViewModel> PlayerMockFor(string name)
        {
            var playerViewModelMock = new Mock<IPlayerStatisticsViewModel>();
            playerViewModelMock.SetupGet(pvm => pvm.PlayerName).Returns(name);
            return playerViewModelMock;
        }

        Mock<IPlayerStatisticsViewModel> AddPlayerMock(string name)
        {
            var playerViewModelMock = PlayerMockFor(name);
            _sut.Players.Add(playerViewModelMock.Object);
            return playerViewModelMock;
        }

        IPlayerStatistics PlayerStatisticsStubFor(string name)
        {
            var playerIdentityStub = _stub.Setup<IPlayerIdentity>()
                .Get(pi => pi.Name).Returns(name).Out;

            return _stub.Setup<IPlayerStatistics>()
                .Get(ps => ps.PlayerIdentity).Returns(playerIdentityStub).Out;
        }

        #endregion
    }
}