namespace PokerTell.LiveTracker.Interfaces
{
    using Infrastructure.Interfaces.PokerHand;
    using Infrastructure.Interfaces.Statistics;

    using ViewModels;

    public interface IPlayerOverlayViewModel
    {
        ITableOverlaySettingsViewModel Settings { get; }

        IPlayerStatisticsViewModel PlayerStatistics { get; }

        double Left { get; set; }

        double Top { get; set; }

        IPlayerStatusViewModel PlayerStatus { get; set; }

        IPlayerOverlayViewModel InitializeWith(ITableOverlaySettingsViewModel settings, int seatNumber);

        IPlayerOverlayViewModel UpdateWith(IPlayerStatisticsViewModel playerStatistics, IConvertedPokerPlayer pokerPlayer);

        IPlayerOverlayViewModel ShowHoleCardsFor(int showHoleCardsDuration);
    }
}