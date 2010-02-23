namespace PokerTell.LiveTracker.Design.LiveTracker
{
    using PokerTell.LiveTracker.ViewModels.Overlay;

    using Tools.WPF.ViewModels;

    public class TableOverlaySettingsDesignModel : TableOverlaySettingsViewModel
    {
        public TableOverlaySettingsDesignModel(bool showPreFlop, bool showFlop, bool showTurn, bool showRiver, bool showHarringtonM, double width, double height, string background, string outOfPositionForeground, string inPositionForeground, int preferredSeat)
        {
            ShowPreFlop = showPreFlop;
            ShowFlop = showFlop;
            ShowTurn = showTurn;
            ShowRiver = showRiver;

            ShowHarringtonM = showHarringtonM;

            Width = width;
            Height = height;

            Background = new ColorViewModel(background);

            OutOfPositionForeground = new ColorViewModel(outOfPositionForeground);
            InPositionForeground = new ColorViewModel(inPositionForeground);

            PreferredSeat = preferredSeat;
        }
    }

    public static class TableOverlaySettingsDesign
    {
        public static TableOverlaySettingsViewModel Model
        {
            get { return new TableOverlaySettingsDesignModel(true, true, true, false, true, 100, 50, "#FF0000FF", "White", "Yellow", 0); }
        }
    }
}