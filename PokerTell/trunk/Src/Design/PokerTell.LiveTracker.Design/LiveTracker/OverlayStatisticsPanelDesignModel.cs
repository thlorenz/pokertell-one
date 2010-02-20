namespace PokerTell.LiveTracker.Design.LiveTracker
{
    using PokerTell.LiveTracker.Design.Statistics;
    using PokerTell.LiveTracker.ViewModels.Overlay;

    public class OverlayStatisticsPanelDesignModel : OverlayStatisticsPanelViewModel
    {
        public OverlayStatisticsPanelDesignModel()
        {
            InitializeWith(new PlayerStatisticsDesignModel(), new OverlayStatisticsPanelSettingsDesignModel(true, true, true, false, 200, 100, 0.8));

            Left = 50;
            Top = 10;
        }
    }

    public static class OverlayStatisticsPanelDesign
    {
        public static OverlayStatisticsPanelDesignModel Model = new OverlayStatisticsPanelDesignModel();
    }
}