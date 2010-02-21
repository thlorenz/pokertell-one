namespace PokerTell.LiveTracker.ViewModels.Overlay
{
    using PokerTell.Infrastructure.Interfaces.Statistics;
    using PokerTell.LiveTracker.Interfaces;

    using Tools.WPF.ViewModels;

    public class PlayerOverlayViewModel : NotifyPropertyChanged, IPlayerOverlayViewModel
    {
        public PlayerOverlayViewModel(IPlayerStatusViewModel playerStatus)
        {
            PlayerStatus = playerStatus;
        }

        public IPlayerOverlayViewModel InitializeWith(IPlayerStatisticsViewModel playerStatistics, ITableOverlaySettingsViewModel settings)
        {
            PlayerStatistics = playerStatistics;
            Settings = settings;
            return this;
        }

        // UpdateWith PlayerStatistics and PlayerStatus?
        public ITableOverlaySettingsViewModel Settings { get; set; }

        public IPlayerStatisticsViewModel PlayerStatistics { get; set; }

        public IPlayerStatusViewModel PlayerStatus { get; set; }


        double _left;

        public double Left
        {
            get { return _left; }
            set
            {
                _left = value;
                RaisePropertyChanged(() => Left);
            }
        }

        double _top;

        public double Top
        {
            get { return _top; }
            set
            {
                _top = value;
                RaisePropertyChanged(() => Top);
            }
        }
    }
}