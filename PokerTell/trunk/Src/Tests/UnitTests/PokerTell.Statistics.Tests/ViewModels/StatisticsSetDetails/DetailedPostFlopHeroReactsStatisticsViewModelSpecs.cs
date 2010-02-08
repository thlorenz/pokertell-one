namespace PokerTell.Statistics.Tests.ViewModels.StatisticsSetDetails
{
    using System.Collections.Generic;

    using Machine.Specifications;

    using Moq;

    using PokerTell.Infrastructure.Enumerations.PokerHand;
    using PokerTell.Infrastructure.Interfaces.PokerHand;
    using PokerTell.Infrastructure.Interfaces.Statistics;
    using PokerTell.Statistics.Interfaces;
    using PokerTell.Statistics.ViewModels.StatisticsSetDetails;

    using Tools.FunctionalCSharp;
    using Tools.Interfaces;

    using It = Machine.Specifications.It;

    // ReSharper disable InconsistentNaming
    public class DetailedPostFlopHeroReactsStatisticsViewModelSpecs
    {
        /*
         * Specifications
         * Subject: DetailedPostFlopHeroActsStatisticsViewModel
         * 
         * Investigate Raise
         *      
         *      CanExecute
         *          It should be false when no cells have been selected
         *          It should be true when one cell in raising column has been selected
         *          It should be true when two cells in raising column have been selected
         *          It should be false when one cell in folding column have been selected
         *          It should be false when one cell in calling column have been selected
         *      Execute
         *          It should initialize the raise reaction statistics model with the data of the selected cells
         *          It should assign the raise reaction statistics model to its child view model
         */
        protected static Mock<IHandBrowserViewModel> _handBrowserViewModelStub;

        protected static Mock<IPostFlopHeroReactsRaiseReactionStatisticsViewModel> _raiseReactionStatisticsViewModelMock;

        protected static Mock<IActionSequenceStatisticsSet> _statisticsSetStub;

        protected static DetailedPostFlopHeroReactsStatisticsViewModel _sut;

        static Mock<IActionSequenceStatistic> _foldStatisticStub;

        Establish context = () => {
            _raiseReactionStatisticsViewModelMock = new Mock<IPostFlopHeroReactsRaiseReactionStatisticsViewModel>();

            _handBrowserViewModelStub = new Mock<IHandBrowserViewModel>();

            _foldStatisticStub = new Mock<IActionSequenceStatistic>();
            _foldStatisticStub.SetupGet(s => s.ActionSequence).Returns(ActionSequences.OppBHeroF);

           _callStatisticStub = new Mock<IActionSequenceStatistic>();
            _callStatisticStub.SetupGet(s => s.ActionSequence).Returns(ActionSequences.OppBHeroC);

            _raiseStatisticStub = new Mock<IActionSequenceStatistic>();
            _raiseStatisticStub.SetupGet(s => s.ActionSequence).Returns(ActionSequences.OppBHeroR);

            _statisticsSetStub = new Mock<IActionSequenceStatisticsSet>();
            _statisticsSetStub.SetupGet(s => s.ActionSequenceStatistics).Returns(new[]
                {
                    _foldStatisticStub.Object, _callStatisticStub.Object, _raiseStatisticStub.Object
                });

            _sut = new DetailedPostFlopHeroReactsStatisticsViewModel(_handBrowserViewModelStub.Object, 
                                                                     _raiseReactionStatisticsViewModelMock.Object);
            _sut.InitializeWith(_statisticsSetStub.Object);
        };

        static Mock<IActionSequenceStatistic> _callStatisticStub;

        static Mock<IActionSequenceStatistic> _raiseStatisticStub;
    }

    [SetupForEachSpecification]
    public class CanExecute : DetailedPostFlopHeroReactsStatisticsViewModelSpecs
    {
        It should_be_false_when_no_cells_have_been_selected
            = () => _sut.InvestigateRaiseReactionCommand.CanExecute(null).ShouldBeFalse();

        It should_be_true_when_one_cell_in_raisin_column_has_been_selected
            = () => {
                _sut.SelectedCells.Add(Tuple.New(2, 0));
                _sut.InvestigateRaiseReactionCommand.CanExecute(null).ShouldBeTrue();
            };

        It should_be_false_when_one_cell_in_folding_column_has_been_selected
            = () => {
                _sut.SelectedCells.Add(Tuple.New(0, 0));
                _sut.InvestigateRaiseReactionCommand.CanExecute(null).ShouldBeFalse();
            };

        It should_be_false_when_one_cell_in_calling_column_has_been_selected
            = () => {
                _sut.SelectedCells.Add(Tuple.New(1, 0));
                _sut.InvestigateRaiseReactionCommand.CanExecute(null).ShouldBeFalse();
            };
    }

    public class Execute : DetailedPostFlopHeroReactsStatisticsViewModelSpecs
    {
            Establish context_InvestigateWith_Returns_Itself = () =>
            {
                _raiseReactionStatisticsViewModelMock.Setup(
                    r => r.InitializeWith(Moq.It.IsAny<IEnumerable<IAnalyzablePokerPlayer>>(),
                                          Moq.It.IsAny<ITuple<double, double>>(),
                                          Moq.It.IsAny<string>(),
                                          Moq.It.IsAny<ActionSequences>(),
                                          Moq.It.IsAny<Streets>()))
                    .Returns(_raiseReactionStatisticsViewModelMock.Object);

                _sut.SelectedCells.Add(Tuple.New(2, 0));
            };

            Because of = () => _sut.InvestigateRaiseReactionCommand.Execute(null);

            It should_assign_the_raise_reaction_statistics_model_to_its_child_view_model
                = () => _sut.ChildViewModel.ShouldBeTheSameAs(_raiseReactionStatisticsViewModelMock.Object);

            It should_initialize_the_raise_reaction_statistics_model_with_the_data_of_the_selected_cells
                = () => _raiseReactionStatisticsViewModelMock.Verify(
                            r => r.InitializeWith(Moq.It.IsAny<IEnumerable<IAnalyzablePokerPlayer>>(),
                                             Moq.It.IsAny<ITuple<double, double>>(),
                                             Moq.It.IsAny<string>(),
                                             ActionSequences.OppBHeroR,
                                             Moq.It.IsAny<Streets>()));
    }
}