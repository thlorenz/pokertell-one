namespace PokerTell.LiveTracker.Interfaces
{
    using Tools.WPF.Interfaces;

    public interface ITableOverlaySettingsViewModel
    {
        bool ShowPreFlop { get; set; }

        bool ShowFlop { get; set; }

        bool ShowTurn { get; set; }

        bool ShowRiver { get; set; }

        double Width { get; set; }

        double Height { get; set; }

        IColorViewModel Background { get; set; }

        IColorViewModel InPositionForeground { get; set; }

        IColorViewModel OutOfPositionForeground { get; set; }

        int PreferredSeat { get; set; }

        bool ShowHarringtonM { get; set; }
    }
}