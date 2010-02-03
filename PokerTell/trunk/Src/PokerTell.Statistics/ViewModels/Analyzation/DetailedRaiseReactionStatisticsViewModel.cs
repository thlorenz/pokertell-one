namespace PokerTell.Statistics.ViewModels.Analyzation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Input;

    using Base;

    using Infrastructure.Enumerations.PokerHand;
    using Infrastructure.Interfaces.PokerHand;

    using Interfaces;

    using Tools.Interfaces;
    using Tools.WPF;

    public class DetailedRaiseReactionStatisticsViewModel : StatisticsTableViewModel
    {
        readonly IHandBrowserViewModel _handBrowserViewModel;

        readonly IPostFlopHeroActsRaiseReactionDescriber _raiseReactionDescriber;

        readonly IRaiseReactionStatisticsBuilder _raiseReactionStatisticsBuilder;

        ActionSequences _actionSequence;

        IEnumerable<IAnalyzablePokerPlayer> _analyzablePokerPlayers;

        ICommand _browseHandsCommand;

        bool _inPosition;

        string _playerName;

        IRaiseReactionStatistics _raiseReactionStatistics;

        ITuple<double, double> _selectedBetSizeSpan;

        Streets _street;

        public DetailedRaiseReactionStatisticsViewModel(
            IHandBrowserViewModel handBrowserViewModel,
            IRaiseReactionStatisticsBuilder raiseReactionStatisticsBuilder,
            IPostFlopHeroActsRaiseReactionDescriber raiseReactionDescriber)
            : base("Raise Size")
        {
            _raiseReactionDescriber = raiseReactionDescriber;
            _raiseReactionStatisticsBuilder = raiseReactionStatisticsBuilder;
            _handBrowserViewModel = handBrowserViewModel;
        }

        public ICommand BrowseHandsCommand
        {
            get
            {
                return _browseHandsCommand ?? (_browseHandsCommand = new SimpleCommand {
                    ExecuteDelegate = arg => {
                        _handBrowserViewModel.InitializeWith(SelectedAnalyzablePlayers);
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
                                .Value[(int)_raiseReactionStatistics.AnalyzablePlayersDictionary.Keys.ElementAt(col)];
                    });
            }
        }

        public DetailedRaiseReactionStatisticsViewModel InitializeWith(
            IEnumerable<IAnalyzablePokerPlayer> analyzablePokerPlayers,
            ITuple<double, double> selectedBetSizeSpan,
            string playerName,
            ActionSequences actionSequence,
            bool inPosition,
            Streets street)
        {
            _playerName = playerName;
            _selectedBetSizeSpan = selectedBetSizeSpan;
            _street = street;
            _inPosition = inPosition;
            _actionSequence = actionSequence;

            _analyzablePokerPlayers = analyzablePokerPlayers;

            if (analyzablePokerPlayers.Count() < 1)
            {
                throw new ArgumentException("need at least one analyzable Player");
            }

            CreateTableAndDescription();

            return this;
        }

        protected void CreateTableAndDescription()
        {
            _raiseReactionStatistics = _raiseReactionStatisticsBuilder
                .Build(_analyzablePokerPlayers, _actionSequence, _street);

            Rows = new List<IStatisticsTableRowViewModel> {
                new StatisticsTableRowViewModel(
                    "Fold", _raiseReactionStatistics.PercentagesDictionary[ActionTypes.F].Values, "%"),
                new StatisticsTableRowViewModel(
                    "Call", _raiseReactionStatistics.PercentagesDictionary[ActionTypes.C].Values, "%"),
                new StatisticsTableRowViewModel(
                    "Raise", _raiseReactionStatistics.PercentagesDictionary[ActionTypes.R].Values, "%"),
                new StatisticsTableRowViewModel(
                    "Count", _raiseReactionStatistics.TotalCountsByColumnDictionary.Values, string.Empty)
            };

            StatisticsDescription = _raiseReactionDescriber
                .Describe(_playerName,
                          _analyzablePokerPlayers.First(),
                          _street,
                          _selectedBetSizeSpan);
        }
    }
}