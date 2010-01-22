namespace PokerTell.Statistics.Tests.ViewModels.StatisticsSetDetails
{
    using System;

    using Infrastructure.Interfaces.Statistics;

    using NUnit.Framework;

    using Statistics.ViewModels;
    using Statistics.ViewModels.StatisticsSetDetails;

    using Tools.FunctionalCSharp;

    using UnitTests.Tools;

    public class DetailedStatisticsViewModelTests
    {
        IDetailedStatisticsViewModel _sut;

        [SetUp]
        public void _Init()
        {
            _sut = new DetailedStatisticsViewModelImpl();
        }

        [Test]
        public void AddSelection_RowAndColumn_AddsTupleForRowColumnToSelectedCells()
        {
            const int row = 0;
            const int column = 1;
            _sut.AddToSelection(row, column);

            _sut.SelectedCells.ShouldContain(new Tuple<int, int>(row, column));
        }

        [Test]
        public void ClearSelection_SelectedCellsContainItems_SelectedCellsAreCleared()
        {
            _sut.AddToSelection(0, 0);

            _sut.ClearSelection();

            _sut.SelectedCells.ShouldBeEmpty();
        }
    }

    class DetailedStatisticsViewModelImpl : DetailedStatisticsViewModel
    {
        public DetailedStatisticsViewModelImpl()
            : base("columnHeaderTitle")
        {
        }

        public override IDetailedStatisticsViewModel InitializeWith(IActionSequenceStatisticsSet statisticsSet)
        {
            return this;
        }
    }
}