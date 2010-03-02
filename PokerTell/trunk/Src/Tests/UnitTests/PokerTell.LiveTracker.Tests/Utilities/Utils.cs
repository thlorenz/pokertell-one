namespace PokerTell.LiveTracker.Tests.Utilities
{
    using Moq;

    using PokerTell.Infrastructure.Interfaces.Statistics;
    using PokerTell.LiveTracker.Interfaces;
    using PokerTell.LiveTracker.ViewModels.Overlay;

    using Tools.WPF.ViewModels;

    internal static class Utils
    {
        internal static Mock<IPlayerStatisticsViewModel> PlayerStatisticsVM_MockFor(string name)
        {
            var playerViewModelMock = new Mock<IPlayerStatisticsViewModel>();
            playerViewModelMock.SetupGet(pvm => pvm.PlayerName).Returns(name);
            return playerViewModelMock;
        }

        internal static ITableOverlaySettingsViewModel GetOverlaySettings_2Max()
        {
            var statisticsPositions = new[]
                {
                    new PositionViewModel(600, 20), 
                    new PositionViewModel(300, 220), 
                };

            var harringtonMPositions = new[]
                {
                    new PositionViewModel(600, 120), 
                    new PositionViewModel(50, 120), 
                };

            var holeCardsPositions = new[]
                {
                    new PositionViewModel(550, 10), 
                    new PositionViewModel(550, 210), 
                };

            return new TableOverlaySettingsViewModel().InitializeWith(
                2,
                true,
                true,
                true,
                true,
                true,
                120,
                55,
                "#FF0000FF",
                "White",
                "Yellow",
                0,
                statisticsPositions,
                harringtonMPositions,
                holeCardsPositions,
                new PositionViewModel(300, 10),
                new PositionViewModel(300, 210),
                400,
                200);
        }

        internal static ITableOverlaySettingsViewModel GetOverlaySettings_5Max()
        {
            var statisticsPositions = new[]
                {
                    new PositionViewModel(600, 20), 
                    new PositionViewModel(650, 120), 
                    new PositionViewModel(600, 220), 
                    new PositionViewModel(300, 220), 
                    new PositionViewModel(100, 120), 
                };

            var harringtonMPositions = new[]
                {
                    new PositionViewModel(550, 20), 
                    new PositionViewModel(600, 120), 
                    new PositionViewModel(550, 220), 
                    new PositionViewModel(250, 220), 
                    new PositionViewModel(50, 120), 
                };

            var holeCardsPositions = new[]
                {
                    new PositionViewModel(550, 10), 
                    new PositionViewModel(600, 110), 
                    new PositionViewModel(550, 210), 
                    new PositionViewModel(250, 210), 
                    new PositionViewModel(50, 110), 
                };

            return new TableOverlaySettingsViewModel().InitializeWith(
                5,
                true, 
                true, 
                true, 
                true, 
                true, 
                120, 
                55, 
                "#FF0000FF", 
                "White", 
                "Yellow", 
                0, 
                statisticsPositions, 
                harringtonMPositions, 
                holeCardsPositions, 
                new PositionViewModel(300, 10), 
                new PositionViewModel(300, 210),
                400,
                200);
        }

        internal static ITableOverlaySettingsViewModel GetOverlaySettings_6Max()
        {
            var statisticsPositions = new[]
                {
                    new PositionViewModel(600, 20), 
                    new PositionViewModel(650, 120), 
                    new PositionViewModel(600, 220), 
                    new PositionViewModel(300, 220), 
                    new PositionViewModel(100, 120), 
                    new PositionViewModel(300, 20), 
                };

            var harringtonMPositions = new[]
                {
                    new PositionViewModel(550, 20), 
                    new PositionViewModel(600, 120), 
                    new PositionViewModel(550, 220), 
                    new PositionViewModel(250, 220), 
                    new PositionViewModel(50, 120), 
                    new PositionViewModel(250, 20), 
                };

            var holeCardsPositions = new[]
                {
                    new PositionViewModel(550, 10), 
                    new PositionViewModel(600, 110), 
                    new PositionViewModel(550, 210), 
                    new PositionViewModel(250, 210), 
                    new PositionViewModel(50, 110), 
                    new PositionViewModel(250, 10), 
                };

            return new TableOverlaySettingsViewModel().InitializeWith(
                6, 
                true, 
                true, 
                true, 
                true, 
                true, 
                100, 
                50, 
                "#FF0000FF", 
                "White", 
                "Yellow", 
                0, 
                statisticsPositions, 
                harringtonMPositions, 
                holeCardsPositions, 
                new PositionViewModel(300, 10), 
                new PositionViewModel(300, 210),
                400,
                200);
        }
    }
}