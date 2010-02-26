namespace PokerTell.LiveTracker.Design.LiveTracker
{
    using System.Collections.Generic;
    using System.Windows;

    using Interfaces;

    using PokerTell.LiveTracker.ViewModels.Overlay;

    using Tools.WPF.Interfaces;
    using Tools.WPF.ViewModels;

    public class TableOverlaySettingsDesignModel : TableOverlaySettingsViewModel
    {
        public TableOverlaySettingsDesignModel(
            int totalSeats,
            bool showPreFlop, 
            bool showFlop, 
            bool showTurn, 
            bool showRiver, 
            bool showHarringtonM, 
            double width, 
            double height, 
            string background, 
            string outOfPositionForeground, 
            string inPositionForeground, 
            int preferredSeat, 
            IEnumerable<IPositionViewModel> playerStatisticPositions, 
            IList<IPositionViewModel> harringtonMPositions, 
            IList<IPositionViewModel> holeCardsPositions, 
            IPositionViewModel boardPosition)
        {
            TotalSeats = totalSeats;
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
            PlayerStatisticsPanelPositions = new List<IPositionViewModel>(playerStatisticPositions);

            HarringtonMPositions = harringtonMPositions;
            HoleCardsPositions = holeCardsPositions;
            BoardPosition = boardPosition;
        }
    }

    public static class TableOverlaySettingsDesign
    {
        static readonly IEnumerable<Point> StatisticsPositions = new[]
            {
                new Point(600, 20), 
                new Point(650, 120), 
                new Point(600, 220), 
                new Point(300, 220), 
                new Point(100, 120), 
                new Point(300, 20), 
            };

        public static ITableOverlaySettingsViewModel Model
        {
            get
            {
                return AutoWiring.ConfigureTableOverlayDependencies().Resolve<ITableOverlaySettingsViewModel>();
            }
        }
    }
}