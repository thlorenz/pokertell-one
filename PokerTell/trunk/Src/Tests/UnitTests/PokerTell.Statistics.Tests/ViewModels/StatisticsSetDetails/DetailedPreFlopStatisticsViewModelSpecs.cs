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
                » should be false when cell in the fold row has been selected and it contains analyzable players
                » should be true when cell in raise row has been selected and it contains analyzable players
                » should be false when cell in raise row has been selected but it contains no analyzable players
          
            when given ActionSequence HeroC Executing InvestigateHoleCards
                » should initialize unraised pot calling statistics viewmodel with selected analyzable players and selected actionsequence
                » should assign the unraised pot statistics viewmodel to the child viewmodel

            when given ActionSequence HeroR Executing InvestigateHoleCards
                » should initialize raising statistics viewmodel with selected analyzable players and selected actionsequence
                » should assign the raising statistics viewmodel to the child viewmodel

            when given ActionSequence OppRHeroR Executing InvestigateHoleCards
                » should initialize raising statistics viewmodel with selected analyzable players and selected actionsequence
                » should assign the raising statistics viewmodel to the child viewmodel

            when given ActionSequence OppRHeroC Executing InvestigateHoleCards
                » should initialize raised pot calling statistics viewmodel with selected analyzable players and selected actionsequence
                » should assign the raised pot statistics viewmodel to the child viewmodel
        */
        protected static Mock<IHandBrowserViewModel> _handBrowserViewModelStub;

        protected static Mock<IPreFlopRaiseReactionStatisticsViewModel> _raiseReactionStatisticsViewModelMock;

        protected static Mock<IActionSequenceStatisticsSet> _statisticsSetStub;

        protected static DetailedPreFlopStatisticsViewModelImpl _sut;

        protected static Mock<IPreFlopRaisingHandStrengthStatisticsViewModel> _preFlopRaisingHandStrengthStatisticsViewModelMock;

        protected static Mock<IPreFlopRaisedPotCallingHandStrengthStatisticsViewModel> _preFlopRaisedPotCallingHandStrengthStatisticsViewModelMock;

        protected static Mock<IPreFlopUnraisedPotCallingHandStrengthStatisticsViewModel> _preFlopUnraisedPotCallingHandStrengthStatisticsViewModelMock;

        static Mock<IActionSequenceStatistic> _callStatisticStub;

        static Mock<IActionSequenceStatistic> _foldStatisticStub;

        static Mock<IActionSequenceStatistic> _raiseStatisticStub;
        const string SomePlayerName = "somePlayer";

        Establish spec_Context = () => {
            _raiseReactionStatisticsViewModelMock = new Mock<IPreFlopRaiseReactionStatisticsViewModel>();

            _handBrowserViewModelStub = new Mock<IHandBrowserViewModel>();

            _preFlopUnraisedPotCallingHandStrengthStatisticsViewModelMock =
                new Mock<IPreFlopUnraisedPotCallingHandStrengthStatisticsViewModel>();
            _preFlopRaisedPotCallingHandStrengthStatisticsViewModelMock =
                new Mock<IPreFlopRaisedPotCallingHandStrengthStatisticsViewModel>();
            _preFlopRaisingHandStrengthStatisticsViewModelMock = new Mock<IPreFlopRaisingHandStrengthStatisticsViewModel>();

            _foldStatisticStub = new Mock<IActionSequenceStatistic>();
            _callStatisticStub = new Mock<IActionSequenceStatistic>();
            _raiseStatisticStub = new Mock<IActionSequenceStatistic>();

            _statisticsSetStub = new Mock<IActionSequenceStatisticsSet>();

            _statisticsSetStub.SetupGet(s => s.ActionSequenceStatistics).Returns(new[] {
                    _foldStatisticStub.Object, _callStatisticStub.Object, _raiseStatisticStub.Object
                });
            _statisticsSetStub.SetupGet(s => s.PlayerName).Returns(SomePlayerName);

            _sut = new DetailedPreFlopStatisticsViewModelImpl(
                _handBrowserViewModelStub.Object, 
                _preFlopUnraisedPotCallingHandStrengthStatisticsViewModelMock.Object, 
                _preFlopRaisedPotCallingHandStrengthStatisticsViewModelMock.Object, 
                _preFlopRaisingHandStrengthStatisticsViewModelMock.Object, 
                _raiseReactionStatisticsViewModelMock.Object);
        };

        public class DetailedPreFlopStatisticsViewModelImpl : DetailedPreFlopStatisticsViewModel
        {
            public DetailedPreFlopStatisticsViewModelImpl(
                IHandBrowserViewModel handBrowserViewModel, 
                IPreFlopUnraisedPotCallingHandStrengthStatisticsViewModel preFlopUnraisedPotCallingHandStrengthStatisticsViewModel, 
                IPreFlopRaisedPotCallingHandStrengthStatisticsViewModel preFlopRaisedPotCallingHandStrengthStatisticsViewModel, 
                IPreFlopRaisingHandStrengthStatisticsViewModel preFlopRaisingHandStrengthStatisticsViewModel, 
                IPreFlopRaiseReactionStatisticsViewModel raiseReactionStatisticsViewModel)
                : base(
                    handBrowserViewModel, 
                    preFlopUnraisedPotCallingHandStrengthStatisticsViewModel, 
                    preFlopRaisedPotCallingHandStrengthStatisticsViewModel, 
                    preFlopRaisingHandStrengthStatisticsViewModel, 
                    raiseReactionStatisticsViewModel, 
                    new StubBuilder().Out<IDetailedPreFlopStatisticsDescriber>())
            {
                SelectedAnalyzablePlayersSet = new List<IAnalyzablePokerPlayer>();
                SelectedActionSequenceSet = ActionSequences.NonStandard;
            }

            public List<IAnalyzablePokerPlayer> SelectedAnalyzablePlayersSet { private get; set; }

            public override IEnumerable<IAnalyzablePokerPlayer> SelectedAnalyzablePlayers
            {
                get { return SelectedAnalyzablePlayersSet; }
            }


            public ActionSequences SelectedActionSequenceSet { private get; set; }

            // only override when it was set to something during spec setup
            public override ActionSequences SelectedActionSequence
            {
                get { return SelectedActionSequenceSet != ActionSequences.NonStandard ? SelectedActionSequenceSet : base.SelectedActionSequence; }
            }
        }

        public abstract class Ctx_DetailedPreFlopStatisticsViewModel_UnraisedPot
            : DetailedPreFlopStatisticsViewModelSpecs
        {
            Establish context_UnraisedPot = () => {
                _foldStatisticStub.SetupGet(s => s.ActionSequence).Returns(ActionSequences.HeroF);
                _callStatisticStub.SetupGet(s => s.ActionSequence).Returns(ActionSequences.HeroC);
                _raiseStatisticStub.SetupGet(s => s.ActionSequence).Returns(ActionSequences.HeroR);

                _sut.InitializeWith(_statisticsSetStub.Object);
            };
        }

        public abstract class Ctx_DetailedPreFlopStatisticsViewModel_Initialized
            : DetailedPreFlopStatisticsViewModelSpecs
        {
            Establish context_Initialized = () => _sut.InitializeWith(_statisticsSetStub.Object);
        }

        [Subject(typeof(DetailedPreFlopStatisticsViewModel), "Investigate Raise")]
        [SetupForEachSpecification]
        public class InvestigateRaise_Execute : Ctx_DetailedPreFlopStatisticsViewModel_UnraisedPot
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
        public class given_statistics_for_raised_pot_InvestigateRaise_CanExecute
            : DetailedPreFlopStatisticsViewModelSpecs
        {
            Establish context_raisedPot = () => {
                _foldStatisticStub.SetupGet(s => s.ActionSequence).Returns(ActionSequences.OppRHeroF);
                _callStatisticStub.SetupGet(s => s.ActionSequence).Returns(ActionSequences.OppRHeroC);
                _raiseStatisticStub.SetupGet(s => s.ActionSequence).Returns(ActionSequences.OppRHeroR);

                _sut.InitializeWith(_statisticsSetStub.Object);
            };

            It should_be_false_when_no_cells_have_been_selected
                = () => _sut.InvestigateRaiseReactionCommand.CanExecute(null).ShouldBeFalse();

            It should_be_false_when_only_cells_in_the_call_row_have_been_selected_and_it_contains_analyzable_players
                = () => {
                    _sut.SelectedCells.Add(Tuple.New(1, 0));
                    _sut.SelectedAnalyzablePlayersSet = new List<IAnalyzablePokerPlayer>
                        {
                            new Mock<IAnalyzablePokerPlayer>().Object
                        };
                    _sut.InvestigateRaiseReactionCommand.CanExecute(null).ShouldBeFalse();
                };

            It should_be_false_when_only_cells_in_the_fold_row_have_been_selected_and_it_contains_analyzable_players
                = () => {
                    _sut.SelectedCells.Add(Tuple.New(0, 0));
                    _sut.SelectedAnalyzablePlayersSet = new List<IAnalyzablePokerPlayer>
                        {
                            new Mock<IAnalyzablePokerPlayer>().Object
                        };
                    _sut.InvestigateRaiseReactionCommand.CanExecute(null).ShouldBeFalse();
                };

            It should_be_true_when_one_cell_in_raise_row_has_been_selected_and_it_contains_analyzable_players
                = () => {
                    _sut.SelectedCells.Add(Tuple.New(2, 0));
                    _sut.SelectedAnalyzablePlayersSet = new List<IAnalyzablePokerPlayer>
                        {
                            new Mock<IAnalyzablePokerPlayer>().Object 
                        };

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
        public class given_statistics_for_unraised_pot_InvestigateRaise_CanExecute
            : Ctx_DetailedPreFlopStatisticsViewModel_UnraisedPot
        {
            It should_be_false_when_no_cells_have_been_selected
                = () => _sut.InvestigateRaiseReactionCommand.CanExecute(null).ShouldBeFalse();

            It should_be_false_when_only_cells_in_the_call_row_have_been_selected_and_it_contains_analyzable_players
                = () => {
                    _sut.SelectedCells.Add(Tuple.New(1, 0));
                    _sut.SelectedAnalyzablePlayersSet = new List<IAnalyzablePokerPlayer>
                        {
                            new Mock<IAnalyzablePokerPlayer>().Object
                        };
                    _sut.InvestigateRaiseReactionCommand.CanExecute(null).ShouldBeFalse();
                };

            It should_be_false_when_only_cells_in_the_fold_row_have_been_selected_and_it_contains_analyzable_players
                = () => {
                    _sut.SelectedCells.Add(Tuple.New(0, 0));
                    _sut.SelectedAnalyzablePlayersSet = new List<IAnalyzablePokerPlayer>
                        {
                            new Mock<IAnalyzablePokerPlayer>().Object
                        };
                    _sut.InvestigateRaiseReactionCommand.CanExecute(null).ShouldBeFalse();
                };

            It should_be_true_when_one_cell_in_raise_row_has_been_selected_and_it_contains_analyzable_players
                = () => {
                    _sut.SelectedCells.Add(Tuple.New(2, 0));
                    _sut.SelectedAnalyzablePlayersSet = new List<IAnalyzablePokerPlayer>
                        {
                            new Mock<IAnalyzablePokerPlayer>().Object
                        };
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
                    _sut.SelectedAnalyzablePlayersSet = new List<IAnalyzablePokerPlayer>
                        {
                            new Mock<IAnalyzablePokerPlayer>().Object
                        };
                    _sut.InvestigateHoleCardsCommand.CanExecute(null).ShouldBeTrue();
                };

            It should_be_false_when_cell_in_the_call_row_has_been_selected_but_it_contains_no_analyzable_players
                = () => {
                    _sut.SelectedCells.Add(Tuple.New(1, 0));
                    _sut.InvestigateHoleCardsCommand.CanExecute(null).ShouldBeFalse();
                };

            It should_be_false_when_cell_in_the_fold_row_has_been_selected_and_it_contains_analyzable_players = () => {
                    _sut.SelectedCells.Add(Tuple.New(0, 0));
                    _sut.SelectedAnalyzablePlayersSet = new List<IAnalyzablePokerPlayer>
                        {
                            new Mock<IAnalyzablePokerPlayer>().Object
                        };
                    _sut.InvestigateHoleCardsCommand.CanExecute(null).ShouldBeFalse();
                };

            It should_be_true_when_cell_in_raise_row_has_been_selected_and_it_contains_analyzable_players
                = () => {
                    _sut.SelectedCells.Add(Tuple.New(2, 0));
                    _sut.SelectedAnalyzablePlayersSet = new List<IAnalyzablePokerPlayer>
                        {
                            new Mock<IAnalyzablePokerPlayer>().Object
                        };
                    _sut.InvestigateHoleCardsCommand.CanExecute(null).ShouldBeTrue();
                };

            It should_be_false_when_cell_in_raise_row_has_been_selected_but_it_contains_no_analyzable_players
                = () => {
                    _sut.SelectedCells.Add(Tuple.New(2, 0));
                    _sut.InvestigateHoleCardsCommand.CanExecute(null).ShouldBeFalse();
                };
        }

        [Subject(typeof(DetailedPreFlopStatisticsViewModel), "InvestigateHoleCards")]
        public class when_given_ActionSequence_HeroC_Executing_InvestigateHoleCards :
            Ctx_DetailedPreFlopStatisticsViewModel_Initialized
        {
            Establish context = () => {
                _sut.SelectedActionSequenceSet = ActionSequences.HeroC;
                _sut.SelectedAnalyzablePlayersSet = new List<IAnalyzablePokerPlayer> { new Mock<IAnalyzablePokerPlayer>().Object };
            };

            Because of = () => _sut.InvestigateHoleCardsCommand.Execute(null);

            It should_initialize_unraised_pot_calling_statistics_viewmodel_with_selected_analyzable_players_and_selected_actionsequence
                = () => _preFlopUnraisedPotCallingHandStrengthStatisticsViewModelMock.Verify(
                            m => m.InitializeWith(_sut.SelectedAnalyzablePlayers, SomePlayerName, _sut.SelectedActionSequence));

            It should_assign_the_unraised_pot_statistics_viewmodel_to_the_child_viewmodel
                = () => _sut.ChildViewModel.ShouldBeTheSameAs(_preFlopUnraisedPotCallingHandStrengthStatisticsViewModelMock.Object);
        }

        [Subject(typeof(DetailedPreFlopStatisticsViewModel), "InvestigateHoleCards")]
        public class when_given_ActionSequence_HeroR_Executing_InvestigateHoleCards :
            Ctx_DetailedPreFlopStatisticsViewModel_Initialized
        {
            Establish context = () => {
                _sut.SelectedActionSequenceSet = ActionSequences.HeroR;
                _sut.SelectedAnalyzablePlayersSet = new List<IAnalyzablePokerPlayer> { new Mock<IAnalyzablePokerPlayer>().Object };
            };

            Because of = () => _sut.InvestigateHoleCardsCommand.Execute(null);

            It should_initialize_raising_statistics_viewmodel_with_selected_analyzable_players_and_selected_actionsequence
                = () => _preFlopRaisingHandStrengthStatisticsViewModelMock.Verify(
                        m => m.InitializeWith(_sut.SelectedAnalyzablePlayers, SomePlayerName, _sut.SelectedActionSequence));

            It should_assign_the_raising_statistics_viewmodel_to_the_child_viewmodel
                = () => _sut.ChildViewModel.ShouldBeTheSameAs(_preFlopRaisingHandStrengthStatisticsViewModelMock.Object);
        }
        
        [Subject(typeof(DetailedPreFlopStatisticsViewModel), "InvestigateHoleCards")]
        public class when_given_ActionSequence_OppRHeroR_Executing_InvestigateHoleCards :
            Ctx_DetailedPreFlopStatisticsViewModel_Initialized
        {
            Establish context = () => {
                _sut.SelectedActionSequenceSet = ActionSequences.OppRHeroR;
                _sut.SelectedAnalyzablePlayersSet = new List<IAnalyzablePokerPlayer> { new Mock<IAnalyzablePokerPlayer>().Object };
            };

            Because of = () => _sut.InvestigateHoleCardsCommand.Execute(null);

            It should_initialize_raising_statistics_viewmodel_with_selected_analyzable_players_and_selected_actionsequence
                = () => _preFlopRaisingHandStrengthStatisticsViewModelMock.Verify(
                        m => m.InitializeWith(_sut.SelectedAnalyzablePlayers, SomePlayerName, _sut.SelectedActionSequence));
            
            It should_assign_the_raising_statistics_viewmodel_to_the_child_viewmodel
                = () => _sut.ChildViewModel.ShouldBeTheSameAs(_preFlopRaisingHandStrengthStatisticsViewModelMock.Object);
        }

        [Subject(typeof(DetailedPreFlopStatisticsViewModel), "InvestigateHoleCards")]
        public class when_given_ActionSequence_OppRHeroC_Executing_InvestigateHoleCards :
            Ctx_DetailedPreFlopStatisticsViewModel_Initialized
        {
            Establish context = () => {
                _sut.SelectedActionSequenceSet = ActionSequences.OppRHeroC;
                _sut.SelectedAnalyzablePlayersSet = new List<IAnalyzablePokerPlayer> { new Mock<IAnalyzablePokerPlayer>().Object };
            };

            Because of = () => _sut.InvestigateHoleCardsCommand.Execute(null);

            It should_initialize_raised_pot_calling_statistics_viewmodel_with_selected_analyzable_players_and_selected_actionsequence
                = () => _preFlopRaisedPotCallingHandStrengthStatisticsViewModelMock.Verify(
                        m => m.InitializeWith(_sut.SelectedAnalyzablePlayers, SomePlayerName, _sut.SelectedActionSequence));

            It should_assign_the_raised_pot_statistics_viewmodel_to_the_child_viewmodel
                = () => _sut.ChildViewModel.ShouldBeTheSameAs(_preFlopRaisedPotCallingHandStrengthStatisticsViewModelMock.Object);
        }
    }
}