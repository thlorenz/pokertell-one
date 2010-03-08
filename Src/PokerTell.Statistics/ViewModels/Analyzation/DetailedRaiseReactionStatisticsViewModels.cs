namespace PokerTell.Statistics.ViewModels.Analyzation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Input;

    using PokerTell.Infrastructure;
    using PokerTell.Infrastructure.Enumerations.PokerHand;
    using PokerTell.Infrastructure.Interfaces.PokerHand;
    using PokerTell.Statistics.Interfaces;
    using PokerTell.Statistics.ViewModels.Base;

    using Tools.Interfaces;
    using Tools.WPF;

    public class PostFlopHeroActsRaiseReactionStatisticsViewModel : DetailedRaiseReactionStatisticsViewModel<double>, 
                                                                    IPostFlopHeroActsRaiseReactionStatisticsViewModel
    {
        public PostFlopHeroActsRaiseReactionStatisticsViewModel(
            IRepositoryHandBrowserViewModel handBrowserViewModel, 
            IRaiseReactionStatisticsBuilder raiseReactionStatisticsBuilder, 
            IPostFlopHeroActsRaiseReactionDescriber raiseReactionDescriber)
            : base(handBrowserViewModel, raiseReactionStatisticsBuilder, raiseReactionDescriber)
        {
            MayBrowseHands = true;
            MayInvestigateHoleCards = false;
            MayInvestigateRaise = false;
            ConsiderOpponentsRaiseSize = true;
        }
    }

    public class PostFlopHeroReactsRaiseReactionStatisticsViewModel : DetailedRaiseReactionStatisticsViewModel<double>, 
                                                                      IPostFlopHeroReactsRaiseReactionStatisticsViewModel
    {
        public PostFlopHeroReactsRaiseReactionStatisticsViewModel(
            IRepositoryHandBrowserViewModel handBrowserViewModel, 
            IRaiseReactionStatisticsBuilder raiseReactionStatisticsBuilder, 
            IPostFlopHeroReactsRaiseReactionDescriber raiseReactionDescriber)
            : base(handBrowserViewModel, raiseReactionStatisticsBuilder, raiseReactionDescriber)
        {
            MayBrowseHands = true;
            MayInvestigateHoleCards = false;
            MayInvestigateRaise = false;
            ConsiderOpponentsRaiseSize = false;
        }
    }

    public class PreFlopRaiseReactionStatisticsViewModel : DetailedRaiseReactionStatisticsViewModel<StrategicPositions>, 
                                                           IPreFlopRaiseReactionStatisticsViewModel
    {
        public PreFlopRaiseReactionStatisticsViewModel(
            IRepositoryHandBrowserViewModel handBrowserViewModel, 
            IRaiseReactionStatisticsBuilder raiseReactionStatisticsBuilder, 
            IPreFlopRaiseReactionDescriber raiseReactionDescriber)
            : base(handBrowserViewModel, raiseReactionStatisticsBuilder, raiseReactionDescriber)
        {
            MayBrowseHands = true;
            MayInvestigateHoleCards = false;
            MayInvestigateRaise = false;
            ConsiderOpponentsRaiseSize = false;
        }
    }

    public class DetailedRaiseReactionStatisticsViewModel<T> : StatisticsTableViewModel, 
                                                               IDetailedRaiseReactionStatisticsViewModel<T>
    {
        readonly IRepositoryHandBrowserViewModel _handBrowserViewModel;

        readonly IRaiseReactionDescriber<T> _raiseReactionDescriber;

        readonly IRaiseReactionStatisticsBuilder _raiseReactionStatisticsBuilder;

        ActionSequences _actionSequence;

        IEnumerable<IAnalyzablePokerPlayer> _analyzablePokerPlayers;

        string _playerName;

        IRaiseReactionStatistics _raiseReactionStatistics;

        ITuple<T, T> _selectedRationSizeSpan;

        Streets _street;
        protected bool ConsiderOpponentsRaiseSize;

        public DetailedRaiseReactionStatisticsViewModel(
            IRepositoryHandBrowserViewModel handBrowserViewModel, 
            IRaiseReactionStatisticsBuilder raiseReactionStatisticsBuilder, 
            IRaiseReactionDescriber<T> raiseReactionDescriber)
            : base("Raise Size")
        {
            _raiseReactionDescriber = raiseReactionDescriber;
            _raiseReactionStatisticsBuilder = raiseReactionStatisticsBuilder;
            _handBrowserViewModel = handBrowserViewModel;
        }

        ICommand _browseHandsCommand;

        public ICommand BrowseHandsCommand
        {
            get
            {
                return _browseHandsCommand ?? (_browseHandsCommand = new SimpleCommand
                    {
                        ExecuteDelegate = arg => {
                            SaveSelectedCells();
                            _handBrowserViewModel.InitializeWith(SelectedAnalyzablePlayers);
                            ChildViewModel = _handBrowserViewModel;
                        }, 
                        CanExecuteDelegate = arg => SelectedCells.Count > 0 && SelectedAnalyzablePlayers.Count() > 0
                    });
            }
        }

        protected virtual IEnumerable<IAnalyzablePokerPlayer> SelectedAnalyzablePlayers
        {
            get
            {
                return SelectedCells
                    .SelectMany(
                    selectedCell => {
                        int row = selectedCell.First;
                        int col = selectedCell.Second;
                        return
                            _raiseReactionStatistics.AnalyzablePlayersDictionary
                                .ElementAt(row).Value
                                .ElementAt(col).Value;
                    });
            }
        }

        public IDetailedRaiseReactionStatisticsViewModel<T> InitializeWith(
            IEnumerable<IAnalyzablePokerPlayer> analyzablePokerPlayers, 
            ITuple<T, T> selectedRatioSizeSpan, 
            string playerName, 
            ActionSequences actionSequence, 
            Streets street)
        {
            _playerName = playerName;
            _selectedRationSizeSpan = selectedRatioSizeSpan;
            _street = street;
            _actionSequence = actionSequence;

            _analyzablePokerPlayers = analyzablePokerPlayers;

            if (analyzablePokerPlayers.Count() < 1)
            {
                throw new ArgumentException("need at least one analyzable Player");
            }

            _raiseReactionStatisticsBuilder.InitializeWith(ApplicationProperties.RaiseSizeKeys);
            CreateTableAndDescription();

            return this;
        }

        protected void CreateTableAndDescription()
        {
            _raiseReactionStatistics = _raiseReactionStatisticsBuilder
                .Build(_analyzablePokerPlayers, _actionSequence, _street, ConsiderOpponentsRaiseSize);

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

            StatisticsDescription = _raiseReactionDescriber
                .Describe(_playerName, 
                          _analyzablePokerPlayers.First(), 
                          _street, 
                          _selectedRationSizeSpan);
            StatisticsHint = _raiseReactionDescriber
                .Hint(_playerName, _analyzablePokerPlayers.First());
        }
    }
}