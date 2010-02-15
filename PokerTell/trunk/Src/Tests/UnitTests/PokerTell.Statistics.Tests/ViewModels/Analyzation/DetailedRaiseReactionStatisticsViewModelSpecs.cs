namespace PokerTell.Statistics.Tests.ViewModels.Analyzation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Infrastructure.Enumerations.PokerHand;
    using Infrastructure.Interfaces.PokerHand;
    using Infrastructure.Services;

    using Interfaces;

    using Machine.Specifications;

    using Moq;

    using Statistics.ViewModels.Analyzation;

    using Tools.FunctionalCSharp;
    using Tools.Interfaces;

    using It = Machine.Specifications.It;

    // ReSharper disable InconsistentNaming
    public abstract class DetailedRaiseReactionStatisticsViewModelSpecs
    {
        /* 
          *  Specification
          *  Subject is DetailedRaiseReactionStatisticsViewModel
          *  
             Initialization
                 given 0 analyzable players
                    » should throw an ArgumentException

                 given valid data
                    » should build raise reaction statistics for it
                    » should get statistics description from raise reaction describer

                 given valid data and valid RaiseReactionStatistics
                    » should create 4 rows
                    » should create one column for each item in the statistics PercentagesDictionary
                    » should create the first second and third row with percentage unit
                    » should create the fourth row without a unit since it shows the counts

                 given the PercentagesDictionary has value 0 at 0 0 and value 50 at 1 1
                    » should have value 0 at row 0 col 0
                    » should have value 50 at row 1 col 1

                 given the TotalCountsDictionary has value 0 in col 0 and value 1 in col 1
                    » should have value 0 in the counts row at col 0
                    » should have value 1 in the counts row at col 1

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
          *      
          */

        protected static Mock<IAnalyzablePokerPlayer> _analyzablePokerPlayerStub;

        protected static Dictionary<int, int> _emptyCountsDictionary;

        protected static Dictionary<int, int> _emptyPercentageDictionary;

        protected static Mock<IHandBrowserViewModel> _handBrowserViewModelMock;

        protected static string _playerName;

        protected static Constructor<IRaiseReactionAnalyzer> _raiseReactionAnalyzerMake;

        protected static Mock<IRaiseReactionAnalyzer> _raiseReactionAnalyzerStub;

        protected static Mock<IRaiseReactionDescriber<double>> _raiseReactionDescriberStub;

        protected static Mock<IRaiseReactionStatisticsBuilder> _raiseReactionStatisticsBuilderMock;

        protected static Mock<IRaiseReactionStatistics> _raiseReactionStatisticsStub;


        protected static DetailedRaiseReactionStatisticsViewModelImpl _sut;

        protected static ActionSequences _validActionSequence;

        protected static IAnalyzablePokerPlayer[] _validAnalyzablePokerPlayersStub;

        protected static ITuple<double, double> _validBetSizes;
        protected static bool _considerOpponentsRaiseSize; 

        protected static Streets _validStreet;

        Establish context = () => {
            _playerName = "somePlayer";
            _validStreet = Streets.Flop;
            _validActionSequence = ActionSequences.HeroB;
            _validBetSizes = Tuple.New(0.5, 0.7);
            _analyzablePokerPlayerStub = new Mock<IAnalyzablePokerPlayer>();
            _validAnalyzablePokerPlayersStub = new[] { _analyzablePokerPlayerStub.Object };
            _considerOpponentsRaiseSize = false;           
            _raiseReactionStatisticsStub = new Mock<IRaiseReactionStatistics>();

            _emptyPercentageDictionary = new Dictionary<int, int>();
            _emptyCountsDictionary = new Dictionary<int, int>();

            _raiseReactionStatisticsStub.SetupGet(r => r.PercentagesDictionary).Returns(
                new Dictionary<ActionTypes, IDictionary<int, int>> {
                    { ActionTypes.F, _emptyPercentageDictionary },
                    { ActionTypes.C, _emptyPercentageDictionary },
                    { ActionTypes.R, _emptyPercentageDictionary }
                });
            _raiseReactionStatisticsStub.SetupGet(r => r.TotalCountsByColumnDictionary)
                .Returns(_emptyCountsDictionary);

            _raiseReactionAnalyzerStub = new Mock<IRaiseReactionAnalyzer>();
            _raiseReactionAnalyzerMake = new Constructor<IRaiseReactionAnalyzer>(() => _raiseReactionAnalyzerStub.Object);

            _handBrowserViewModelMock = new Mock<IHandBrowserViewModel>();
           
            _raiseReactionStatisticsBuilderMock = new Mock<IRaiseReactionStatisticsBuilder>();
            _raiseReactionStatisticsBuilderMock
                .Setup(b => b.Build(_validAnalyzablePokerPlayersStub, _validActionSequence, _validStreet, _considerOpponentsRaiseSize))
                .Returns(_raiseReactionStatisticsStub.Object);
            _raiseReactionDescriberStub = new Mock<IRaiseReactionDescriber<double>>();

            _sut = new DetailedRaiseReactionStatisticsViewModelImpl(
                _handBrowserViewModelMock.Object,
                _raiseReactionStatisticsBuilderMock.Object,
                _raiseReactionDescriberStub.Object);
        };
    }

    public class DetailedRaiseReactionStatisticsViewModelImpl : DetailedRaiseReactionStatisticsViewModel<double>
    {
        public DetailedRaiseReactionStatisticsViewModelImpl(IHandBrowserViewModel handBrowserViewModel, IRaiseReactionStatisticsBuilder raiseReactionStatisticsBuilder, IRaiseReactionDescriber<double> raiseReactionDescriber)
            : base(handBrowserViewModel, raiseReactionStatisticsBuilder, raiseReactionDescriber)
        {
        }

        internal List<IAnalyzablePokerPlayer> SelectedAnalyzablePlayersSet { private get; set; }

        protected override IEnumerable<IAnalyzablePokerPlayer> SelectedAnalyzablePlayers
        {
            get
            {
                return SelectedAnalyzablePlayersSet;
            }
        }
    }

    public abstract class Ctx_DetailedRaiseReactionStatisticsViewModel_Initialized :
        DetailedRaiseReactionStatisticsViewModelSpecs
    {
        Establish context = () => _sut.InitializeWith(_validAnalyzablePokerPlayersStub,
                                      _validBetSizes,
                                      _playerName,
                                      _validActionSequence,
                                      _validStreet);
    }

    [Subject(typeof(DetailedRaiseReactionStatisticsViewModel<double>), "Initialization")]
    public class given_0_analyzable_players : DetailedRaiseReactionStatisticsViewModelSpecs
    {
        static Exception exception;

        Because of = () => exception = Catch.Exception(
                                           () => _sut.InitializeWith(Enumerable.Empty<IAnalyzablePokerPlayer>(),
                                                     _validBetSizes,
                                                     _playerName,
                                                     _validActionSequence,
                                                     _validStreet));

        It should_throw_an_ArgumentException
            = () => exception.ShouldBeOfType<ArgumentException>();
    }

    [Subject(typeof(DetailedRaiseReactionStatisticsViewModel<double>), "Initialization")]
    public class given_valid_data : DetailedRaiseReactionStatisticsViewModelSpecs
    {
        static string description;
        
        Establish context = () => {
            description = "some description";
            _raiseReactionDescriberStub.Setup(
                d => d.Describe(_playerName, _validAnalyzablePokerPlayersStub.First(), _validStreet, _validBetSizes))
                .Returns(description);
        };
        
        Because of = () => _sut.InitializeWith(_validAnalyzablePokerPlayersStub,
                               _validBetSizes,
                               _playerName,
                               _validActionSequence,
                               _validStreet);

        It should_build_raise_reaction_statistics_for_it
            = () => _raiseReactionStatisticsBuilderMock.Verify(b => b.Build(_validAnalyzablePokerPlayersStub, _validActionSequence, _validStreet, _considerOpponentsRaiseSize));

        It should_get_statistics_description_from_raise_reaction_describer
            = () => _sut.StatisticsDescription.ShouldEqual(description);
    }

    [Subject(typeof(DetailedRaiseReactionStatisticsViewModel<double>), "Initialization")]
    public class given_valid_data_and_valid_RaiseReactionStatistics : DetailedRaiseReactionStatisticsViewModelSpecs
    {
        Establish context = () => {
            _emptyPercentageDictionary.Add(1, 0);
            _emptyPercentageDictionary.Add(2, 0);

            _raiseReactionStatisticsStub.SetupGet(r => r.PercentagesDictionary).Returns(
                new Dictionary<ActionTypes, IDictionary<int, int>> {
                    { ActionTypes.F, _emptyPercentageDictionary },
                    { ActionTypes.C, _emptyPercentageDictionary },
                    { ActionTypes.R, _emptyPercentageDictionary }
                });
        };

        Because of = () => _sut.InitializeWith(_validAnalyzablePokerPlayersStub,
                               _validBetSizes,
                               _playerName,
                               _validActionSequence,
                               _validStreet);

        It should_create_4_rows
            = () => _sut.Rows.Count().ShouldEqual(4);

        It should_create_one_column_for_each_item_in_the_statistics_PercentagesDictionary
            = () => _sut.Rows.First().Cells.Count.ShouldEqual(_emptyPercentageDictionary.Count);

        It should_create_the_first_second_and_third_row_with_percentage_unit
            = () => {
                _sut.Rows.First().Unit.ShouldEqual("%");
                _sut.Rows.ElementAt(1).Unit.ShouldEqual("%");
                _sut.Rows.ElementAt(2).Unit.ShouldEqual("%");
            };

        It should_create_the_fourth_row_without_a_unit_since_it_shows_the_counts
            = () => _sut.Rows.Last().Unit.ShouldBeEmpty();
    }


    [Subject(typeof(DetailedRaiseReactionStatisticsViewModel<double>), "Initialization")]
    public class given_the_PercentagesDictionary_has_value_0_at_0_0_and_value_50_at_1_1
        : DetailedRaiseReactionStatisticsViewModelSpecs
    {
        Establish context = () => _raiseReactionStatisticsStub.SetupGet(r => r.PercentagesDictionary).Returns(
                                      new Dictionary<ActionTypes, IDictionary<int, int>> {
                                          { ActionTypes.F, new Dictionary<int, int> { { 1, 0 }, { 2, 0 } } },
                                          { ActionTypes.C, new Dictionary<int, int> { { 1, 0 }, { 2, 50 } } },
                                          { ActionTypes.R, _emptyPercentageDictionary }
                                      });

        Because of = () => _sut.InitializeWith(_validAnalyzablePokerPlayersStub,
                               _validBetSizes,
                               _playerName,
                               _validActionSequence,
                               _validStreet);

        It should_have_value_0_at_row_0_col_0
            = () => _sut.Rows.ElementAt(0).Cells[0].Value.ShouldEqual("0");

        It should_have_value_50_at_row_1_col_1
            = () => _sut.Rows.ElementAt(1).Cells[1].Value.ShouldEqual("50");
    }

    [Subject(typeof(DetailedRaiseReactionStatisticsViewModel<double>), "Initialization")]
    public class given_the_TotalCountsDictionary_has_value_0_in_col_0_and_value_1_in_col_1
        : DetailedRaiseReactionStatisticsViewModelSpecs
    {
        Establish context = () => _raiseReactionStatisticsStub.SetupGet(r => r.TotalCountsByColumnDictionary).Returns(
                                      new Dictionary<int, int> {
                                          { 1, 0 },
                                          { 2, 1 }
                                      });

        Because of = () => _sut.InitializeWith(_validAnalyzablePokerPlayersStub,
                               _validBetSizes,
                               _playerName,
                               _validActionSequence,
                               _validStreet);

        It should_have_value_0_in_the_counts_row_at_col_0
            = () => _sut.Rows.Last().Cells[0].Value.ShouldEqual("0");

        It should_have_value_1_in_the_counts_row_at_col_1
            = () => _sut.Rows.Last().Cells[1].Value.ShouldEqual("1");
    }

    [Subject(typeof(DetailedRaiseReactionStatisticsViewModel<double>), "Browse hands command")]
    public class given_no_cell_has_been_selected
        : Ctx_DetailedRaiseReactionStatisticsViewModel_Initialized
    {
        It should_not_be_executable
            = () => _sut.BrowseHandsCommand.CanExecute(null).ShouldBeFalse();
    }

    [Subject(typeof(DetailedRaiseReactionStatisticsViewModel<double>), "Browse hands command")]
    public class given_one_cell_has_been_selected_but_it_contains_no_analyzable_players : Ctx_DetailedRaiseReactionStatisticsViewModel_Initialized
    {
        Establish context = () =>
        {
            _sut.AddToSelection(0, 0);
            _sut.SelectedAnalyzablePlayersSet = new List<IAnalyzablePokerPlayer>();
        };

        It should_not_be_executable = () => _sut.BrowseHandsCommand.CanExecute(null).ShouldBeFalse();
    }

    [Subject(typeof(DetailedRaiseReactionStatisticsViewModel<double>), "Browse hands command")]
    public class given_one_cell_is_selected_and_it_contains_analyzable_players : Ctx_DetailedRaiseReactionStatisticsViewModel_Initialized
    {
        Establish context = () =>
        {
            _sut.AddToSelection(0, 0);
            _sut.SelectedAnalyzablePlayersSet = _validAnalyzablePokerPlayersStub.ToList(); 
        };

        It should_be_executable
            = () => _sut.BrowseHandsCommand.CanExecute(null).ShouldBeTrue();
    }

    [Subject(typeof(DetailedRaiseReactionStatisticsViewModel<double>), "Browse hands command")]
    public class given_one_cell_is_selected_and_the_user_executes_the_command :
        Ctx_DetailedRaiseReactionStatisticsViewModel_Initialized
    {
        Establish context = () =>
        {
            _sut.AddToSelection(0, 0);
            _sut.SelectedAnalyzablePlayersSet = _validAnalyzablePokerPlayersStub.ToList();
        };

        Because of = () => _sut.BrowseHandsCommand.Execute(null);

        It should_initialize_HandBrowserViewModel_with_the_analyzable_players_who_correspond_to_the_cell
            = () => _handBrowserViewModelMock.Verify(hb => hb.InitializeWith(_validAnalyzablePokerPlayersStub));

        It should_set_its_ChildViewModel_to_the_HandBrowserViewModel
            = () => _sut.ChildViewModel.ShouldEqual(_handBrowserViewModelMock.Object);
    }

    // ReSharper restore InconsistentNaming
}