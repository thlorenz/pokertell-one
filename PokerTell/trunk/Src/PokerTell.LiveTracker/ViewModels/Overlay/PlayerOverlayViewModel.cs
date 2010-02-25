namespace PokerTell.LiveTracker.ViewModels.Overlay
{
    using System.Windows;

    using PokerTell.Infrastructure.Interfaces.PokerHand;
    using PokerTell.Infrastructure.Interfaces.Statistics;
    using PokerTell.LiveTracker.Interfaces;

    using Tools.WPF.ViewModels;

    public class PlayerOverlayViewModel : NotifyPropertyChanged, IPlayerOverlayViewModel
    {
        public PlayerOverlayViewModel(IPlayerStatusViewModel playerStatus)
        {
            PlayerStatus = playerStatus;
        }

        public IPlayerOverlayViewModel InitializeWith(ITableOverlaySettingsViewModel settings, int seatNumber)
        {
            _seatNumber = seatNumber;
            Settings = settings;

            PlayerStatus.InitializeWith(Settings.HoleCardsPositions, Settings.HarringtonMPositions, seatNumber);
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

        public double Left
        {
            get { return Settings.PlayerStatisticsPanelPositions[_seatNumber].X; }
            set
            {
                Settings.PlayerStatisticsPanelPositions[_seatNumber] = new Point(value, Top);
                RaisePropertyChanged(() => Left);
            }
        }

        int _seatNumber;

        IConvertedPokerPlayer _mostRecentPokerPlayer;

        public double Top
        {
            get { return Settings.PlayerStatisticsPanelPositions[_seatNumber].Y; }
            set
            {
                Settings.PlayerStatisticsPanelPositions[_seatNumber] = new Point(Top, value);
                RaisePropertyChanged(() => Top);
            }
        }
    }
}