namespace PokerTell.LiveTracker.Design.LiveTracker
{
    using System.Linq;
    using Microsoft.Practices.Unity;
    using PokerTell.LiveTracker.Interfaces;
    using PokerTell.LiveTracker.ViewModels.Overlay;
    using PokerTell.Statistics.Design.DetailedStatistics;

    public class PlayerOverlayDesignModel : PlayerOverlayViewModel
    {
        public PlayerOverlayDesignModel(IPlayerStatusViewModel playerStatus, ITableOverlaySettingsViewModel overlaySettings)
            : this(0, playerStatus, overlaySettings)
        {
        }

        public PlayerOverlayDesignModel(int seatNumber, IPlayerStatusViewModel playerStatus, ITableOverlaySettingsViewModel overlaySettings)
            : base(playerStatus)
        {
            InitializeWith(overlaySettings, seatNumber);

            PlayerStatistics = new PlayerStatisticsDesignModel(seatNumber);
        }
    }

    public static class PlayerOverlayDesign
    {
        public static IPlayerOverlayViewModel Model =
            AutoWirerForTableOverlay
                .ConfigureTableOverlayDependencies()
                .Resolve<ITableOverlayViewModel>()
                .PlayerOverlays
                .First();
    }
}