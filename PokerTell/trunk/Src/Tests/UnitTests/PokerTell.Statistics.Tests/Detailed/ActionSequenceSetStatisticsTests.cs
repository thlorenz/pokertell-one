namespace PokerTell.Statistics.Tests.Detailed
{
    using System;

    using Infrastructure.Interfaces.PokerHand;

    using Interfaces;


    using Moq;

    using NUnit.Framework;

    using Statistics.Detailed;

    public class ActionSequenceSetStatisticsTests
    {
        Mock<IActionSequenceStatistic> _statisticMock;
        Mock<IAnalyzablePokerPlayer> _playerMock;

        Mock<IPercentagesCalculator> _calculatorMock;

        [SetUp]
        public void _Init()
        {
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
    }
}