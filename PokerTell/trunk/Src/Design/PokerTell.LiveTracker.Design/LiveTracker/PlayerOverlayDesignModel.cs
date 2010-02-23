namespace PokerTell.LiveTracker.Design.LiveTracker
{
    using PokerTell.LiveTracker.Design.Statistics;
    using PokerTell.LiveTracker.Interfaces;
    using PokerTell.LiveTracker.ViewModels;
    using PokerTell.LiveTracker.ViewModels.Overlay;

    public class PlayerOverlayDesignModel : PlayerOverlayViewModel
    {
        public PlayerOverlayDesignModel(int seatNumber, IPlayerStatusViewModel playerStatus, double left, double top, ITableOverlaySettingsViewModel overlaySettings)
            : base(playerStatus)
        {
            InitializeWith(overlaySettings, seatNumber);

            Left = left;
            Top = top;
        }
    }

    public static class PlayerOverlayDesign
    {
        static readonly IPlayerStatusViewModel PlayerStatus = new PlayerStatusDesignModel(true, new HarringtonMDesignModel(10, 20, 40));

        public static PlayerOverlayDesignModel Model = new PlayerOverlayDesignModel(1, PlayerStatus, 50, 10, TableOverlaySettingsDesign.Model);
    }
}