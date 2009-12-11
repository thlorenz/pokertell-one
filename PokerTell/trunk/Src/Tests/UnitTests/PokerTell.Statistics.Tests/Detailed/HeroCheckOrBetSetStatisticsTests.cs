namespace PokerTell.Statistics.Tests.Detailed
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using Infrastructure.Enumerations.PokerHand;
    using Infrastructure.Interfaces.PokerHand;

    using Interfaces;

    using Moq;

    using NUnit.Framework;

    using Statistics.Detailed;

    public class HeroCheckOrBetSetStatisticsTests
    {

        Mock<IActionSequenceStatistic> _heroXStatisticStub;
        Mock<IAnalyzablePokerPlayer> _playerStub;

        Mock<IPercentagesCalculator> _calculatorMock;

        StubBuilder _stub;

        Mock<IActionSequenceStatistic> _heroBStatisticStub;

        List<IActionSequenceStatistic> _statisticsStub;

        [SetUp]
        public void _Init()
        {
            _stub = new StubBuilder();
           
            _heroXStatisticStub = new Mock<IActionSequenceStatistic>();
            _heroXStatisticStub
                .SetupGet(s => s.ActionSequence)
                .Returns(ActionSequences.HeroX);
           
            _heroBStatisticStub = new Mock<IActionSequenceStatistic>();
            _heroBStatisticStub
                .SetupGet(s => s.ActionSequence)
                .Returns(ActionSequences.HeroB);

            _statisticsStub = new List<IActionSequenceStatistic> { _heroXStatisticStub.Object, _heroBStatisticStub.Object };

            _playerStub = new Mock<IAnalyzablePokerPlayer>();

            _calculatorMock = new Mock<IPercentagesCalculator>();
        }
        
        [Test]
        public void UpdateWith_ListOfPlayers_CallToPercentageCalculatorPassesOneAsGetRowCountFunction()
        {
            var sut = new HeroCheckOrBetSetStatistics(_statisticsStub, _calculatorMock.Object);

            Expression<Predicate<Func<int>>> getRowCountsExpression = func => func() == 1;

            sut.UpdateWith(new[] { _playerStub.Object });

            _calculatorMock.Verify(pc => pc.CalculatePercentages(
                It.Is(getRowCountsExpression),
                It.IsAny<Func<int, int>>(),
                It.IsAny<Func<int, int, int>>(),
                It.IsAny<Action<int, int, int>>()));
        }

        [Test]
        public void
            UpdateWith_ListOfPlayers_CallToPercentageCalculatorPassesHeroBStatisticsColumnCountAsGetColumnCountFunction(
            )
        {
            const int columnCount = 2;
            _heroBStatisticStub.SetupGet(s => s.MatchingPlayers)
                .Returns(new IList<IAnalyzablePokerPlayer>[columnCount]);

            var sut = new HeroCheckOrBetSetStatistics(_statisticsStub, _calculatorMock.Object);

            Expression<Predicate<Func<int, int>>> getColumnCountsExpression =
                func => func(0) == columnCount;

            sut.UpdateWith(new[] { _playerStub.Object });

            _calculatorMock.Verify(pc => pc.CalculatePercentages(
                                             It.IsAny<Func<int>>(),
                                             It.Is(getColumnCountsExpression),
                                             It.IsAny<Func<int, int, int>>(),
                                             It.IsAny<Action<int, int, int>>()));
        }
    }
}