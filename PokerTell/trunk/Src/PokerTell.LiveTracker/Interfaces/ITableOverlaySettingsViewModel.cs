namespace PokerTell.LiveTracker.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Windows;

    using Tools.Interfaces;
    using Tools.WPF.Interfaces;

    public interface ITableOverlaySettingsViewModel : IFluentInterface
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

        IList<IPositionViewModel> PlayerStatisticsPanelPositions { get; }

        IPositionViewModel BoardPosition { get; set; }

        IList<IPositionViewModel> HarringtonMPositions { get; }

        IList<IPositionViewModel> HoleCardsPositions { get; }

        int TotalSeats { get; set; }

        bool PositioningMuckedCards { get; set; }

        event Action PreferredSeatChanged;
    }
}