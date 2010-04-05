namespace PokerTell.LiveTracker.ViewModels.Overlay
{
    using System;
    using System.Windows.Input;

    using PokerTell.Infrastructure.Interfaces.PokerHand;
    using PokerTell.Infrastructure.Interfaces.Statistics;
    using PokerTell.LiveTracker.Interfaces;

    using Tools.WPF;
    using Tools.WPF.Interfaces;
    using Tools.WPF.ViewModels;

    public class PlayerOverlayViewModel : NotifyPropertyChanged, IPlayerOverlayViewModel
    {
        public PlayerOverlayViewModel(IPlayerStatusViewModel playerStatus)
        {
            PlayerStatus = playerStatus;
        }

        public ITableOverlaySettingsViewModel Settings { get; set; }

        IPlayerStatisticsViewModel _playerStatistics;

        public IPlayerStatisticsViewModel PlayerStatistics
        {
            get { return _playerStatistics; }
            set
            {
                _playerStatistics = value;
                RaisePropertyChanged(() => PlayerStatistics);
            }
        }

        public IPlayerStatusViewModel PlayerStatus { get; set; }

        public IPositionViewModel Position { get; protected set; }

        public string PlayerName { get; protected set; }

        bool _isPresentAndHasStatistics;

        public bool IsPresentAndHasStatistics
        {
            get { return _isPresentAndHasStatistics; }
            protected set
            {
                _isPresentAndHasStatistics = value;
                RaisePropertyChanged(() => IsPresentAndHasStatistics);
            }
        }

        IConvertedPokerPlayer _mostRecentPokerPlayer;

        public IPlayerOverlayViewModel InitializeWith(ITableOverlaySettingsViewModel settings, int seatNumber)
        {
            Settings = settings;

            Position = Settings.PlayerStatisticsPanelPositions[seatNumber];

            PlayerStatus.InitializeWith(Settings.HoleCardsPositions[seatNumber], Settings.HarringtonMPositions[seatNumber]);
            return this;
        }

        public IPlayerOverlayViewModel UpdateStatusWith(IConvertedPokerPlayer pokerPlayer)
        {
            _mostRecentPokerPlayer = pokerPlayer;

            if (pokerPlayer == null)
            {
                PlayerStatus.IsPresent = false;
                return this;
            }

            PlayerName = pokerPlayer.Name;
            PlayerStatus.HarringtonM.Value = pokerPlayer.MAfter;

            // An M value <= 0 indicates that he was eliminated during the last hand
            PlayerStatus.IsPresent = pokerPlayer.MAfter > 0;

            return this;
        }

        public void UpdateStatisticsWith(IPlayerStatisticsViewModel playerStatistics)
        {
            if (playerStatistics == null)
            {
                IsPresentAndHasStatistics = false;
            }
            else
            {
                PlayerStatistics = playerStatistics;
                IsPresentAndHasStatistics = PlayerStatus.IsPresent;
            }
        }

        public IPlayerOverlayViewModel ShowHoleCardsFor(int showHoleCardsDuration)
        {
            if (PlayerStatus.IsPresent && !string.IsNullOrEmpty(_mostRecentPokerPlayer.Holecards))
            {
                PlayerStatus.ShowHoleCardsFor(showHoleCardsDuration, _mostRecentPokerPlayer.Holecards);
            }

            return this;
        }

        ICommand _filterAdjustmentRequestedCommand;

        public ICommand FilterAdjustmentRequestedCommand
        {
            get
            {
                return _filterAdjustmentRequestedCommand ?? (_filterAdjustmentRequestedCommand = new SimpleCommand
                    {
                        ExecuteDelegate = arg => FilterAdjustmentRequested(PlayerStatistics)
                    });
            }
        }

        public event Action<IPlayerStatisticsViewModel> FilterAdjustmentRequested = delegate { };
    }
}