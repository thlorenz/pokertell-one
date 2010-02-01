namespace PokerTell.Statistics.Tests.ViewModels.StatisticsSetDetails
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using Infrastructure.Enumerations.PokerHand;
    using Infrastructure.Interfaces.PokerHand;
    using Infrastructure.Interfaces.Statistics;

    using Interfaces;

    using Moq;

    using NUnit.Framework;

    using Statistics.Detailed;
    using Statistics.ViewModels;
    using Statistics.ViewModels.StatisticsSetDetails;

    using Tools.FunctionalCSharp;
    using Tools.Interfaces;

    using UnitTests.Tools;

    public class DetailedStatisticsViewModelTests
    {
        DetailedStatisticsViewModelImpl _sut;

        StubBuilder _stub;

        [SetUp]
        public void _Init()
        {
            _stub = new StubBuilder();
            _sut = new DetailedStatisticsViewModelImpl();
           
        }

        

        [Test]
        public void SelectedAnalyzablePlayers_SelectedCellsAreEmpty_ReturnsEmptyList()
        {
            _sut.SelectedAnalyzablePlayers.ShouldBeEmpty();
        }

        [Test]
        public void SelectedAnalyzablePlayers_TwoCellsHaveData_CellAtZeroZeroIsSelected_ReturnsAnalyzablePlayersDataOfCellZeroZeroOnly()
        {
            var analyzablePokerPlayer1 = _stub.Out<IAnalyzablePokerPlayer>();
            var analyzablePokerPlayer2 = _stub.Out<IAnalyzablePokerPlayer>();

            var analyzablePokerPlayersStub = new[] { new List<IAnalyzablePokerPlayer> { analyzablePokerPlayer1 }, new List<IAnalyzablePokerPlayer> { analyzablePokerPlayer2 } };
            var statisticStub = _stub.Setup<IActionSequenceStatistic>()
                .Get(s => s.MatchingPlayers).Returns(analyzablePokerPlayersStub)
                .Out;
            var statisticSetStub = _stub.Setup<IActionSequenceStatisticsSet>()
                .Get(s => s.ActionSequenceStatistics).Returns(new[] { statisticStub })
                .Out;

            _sut.InitializeWith(statisticSetStub)
                .AddToSelection(0, 0);

            _sut.SelectedAnalyzablePlayers
                .ShouldContain(analyzablePokerPlayer1)
                .ShouldNotContain(analyzablePokerPlayer2);
        }

        [Test]
        public void SelectedAnalyzablePlayers_TwoCellsHaveData_CellsAtZeroZeroAndZeroOneAreSelected_ReturnsAnalyzablePlayersDataOfBothCells()
        {
            var analyzablePokerPlayer1 = _stub.Out<IAnalyzablePokerPlayer>();
            var analyzablePokerPlayer2 = _stub.Out<IAnalyzablePokerPlayer>();

            var analyzablePokerPlayersStub = new[] { new List<IAnalyzablePokerPlayer> { analyzablePokerPlayer1 }, new List<IAnalyzablePokerPlayer> { analyzablePokerPlayer2 } };
            var statisticStub = _stub.Setup<IActionSequenceStatistic>()
                .Get(s => s.MatchingPlayers).Returns(analyzablePokerPlayersStub)
                .Out;
            var statisticSetStub = _stub.Setup<IActionSequenceStatisticsSet>()
                .Get(s => s.ActionSequenceStatistics).Returns(new[] { statisticStub })
                .Out;

            _sut.InitializeWith(statisticSetStub)
                .AddToSelection(0, 0)
                .AddToSelection(0, 1);

            _sut.SelectedAnalyzablePlayers
                .ShouldContain(analyzablePokerPlayer1)
                .ShouldContain(analyzablePokerPlayer2);
        }

        [Test]
        public void SelectedActionSequence_OneCellWithOppBHeroRDataSelected_ReturnsOppBHeroR()
        {
            const ActionSequences actionSequence = ActionSequences.OppBHeroR;
            var statisticStub = _stub.Setup<IActionSequenceStatistic>()
                .Get(s => s.ActionSequence).Returns(actionSequence)
                .Out;
            var statisticSetStub = _stub.Setup<IActionSequenceStatisticsSet>()
                .Get(s => s.ActionSequenceStatistics).Returns(new[] { statisticStub })
                .Out;

            _sut.InitializeWith(statisticSetStub)
                .AddToSelection(0, 0);

            _sut.SelectedActionSequence.ShouldBeEqualTo(actionSequence);
        }

        [Test]
        public void SelectedColumnsSpan_Cell_0_0_Selected_Returns_0_0()
        {
            _sut.AddToSelection(0, 0);

            _sut.SelectedColumnsSpanGet.ShouldBeEqualTo(Tuple.New(0, 0));
        }

        [Test]
        public void SelectedColumnsSpan_Cells_0_1_0_2_Selected_Returns_1_2()
        {
            _sut
                .AddToSelection(0, 1)
                .AddToSelection(0, 2);

            _sut.SelectedColumnsSpanGet.ShouldBeEqualTo(Tuple.New(1, 2));
        }

        
    }

    class DetailedStatisticsViewModelImpl : DetailedStatisticsViewModel
    {
        public IDetailedStatisticsViewModel ChildViewModelSet { set { ChildViewModel = value; } }

        public DetailedStatisticsViewModelImpl()
            : base("columnHeaderTitle")
        {
        }

        public ITuple<int, int> SelectedColumnsSpanGet
        {
            get { return base.SelectedColumnsSpan; }
        }

        protected override IDetailedStatisticsViewModel CreateTableAndDescriptionFor(IActionSequenceStatisticsSet statisticsSet)
        {
            return this;
        }
    }
}