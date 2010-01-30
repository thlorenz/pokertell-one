namespace PokerTell.Statistics.Tests.ViewModels.Analyzation
{
    // ReSharper disable InconsistentNaming
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Infrastructure.Enumerations.PokerHand;
    using Infrastructure.Interfaces.PokerHand;
    using Infrastructure.Interfaces.Statistics;
    using Infrastructure.Services;

    using Interfaces;

    using Machine.Specifications;

    using Moq;

    using Statistics.ViewModels.Analyzation;

    using Tools.FunctionalCSharp;
    using Tools.Interfaces;

    using It = Machine.Specifications.It;

    public abstract class DetailedRaiseReactionStatisticsViewModelSpecs
    {
        /* 
         *  Specification
         *  Subject is DetailedRaiseReactionStatisticsViewModel
         *  
         *  Constructor
         *  
         *      * given 0 analyzable players
         *          it should throw an ArgumentException
         *      
         *      * given valid data
         *          it should contain player name in description
         *          it should contain and was raised in description
         *          it should analyze the data with the RaiseReactionsAnalyzer
         *          it should intialize RaiseReactionStatistics with the RaiseReactionAnalyzer
         *      
         *      * given valid data and and valid RaiseReactionStatistics
         *          it should create 4 rows
         *          it should create the first, second and third row with percentage unit
         *          it should create the fourth row without a unit since it shows the counts
         *          it should create one column for each item in the statistics PercentagesDictionary
         *          
         *          
         *      * given valid flop data with ActionSequence HeroB and out of position and betsizes 0.2 and 0.5
         *          it should contain bet in description
         *          it should contain out of position in description
         *          it should contain flop in description
         *          it should contain 0.2 to 0.5 of the pot in description
         *          
         *      * given valid turn data with ActionSequence HeroB and in position and betsizes 0.2 and 0.2
         *          it should contain bet in description
         *          it should contain in position in description
         *          it should contain turn in description
         *          it should contain 0.2 of the pot in description
         *          it should not contain 0.2 to 0.2 in description           
         *          
         *      * given the PercentagesDictionary has value 0 at 0 0 and value 50 at 1 1
         *          it should have value 0 at row 0 col 0
         *          it should have value 50 at row 1 col 1
         *          
         *      * given the TotalCountsDictionary has value 0 in col 0 and value 1 in col 1
         *          it should have value 0 in the counts row at col 0
         *          it should have value 1 in the counts row at col 1
         *      
         */

        #region Constants and Fields

        protected static Mock<IActionSequenceStatisticsSet> _actionStatisticsSetStub;

        protected static Mock<IAnalyzablePokerPlayer> _analyzablePokerPlayerStub;

        protected static Dictionary<int, int> _emptyCountsDictionary;

        protected static Dictionary<int, int> _emptyPercentageDictionary;

        protected static string _playerName;

        protected static Constructor<IRaiseReactionAnalyzer> _raiseReactionAnalyzerMake;

        protected static Mock<IRaiseReactionAnalyzer> _raiseReactionAnalyzerStub;

        protected static Mock<IRaiseReactionStatistics> _raiseReactionStatisticsMock;

        protected static Mock<IRaiseReactionsAnalyzer> _raiseReactionsAnalyzerMock;

        protected static DetailedRaiseReactionStatisticsViewModel _sut;

        protected static ActionSequences _validActionSequence;

        protected static IAnalyzablePokerPlayer[] _validAnalyzablePokerPlayersStub;

        protected static ITuple<double, double> _validBetSizes;

        protected static Streets _validStreet;

        Establish context = () => {
            _playerName = "somePlayer";
            _validStreet = Streets.Flop;
            _validActionSequence = ActionSequences.HeroB;
            _validBetSizes = Tuple.New(0.5, 0.7);
            _analyzablePokerPlayerStub = new Mock<IAnalyzablePokerPlayer>();
            _validAnalyzablePokerPlayersStub = new[] { _analyzablePokerPlayerStub.Object };
            _raiseReactionsAnalyzerMock = new Mock<IRaiseReactionsAnalyzer>();

            _raiseReactionStatisticsMock = new Mock<IRaiseReactionStatistics>();

            _emptyPercentageDictionary = new Dictionary<int, int>();
            _emptyCountsDictionary = new Dictionary<int, int>();

            _raiseReactionStatisticsMock.SetupGet(r => r.PercentagesDictionary).Returns(
                new Dictionary<ActionTypes, IDictionary<int, int>>
                    {
                        { ActionTypes.F, _emptyPercentageDictionary },
                        { ActionTypes.C, _emptyPercentageDictionary },
                        { ActionTypes.R, _emptyPercentageDictionary }
                    });
            _raiseReactionStatisticsMock.SetupGet(r => r.TotalCountsByColumnDictionary)
                .Returns(_emptyCountsDictionary);

            _raiseReactionAnalyzerStub = new Mock<IRaiseReactionAnalyzer>();
            _raiseReactionAnalyzerMake = new Constructor<IRaiseReactionAnalyzer>(() => _raiseReactionAnalyzerStub.Object);

            _actionStatisticsSetStub = new Mock<IActionSequenceStatisticsSet>();
            _actionStatisticsSetStub.SetupGet(s => s.PlayerName).Returns(_playerName);
            _actionStatisticsSetStub.SetupGet(s => s.ActionSequence).Returns(_validActionSequence);
            _actionStatisticsSetStub.SetupGet(s => s.Street).Returns(_validStreet);
        };

        #endregion
    }

    [Subject(typeof(DetailedRaiseReactionStatisticsViewModel), "Constructor")]
    public class given_0_analyzable_players : DetailedRaiseReactionStatisticsViewModelSpecs
    {
        #region Constants and Fields

        static Exception exception;

        Because of = () => exception = Catch.Exception(
                                           () => _sut = new DetailedRaiseReactionStatisticsViewModel(
                                                            _raiseReactionStatisticsMock.Object,
                                                            _raiseReactionsAnalyzerMock.Object,
                                                            _raiseReactionAnalyzerMake,
                                                            Enumerable.Empty<IAnalyzablePokerPlayer>(),
                                                            _actionStatisticsSetStub.Object,
                                                            _validBetSizes));

        It should_throw_an_ArgumentException
            = () => exception.ShouldBeOfType<ArgumentException>();

        #endregion
    }

    [Subject(typeof(DetailedRaiseReactionStatisticsViewModel), "Constructor")]
    public class given_valid_data : DetailedRaiseReactionStatisticsViewModelSpecs
    {
        #region Constants and Fields

        Because of = () => _sut = new DetailedRaiseReactionStatisticsViewModel(
                                             _raiseReactionStatisticsMock.Object,
                                             _raiseReactionsAnalyzerMock.Object,
                                             _raiseReactionAnalyzerMake,
                                             _validAnalyzablePokerPlayersStub,
                                             _actionStatisticsSetStub.Object,
                                             _validBetSizes);

        It should_analyze_the_data_with_the_RaiseReactionsAnalyzer
            = () => _raiseReactionsAnalyzerMock.Verify(
                        a => a.AnalyzeAndAdd(
                                 _raiseReactionAnalyzerStub.Object,
                                 _analyzablePokerPlayerStub.Object,
                                 _validStreet,
                                 _validActionSequence));

        It should_contain__and_was_raised__in_description
            = () => _sut.DetailedStatisticsDescription.ShouldContain("and was raised");

        It should_contain_player_name_in_description
            = () => _sut.DetailedStatisticsDescription.ShouldContain(_playerName);


        It should_initialize_RaiseReactionStatistics_with_the_RaiseReactionAnalyzer =
            () => _raiseReactionStatisticsMock.Verify(s => s.InitializeWith(_raiseReactionsAnalyzerMock.Object));

        
        #endregion
    }

    [Subject(typeof(DetailedRaiseReactionStatisticsViewModel), "Constructor")]
    public class given_valid_data_and_valid_RaiseReactionStatistics : DetailedRaiseReactionStatisticsViewModelSpecs
    {
        Establish context = () =>
        {
            _emptyPercentageDictionary.Add(1, 0);
            _emptyPercentageDictionary.Add(2, 0);

            _raiseReactionStatisticsMock.SetupGet(r => r.PercentagesDictionary).Returns(
                new Dictionary<ActionTypes, IDictionary<int, int>>
                    {
                        { ActionTypes.F, _emptyPercentageDictionary },
                        { ActionTypes.C, _emptyPercentageDictionary },
                        { ActionTypes.R, _emptyPercentageDictionary }
                    });
            };

        Because of = () => _sut = new DetailedRaiseReactionStatisticsViewModel(
                                     _raiseReactionStatisticsMock.Object,
                                     _raiseReactionsAnalyzerMock.Object,
                                     _raiseReactionAnalyzerMake,
                                     _validAnalyzablePokerPlayersStub,
                                     _actionStatisticsSetStub.Object,
                                     _validBetSizes);


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

    [Subject(typeof(DetailedRaiseReactionStatisticsViewModel), "Constructor")]
    public class given_valid_flop_data_with_ActionSequence_HeroB_and_out_of_position_and_betsizes_0_2_and_0_5
        : DetailedRaiseReactionStatisticsViewModelSpecs
    {
        #region Constants and Fields

        Establish context = () => {
            _actionStatisticsSetStub.SetupGet(s => s.Street).Returns(Streets.Flop);
            _actionStatisticsSetStub.SetupGet(s => s.ActionSequence).Returns(ActionSequences.HeroB);
            _actionStatisticsSetStub.SetupGet(s => s.InPosition).Returns(false);
            _validBetSizes = Tuple.New(0.2, 0.5);
        };

        Because of = () => _sut = new DetailedRaiseReactionStatisticsViewModel(
                                      _raiseReactionStatisticsMock.Object,
                                      _raiseReactionsAnalyzerMock.Object,
                                      _raiseReactionAnalyzerMake,
                                      _validAnalyzablePokerPlayersStub,
                                      _actionStatisticsSetStub.Object,
                                      _validBetSizes);

        It should_contain__0_2_to_0_5_of_the_pot__in_description
            = () => _sut.DetailedStatisticsDescription.ShouldContain("0.2 to 0.5");

        It should_contain__bet__in_description
            = () => _sut.DetailedStatisticsDescription.ShouldContain("bet");

        It should_contain__flop__in_description
            = () => _sut.DetailedStatisticsDescription.ShouldContain("flop");

        It should_contain__out_of_position__in_description
            = () => _sut.DetailedStatisticsDescription.ShouldContain("out of position");

        #endregion
    }

    [Subject(typeof(DetailedRaiseReactionStatisticsViewModel), "Constructor")]
    public class given_valid_turn_data_with_ActionSequence_HeroB_and_in_position_and_betsizes_0_2_and_0_2
        : DetailedRaiseReactionStatisticsViewModelSpecs
    {
        #region Constants and Fields

        Establish context = () => {
            _actionStatisticsSetStub.SetupGet(s => s.Street).Returns(Streets.Turn);
            _actionStatisticsSetStub.SetupGet(s => s.ActionSequence).Returns(ActionSequences.HeroB);
            _actionStatisticsSetStub.SetupGet(s => s.InPosition).Returns(true);
            _validBetSizes = Tuple.New(0.2, 0.2);
        };

        Because of = () => _sut = new DetailedRaiseReactionStatisticsViewModel(
                                      _raiseReactionStatisticsMock.Object,
                                      _raiseReactionsAnalyzerMock.Object,
                                      _raiseReactionAnalyzerMake,
                                      _validAnalyzablePokerPlayersStub,
                                      _actionStatisticsSetStub.Object,
                                      _validBetSizes);

        It should_contain__0_2_of_the_pot__in_description
            = () => _sut.DetailedStatisticsDescription.ShouldContain("0.2 of the pot");

        It should_contain__bet__in_description
            = () => _sut.DetailedStatisticsDescription.ShouldContain("bet");

        It should_contain__in_position__in_description
            = () => _sut.DetailedStatisticsDescription.ShouldContain("in position");

        It should_contain__turn__in_description
            = () => _sut.DetailedStatisticsDescription.ShouldContain("turn");

        It should_not_contain__0_2_to_0_2__in_description
            = () => _sut.DetailedStatisticsDescription.Contains("0.2 to 0.2").ShouldBeFalse();

        #endregion
    }

    [Subject(typeof(DetailedRaiseReactionStatisticsViewModel), "Constructor")]
    public class given_the_PercentagesDictionary_has_value_0_at_0_0_and_value_50_at_1_1
        : DetailedRaiseReactionStatisticsViewModelSpecs
    {
        Establish context = () => _raiseReactionStatisticsMock.SetupGet(r => r.PercentagesDictionary).Returns(
                                      new Dictionary<ActionTypes, IDictionary<int, int>>
                                          {
                                              { ActionTypes.F, new Dictionary<int, int> {{1, 0}, {2, 0}} },
                                              { ActionTypes.C, new Dictionary<int, int> {{1, 0}, {2, 50}} },
                                              { ActionTypes.R, _emptyPercentageDictionary }
                                          });

        Because of = () => _sut = new DetailedRaiseReactionStatisticsViewModel(
                                     _raiseReactionStatisticsMock.Object,
                                     _raiseReactionsAnalyzerMock.Object,
                                     _raiseReactionAnalyzerMake,
                                     _validAnalyzablePokerPlayersStub,
                                     _actionStatisticsSetStub.Object,
                                     _validBetSizes);


        #region Constants and Fields

        It should_have_value_0_at_row_0_col_0
            = () => _sut.Rows.ElementAt(0).Cells[0].Value.ShouldEqual(0);

        It should_have_value_50_at_row_1_col_1
            = () => _sut.Rows.ElementAt(1).Cells[1].Value.ShouldEqual(50);

        #endregion
    }

    [Subject(typeof(DetailedRaiseReactionStatisticsViewModel), "Constructor")]
    public class given_the_TotalCountsDictionary_has_value_0_in_col_0_and_value_1_in_col_1
        : DetailedRaiseReactionStatisticsViewModelSpecs
    {
        #region Constants and Fields

        Establish context = () => _raiseReactionStatisticsMock.SetupGet(r => r.TotalCountsByColumnDictionary).Returns(
                                      new Dictionary<int, int>
                                          {
                                              { 1, 0 },
                                              { 2, 1 }
                                          });

        Because of = () => _sut = new DetailedRaiseReactionStatisticsViewModel(
                                     _raiseReactionStatisticsMock.Object,
                                     _raiseReactionsAnalyzerMock.Object,
                                     _raiseReactionAnalyzerMake,
                                     _validAnalyzablePokerPlayersStub,
                                     _actionStatisticsSetStub.Object,
                                     _validBetSizes);


        It should_have_value_0_in_the_counts_row_at_col_0
            = () => _sut.Rows.Last().Cells[0].Value.ShouldEqual(0);

        It should_have_value_1_in_the_counts_row_at_col_1
            = () => _sut.Rows.Last().Cells[1].Value.ShouldEqual(1);

        #endregion
    }

    // ReSharper restore InconsistentNaming
}