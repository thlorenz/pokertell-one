namespace PokerTell.Statistics.ViewModels
{
    using System.Linq;
    using System.Windows.Input;

    using Infrastructure.Enumerations.PokerHand;
    using Infrastructure.Interfaces;
    using Infrastructure.Interfaces.Statistics;

    using Tools.WPF;
    using Tools.WPF.ViewModels;

    public class RepositoryPlayersStatisticsViewModel : NotifyPropertyChanged
    {
        readonly IConstructor<IPlayerStatisticsViewModel> _playerStatisticsViewModelMake;

        IPlayerStatisticsViewModel _selectedPlayer;

        ICommand _filterAdjustmentRequestedCommand;

        public RepositoryPlayersStatisticsViewModel(
            IConstructor<IPlayerStatisticsViewModel> playerStatisticsViewModelMake, 
            IDetailedStatisticsAnalyzerViewModel detailedStatisticsAnalyzerViewModel, 
            IFilterPopupViewModel filterPopupViewModel)
        {
            _playerStatisticsViewModelMake = playerStatisticsViewModelMake;
            DetailedStatisticsAnalyzer = detailedStatisticsAnalyzerViewModel;
            FilterPopup = filterPopupViewModel;
        }

        public IFilterPopupViewModel FilterPopup { get; set; }

        public IDetailedStatisticsAnalyzerViewModel DetailedStatisticsAnalyzer { get; protected set; }

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

        public ICommand FilterAdjustmentRequestedCommand
        {
            get
            {
                return _filterAdjustmentRequestedCommand ?? (_filterAdjustmentRequestedCommand = new SimpleCommand {
                        ExecuteDelegate = arg => FilterPopup.ShowFilter(SelectedPlayer.PlayerName, SelectedPlayer.Filter, ApplyFilterTo), 
                        CanExecuteDelegate = arg => SelectedPlayer != null
                    });
            }
        }

        protected void ApplyFilterTo(string ignore, IAnalyzablePokerPlayersFilter adjustedFilter)
        {
            if (SelectedPlayer != null)
            {
                SelectedPlayer.Filter = adjustedFilter;
            }
        }

        protected virtual void BrowseAllHandsOf(IPlayerStatisticsViewModel selectedPlayer)
        {
            if (selectedPlayer != null && selectedPlayer.PlayerStatistics != null)
            {
                var activeAnalyzablePlayers =
                    from player in selectedPlayer.PlayerStatistics.FilteredAnalyzablePokerPlayers
                    where
                        player.ActionSequences[(int)Streets.PreFlop] != ActionSequences.HeroF &&
                        player.ActionSequences[(int)Streets.PreFlop] != ActionSequences.OppRHeroF &&
                        player.ActionSequences[(int)Streets.PreFlop] != ActionSequences.NonStandard
                    select player;

                if (activeAnalyzablePlayers.Count() > 0)
                    DetailedStatisticsAnalyzer.InitializeWith(activeAnalyzablePlayers, selectedPlayer.PlayerName);
            }
        }
    }
}