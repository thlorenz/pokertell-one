namespace PokerTell.LiveTracker.Design.LiveTracker
{
    using ViewModels.Overlay;

    public class OverlayStatisticsPanelSettingsDesignModel : OverlayStatisticsPanelSettingsViewModel
    {
        public OverlayStatisticsPanelSettingsDesignModel(
            bool showPreFlop, bool showFlop, bool showTurn, bool showRiver, double width, double height, double opacity)
        {
            ShowPreFlop = showPreFlop;
            ShowFlop = showFlop;
            ShowTurn = showTurn;
            ShowRiver = showRiver;

            Width = width;
            Height = height;

            Opacity = opacity;
            Background = "#FF0000FF";
        }
    }
}