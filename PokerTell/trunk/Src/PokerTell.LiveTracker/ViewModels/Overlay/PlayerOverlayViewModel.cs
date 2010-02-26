namespace PokerTell.LiveTracker.ViewModels.Overlay
{
    using System.Windows;

    using PokerTell.Infrastructure.Interfaces.PokerHand;
    using PokerTell.Infrastructure.Interfaces.Statistics;
    using PokerTell.LiveTracker.Interfaces;

    using Tools.WPF.Interfaces;
    using Tools.WPF.ViewModels;

    public class PlayerOverlayViewModel : NotifyPropertyChanged, IPlayerOverlayViewModel
    {
        public PlayerOverlayViewModel(IPlayerStatusViewModel playerStatus)
        {
            PlayerStatus = playerStatus;
        }

        public IPlayerOverlayViewModel InitializeWith(ITableOverlaySettingsViewModel settings, int seatNumber)
        {
            Settings = settings;

            Position = Settings.PlayerStatisticsPanelPositions[seatNumber];

            PlayerStatus.InitializeWith(Settings.HoleCardsPositions[seatNumber], Settings.HarringtonMPositions[seatNumber]);
            return this;
        }

        public IPlayerOverlayViewModel UpdateWith(IPlayerStatisticsViewModel playerStatistics, IConvertedPokerPlayer pokerPlayer)
        {
            PlayerStatistics = playerStatistics;

            _mostRecentPokerPlayer = pokerPlayer;

            if (playerStatistics == null || pokerPlayer == null)
            {
                PlayerStatus.IsPresent = false;
                return this;
            }

            PlayerStatus.HarringtonM.Value = pokerPlayer.MAfter;
            PlayerStatus.IsPresent = true;
            return this;
        }

        public IPlayerOverlayViewModel ShowHoleCardsFor(int showHoleCardsDuration)
        {
            if (PlayerStatus.IsPresent && !string.IsNullOrEmpty(_mostRecentPokerPlayer.Holecards))
            {
                PlayerStatus.ShowHoleCardsFor(showHoleCardsDuration, _mostRecentPokerPlayer.Holecards);
            }

            return this;
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

        public IPositionViewModel Position { get; protected set;  }

        IConvertedPokerPlayer _mostRecentPokerPlayer;
    }
}