namespace PokerTell.Statistics.ViewModels.Analyzation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Input;

    using Base;

    using Infrastructure.Enumerations.PokerHand;
    using Infrastructure.Interfaces;
    using Infrastructure.Interfaces.PokerHand;

    using Interfaces;

    using Tools.FunctionalCSharp;
    using Tools.Interfaces;
    using Tools.WPF;

    public class DetailedRaiseReactionStatisticsViewModel : StatisticsTableViewModel
    {
        #region Constants and Fields

        readonly ActionSequences _actionSequence;

        readonly IEnumerable<IAnalyzablePokerPlayer> _analyzablePokerPlayers;

        readonly IHandBrowserViewModel _handBrowserViewModel;

        readonly bool _inPosition;

        readonly string _playerName;

        readonly IConstructor<IRaiseReactionAnalyzer> _raiseReactionAnalyzerMake;

        readonly IRaiseReactionStatistics _raiseReactionStatistics;

        readonly IRaiseReactionsAnalyzer _raiseReactionsAnalyzer;

        readonly ITuple<double, double> _selectedBetSizeSpan;

        readonly Streets _street;

        ICommand _browseHandsCommand;

        #endregion

        #region Constructors and Destructors

        public DetailedRaiseReactionStatisticsViewModel(
            IHandBrowserViewModel handBrowserViewModel,
            IRaiseReactionStatistics raiseReactionStatistics,
            IRaiseReactionsAnalyzer raiseReactionsAnalyzer,
            IConstructor<IRaiseReactionAnalyzer> raiseReactionAnalyzerMake,
            IEnumerable<IAnalyzablePokerPlayer> analyzablePokerPlayers,
            ITuple<double, double> selectedBetSizeSpan,
            string playerName,
            ActionSequences actionSequence,
            bool inPosition,
            Streets street)
            : base("Raise Size")
        {
            _handBrowserViewModel = handBrowserViewModel;
            _playerName = playerName;
            _selectedBetSizeSpan = selectedBetSizeSpan;
            _street = street;
            _inPosition = inPosition;
            _actionSequence = actionSequence;
            _raiseReactionAnalyzerMake = raiseReactionAnalyzerMake;
            _raiseReactionsAnalyzer = raiseReactionsAnalyzer;
            _raiseReactionStatistics = raiseReactionStatistics;
            _analyzablePokerPlayers = analyzablePokerPlayers;

            if (analyzablePokerPlayers.Count() < 1)
            {
                throw new ArgumentException("need at least one analyzable Player");
            }

            CreateTableAndDescription();
        }

        #endregion

        #region Properties

        public ICommand BrowseHandsCommand
        {
            get
            {
                return _browseHandsCommand ?? (_browseHandsCommand = new SimpleCommand
                    {
                        ExecuteDelegate = arg => {
                            _handBrowserViewModel.Browse(SelectedAnalyzablePlayers);
                            ChildViewModel = _handBrowserViewModel;
                        },
                        CanExecuteDelegate = arg => SelectedCells.Count > 0
                    });
            }
        }

        protected IEnumerable<IAnalyzablePokerPlayer> SelectedAnalyzablePlayers
        {
            get
            {
                return SelectedCells.SelectMany(
                    selectedCell => {
                        int row = selectedCell.First;
                        int col = selectedCell.Second;
                        return
                            _raiseReactionStatistics.AnalyzablePlayersDictionary.ElementAt(row)
                                .Value[(int)_raiseReactionsAnalyzer.RaiseSizeKeys[col]];
                    });
            }
        }

        #endregion

        #region Methods

        protected void CreateTableAndDescription()
        {
            CreateDescription();

            AnalyzeStatistics();

            Rows = new List<IStatisticsTableRowViewModel>
                {
                    new StatisticsTableRowViewModel(
                        "Fold", _raiseReactionStatistics.PercentagesDictionary[ActionTypes.F].Values, "%"),
                    new StatisticsTableRowViewModel(
                        "Call", _raiseReactionStatistics.PercentagesDictionary[ActionTypes.C].Values, "%"),
                    new StatisticsTableRowViewModel(
                        "Raise", _raiseReactionStatistics.PercentagesDictionary[ActionTypes.R].Values, "%"),
                    new StatisticsTableRowViewModel(
                        "Count", _raiseReactionStatistics.TotalCountsByColumnDictionary.Values, string.Empty)
                };
        }

        void AnalyzeStatistics()
        {
            _analyzablePokerPlayers.ForEach(
                analyzablePlayer => _raiseReactionsAnalyzer
                                        .AnalyzeAndAdd(_raiseReactionAnalyzerMake.New, analyzablePlayer, _street, _actionSequence));

            _raiseReactionStatistics.InitializeWith(_raiseReactionsAnalyzer);
        }

        void CreateDescription()
        {
            StatisticsDescription =
                string.Format(
                    "{0} {1} {2} of the pot {3} on the {4} and was raised",
                    _playerName,
                    ActionSequencesUtility.NameLastActionInSequence(_actionSequence).ToLower(),
                    (_selectedBetSizeSpan.First == _selectedBetSizeSpan.Second)
                        ? _selectedBetSizeSpan.First.ToString()
                        : _selectedBetSizeSpan.First + " to " + _selectedBetSizeSpan.Second,
                    _inPosition ? "in position" : "out of position",
                    _street.ToString().ToLower()
                    );
        }

        #endregion
    }
}