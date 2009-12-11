namespace PokerTell.Statistics.Tests.Detailed
{
    using System;
    using System.Linq.Expressions;

    using Infrastructure.Interfaces.PokerHand;

    using Interfaces;

    using System.Collections.Generic;

    using Moq;

    using NUnit.Framework;

    using Statistics.Detailed;

    public class ActionSequenceSetStatisticsTests
    {
        Mock<IActionSequenceStatistic> _statisticMock;
        Mock<IAnalyzablePokerPlayer> _playerMock;

        Mock<IPercentagesCalculator> _calculatorMock;

        StubBuilder _stub;

        [SetUp]
        public void _Init()
        {
            _stub = new StubBuilder();
            _statisticMock = new Mock<IActionSequenceStatistic>();
            _playerMock = new Mock<IAnalyzablePokerPlayer>();
            _calculatorMock = new Mock<IPercentagesCalculator>();
        }

        [Test]
        public void UpdateWith_ListOfPlayers_UpdatesAllStatisticsWithThosePlayers()
        {
            var sut = new ActionSequenceSetStatistics(new[] { _statisticMock.Object }, _calculatorMock.Object);

            var analyzablePokerPlayers = new[] { _playerMock.Object };
            sut.UpdateWith(analyzablePokerPlayers);

            _statisticMock.Verify(st => st.UpdateWith(analyzablePokerPlayers));
        }

        [Test]
        public void UpdateWith_ListOfPlayers_CallsPercentageCalculator()
        {
            var sut = new ActionSequenceSetStatistics(new[] { _statisticMock.Object }, _calculatorMock.Object);

            var analyzablePokerPlayers = new[] { _playerMock.Object };
            sut.UpdateWith(analyzablePokerPlayers);

            _calculatorMock.Verify(pc => pc.CalculatePercentages(
                It.IsAny<Func<int>>(), 
                It.IsAny<Func<int, int>>(), 
                It.IsAny<Func<int, int, int>>(), 
                It.IsAny<Action<int, int, int>>()));
        }

        [Test]
        public void UpdateWith_ListOfPlayers_CallToPercentageCalculatorPassesStatisticsCountAsGetRowCountFunction()
        {
            var statistics = new List<IActionSequenceStatistic> { _statisticMock.Object, _statisticMock.Object };
            var sut = new ActionSequenceSetStatistics(statistics, _calculatorMock.Object);
            
            Expression<Predicate<Func<int>>> getRowCountsExpression = func => func() == statistics.Count;

            var analyzablePokerPlayers = new[] { _playerMock.Object };
            sut.UpdateWith(analyzablePokerPlayers);

            _calculatorMock.Verify(pc => pc.CalculatePercentages(
                It.Is(getRowCountsExpression),
                It.IsAny<Func<int, int>>(),
                It.IsAny<Func<int, int, int>>(),
                It.IsAny<Action<int, int, int>>()));
        }

        [Test]
        public void
            UpdateWith_ListOfPlayers_CallToPercentageCalculatorPassesFirstStatisticsColumnCountAsGetColumnCountFunction(
            )
        {
            const int columnCount = 2;
            _statisticMock.SetupGet(s => s.MatchingPlayers)
                .Returns(new IList<IAnalyzablePokerPlayer>[columnCount]);
            var statistics = new List<IActionSequenceStatistic> { _statisticMock.Object };

            var sut = new ActionSequenceSetStatistics(statistics, _calculatorMock.Object);

            Expression<Predicate<Func<int, int>>> getColumnCountsExpression =
                func => func(0) == columnCount;

            var analyzablePokerPlayers = new[] { _playerMock.Object };
            sut.UpdateWith(analyzablePokerPlayers);

            _calculatorMock.Verify(pc => pc.CalculatePercentages(
                                             It.IsAny<Func<int>>(),
                                             It.Is(getColumnCountsExpression),
                                             It.IsAny<Func<int, int, int>>(),
                                             It.IsAny<Action<int, int, int>>()));
        }
    }
}