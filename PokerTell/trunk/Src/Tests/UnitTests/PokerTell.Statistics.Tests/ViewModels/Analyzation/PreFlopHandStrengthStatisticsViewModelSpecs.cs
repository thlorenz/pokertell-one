namespace PokerTell.Statistics.Tests.ViewModels.Analyzation
{
    using System.Collections.Generic;

    using Infrastructure.Enumerations.PokerHand;
    using Infrastructure.Interfaces.PokerHand;

    using Machine.Specifications;

    using Moq;

    using PokerTell.Statistics.Interfaces;
    using PokerTell.Statistics.ViewModels.Analyzation;

    using Statistics.Analyzation;

    using Tools.FunctionalCSharp;

    using It = Machine.Specifications.It;

    // Resharper disable InconsistentNaming
    public abstract class PreFlopHandStrengthStatisticsViewModelSpecs
    {
        protected static Mock<IPreFlopHandStrengthStatistics> _handStrengthStatisticsStub;

        protected static Mock<IPreFlopStartingHandsVisualizerViewModel> _startingHandsVisualizerViewModelMock;

        protected static readonly StubBuilder _stub = new StubBuilder();

        protected static PreFlopHandStrengthStatisticsViewModelImpl _sut;

        Establish specContext = () => {
            _handStrengthStatisticsStub = new Mock<IPreFlopHandStrengthStatistics>();
            _startingHandsVisualizerViewModelMock = new Mock<IPreFlopStartingHandsVisualizerViewModel>();

            _sut = new PreFlopHandStrengthStatisticsViewModelImpl(_startingHandsVisualizerViewModelMock.Object, 
                                                                  _handStrengthStatisticsStub.Object, 
                                                                  _stub.Out<IPreFlopHandStrengthDescriber>(), 
                                                                  "someTitle");

            _sut.InitializeWith(_stub.Out<IEnumerable<IAnalyzablePokerPlayer>>(), "somePlayer", _stub.Some<ActionSequences>());
        };

        protected class PreFlopHandStrengthStatisticsViewModelImpl : PreFlopHandStrengthStatisticsViewModel
        {
            public PreFlopHandStrengthStatisticsViewModelImpl(
                IPreFlopStartingHandsVisualizerViewModel startingHandsVisualizerViewModel, 
                IPreFlopHandStrengthStatistics preFlopHandStrengthStatistics, 
                IPreFlopHandStrengthDescriber handStrengthDescriber, 
                string columnHeaderTitle)
                : base(startingHandsVisualizerViewModel, preFlopHandStrengthStatistics, handStrengthDescriber, columnHeaderTitle)
            {
            }
        }

        [Subject(typeof(PreFlopHandStrengthStatisticsViewModel), "VisualizeStartingHandsCommand, CanExecute")]
        public class when_no_cells_have_been_selected : PreFlopHandStrengthStatisticsViewModelSpecs
        {
            It should_return_false = () => _sut.VisualizeStartingHandsCommand.CanExecute(null).ShouldBeFalse(); 
        }

        [Subject(typeof(PreFlopHandStrengthStatisticsViewModel), "VisualizeStartingHandsCommand, CanExecute")]
        public class when_one_cell_has_been_selected : PreFlopHandStrengthStatisticsViewModelSpecs
        {
            Because of = () => _sut.SelectedCells.Add(Tuple.New(0, 0));

            It should_return_true = () => _sut.VisualizeStartingHandsCommand.CanExecute(null).ShouldBeTrue(); 
        }

        public class Ctx_HoleCards_In_First_And_Second_Column_Of_HandStrengthStatistics : PreFlopHandStrengthStatisticsViewModelSpecs
        {
            Establish context = () => {
                var valuedHoleCardsStub1 = new Mock<IValuedHoleCards>();
                var valuedHoleCardsStub2 = new Mock<IValuedHoleCards>();
                var valuedHoleCardsStub3 = new Mock<IValuedHoleCards>();
                valuedHoleCardsStub1.SetupGet(v => v.Name).Returns("AA");
                valuedHoleCardsStub2.SetupGet(v => v.Name).Returns("AKs");
                valuedHoleCardsStub3.SetupGet(v => v.Name).Returns("T9");

                _handStrengthStatisticsStub.SetupGet(h => h.KnownCards).Returns(new[]
                    { new[] { valuedHoleCardsStub1.Object, valuedHoleCardsStub2.Object },
                       new[] { valuedHoleCardsStub3.Object } });
            };
        }

        [Subject(typeof(PreFlopHandStrengthStatisticsViewModel), "VisualizeStartingHandsCommand, Execute")]
        public class when_the_first_column_is_selected : Ctx_HoleCards_In_First_And_Second_Column_Of_HandStrengthStatistics
        {
            Establish context = () => _sut.SelectedCells.Add(Tuple.New(0, 0));

            Because of = () => _sut.VisualizeStartingHandsCommand.Execute(null);

            It should_visualize_the_Cards_in_the_first_column 
                = () => _startingHandsVisualizerViewModelMock.Verify(v => v.Visualize(new[] { "AA", "AKs" }));
        }

        [Subject(typeof(PreFlopHandStrengthStatisticsViewModel), "VisualizeStartingHandsCommand, Execute")]
        public class when_the_second_column_is_selected : Ctx_HoleCards_In_First_And_Second_Column_Of_HandStrengthStatistics
        {
            Establish context = () => _sut.SelectedCells.Add(Tuple.New(0, 1));

            Because of = () => _sut.VisualizeStartingHandsCommand.Execute(null);

            It should_visualize_the_Cards_in_the_second_column 
                = () => _startingHandsVisualizerViewModelMock.Verify(v => v.Visualize(new[] { "T9" }));
        }
         
        [Subject(typeof(PreFlopHandStrengthStatisticsViewModel), "VisualizeStartingHandsCommand, Execute")]
        public class when_the_first_and_second_columns_are_selected : Ctx_HoleCards_In_First_And_Second_Column_Of_HandStrengthStatistics
        {
            Establish context = () => {
                _sut.SelectedCells.Add(Tuple.New(0, 0));
                _sut.SelectedCells.Add(Tuple.New(0, 1));
            };

            Because of = () => _sut.VisualizeStartingHandsCommand.Execute(null);

            It should_visualize_the_Cards_in_the_first_and_second_columns
                = () => _startingHandsVisualizerViewModelMock.Verify(v => v.Visualize(new[] { "AA", "AKs", "T9" }));

            It should_assign_the_VisualizerViewModel_to_the_ChildViewModel = () => _sut.ChildViewModel.ShouldBeTheSameAs(_startingHandsVisualizerViewModelMock.Object);
        }
    }
}