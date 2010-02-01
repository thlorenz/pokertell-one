namespace PokerTell.Statistics.Tests.ViewModels.Base
{
    using Interfaces;

    using Moq;

    using NUnit.Framework;

    using PokerTell.Statistics.ViewModels.Base;

    using Tools.FunctionalCSharp;

    using PokerTell.UnitTests.Tools;

    public class StatisticsTableViewModelTests
    {
        StatisticsTableViewModel _sut;

        StubBuilder _stub;

        [SetUp]
        public void _Init()
        {
            _sut = new StatisticsTableViewModel("someDescription");
            _stub = new StubBuilder();
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

        [Test]
        public void SetChildViewModel_Always_RaisesChildViewModelChangedEventWithThatViewModel()
        {
            var viewModelStub = _stub.Out<IStatisticsTableViewModel>();
            var wasRaisedWithViewModel = false;

            _sut.ChildViewModelChanged += vm => wasRaisedWithViewModel = vm.Equals(viewModelStub);

            _sut.ChildViewModel = viewModelStub;

            wasRaisedWithViewModel.ShouldBeTrue();
        }
    }
}