namespace PokerTell.LiveTracker.Interfaces
{
    public interface IOverlayStatisticsPanelSettingsViewModel
    {
        bool ShowPreFlop { get; set; }

        bool ShowFlop { get; set; }

        bool ShowTurn { get; set; }

        bool ShowRiver { get; set; }

        double Width { get; set; }

        double Height { get; set; }

        double Opacity { get; set; }
    }
}