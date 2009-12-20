namespace PokerTell.LiveTracker.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    using PokerTell.Infrastructure.Interfaces;
    using PokerTell.Infrastructure.Interfaces.Statistics;

    using Tools.WPF.ViewModels;

    public class TableStatisticsViewModel : NotifyPropertyChanged
    {
        #region Constants and Fields

        readonly IConstructor<IPlayerStatisticsViewModel> _playerStatisticsViewModelMake;

        IPlayerStatisticsViewModel _selectedPlayer;

        #endregion

        #region Constructors and Destructors

        public TableStatisticsViewModel(IConstructor<IPlayerStatisticsViewModel> playerStatisticsViewModelMake)
        {
            _playerStatisticsViewModelMake = playerStatisticsViewModelMake;
            Players = new ObservableCollection<IPlayerStatisticsViewModel>();
        }

        #endregion

        #region Properties

        public ObservableCollection<IPlayerStatisticsViewModel> Players { get; protected set; }

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

        public TableStatisticsViewModel UpdateWith(IEnumerable<IPlayerStatistics> playersStatistics)
        {
            var playersStatisticsList = ValidateAndConvertToList(playersStatistics);
            var playersList = Players.ToList();

            RemovePlayersThatAreNotAtTheTableAnymore(playersList, playersStatisticsList);

            UpdatePlayersWithStatisticsAddingNewOnesIfNeeded(playersList, playersStatisticsList);

            SelectFirstPlayerIfSelectedPlayerIsNotAtTheTableAnymore();

            return this;
        }

        #endregion

        #region Methods

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
                matchingPlayer.SelectedStatisticsSetEvent += (name, statisticsSet, street) =>
                    Console.WriteLine("Name: {0}, Street: {1} \n {2}", name, street, statisticsSet);
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