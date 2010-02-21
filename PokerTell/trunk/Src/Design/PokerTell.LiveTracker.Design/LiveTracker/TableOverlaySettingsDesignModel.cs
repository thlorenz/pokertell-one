namespace PokerTell.LiveTracker.Design.LiveTracker
{
    using ViewModels.Overlay;

    public class TableOverlaySettingsDesignModel : TableOverlaySettingsViewModel
    {
        public TableOverlaySettingsDesignModel(
            bool showPreFlop, bool showFlop, bool showTurn, bool showRiver, double width, double height, double opacity, string background, string outOfPositionForeground, string inPositionForeground)
        {
            ShowPreFlop = showPreFlop;
            ShowFlop = showFlop;
            ShowTurn = showTurn;
            ShowRiver = showRiver;

            Width = width;
            Height = height;

            Opacity = opacity;
            Background = background;

            OutOfPositionForeground = outOfPositionForeground;
            InPositionForeground = inPositionForeground;
        }
    }
}