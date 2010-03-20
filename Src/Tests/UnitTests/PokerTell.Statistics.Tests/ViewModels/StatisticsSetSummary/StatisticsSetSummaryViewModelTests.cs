namespace PokerTell.Statistics.Tests.ViewModels.StatisticsSetSummary
{
    using System;
    using System.Collections.Generic;

    using Infrastructure.Enumerations.PokerHand;
    using Infrastructure.Interfaces.Statistics;

    using Moq;

    using NUnit.Framework;

    using Statistics.ViewModels.StatisticsSetSummary;

    using UnitTests;
    using UnitTests.Tools;

    using System.Linq;

    public class StatisticsSetSummaryViewModelTests : TestWithLog
    {
        StatisticsSetSummaryViewModel _sut;

        Mock<IActionSequenceStatisticsSet> _statisticsSetStub;

        StubBuilder _stub;

        IActionSequenceStatistic _actionSequenceStatisticStub;

        int[] _cumulativePercentages;

        [SetUp]
        public void _Init()
        {
            _sut = new StatisticsSetSummaryViewModel();
            _stub = new StubBuilder();
            _statisticsSetStub = new Mock<IActionSequenceStatisticsSet>();
            _cumulativePercentages = new[] { 1, 2, 3, 4, 5 };
            _statisticsSetStub
                .SetupGet(ss => ss.CumulativePercentagesByRow)
                .Returns(_cumulativePercentages);

            _actionSequenceStatisticStub = _stub.Setup<IActionSequenceStatistic>()
                .Get(s => s._actionSequence).Returns(ActionSequences.HeroB).Out;
        }

        [Test]
        public void Constructor_Always_CreatesEmptyRows()
        {
            _sut.Rows.ShouldHaveCount(0);
        }

        [Test]
        public void UpdateWith_EmptyStatisticsSet_ThrowsArgumenException()
        {
            _statisticsSetStub
                .SetupGet(ss => ss.ActionSequenceStatistics)
                .Returns(new List<IActionSequenceStatistic>());

            Assert.Throws<ArgumentException>(() => _sut.UpdateWith(_statisticsSetStub.Object));
        }

        [Test]
        public void UpdateWith_OneStatisticNoRowsCreatedSoFar_CreatesOneRow()
        {
            _statisticsSetStub
                .SetupGet(ss => ss.ActionSequenceStatistics)
                .Returns(new List<IActionSequenceStatistic> { _actionSequenceStatisticStub });

            _sut.UpdateWith(_statisticsSetStub.Object);

            _sut.Rows.ShouldHaveCount(1);
        }

        [Test]
        public void UpdateWith_TwoStatisticNoRowsCreatedSoFar_CreatesTwoRows()
        {
            _statisticsSetStub
                .SetupGet(ss => ss.ActionSequenceStatistics)
                .Returns(new List<IActionSequenceStatistic> { _actionSequenceStatisticStub, _actionSequenceStatisticStub });

            _sut.UpdateWith(_statisticsSetStub.Object);

            _sut.Rows.ShouldHaveCount(2);
        }

        [Test]
        public void UpdateWith_OneStatisticOneRowAddedBefore_DoesNotCreateExtraRow()
        {
            _statisticsSetStub
                .SetupGet(ss => ss.ActionSequenceStatistics)
                .Returns(new List<IActionSequenceStatistic> { _actionSequenceStatisticStub });

            _sut.Rows.Add(_stub.Out<IStatisticsSetSummaryRowViewModel>());
             
            _sut.UpdateWith(_statisticsSetStub.Object);

            _sut.Rows.ShouldHaveCount(1);
        }

        [Test]
        public void UpdateWith_OneStatistic_UpdatesRowWithDataFromActionSequenceStatistic()
        {
            var statisticStub = _stub.Setup<IActionSequenceStatistic>()
                .Get(s => s.Percentages).Returns(new[] { 1, 2 })
                .Out;
            
            _statisticsSetStub
                .SetupGet(ss => ss.ActionSequenceStatistics)
                .Returns(new List<IActionSequenceStatistic> { statisticStub });

            var statisticsSetRowMock = new Mock<IStatisticsSetSummaryRowViewModel>();
            _sut.Rows.Add(statisticsSetRowMock.Object);

            _sut.UpdateWith(_statisticsSetStub.Object);

            statisticsSetRowMock.Verify(row => row.UpdateWith(_cumulativePercentages[0], statisticStub.Percentages));
        }

        [Test]
        public void SelectStatisticsSetCommandExecute_WasUpdatedWithStatisticsSet_RaisesStatisticsSetSelectedEvent()
        {
            bool eventWasRaisedWithStatisticsSet = false;

            _sut.StatisticsSetSelectedEvent += arg => eventWasRaisedWithStatisticsSet = arg == _statisticsSetStub.Object;

            _statisticsSetStub
               .SetupGet(ss => ss.ActionSequenceStatistics)
               .Returns(new List<IActionSequenceStatistic> { _actionSequenceStatisticStub });

            _sut.UpdateWith(_statisticsSetStub.Object);

            _sut.SelectStatisticsSetCommand.Execute(null);

            eventWasRaisedWithStatisticsSet.ShouldBeTrue();
        }
    }
}