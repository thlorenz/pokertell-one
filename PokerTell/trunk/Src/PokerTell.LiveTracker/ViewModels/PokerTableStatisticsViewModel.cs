namespace PokerTell.LiveTracker.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Input;

    using Interfaces;

    using Microsoft.Practices.Composite.Events;

    using PokerTell.Infrastructure.Events;
    using PokerTell.Infrastructure.Interfaces;
    using PokerTell.Infrastructure.Interfaces.Statistics;

    using Tools.WPF;
    using Tools.WPF.ViewModels;

    public class PokerTableStatisticsViewModel : NotifyPropertyChanged, IPokerTableStatisticsViewModel
    {
        #region Constants and Fields

        readonly IEventAggregator _eventAggregator;

        readonly IConstructor<IPlayerStatisticsViewModel> _playerStatisticsViewModelMake;

        ICommand _filterAdjustmentRequestedCommand;

        IPlayerStatisticsViewModel _selectedPlayer;

        readonly IDetailedStatisticsAnalyzerViewModel _detailedStatisticsAnalyzer;

        #endregion

        #region Constructors and Destructors

        public PokerTableStatisticsViewModel(
            IEventAggregator eventAggregator, 
            IConstructor<IPlayerStatisticsViewModel> playerStatisticsViewModelMake,
            IDetailedStatisticsAnalyzerViewModel detailedStatisticsAnalyzerViewModel)
        {
            _eventAggregator = eventAggregator;
            _playerStatisticsViewModelMake = playerStatisticsViewModelMake;
            _detailedStatisticsAnalyzer = detailedStatisticsAnalyzerViewModel;

            Players = new ObservableCollection<IPlayerStatisticsViewModel>();
        }

        #endregion

        #region Properties

        public IDetailedStatisticsAnalyzerViewModel DetailedStatisticsAnalyzer
        {
            get { return _detailedStatisticsAnalyzer; }
        }

        public ICommand FilterAdjustmentRequestedCommand
        {
            get
            {
                return _filterAdjustmentRequestedCommand ?? (_filterAdjustmentRequestedCommand = new SimpleCommand
                    {
                        ExecuteDelegate = arg =>
                                          _eventAggregator
                                              .GetEvent<AdjustAnalyzablePokerPlayersFilterEvent>()
                                              .Publish(new AdjustAnalyzablePokerPlayersFilterEventArgs(
                                                           SelectedPlayer.PlayerName, 
                                                           SelectedPlayer.Filter, 
                                                           ApplyFilterTo, 
                                                           ApplyFilterToAll)), 
                        CanExecuteDelegate = arg => SelectedPlayer != null
                    });
            }
        }

        public IList<IPlayerStatisticsViewModel> Players { get; protected set; }

        public IPlayerStatisticsViewModel SelectedPlayer
        {
            get { return _selectedPlayer; }
            set
            {
                _selectedPlayer = value;
                
                RaisePropertyChanged(() => SelectedPlayer);
            }
        }

        #endregion

        #region Public Methods

        public IPokerTableStatisticsViewModel UpdateWith(IEnumerable<IPlayerStatistics> playersStatistics)
        {
            var playersStatisticsList = ValidateAndConvertToList(playersStatistics);
            var playersList = Players.ToList();

            RemovePlayersThatAreNotAtTheTableAnymore(playersList, playersStatisticsList);

            UpdatePlayersWithStatisticsAddingNewOnesIfNeeded(playersList, playersStatisticsList);

            SelectFirstPlayerIfSelectedPlayerIsNotAtTheTableAnymore();

            return this;
        }

        public IPlayerStatisticsViewModel GetPlayerStatisticsViewModelFor(string playerName)
        {
            return Players.FirstOrDefault(p => p.PlayerName == playerName);
        }

        #endregion

        #region Methods

        protected void ApplyFilterTo(string playerName, IAnalyzablePokerPlayersFilter adjustedFilter)
        {
            var playerToUpdate = Players.ToList().FirstOrDefault(p => p.PlayerName == playerName);

            if (playerToUpdate != null)
            {
                playerToUpdate.Filter = adjustedFilter;
            }
        }

        protected void ApplyFilterToAll(IAnalyzablePokerPlayersFilter adjustedFilter)
        {
            Players.ToList().ForEach(p => p.Filter = adjustedFilter);
        }

        static List<IPlayerStatistics> ValidateAndConvertToList(IEnumerable<IPlayerStatistics> playersStatistics)
        {
            if (playersStatistics == null)
            {
                throw new ArgumentNullException("playersStatistics");
            }

            var playersStatisticsList = playersStatistics.ToList();
            if (playersStatisticsList.Count < 1)
            {
                throw new ArgumentException("Need at least one Player", "playersStatistics");
            }

            return playersStatisticsList;
        }

        IPlayerStatisticsViewModel AddNewPlayerToPlayersIfNotFound(IPlayerStatisticsViewModel matchingPlayer)
        {
            if (matchingPlayer == null)
            {
                matchingPlayer = _playerStatisticsViewModelMake.New;
                
                matchingPlayer.SelectedStatisticsSetEvent +=
                    sequenceStatisticsSet => DetailedStatisticsAnalyzer.InitializeWith(sequenceStatisticsSet);
                
                Players.Add(matchingPlayer);
            }

            return matchingPlayer;
        }

        IPlayerStatisticsViewModel FindOrAddMatchingPlayer(
            IPlayerStatistics playerStatistics, List<IPlayerStatisticsViewModel> playersList)
        {
            var nameToFind = playerStatistics.PlayerIdentity.Name;
            var matchingPlayer = playersList
                .Find(player => player.PlayerName == nameToFind);

            matchingPlayer = AddNewPlayerToPlayersIfNotFound(matchingPlayer);
            return matchingPlayer;
        }

        void RemovePlayersThatAreNotAtTheTableAnymore(
            List<IPlayerStatisticsViewModel> playersList, List<IPlayerStatistics> playersStatisticsList)
        {
            playersList.ForEach(player => {
                var nameToFind = player.PlayerName;

                var matchingPlayerStatistics = playersStatisticsList
                    .Find(ps => ps.PlayerIdentity.Name == nameToFind);

                if (matchingPlayerStatistics == null)
                {
                    Players.Remove(player);
                }
            });
        }

        void SelectFirstPlayerIfSelectedPlayerIsNotAtTheTableAnymore()
        {
            if (! Players.Contains(SelectedPlayer))
            {
                SelectedPlayer = Players.First();
            }
        }

        void UpdatePlayersWithStatisticsAddingNewOnesIfNeeded(
            List<IPlayerStatisticsViewModel> playersList, List<IPlayerStatistics> playersStatisticsList)
        {
            playersStatisticsList.ForEach(playerStatistics => {
                var matchingPlayer = FindOrAddMatchingPlayer(playerStatistics, playersList);

                matchingPlayer.UpdateWith(playerStatistics);
            });
        }

        #endregion
    }
}