namespace PokerTell.LiveTracker.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Drawing;
    using System.Linq;
    using System.Reflection;
    using System.Windows.Input;

    using log4net;

    using PokerTell.Infrastructure.Enumerations.PokerHand;
    using PokerTell.Infrastructure.Interfaces;
    using PokerTell.Infrastructure.Interfaces.Statistics;
    using PokerTell.LiveTracker.Interfaces;

    using Tools.WPF;
    using Tools.WPF.Interfaces;
    using Tools.WPF.ViewModels;

    public class PokerTableStatisticsViewModel : NotifyPropertyChanged, IPokerTableStatisticsViewModel
    {
        public const string DimensionsKey = "PokerTell.LiveTracker.PokerTableStatistics.Dimensions";

        static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        readonly IConstructor<IPlayerStatisticsViewModel> _playerStatisticsViewModelMake;

        readonly ISettings _settings;

        ICommand _filterAdjustmentRequestedCommand;

        IPlayerStatisticsViewModel _selectedPlayer;

        string _tableName;

        readonly IActiveAnalyzablePlayersSelector _activePlayersSelector;

        public IFilterPopupViewModel FilterPopup { get; set; }

        public PokerTableStatisticsViewModel(
            ISettings settings,
            IDimensionsViewModel dimensions,
            IConstructor<IPlayerStatisticsViewModel> playerStatisticsViewModelMake,
            IDetailedStatisticsAnalyzerViewModel detailedStatisticsAnalyzerViewModel,
            IActiveAnalyzablePlayersSelector activePlayersSelector,
            IFilterPopupViewModel filterPopupViewModel)
        {
            _settings = settings;
            _playerStatisticsViewModelMake = playerStatisticsViewModelMake;
            DetailedStatisticsAnalyzer = detailedStatisticsAnalyzerViewModel;

            _activePlayersSelector = activePlayersSelector;

            FilterPopup = filterPopupViewModel;
            
            Dimensions = dimensions.InitializeWith(settings.RetrieveRectangle(DimensionsKey, new Rectangle(0, 0, 600, 400)));

            Players = new ObservableCollection<IPlayerStatisticsViewModel>();
        }

        public event Action PlayersStatisticsWereUpdated = delegate { };

        public event Action<IPlayerStatisticsViewModel> UserBrowsedAllHands = delegate { };

        public event Action<IActionSequenceStatisticsSet> UserSelectedStatisticsSet = delegate { };

        public IDetailedStatisticsAnalyzerViewModel DetailedStatisticsAnalyzer { get; protected set; }

        public IDimensionsViewModel Dimensions { get; protected set; }

        public ICommand FilterAdjustmentRequestedCommand
        {
            get
            {
                return _filterAdjustmentRequestedCommand ?? (_filterAdjustmentRequestedCommand = new SimpleCommand
                    {
                        ExecuteDelegate = arg => DisplayFilterAdjustmentPopup(SelectedPlayer), 
                        CanExecuteDelegate = arg => SelectedPlayer != null
                    });
            }
        }

        ICommand _browseAllHandsOfSelectedPlayerCommand;

        public ICommand BrowseAllHandsOfSelectedPlayerCommand
        {
            get
            {
                return _browseAllHandsOfSelectedPlayerCommand ?? (_browseAllHandsOfSelectedPlayerCommand = new SimpleCommand
                    {
                        ExecuteDelegate = arg => BrowseAllHandsOf(SelectedPlayer),
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

                BrowseAllHandsOf(_selectedPlayer);

                RaisePropertyChanged(() => SelectedPlayer);
            }
        }

        public string TableName
        {
            get { return _tableName; }
            set
            {
                _tableName = value;
                RaisePropertyChanged(() => TableName);
            }
        }

        public virtual void DisplayFilterAdjustmentPopup(IPlayerStatisticsViewModel player)
        {
            FilterPopup.ShowFilter(player.PlayerName, player.Filter, ApplyFilterTo, ApplyFilterToAll);
        }

        public IPlayerStatisticsViewModel GetPlayerStatisticsViewModelFor(string playerName)
        {
            return Players.FirstOrDefault(p => p.PlayerName == playerName);
        }

        public IPokerTableStatisticsViewModel SaveDimensions()
        {
            _settings.Set(DimensionsKey, Dimensions.Rectangle);

            return this;
        }

        public void UpdateWith(IEnumerable<IPlayerStatistics> playersStatistics)
        {
            var playersStatisticsList = ValidateAndConvertToList(playersStatistics);
            var playersList = Players.ToList();

            RemovePlayersThatAreNotAtTheTableAnymore(playersList, playersStatisticsList);

            UpdatePlayersWithStatisticsAddingNewOnesIfNeeded(playersList, playersStatisticsList);

            SelectFirstPlayerIfSelectedPlayerIsNotAtTheTableAnymore();

            PlayersStatisticsWereUpdated();
        }

        protected IPlayerStatisticsViewModel AddNewPlayerToPlayersIfNotFound(IPlayerStatisticsViewModel matchingPlayer)
        {
            if (matchingPlayer == null)
            {
                matchingPlayer = _playerStatisticsViewModelMake.New;

                matchingPlayer.SelectedStatisticsSetEvent +=
                    sequenceStatisticsSet => {
                        DetailedStatisticsAnalyzer.InitializeWith(sequenceStatisticsSet);
                        UserSelectedStatisticsSet(sequenceStatisticsSet);
                    };

                matchingPlayer.BrowseAllMyHandsRequested += player => {
                    BrowseAllHandsOf(player);
                    UserBrowsedAllHands(player);
                };

                Players.Add(matchingPlayer);
            }

            return matchingPlayer;
        }

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

        protected virtual void BrowseAllHandsOf(IPlayerStatisticsViewModel selectedPlayer)
        {
            if (selectedPlayer != null)
            {
                var activeAnalyzablePlayers = _activePlayersSelector.SelectFrom(selectedPlayer.PlayerStatistics);

                if (activeAnalyzablePlayers.Count() > 0)
                    DetailedStatisticsAnalyzer.InitializeWith(activeAnalyzablePlayers, selectedPlayer.PlayerName);
            }
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
                    .Find(ps => ps != null && ps.PlayerIdentity != null && ps.PlayerIdentity.Name == nameToFind);
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
                if (playerStatistics.PlayerIdentity != null)
                {
                    var matchingPlayer = FindOrAddMatchingPlayer(playerStatistics, playersList);

                    matchingPlayer.UpdateWith(playerStatistics);
                }
                else
                {
                    Log.DebugFormat("Found a null playeridentity.");
                }
            });
        }
    }
}