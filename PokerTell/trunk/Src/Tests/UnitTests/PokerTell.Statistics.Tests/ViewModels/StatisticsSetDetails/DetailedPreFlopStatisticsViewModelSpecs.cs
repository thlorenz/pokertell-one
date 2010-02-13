namespace PokerTell.Statistics.Tests.ViewModels.StatisticsSetDetails
{
    using System.Collections.Generic;

    using Infrastructure.Enumerations.PokerHand;
    using Infrastructure.Interfaces.PokerHand;
    using Infrastructure.Interfaces.Statistics;

    using Interfaces;

    using Machine.Specifications;

    using Moq;

    using Statistics.ViewModels.StatisticsSetDetails;

    using Tools.FunctionalCSharp;
    using Tools.Interfaces;

    using It = Machine.Specifications.It;

    // ReSharper disable InconsistentNaming
    [Subject(typeof(DetailedPreFlopStatisticsViewModel))]
    public abstract class DetailedPreFlopStatisticsViewModelSpecs
    {
        /*
        * Specifications
        * Subject: DetailedPreFlopStatisticsViewModel
        * 
        Investigate Raise
            Execute
                » should assign the raise reaction statistics model to its child view model
                » should initialize the raise reaction statistics model with the data of the selected cells

           given statistics for raised pot CanExecute
                » should be false when no cells have been selected
                » should be false when only cells in the call row have been selected and it contains analyzable players
                » should be false when only cells in the fold row have been selected and it contains analyzable players
                » should be true when one cell in raise row has been selected and it contains analyzable players
                » should be false when one cell in raise row has been selected but it contains no analyzable players

            given statistics for unraised pot CanExecute
                » should be false when no cells have been selected
                » should be false when only cells in the call row have been selected and it contains analyzable players
                » should be false when only cells in the fold row have been selected and it contains analyzable players
                » should be true when one cell in raise row has been selected and it contains analyzable players
                » should be false when one cell in raise row has been selected but it contains no analyzable players
          
        InvestigateHoleCards 
            given statistics for unraised pot CanExecute
                » should be false when no cells have been selected
                » should be true when cell in the call row has been selected and it contains analyzable players
                » should be false when cell in the call row has been selected but it contains no analyzable players
                » should be true when cell in the fold row has been selected and it contains analyzable players
                » should be false when cell in the fold row has been selected but it contains no analyzable players
                » should be true when cell in raise row has been selected and it contains analyzable players
                » should be false when cell in raise row has been selected but it contains no analyzable players
        */

        protected static Mock<IHandBrowserViewModel> _handBrowserViewModelStub;

        protected static Mock<IPreFlopRaiseReactionStatisticsViewModel> _raiseReactionStatisticsViewModelMock;

        protected static Mock<IActionSequenceStatisticsSet> _statisticsSetStub;

        protected static DetailedPreFlopStatisticsViewModelImpl _sut;

        static Mock<IActionSequenceStatistic> _callStatisticStub;

        static Mock<IActionSequenceStatistic> _foldStatisticStub;

        static Mock<IActionSequenceStatistic> _raiseStatisticStub;

        Establish context = () => {
            _raiseReactionStatisticsViewModelMock = new Mock<IPreFlopRaiseReactionStatisticsViewModel>();

            _handBrowserViewModelStub = new Mock<IHandBrowserViewModel>();

            _foldStatisticStub = new Mock<IActionSequenceStatistic>();
            _callStatisticStub = new Mock<IActionSequenceStatistic>();
            _raiseStatisticStub = new Mock<IActionSequenceStatistic>();

            _statisticsSetStub = new Mock<IActionSequenceStatisticsSet>();
            _statisticsSetStub.SetupGet(s => s.ActionSequenceStatistics).Returns(new[]
            { _foldStatisticStub.Object, _callStatisticStub.Object, _raiseStatisticStub.Object });
        };

        public class DetailedPreFlopStatisticsViewModelImpl : DetailedPreFlopStatisticsViewModel
        {
            public DetailedPreFlopStatisticsViewModelImpl(IHandBrowserViewModel handBrowserViewModel, IPreFlopRaiseReactionStatisticsViewModel raiseReactionStatisticsViewModel)
                : base(handBrowserViewModel, raiseReactionStatisticsViewModel, new StubBuilder().Out<IDetailedPreFlopStatisticsDescriber>())
            {
                SelectedAnalyzablePlayersSet = new List<IAnalyzablePokerPlayer>();
            }
            
            public List<IAnalyzablePokerPlayer> SelectedAnalyzablePlayersSet { private get; set; }

            public override IEnumerable<IAnalyzablePokerPlayer> SelectedAnalyzablePlayers
            {
                get { return SelectedAnalyzablePlayersSet; }
            }
        }

        public abstract class Ctx_DetailedPreFlopStatisticsViewModel_UnraisedPot
            : DetailedPreFlopStatisticsViewModelSpecs
        {
            Establish context_UnraisedPot = () =>
            {
                _foldStatisticStub.SetupGet(s => s.ActionSequence).Returns(ActionSequences.HeroF);
                _callStatisticStub.SetupGet(s => s.ActionSequence).Returns(ActionSequences.HeroC);
                _raiseStatisticStub.SetupGet(s => s.ActionSequence).Returns(ActionSequences.HeroR);

                _sut = new DetailedPreFlopStatisticsViewModelImpl(_handBrowserViewModelStub.Object,
                                                                  _raiseReactionStatisticsViewModelMock.Object);
                _sut.InitializeWith(_statisticsSetStub.Object);
            };
        }

        [Subject(typeof(DetailedPreFlopStatisticsViewModel), "Investigate Raise")]
        [SetupForEachSpecification]
        public class Execute
            : Ctx_DetailedPreFlopStatisticsViewModel_UnraisedPot
        {
            Establish context_InvestigateWith_Returns_Itself = () => {
                _raiseReactionStatisticsViewModelMock.Setup(
                    r => r.InitializeWith(Moq.It.IsAny<IEnumerable<IAnalyzablePokerPlayer>>(),
                                          Moq.It.IsAny<ITuple<StrategicPositions, StrategicPositions>>(),
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
                                          Moq.It.IsAny<ITuple<StrategicPositions, StrategicPositions>>(),
                                          Moq.It.IsAny<string>(),
                                          ActionSequences.HeroR,
                                          Streets.PreFlop));
        }

        [Subject(typeof(DetailedPreFlopStatisticsViewModel), "Investigate Raise")]
        [SetupForEachSpecification]
        public class given_statistics_for_raised_pot_CanExecute
            : DetailedPreFlopStatisticsViewModelSpecs
        {
            Establish context_raisedPot = () => {
                _foldStatisticStub.SetupGet(s => s.ActionSequence).Returns(ActionSequences.OppRHeroF);
                _callStatisticStub.SetupGet(s => s.ActionSequence).Returns(ActionSequences.OppRHeroC);
                _raiseStatisticStub.SetupGet(s => s.ActionSequence).Returns(ActionSequences.OppRHeroR);

                _sut = new DetailedPreFlopStatisticsViewModelImpl(_handBrowserViewModelStub.Object,
                                                                  _raiseReactionStatisticsViewModelMock.Object);
                _sut.InitializeWith(_statisticsSetStub.Object);
            };

            It should_be_false_when_no_cells_have_been_selected
                = () => _sut.InvestigateRaiseReactionCommand.CanExecute(null).ShouldBeFalse();

            It should_be_false_when_only_cells_in_the_call_row_have_been_selected_and_it_contains_analyzable_players
                = () => {
                    _sut.SelectedCells.Add(Tuple.New(1, 0));
                    _sut.SelectedAnalyzablePlayersSet = new List<IAnalyzablePokerPlayer> { new Mock<IAnalyzablePokerPlayer>().Object };
                    _sut.InvestigateRaiseReactionCommand.CanExecute(null).ShouldBeFalse();
                };

            It should_be_false_when_only_cells_in_the_fold_row_have_been_selected_and_it_contains_analyzable_players 
                = () => {
                    _sut.SelectedCells.Add(Tuple.New(0, 0));
                    _sut.SelectedAnalyzablePlayersSet = new List<IAnalyzablePokerPlayer> { new Mock<IAnalyzablePokerPlayer>().Object };
                    _sut.InvestigateRaiseReactionCommand.CanExecute(null).ShouldBeFalse();
                };

            It should_be_true_when_one_cell_in_raise_row_has_been_selected_and_it_contains_analyzable_players 
                = () => {
                    _sut.SelectedCells.Add(Tuple.New(2, 0));
                    _sut.SelectedAnalyzablePlayersSet = new List<IAnalyzablePokerPlayer> { new Mock<IAnalyzablePokerPlayer>().Object };
                    _sut.InvestigateRaiseReactionCommand.CanExecute(null).ShouldBeTrue();
                };

            It should_be_false_when_one_cell_in_raise_row_has_been_selected_but_it_contains_no_analyzable_players 
                = () => {
                    _sut.SelectedCells.Add(Tuple.New(2, 0));
                    _sut.InvestigateRaiseReactionCommand.CanExecute(null).ShouldBeFalse();
                };
        }

        [Subject(typeof(DetailedPreFlopStatisticsViewModel), "Investigate Raise")]
        [SetupForEachSpecification]
        public class given_statistics_for_unraised_pot_CanExecute
            : Ctx_DetailedPreFlopStatisticsViewModel_UnraisedPot
        {
            It should_be_false_when_no_cells_have_been_selected
                = () => _sut.InvestigateRaiseReactionCommand.CanExecute(null).ShouldBeFalse();

            It should_be_false_when_only_cells_in_the_call_row_have_been_selected_and_it_contains_analyzable_players
                = () => {
                    _sut.SelectedCells.Add(Tuple.New(1, 0));
                    _sut.SelectedAnalyzablePlayersSet = new List<IAnalyzablePokerPlayer> { new Mock<IAnalyzablePokerPlayer>().Object };
                    _sut.InvestigateRaiseReactionCommand.CanExecute(null).ShouldBeFalse();
                };

            It should_be_false_when_only_cells_in_the_fold_row_have_been_selected_and_it_contains_analyzable_players 
                = () => {
                    _sut.SelectedCells.Add(Tuple.New(0, 0));
                    _sut.SelectedAnalyzablePlayersSet = new List<IAnalyzablePokerPlayer> { new Mock<IAnalyzablePokerPlayer>().Object };
                    _sut.InvestigateRaiseReactionCommand.CanExecute(null).ShouldBeFalse();
                };

            It should_be_true_when_one_cell_in_raise_row_has_been_selected_and_it_contains_analyzable_players 
                = () => {
                    _sut.SelectedCells.Add(Tuple.New(2, 0));
                    _sut.SelectedAnalyzablePlayersSet = new List<IAnalyzablePokerPlayer> { new Mock<IAnalyzablePokerPlayer>().Object };
                    _sut.InvestigateRaiseReactionCommand.CanExecute(null).ShouldBeTrue();
                };

            It should_be_false_when_one_cell_in_raise_row_has_been_selected_but_it_contains_no_analyzable_players 
                = () => {
                    _sut.SelectedCells.Add(Tuple.New(2, 0));
                    _sut.InvestigateRaiseReactionCommand.CanExecute(null).ShouldBeFalse();
                };
        }
         
        [Subject(typeof(DetailedPreFlopStatisticsViewModel), "InvestigateHoleCards")]
        [SetupForEachSpecification]
        public class given_statistics_for_unraised_pot_InvestigateHoleCards_CanExecute
            : Ctx_DetailedPreFlopStatisticsViewModel_UnraisedPot
        {
            It should_be_false_when_no_cells_have_been_selected
                = () => _sut.InvestigateHoleCardsCommand.CanExecute(null).ShouldBeFalse();

            It should_be_true_when_cell_in_the_call_row_has_been_selected_and_it_contains_analyzable_players
                = () => {
                    _sut.SelectedCells.Add(Tuple.New(1, 0));
                    _sut.SelectedAnalyzablePlayersSet = new List<IAnalyzablePokerPlayer> { new Mock<IAnalyzablePokerPlayer>().Object };
                    _sut.InvestigateHoleCardsCommand.CanExecute(null).ShouldBeTrue();
                };

            It should_be_false_when_cell_in_the_call_row_has_been_selected_but_it_contains_no_analyzable_players
                = () => {
                    _sut.SelectedCells.Add(Tuple.New(1, 0));
                    _sut.InvestigateHoleCardsCommand.CanExecute(null).ShouldBeFalse();
                };

            It should_be_true_when_cell_in_the_fold_row_has_been_selected_and_it_contains_analyzable_players 
                = () => {
                    _sut.SelectedCells.Add(Tuple.New(0, 0));
                    _sut.SelectedAnalyzablePlayersSet = new List<IAnalyzablePokerPlayer> { new Mock<IAnalyzablePokerPlayer>().Object };
                    _sut.InvestigateHoleCardsCommand.CanExecute(null).ShouldBeTrue();
                };

            It should_be_false_when_cell_in_the_fold_row_has_been_selected_but_it_contains_no_analyzable_players 
                = () => {
                    _sut.SelectedCells.Add(Tuple.New(0, 0));
                    _sut.InvestigateHoleCardsCommand.CanExecute(null).ShouldBeFalse();
                };

            It should_be_true_when_cell_in_raise_row_has_been_selected_and_it_contains_analyzable_players 
                = () => {
                    _sut.SelectedCells.Add(Tuple.New(2, 0));
                    _sut.SelectedAnalyzablePlayersSet = new List<IAnalyzablePokerPlayer> { new Mock<IAnalyzablePokerPlayer>().Object };
                    _sut.InvestigateHoleCardsCommand.CanExecute(null).ShouldBeTrue();
                };

            It should_be_false_when_cell_in_raise_row_has_been_selected_but_it_contains_no_analyzable_players 
                = () => {
                    _sut.SelectedCells.Add(Tuple.New(2, 0));
                    _sut.InvestigateHoleCardsCommand.CanExecute(null).ShouldBeFalse();
                };
        }
    }
}