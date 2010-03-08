namespace PokerTell.LiveTracker.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Input;

    using Tools.Interfaces;
    using Tools.WPF.Interfaces;

    public interface ITableOverlaySettingsViewModel : IFluentInterface
    {
        bool ShowPreFlop { get; set; }

        bool ShowFlop { get; set; }

        bool ShowTurn { get; set; }

        bool ShowRiver { get; set; }

        double StatisticsPanelWidth { get; set; }

        double StatisticsPanelHeight { get; set; }

        IColorViewModel Background { get; set; }

        IColorViewModel InPositionForeground { get; set; }

        IColorViewModel OutOfPositionForeground { get; set; }

        int PreferredSeat { get; set; }

        bool ShowHarringtonM { get; set; }

        IList<IPositionViewModel> PlayerStatisticsPanelPositions { get; }

        IPositionViewModel BoardPosition { get; set; }

        IList<IPositionViewModel> HarringtonMPositions { get; }

        IList<IPositionViewModel> HoleCardsPositions { get; }

        int TotalSeats { get; }

        bool PositioningMuckedCards { get; set; }

        ICommand SaveChangesCommand { get; }

        ICommand UndoChangesCommand { get; }

        double OverlayDetailsWidth { get; set; }

        double OverlayDetailsHeight { get; set; }

        IPositionViewModel OverlayDetailsPosition { get; set; }

        event Action PreferredSeatChanged;

        ITableOverlaySettingsViewModel InitializeWith(int totalSeats, bool showPreFlop, bool showFlop, bool showTurn, bool showRiver, bool showHarringtonM, double statisticsPanelWidth, double statisticsPanelHeight, string background, string outOfPositionForeground, string inPositionForeground, int preferredSeat, IList<IPositionViewModel> playerStatisticPositions, IList<IPositionViewModel> harringtonMPositions, IList<IPositionViewModel> holeCardsPositions, IPositionViewModel boardPosition, IPositionViewModel overlayDetailsPosition, double overlayDetailsWidth, double overlayDetailsHeight);

        event Action SaveChanges;

        event Action<Action<ITableOverlaySettingsViewModel>> UndoChanges;

        ITableOverlaySettingsViewModel RevertTo(ITableOverlaySettingsViewModel os);
    }
}