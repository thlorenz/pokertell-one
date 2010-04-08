namespace PokerTell.LiveTracker.Tests.ViewModels
{
    using System;
    using System.Collections.Generic;

    using Microsoft.Practices.Composite.Events;

    using Moq;

    using NUnit.Framework;

    using PokerTell.Infrastructure.Events;
    using PokerTell.Infrastructure.Interfaces;
    using PokerTell.Infrastructure.Interfaces.Statistics;
    using PokerTell.LiveTracker.Tests.Utilities;
    using PokerTell.LiveTracker.ViewModels;
    using PokerTell.UnitTests.Tools;

    using Tools.WPF.Interfaces;

    public class PokerTableStatisticsViewModelTests
    {
        Mock<IConstructor<IPlayerStatisticsViewModel>> _playerStatisticsViewModelMakeStub;

        StubBuilder _stub;

        PokerTableStatisticsViewModelTester _sut;

        [SetUp]
        public void _Init()
        {
            _stub = new StubBuilder();
            _playerStatisticsViewModelMakeStub = new Mock<IConstructor<IPlayerStatisticsViewModel>>();
            _playerStatisticsViewModelMakeStub
                .SetupGet(make => make.New)
                .Returns(_stub.Out<IPlayerStatisticsViewModel>);

            _sut = new PokerTableStatisticsViewModelTester(_playerStatisticsViewModelMakeStub.Object);
        }

        [Test]
        public void ApplyFilterTo_Player1TwoPlayersAtTable_AppliesFilterToPlayer1()
        {
            const string player1 = "player1";
            const string player2 = "player2";

            var player1Mock = AddPlayerMock(player1);
            AddPlayerMock(player2);

            var filterStub = _stub.Out<IAnalyzablePokerPlayersFilter>();

            _sut.ApplyFilterToInvoke(player1, filterStub);

            player1Mock.VerifySet(p => p.Filter = filterStub);
        }

        [Test]
        public void ApplyFilterTo_Player1TwoPlayersAtTable_DoesNotApplyFilterToPlayer2()
        {
            const string player1 = "player1";
            const string player2 = "player2";

            AddPlayerMock(player1);
            var player2Mock = AddPlayerMock(player2);

            var filterStub = _stub.Out<IAnalyzablePokerPlayersFilter>();

            _sut.ApplyFilterToInvoke(player1, filterStub);

            player2Mock.VerifySet(p => p.Filter = filterStub, Times.Never());
        }

        [Test]
        public void ApplyFilterToAll_TwoPlayersAtTable_AppliesFilterToBothPlayers()
        {
            const string player1 = "player1";
            const string player2 = "player2";

            var player1Mock = AddPlayerMock(player1);
            var player2Mock = AddPlayerMock(player2);

            var filterStub = _stub.Out<IAnalyzablePokerPlayersFilter>();

            _sut.ApplyFilterToAllInvoke(filterStub);

            player1Mock.VerifySet(p => p.Filter = filterStub);
            player2Mock.VerifySet(p => p.Filter = filterStub);
        }

        [Test]
        public void Constructor_Always_InitializesPlayers()
        {
            _sut.Players.ShouldHaveCount(0);
        }


        [Test]
        public void UpdateWith_DifferentPlayerThanIsInPlayers_AddsDifferentPlayer()
        {
            const string player1 = "player1";
            const string differentPlayer = "differentPlayer";

            AddPlayerMock(player1);

            var differentPlayerStatistics = Utils.PlayerStatisticsStubFor(differentPlayer);
            var differentPlayerMock = Utils.PlayerStatisticsVM_MockFor(differentPlayer);

            _playerStatisticsViewModelMakeStub
                .SetupGet(make => make.New)
                .Returns(differentPlayerMock.Object);

            _sut.UpdateWith(new[] { differentPlayerStatistics });

            _sut.Players.ShouldContain(differentPlayerMock.Object);
        }

        [Test]
        public void UpdateWith_DifferentPlayerThanIsInPlayers_DoesNotUpdateOriginalPlayer()
        {
            const string player1 = "player1";
            const string differentPlayer = "differentPlayer";

            var player1Mock = AddPlayerMock(player1);

            var differentPlayerStatistics = Utils.PlayerStatisticsStubFor(differentPlayer);
            _sut.UpdateWith(new[] { differentPlayerStatistics });

            player1Mock.Verify(p => p.UpdateWith(differentPlayerStatistics), Times.Never());
        }

        [Test]
        public void UpdateWith_DifferentPlayerThanIsInPlayers_RemovesOriginalPlayer()
        {
            const string player1 = "player1";
            const string differentPlayer = "differentPlayer";

            var player1Mock = AddPlayerMock(player1);

            var differentPlayerStatistics = Utils.PlayerStatisticsStubFor(differentPlayer);
            _sut.UpdateWith(new[] { differentPlayerStatistics });

            _sut.Players.ShouldNotContain(player1Mock.Object);
        }

        [Test]
        public void UpdateWith_DifferentPlayerThanIsInPlayers_UpdatesDifferentPlayer()
        {
            const string player1 = "player1";
            const string differentPlayer = "differentPlayer";

            AddPlayerMock(player1);

            var differentPlayerStatistics = Utils.PlayerStatisticsStubFor(differentPlayer);
            var differentPlayerMock = Utils.PlayerStatisticsVM_MockFor(differentPlayer);

            _playerStatisticsViewModelMakeStub
                .SetupGet(make => make.New)
                .Returns(differentPlayerMock.Object);

            _sut.UpdateWith(new[] { differentPlayerStatistics });

            differentPlayerMock.Verify(p => p.UpdateWith(differentPlayerStatistics));
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

            var player1Statistics = Utils.PlayerStatisticsStubFor(player1);
            _sut.UpdateWith(new[] { player1Statistics });

            player1Mock.Verify(p => p.UpdateWith(player1Statistics));
        }

        [Test]
        public void UpdateWith_SelectedPlayerNotInStatistics_SelectedPlayerIsSetToFirstPlayer()
        {
            const string player1 = "player1";
            const string player2 = "player2";

            var player1Mock = AddPlayerMock(player1);
            var player2Mock = AddPlayerMock(player2);

            _sut.SelectedPlayer = player2Mock.Object;

            _sut.UpdateWith(new[] { Utils.PlayerStatisticsStubFor(player1) });

            _sut.SelectedPlayer.ShouldBeEqualTo(player1Mock.Object);
        }

        Mock<IPlayerStatisticsViewModel> AddPlayerMock(string name)
        {
            var playerViewModelMock = Utils.PlayerStatisticsVM_MockFor(name);
            _sut.Players.Add(playerViewModelMock.Object);
            return playerViewModelMock;
        }

        class PokerTableStatisticsViewModelTester : PokerTableStatisticsViewModel
        {
            public PokerTableStatisticsViewModelTester(IConstructor<IPlayerStatisticsViewModel> playerStatisticsViewModelMake)
                : base(
                    new Mock<ISettings>().Object, 
                    new Mock<IDimensionsViewModel>().Object, 
                    playerStatisticsViewModelMake, 
                    new StubBuilder().Out<IDetailedStatisticsAnalyzerViewModel>(), 
                    new StubBuilder().Out<IActiveAnalyzablePlayersSelector>(),
                    new StubBuilder().Out<IFilterPopupViewModel>())
            {
            }

            public void ApplyFilterToAllInvoke(IAnalyzablePokerPlayersFilter filter)
            {
                ApplyFilterToAll(filter);
            }

            public void ApplyFilterToInvoke(string playerName, IAnalyzablePokerPlayersFilter filter)
            {
                ApplyFilterTo(playerName, filter);
            }
        }
    }
}