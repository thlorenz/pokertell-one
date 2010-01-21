namespace PokerTell.Statistics.Tests.Detailed
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using Infrastructure.Enumerations.PokerHand;
    using Infrastructure.Interfaces.PokerHand;
    using Infrastructure.Interfaces.Statistics;

    using Interfaces;

    using Moq;

    using NUnit.Framework;

    using Statistics.Detailed;

    using UnitTests.Tools;

    public class ActionSequenceStatisticsSetTests
    {
        #region Constants and Fields

        Mock<IPercentagesCalculator> _calculatorMock;

        Mock<IAnalyzablePokerPlayer> _playerMock;

        Mock<IActionSequenceStatistic> _statisticMock;

        StubBuilder _stub;

        const string SomePlayer = "somePlayer";

        const ActionSequences SomeActionSequence = ActionSequences.PreFlopNoFrontRaise;

        const bool SomeRaisedOrNotRaised = false;


        #endregion

        #region Public Methods

        [Test]
        public void CalculateCumulativePercentages_Always_InitializesCumulativePercentagesByRowToStatisticsCount()
        {
            var sut = new ActionSequenceStatisticsSetMock(
                new[]
                    {
                        _stub.Out<IActionSequenceStatistic>(),
                        _stub.Out<IActionSequenceStatistic>()
                    },
                _stub.Out<IPercentagesCalculator>());

            sut.CalculateCumulativePercentagesInvoke();

            sut.CumulativePercentagesByRow.ShouldHaveCount(2);
        }

        [Test]
        public void CalculateCumulativePercentages_TotalCount1_0_TotalCount2_0_Sets_Perc1_0_Perc2_0()
        {
            const int TotalCount1 = 0;
            const int TotalCount2 = 0;

            ActionSequenceStatisticsSetMock sut = GetStatisticsSetStubWithTwoStatisticsReturningTotalCounts(
                TotalCount1, TotalCount2);

            sut.CalculateCumulativePercentagesInvoke();

            sut.CumulativePercentagesByRow[0].ShouldBeEqualTo(0);
            sut.CumulativePercentagesByRow[1].ShouldBeEqualTo(0);
        }

        [Test]
        public void CalculateCumulativePercentages_TotalCount1_1_TotalCount2_0_Sets_Perc1_100_Perc2_0()
        {
            const int TotalCount1 = 1;
            const int TotalCount2 = 0;

            ActionSequenceStatisticsSetMock sut = GetStatisticsSetStubWithTwoStatisticsReturningTotalCounts(
                TotalCount1, TotalCount2);

            sut.CalculateCumulativePercentagesInvoke();

            sut.CumulativePercentagesByRow[0].ShouldBeEqualTo(100);
            sut.CumulativePercentagesByRow[1].ShouldBeEqualTo(0);
        }

        [Test]
        public void CalculateCumulativePercentages_TotalCount1_1_TotalCount2_1_Sets_Perc1_50_Perc2_50()
        {
            const int TotalCount1 = 1;
            const int TotalCount2 = 1;

            ActionSequenceStatisticsSetMock sut = GetStatisticsSetStubWithTwoStatisticsReturningTotalCounts(
                TotalCount1, TotalCount2);

            sut.CalculateCumulativePercentagesInvoke();

            sut.CumulativePercentagesByRow[0].ShouldBeEqualTo(50);
            sut.CumulativePercentagesByRow[1].ShouldBeEqualTo(50);
        }

        [Test]
        public void
            CalculateCumulativePercentages_TotalCount1_1_TotalCount2_1_TotalCount3_2_Sets_Perc1_25_Perc2_25_Perc3_50()
        {
            const int TotalCount1 = 1;
            const int TotalCount2 = 1;
            const int TotalCount3 = 2;

            IActionSequenceStatistic statisticStub1 = _stub.Setup<IActionSequenceStatistic>()
                .Get(s => s.TotalCounts).Returns(TotalCount1).Out;
            IActionSequenceStatistic statisticStub2 = _stub.Setup<IActionSequenceStatistic>()
                .Get(s => s.TotalCounts).Returns(TotalCount2).Out;
            IActionSequenceStatistic statisticStub3 = _stub.Setup<IActionSequenceStatistic>()
                .Get(s => s.TotalCounts).Returns(TotalCount3).Out;

            var sut = new ActionSequenceStatisticsSetMock(
                new[] { statisticStub1, statisticStub2, statisticStub3 },
                _stub.Out<IPercentagesCalculator>());

            sut.CalculateCumulativePercentagesInvoke();

            sut.CumulativePercentagesByRow[0].ShouldBeEqualTo(25);
            sut.CumulativePercentagesByRow[1].ShouldBeEqualTo(25);
            sut.CumulativePercentagesByRow[2].ShouldBeEqualTo(50);
        }

        [Test]
        public void UpdateWith_Always_RaisesStatisticsWereUpdatedWithItselfAsArgument()
        {
            var sut = new ActionSequenceStatisticsSetMock(
                new[] { _stub.Out<IActionSequenceStatistic>() }, _stub.Out<IPercentagesCalculator>());
            
            bool wasRaisedWithCorrectArgument = false;
            sut.StatisticsWereUpdated += arg => wasRaisedWithCorrectArgument = arg == sut;

            sut.UpdateWith(_stub.Out<IEnumerable<IAnalyzablePokerPlayer>>());

            wasRaisedWithCorrectArgument.ShouldBeTrue();
        }

        [Test]
        public void UpdateWith_ListOfPlayers_CalculatesCulumlativePercentages()
        {
            var sut = new ActionSequenceStatisticsSetMock(
                new[] { _stub.Out<IActionSequenceStatistic>() }, _stub.Out<IPercentagesCalculator>());

            var analyzablePokerPlayers = new[] { _stub.Out<IAnalyzablePokerPlayer>() };
            sut.UpdateWith(analyzablePokerPlayers);

            sut.CumulativePercentagesCalculated.ShouldBeTrue();
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

            var sut = new ActionSequenceStatisticsSet(
                _calculatorMock.Object,
                statistics,
                SomePlayer,
                SomeActionSequence,
                SomeRaisedOrNotRaised);

            Expression<Predicate<Func<int, int>>> getColumnCountsExpression =
                func => func(0) == columnCount;

            var analyzablePokerPlayers = new[] { _playerMock.Object };
            sut.UpdateWith(analyzablePokerPlayers);

            _calculatorMock.Verify(
                pc => pc.CalculatePercentages(
                          It.IsAny<Func<int>>(),
                          It.Is(getColumnCountsExpression),
                          It.IsAny<Func<int, int, int>>(),
                          It.IsAny<Action<int, int, int>>()));
        }

        [Test]
        public void UpdateWith_ListOfPlayers_CallToPercentageCalculatorPassesStatisticsCountAsGetRowCountFunction()
        {
            var statistics = new List<IActionSequenceStatistic> { _statisticMock.Object, _statisticMock.Object };
            var sut = new ActionSequenceStatisticsSet(
                 _calculatorMock.Object,
                 statistics,
                SomePlayer,
                SomeActionSequence,
                SomeRaisedOrNotRaised);

            Expression<Predicate<Func<int>>> getRowCountsExpression = func => func() == statistics.Count;

            var analyzablePokerPlayers = new[] { _playerMock.Object };
            sut.UpdateWith(analyzablePokerPlayers);

            _calculatorMock.Verify(
                pc => pc.CalculatePercentages(
                          It.Is(getRowCountsExpression),
                          It.IsAny<Func<int, int>>(),
                          It.IsAny<Func<int, int, int>>(),
                          It.IsAny<Action<int, int, int>>()));
        }

        [Test]
        public void UpdateWith_ListOfPlayers_CallsPercentageCalculator()
        {
            var sut = new ActionSequenceStatisticsSet(
                _calculatorMock.Object,
                new[] { _statisticMock.Object },
               SomePlayer,
                SomeActionSequence,
                SomeRaisedOrNotRaised);

            var analyzablePokerPlayers = new[] { _playerMock.Object };
            sut.UpdateWith(analyzablePokerPlayers);

            _calculatorMock.Verify(
                pc => pc.CalculatePercentages(
                          It.IsAny<Func<int>>(),
                          It.IsAny<Func<int, int>>(),
                          It.IsAny<Func<int, int, int>>(),
                          It.IsAny<Action<int, int, int>>()));
        }

        [Test]
        public void UpdateWith_ListOfPlayers_UpdatesAllStatisticsWithThosePlayers()
        {
            var sut = new ActionSequenceStatisticsSet(
                _calculatorMock.Object,
                new[] { _statisticMock.Object },
                SomePlayer,
                SomeActionSequence,
                SomeRaisedOrNotRaised);

            var analyzablePokerPlayers = new[] { _playerMock.Object };
            sut.UpdateWith(analyzablePokerPlayers);

            _statisticMock.Verify(st => st.UpdateWith(analyzablePokerPlayers));
        }

        [SetUp]
        public void _Init()
        {
            _stub = new StubBuilder();
            _statisticMock = new Mock<IActionSequenceStatistic>();
            _playerMock = new Mock<IAnalyzablePokerPlayer>();
            _calculatorMock = new Mock<IPercentagesCalculator>();
        }

        #endregion

        #region Methods

        ActionSequenceStatisticsSetMock GetStatisticsSetStubWithTwoStatisticsReturningTotalCounts(
            int TotalCount1, int TotalCount2)
        {
            IActionSequenceStatistic statisticStub1 = _stub.Setup<IActionSequenceStatistic>()
                .Get(s => s.TotalCounts).Returns(TotalCount1).Out;
            IActionSequenceStatistic statisticStub2 = _stub.Setup<IActionSequenceStatistic>()
                .Get(s => s.TotalCounts).Returns(TotalCount2).Out;

            return new ActionSequenceStatisticsSetMock(
                new[] { statisticStub1, statisticStub2 },
                _stub.Out<IPercentagesCalculator>());
        }

        #endregion
    }

    internal class ActionSequenceStatisticsSetMock : ActionSequenceStatisticsSet
    {
        
        const string SomePlayer = "somePlayer";

        const ActionSequences SomeActionSequence = ActionSequences.PreFlopNoFrontRaise;

        const bool SomeRaisedOrNotRaised = false;


        #region Constructors and Destructors

        public ActionSequenceStatisticsSetMock(
            IEnumerable<IActionSequenceStatistic> statistics, IPercentagesCalculator percentagesCalculator)
            : base(percentagesCalculator, 
                 statistics, 
                SomePlayer,
                SomeActionSequence,
                SomeRaisedOrNotRaised)
        {
        }

        #endregion

        #region Properties

        public bool CumulativePercentagesCalculated { get; set; }

        public bool IndividualPercentagesCalculated { get; set; }

        #endregion

        #region Public Methods

        public void CalculateCumulativePercentagesInvoke()
        {
            base.CalculateCumulativePercentages();
        }

        public void CalculateIndividualPercentagesInvoke()
        {
            base.CalculateIndividualPercentages();
        }

        #endregion

        #region Methods

        protected override void CalculateCumulativePercentages()
        {
            CumulativePercentagesCalculated = true;
        }

        protected override void CalculateIndividualPercentages()
        {
            IndividualPercentagesCalculated = true;
        }

        #endregion
    }
}