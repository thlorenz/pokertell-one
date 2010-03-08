namespace PokerTell.LiveTracker.Design.LiveTracker
{
    using System.Linq;

    using PokerTell.LiveTracker.Design.Statistics;
    using PokerTell.LiveTracker.Interfaces;
    using PokerTell.LiveTracker.ViewModels.Overlay;

    using Tools.Interfaces;

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
            AutoWiring
                .ConfigureTableOverlayDependencies()
                .Resolve<ITableOverlayViewModel>()
                .PlayerOverlays
                .First();
    }
}