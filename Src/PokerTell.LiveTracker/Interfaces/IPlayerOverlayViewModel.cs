namespace PokerTell.LiveTracker.Interfaces
{
    using Infrastructure.Interfaces.PokerHand;
    using Infrastructure.Interfaces.Statistics;

    using Tools.WPF.Interfaces;

    using ViewModels;

    public interface IPlayerOverlayViewModel
    {
        ITableOverlaySettingsViewModel Settings { get; }

        IPlayerStatisticsViewModel PlayerStatistics { get; }

        IPlayerStatusViewModel PlayerStatus { get; set; }

        IPositionViewModel Position { get; }

        IPlayerOverlayViewModel InitializeWith(ITableOverlaySettingsViewModel settings, int seatNumber);

        IPlayerOverlayViewModel UpdateWith(IPlayerStatisticsViewModel playerStatistics, IConvertedPokerPlayer pokerPlayer);

        IPlayerOverlayViewModel ShowHoleCardsFor(int showHoleCardsDuration);
    }
}