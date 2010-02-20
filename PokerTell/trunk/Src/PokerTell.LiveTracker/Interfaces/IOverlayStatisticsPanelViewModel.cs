namespace PokerTell.LiveTracker.Interfaces
{
    using Infrastructure.Interfaces.Statistics;

    public interface IOverlayStatisticsPanelViewModel
    {
        IOverlayStatisticsPanelSettingsViewModel Settings { get; }

        IPlayerStatisticsViewModel PlayerStatistics { get; }

        double Left { get; set; }

        double Top { get; set; }

        IOverlayStatisticsPanelViewModel InitializeWith(IPlayerStatisticsViewModel playerStatistics, IOverlayStatisticsPanelSettingsViewModel settings);
    }
}