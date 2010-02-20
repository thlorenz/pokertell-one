namespace PokerTell.LiveTracker.ViewModels.Overlay
{
    using Infrastructure.Interfaces.Statistics;

    using Interfaces;

    using Tools.WPF.ViewModels;

    public class OverlayStatisticsPanelViewModel : NotifyPropertyChanged, IOverlayStatisticsPanelViewModel
    {
        public IOverlayStatisticsPanelViewModel InitializeWith(IPlayerStatisticsViewModel playerStatistics, IOverlayStatisticsPanelSettingsViewModel settings)
        {
            PlayerStatistics = playerStatistics;
            Settings = settings;
            return this;
        }

        public IOverlayStatisticsPanelSettingsViewModel Settings { get; set; }

        public IPlayerStatisticsViewModel PlayerStatistics { get; set; }

        public double Left { get; set; }

        public double Top { get; set; }
    }
}