namespace PokerTell.Statistics.Tests.Filters
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using Fakes;

    using Infrastructure.Enumerations.PokerHand;
    using Infrastructure.Interfaces.PokerHand;
    using Infrastructure.Interfaces.Statistics;

    using Moq;

    using NUnit.Framework;

    using Statistics.Filters;

    using Tools.Extensions;
    using Tools.GenericRanges;

    using UnitTests.Tools;

    [TestFixture]
    public class AnalyzablePokerPlayersFilterTests
    {
        AnalyzablePokerPlayersFilterTester _sut;

        StubBuilder _stub;

        [SetUp]
        public void _Init()
        {
            _stub = new StubBuilder();

            _sut = new AnalyzablePokerPlayersFilterTester();
        }

        [Test]
        public void Filter_EmptyList_ReturnsEmptyList()
        {
            _sut.Filter(new List<IAnalyzablePokerPlayer>()).IsEmpty();
        }

        [Test]
        public void Filter_NoFilterActivated_ReturnsAllPassedPlayers()
        {
            var player1 = _stub.Out<IAnalyzablePokerPlayer>();
            var players = new List<IAnalyzablePokerPlayer> { player1 };

            var filteredPlayers = _sut.Filter(players);

            filteredPlayers.HasCount(players.Count);
        }

        [Test]
        public void Filter_OnlyMFilterActivated_ReturnsPlayersPassingThroughIt()
        {
            const int max = 1;
            var inRangePlayer = _stub.Setup<IAnalyzablePokerPlayer>()
                .Get(ap => ap.MBefore).Returns(max)
                .Out;
            var outOfRangePlayer = _stub.Setup<IAnalyzablePokerPlayer>()
                .Get(ap => ap.MBefore).Returns(max + 1)
                .Out;
            var players = new List<IAnalyzablePokerPlayer> { inRangePlayer, outOfRangePlayer };

            _sut.MFilter.ActivateWith(0, max);

            var filteredPlayers = _sut.Filter(players);

            filteredPlayers.DoesContain(inRangePlayer);
            filteredPlayers.DoesNotContain(outOfRangePlayer);
        }

        [Test]
        public void Filter_OnlyBigBlindFilterActivated_ReturnsPlayersPassingThroughIt()
        {
            const double max = 1.0;
            var inRangePlayer = _stub.Setup<IAnalyzablePokerPlayer>()
                .Get(ap => ap.BB).Returns(max)
                .Out;
            var outOfRangePlayer = _stub.Setup<IAnalyzablePokerPlayer>()
                .Get(ap => ap.BB).Returns(max + 1)
                .Out;
            var players = new List<IAnalyzablePokerPlayer> { inRangePlayer, outOfRangePlayer };

            _sut.BigBlindFilter.ActivateWith(0, max);

            var filteredPlayers = _sut.Filter(players);

            filteredPlayers.DoesContain(inRangePlayer);
            filteredPlayers.DoesNotContain(outOfRangePlayer);
        }

        [Test]
        public void Filter_OnlyAnteFilterActivated_ReturnsPlayersPassingThroughIt()
        {
            const double max = 1.0;
            var inRangePlayer = _stub.Setup<IAnalyzablePokerPlayer>()
                .Get(ap => ap.Ante).Returns(max)
                .Out;
            var outOfRangePlayer = _stub.Setup<IAnalyzablePokerPlayer>()
                .Get(ap => ap.Ante).Returns(max + 1)
                .Out;
            var players = new List<IAnalyzablePokerPlayer> { inRangePlayer, outOfRangePlayer };

            _sut.AnteFilter.ActivateWith(0, max);

            var filteredPlayers = _sut.Filter(players);

            filteredPlayers.DoesContain(inRangePlayer);
            filteredPlayers.DoesNotContain(outOfRangePlayer);
        }

        [Test]
        public void Filter_OnlyTotalPlayersFilterActivated_ReturnsPlayersPassingThroughIt()
        {
            const int max = 2;
            var inRangePlayer = _stub.Setup<IAnalyzablePokerPlayer>()
                .Get(ap => ap.TotalPlayers).Returns(max)
                .Out;
            var outOfRangePlayer = _stub.Setup<IAnalyzablePokerPlayer>()
                .Get(ap => ap.TotalPlayers).Returns(max + 1)
                .Out;
            var players = new List<IAnalyzablePokerPlayer> { inRangePlayer, outOfRangePlayer };

            _sut.TotalPlayersFilter.ActivateWith(0, max);

            var filteredPlayers = _sut.Filter(players);

            filteredPlayers.DoesContain(inRangePlayer);
            filteredPlayers.DoesNotContain(outOfRangePlayer);
        }

        [Test]
        public void Filter_OnlyPlayersInFlopFilterActivated_ReturnsPlayersPassingThroughIt()
        {
            const int max = 2;
            var inRangePlayer = _stub.Setup<IAnalyzablePokerPlayer>()
                .Get(ap => ap.PlayersInFlop).Returns(max)
                .Out;
            var outOfRangePlayer = _stub.Setup<IAnalyzablePokerPlayer>()
                .Get(ap => ap.PlayersInFlop).Returns(max + 1)
                .Out;
            var players = new List<IAnalyzablePokerPlayer> { inRangePlayer, outOfRangePlayer };

            _sut.PlayersInFlopFilter.ActivateWith(0, max);

            var filteredPlayers = _sut.Filter(players);

            filteredPlayers.DoesContain(inRangePlayer);
            filteredPlayers.DoesNotContain(outOfRangePlayer);
        }

        [Test]
        public void Filter_OnlyStrategicPostionFilterActivated_ReturnsPlayersPassingThroughIt()
        {
            const StrategicPositions max = StrategicPositions.MI;
            var inRangePlayer = _stub.Setup<IAnalyzablePokerPlayer>()
                .Get(ap => ap.StrategicPosition).Returns(max)
                .Out;
            var outOfRangePlayer = _stub.Setup<IAnalyzablePokerPlayer>()
                .Get(ap => ap.StrategicPosition).Returns(max + 1)
                .Out;
            var players = new List<IAnalyzablePokerPlayer> { inRangePlayer, outOfRangePlayer };

            _sut.StrategicPositionFilter.ActivateWith(0, max);

            var filteredPlayers = _sut.Filter(players);

            filteredPlayers.DoesContain(inRangePlayer);
            filteredPlayers.DoesNotContain(outOfRangePlayer);
        }

        [Test]
        public void Filter_OnlyTimeRangeFilterActivatedOutOfRangePlayerActedBeforeTimeRange_ReturnsInRangePlayer()
        {
            const int fromMinutes = -20;
            const int toMinutes = 0;
           
            var currentTime = DateTime.MinValue.AddMinutes(60);

            var inRangePlayer = _stub.Setup<IAnalyzablePokerPlayer>()
                .Get(ap => ap.TimeStamp).Returns(currentTime)
                .Out;
            var outOfRangePlayer = _stub.Setup<IAnalyzablePokerPlayer>()
                .Get(ap => ap.TimeStamp).Returns(currentTime.AddMinutes(fromMinutes - 1))
                .Out;
            var players = new List<IAnalyzablePokerPlayer> { inRangePlayer, outOfRangePlayer };

            _sut.CurrentTimeUsed = currentTime;
           _sut.TimeRangeFilter.ActivateWith(fromMinutes, toMinutes);

            var filteredPlayers = _sut.Filter(players);

            filteredPlayers.DoesContain(inRangePlayer);
            filteredPlayers.DoesNotContain(outOfRangePlayer);
        }

        [Test]
        public void Filter_OnlyTimeRangeFilterActivatedOutOfRangePlayerActedAfterTimeRange_ReturnsInRangePlayer()
        {
            const int fromMinutes = -20;
            const int toMinutes = 0;

            var currentTime = DateTime.MinValue.AddMinutes(60);

            var inRangePlayer = _stub.Setup<IAnalyzablePokerPlayer>()
                .Get(ap => ap.TimeStamp).Returns(currentTime)
                .Out;
            var outOfRangePlayer = _stub.Setup<IAnalyzablePokerPlayer>()
                .Get(ap => ap.TimeStamp).Returns(currentTime.AddMinutes(toMinutes + 1))
                .Out;
            var players = new List<IAnalyzablePokerPlayer> { inRangePlayer, outOfRangePlayer };

            _sut.CurrentTimeUsed = currentTime;
            _sut.TimeRangeFilter.ActivateWith(fromMinutes, toMinutes);

            var filteredPlayers = _sut.Filter(players);

            filteredPlayers.DoesContain(inRangePlayer);
            filteredPlayers.DoesNotContain(outOfRangePlayer);
        }

        [Test]
        public void Filter_BigBlindFilterAndMFilterActivated_ReturnsPlayersPassingThroughBothFilters()
        {
            const double maxBB = 1.0;
            const int maxM = 1;
            var passesMFilter = _stub.Setup<IAnalyzablePokerPlayer>()
                .Get(ap => ap.MBefore).Returns(maxM)
                .Get(ap => ap.BB).Returns(maxBB + 1)
                .Out;
            var passesBigBlindFilter = _stub.Setup<IAnalyzablePokerPlayer>()
                .Get(ap => ap.MBefore).Returns(maxM + 1)
                .Get(ap => ap.BB).Returns(maxBB)
                .Out;
            var passesBothFilters = _stub.Setup<IAnalyzablePokerPlayer>()
                .Get(ap => ap.MBefore).Returns(maxM)
                .Get(ap => ap.BB).Returns(maxBB)
                .Out;

            var players = new List<IAnalyzablePokerPlayer> { passesMFilter, passesBigBlindFilter, passesBothFilters };

            _sut.MFilter.Range = new GenericRange<int>(0, maxM);
            _sut.MFilter.ActivateWith(0, maxM);
            _sut.BigBlindFilter.Range = new GenericRange<double>(0, maxBB);
            _sut.BigBlindFilter.ActivateWith(0, maxBB);

            var filteredPlayers = _sut.Filter(players);

            filteredPlayers.DoesNotContain(passesMFilter);
            filteredPlayers.DoesNotContain(passesBigBlindFilter);
            filteredPlayers.DoesContain(passesBothFilters);
        }

        [Test]
        public void Equals_TwoIdenticalFilters_ReturnsTrue()
        {
            var filter1 = AnalyzablePokerPlayersFilter.InactiveFilter;
            var filter2 = AnalyzablePokerPlayersFilter.InactiveFilter;

            filter1.IsEqualTo(filter2);
        }

        [Test]
        public void Equals_TwoFiltersWithDifferentMFilter_ReturnsFalse()
        {
            var filter1 = AnalyzablePokerPlayersFilter.InactiveFilter;
            filter1.MFilter.IsActive = true;
            var filter2 = AnalyzablePokerPlayersFilter.InactiveFilter;

            filter1.IsNotEqualTo(filter2);
        }
    }
}