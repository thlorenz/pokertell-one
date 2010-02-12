namespace PokerTell.Statistics.Tests.ViewModels.StatisticsSetDetails
{
    using System.Collections.Generic;
    using System.Linq;

    using Machine.Specifications;

    using Moq;

    using PokerTell.Infrastructure.Interfaces.PokerHand;
    using PokerTell.Infrastructure.Interfaces.Statistics;
    using PokerTell.Statistics.Interfaces;
    using PokerTell.Statistics.ViewModels.StatisticsSetDetails;

    using Tools.Interfaces;

    using It = Machine.Specifications.It;

    // Resharper disable InconsistentNaming
    public abstract class DetailedStatisticsViewModelSpecs
    {
        /*
         * 
            Browse hands command
                given no cell has been selected
                    ¯ should not be executable

                given one cell has been selected but it contains no analyzable players
                    ¯ should not be executable

                given one cell is selected and it contains analyzable players
                    ¯ should be executable

                given one cell is selected and the user executes the command
                    ¯ should initialize HandBrowserViewModel with the analyzable players who correspond to the cell
                    ¯ should set its ChildViewModel to the HandBrowserViewModel
         */
        protected static DetailedStatisticsViewModelImpl _sut;

        protected static IAnalyzablePokerPlayer[] _validAnalyzablePokerPlayersStub;

        protected static Mock<IHandBrowserViewModel> _handBrowserViewModelMock;

        protected static Mock<IAnalyzablePokerPlayer> _analyzablePokerPlayerStub;

        Establish mainContext = () => {
            _analyzablePokerPlayerStub = new Mock<IAnalyzablePokerPlayer>();
            _validAnalyzablePokerPlayersStub = new[] { _analyzablePokerPlayerStub.Object };
            _handBrowserViewModelMock = new Mock<IHandBrowserViewModel>();

            _sut = new DetailedStatisticsViewModelImpl(_handBrowserViewModelMock.Object);
        };

        protected class DetailedStatisticsViewModelImpl : DetailedStatisticsViewModel
        {
            public DetailedStatisticsViewModelImpl(IHandBrowserViewModel handBrowserViewModel)
                : base(handBrowserViewModel, "columnHeaderTitle")
            {
            }

            public ITuple<int, int> SelectedColumnsSpanGet
            {
                get { return SelectedColumnsSpan; }
            }

            internal List<IAnalyzablePokerPlayer> SelectedAnalyzablePlayersSet { private get; set; }

            public override IEnumerable<IAnalyzablePokerPlayer> SelectedAnalyzablePlayers
            {
                get { return SelectedAnalyzablePlayersSet; }
            }

            protected override IDetailedStatisticsViewModel CreateTableAndDescriptionFor(
                IActionSequenceStatisticsSet statisticsSet)
            {
                return this;
            }
        }

        [Subject(typeof(DetailedStatisticsViewModel), "Browse hands command")]
        public class given_no_cell_has_been_selected : DetailedStatisticsViewModelSpecs
        {
            It should_not_be_executable
                = () => _sut.BrowseHandsCommand.CanExecute(null).ShouldBeFalse();
        }

        [Subject(typeof(DetailedStatisticsViewModel), "Browse hands command")]
        public class given_one_cell_has_been_selected_but_it_contains_no_analyzable_players : DetailedStatisticsViewModelSpecs
        {
            Establish context = () => {
                _sut.AddToSelection(0, 0);
                _sut.SelectedAnalyzablePlayersSet = new List<IAnalyzablePokerPlayer>();
            };

            It should_not_be_executable = () => _sut.BrowseHandsCommand.CanExecute(null).ShouldBeFalse();
        }

        [Subject(typeof(DetailedStatisticsViewModel), "Browse hands command")]
        public class given_one_cell_is_selected_and_it_contains_analyzable_players : DetailedStatisticsViewModelSpecs
        {
            Establish context = () => {
                _sut.AddToSelection(0, 0);
                _sut.SelectedAnalyzablePlayersSet = _validAnalyzablePokerPlayersStub.ToList();
            };

            It should_be_executable
                = () => _sut.BrowseHandsCommand.CanExecute(null).ShouldBeTrue();
        }

        [Subject(typeof(DetailedStatisticsViewModel), "Browse hands command")]
        public class given_one_cell_is_selected_and_the_user_executes_the_command : DetailedStatisticsViewModelSpecs
        {
            Establish context = () => {
                _sut.AddToSelection(0, 0);
                _sut.SelectedAnalyzablePlayersSet = _validAnalyzablePokerPlayersStub.ToList();
            };

            Because of = () => _sut.BrowseHandsCommand.Execute(null);

            It should_initialize_HandBrowserViewModel_with_the_analyzable_players_who_correspond_to_the_cell
                = () => _handBrowserViewModelMock.Verify(hb => hb.InitializeWith(_validAnalyzablePokerPlayersStub));

            It should_set_its_ChildViewModel_to_the_HandBrowserViewModel
                = () => _sut.ChildViewModel.ShouldEqual(_handBrowserViewModelMock.Object);
        }
    }
}