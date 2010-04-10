namespace PokerTell.Statistics.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Input;

    using PokerTell.Infrastructure.Interfaces;
    using PokerTell.Infrastructure.Interfaces.PokerHand;
    using PokerTell.Infrastructure.Interfaces.Repository;
    using PokerTell.Infrastructure.Interfaces.Statistics;
    using PokerTell.Statistics.Interfaces;

    using Tools.WPF;
    using Tools.WPF.ViewModels;

    public class RepositoryPlayersStatisticsViewModel : ItemsRegionViewModel, IRepositoryPlayersStatisticsViewModel
    {
        readonly IActiveAnalyzablePlayersSelector _activePlayersSelector;

        readonly IConstructor<IPlayerStatistics> _playerStatisticsMake;

        readonly IPlayerStatisticsViewModel _playerStatisticsViewModel;

        readonly IRepository _repository;

        ICommand _filterAdjustmentRequestedCommand;

        IPlayerStatisticsViewModel _selectedPlayer;

        IPlayerIdentity _selectedPlayerIdentity;

        readonly IPlayerStatisticsUpdater _playerStatisticsUpdater;

        public RepositoryPlayersStatisticsViewModel(
            IRepository repository, 
            IConstructor<IPlayerStatistics> playerStatisticsMake, 
            IPlayerStatisticsUpdater playerStatisticsUpdater, 
            IPlayerStatisticsViewModel playerStatisticsViewModel, 
            IDetailedStatisticsAnalyzerViewModel detailedStatisticsAnalyzerViewModel, 
            IActiveAnalyzablePlayersSelector activePlayersSelector, 
            IFilterPopupViewModel filterPopupViewModel)
        {
            _repository = repository;
            _playerStatisticsMake = playerStatisticsMake;
            _playerStatisticsUpdater = playerStatisticsUpdater;
            _playerStatisticsViewModel = playerStatisticsViewModel;
            DetailedStatisticsAnalyzer = detailedStatisticsAnalyzerViewModel;
            _activePlayersSelector = activePlayersSelector;
            FilterPopup = filterPopupViewModel;

            PlayerIdentities = new List<IPlayerIdentity>(_repository.RetrieveAllPlayerIdentities().OrderBy(pi => pi.Name));

            RegisterEvents();

            HeaderInfo = "Main";
        }

        void RegisterEvents()
        {
            _playerStatisticsUpdater.FinishedUpdatingPlayerStatistics += playerStatistics => {
               SelectedPlayer = _playerStatisticsViewModel.UpdateWith(playerStatistics);

               var activeAnalyzablePlayers = _activePlayersSelector.SelectFrom(playerStatistics);
               DetailedStatisticsAnalyzer.InitializeWith(activeAnalyzablePlayers, SelectedPlayer.PlayerName);
            };
        }


        public IDetailedStatisticsAnalyzerViewModel DetailedStatisticsAnalyzer { get; protected set; }

        public ICommand FilterAdjustmentRequestedCommand
        {
            get
            {
                return _filterAdjustmentRequestedCommand ?? (_filterAdjustmentRequestedCommand = new SimpleCommand
                    {
                        ExecuteDelegate = arg => FilterPopup.ShowFilter(SelectedPlayer.PlayerName, SelectedPlayer.Filter, ApplyFilterTo), 
                        CanExecuteDelegate = arg => SelectedPlayer != null
                    });
            }
        }

        public IFilterPopupViewModel FilterPopup { get; protected set; }

        public IList<IPlayerIdentity> PlayerIdentities { get; protected set; }

        public IPlayerStatisticsViewModel SelectedPlayer
        {
            get { return _selectedPlayer; }
            protected set
            {
                _selectedPlayer = value;
                RaisePropertyChanged(() => SelectedPlayer);
            }
        }

        public IPlayerIdentity SelectedPlayerIdentity
        {
            get { return _selectedPlayerIdentity; }
            set
            {
                _selectedPlayerIdentity = value;
                
                var playerStatistics = _playerStatisticsMake.New.InitializePlayer(_selectedPlayerIdentity.Name, _selectedPlayerIdentity.Site);
                _playerStatisticsUpdater.Update(playerStatistics);
            }
        }

        protected void ApplyFilterTo(string ignore, IAnalyzablePokerPlayersFilter adjustedFilter)
        {
            if (SelectedPlayer != null)
            {
                SelectedPlayer.Filter = adjustedFilter;
            }
        }
    }
}