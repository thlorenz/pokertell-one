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
    public abstract class DetailedPostFlopHeroActsStatisticsViewModelSpecs
    {
        /*
         * Specifications
         * Subject: DetailedPostFlopHeroActsStatisticsViewModel
         * 
         * Investigate Raise
         *      
                CanExecute
                    » should be false when no cells have been selected
                    » should be false when one cell in betting column has been selected but it contains no analyzable players
                    » should be true when one cell in betting column has been selected and it contains analyzable players

                Execute
                    » should assign the raise reaction statistics model to its child view model
                    » should initialize the raise reaction statistics model with the data of the selected cells
         */
        protected static Mock<IHandBrowserViewModel> _handBrowserViewModelStub;

        protected static Mock<IPostFlopHeroActsRaiseReactionStatisticsViewModel> _raiseReactionStatisticsViewModelMock;

        protected static Mock<IActionSequenceStatisticsSet> _statisticsSetStub;

        protected static DetailedPostFlopHeroActsStatisticsViewModelImpl _sut;

        static Mock<IActionSequenceStatistic> _actionSequenceStatisticStub;

        Establish basicContext = () => {
            _raiseReactionStatisticsViewModelMock = new Mock<IPostFlopHeroActsRaiseReactionStatisticsViewModel>();

            _handBrowserViewModelStub = new Mock<IHandBrowserViewModel>();

            _actionSequenceStatisticStub = new Mock<IActionSequenceStatistic>();
            _actionSequenceStatisticStub.SetupGet(s => s.ActionSequence).Returns(ActionSequences.HeroB);
            _statisticsSetStub = new Mock<IActionSequenceStatisticsSet>();
            _statisticsSetStub.SetupGet(s => s.ActionSequenceStatistics).Returns(new[] { _actionSequenceStatisticStub.Object });

            _sut = new DetailedPostFlopHeroActsStatisticsViewModelImpl(_handBrowserViewModelStub.Object, 
                                                                   _raiseReactionStatisticsViewModelMock.Object);
            _sut.InitializeWith(_statisticsSetStub.Object);
        };

        public class DetailedPostFlopHeroActsStatisticsViewModelImpl : DetailedPostFlopHeroActsStatisticsViewModel
        {
            public DetailedPostFlopHeroActsStatisticsViewModelImpl(
                IHandBrowserViewModel handBrowserViewModel, 
                IPostFlopHeroActsRaiseReactionStatisticsViewModel raiseReactionStatisticsViewModel)
                : base(handBrowserViewModel, raiseReactionStatisticsViewModel, new StubBuilder().Out<IDetailedPostFlopHeroActsStatisticsDescriber>())
            {
                    SelectedAnalyzablePlayersSet = new List<IAnalyzablePokerPlayer>();
            }

            public List<IAnalyzablePokerPlayer> SelectedAnalyzablePlayersSet { private get; set; }

            public override IEnumerable<IAnalyzablePokerPlayer> SelectedAnalyzablePlayers
            {
                get { return SelectedAnalyzablePlayersSet; }
            }
        }

        [SetupForEachSpecification]
        [Subject(typeof(DetailedPostFlopHeroActsStatisticsViewModel), "Investigate Raise")]
        public class CanExecute : DetailedPostFlopHeroActsStatisticsViewModelSpecs
        {
            It should_be_false_when_no_cells_have_been_selected
                = () => _sut.InvestigateRaiseReactionCommand.CanExecute(null).ShouldBeFalse();

            It should_be_false_when_one_cell_in_betting_column_has_been_selected_but_it_contains_no_analyzable_players
                = () => {
                    _sut.SelectedCells.Add(Tuple.New(0, 0));
                    _sut.InvestigateRaiseReactionCommand.CanExecute(null).ShouldBeFalse();
                };

            It should_be_true_when_one_cell_in_betting_column_has_been_selected_and_it_contains_analyzable_players
                = () => {
                    _sut.SelectedCells.Add(Tuple.New(0, 0));
                    _sut.SelectedAnalyzablePlayersSet = new List<IAnalyzablePokerPlayer> { new Mock<IAnalyzablePokerPlayer>().Object };
                    _sut.InvestigateRaiseReactionCommand.CanExecute(null).ShouldBeTrue();
                };
        }

        [Subject(typeof(DetailedPostFlopHeroActsStatisticsViewModel), "Investigate Raise")]
        public class Execute
            : DetailedPostFlopHeroActsStatisticsViewModelSpecs
        {
            Establish context_InvestigateWith_Returns_Itself = () => {
                _raiseReactionStatisticsViewModelMock.Setup(
                    r => r.InitializeWith(Moq.It.IsAny<IEnumerable<IAnalyzablePokerPlayer>>(), 
                                          Moq.It.IsAny<ITuple<double, double>>(), 
                                          Moq.It.IsAny<string>(), 
                                          Moq.It.IsAny<ActionSequences>(), 
                                          Moq.It.IsAny<Streets>()))
                    .Returns(_raiseReactionStatisticsViewModelMock.Object);

                _sut.SelectedCells.Add(Tuple.New(0, 0));
            };

            Because of = () => _sut.InvestigateRaiseReactionCommand.Execute(null);

            It should_assign_the_raise_reaction_statistics_model_to_its_child_view_model
                = () => _sut.ChildViewModel.ShouldBeTheSameAs(_raiseReactionStatisticsViewModelMock.Object);

            It should_initialize_the_raise_reaction_statistics_model_with_the_data_of_the_selected_cells
                = () => _raiseReactionStatisticsViewModelMock.Verify(
                            r => r.InitializeWith(Moq.It.IsAny<IEnumerable<IAnalyzablePokerPlayer>>(), 
                                                  Moq.It.IsAny<ITuple<double, double>>(), 
                                                  Moq.It.IsAny<string>(), 
                                                  ActionSequences.HeroB, 
                                                  Moq.It.IsAny<Streets>()));
        }
    }
}