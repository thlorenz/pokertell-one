namespace PokerTell.LiveTracker.Design.LiveTracker
{
    using System.Collections.Generic;
    using System.Windows;

    using Infrastructure.Interfaces.Statistics;

    using Microsoft.Practices.Unity;

    using Moq;

    using PokerTell.Infrastructure.Interfaces.PokerHand;
    using PokerTell.LiveTracker.Design.Statistics;
    using PokerTell.LiveTracker.Interfaces;
    using PokerTell.LiveTracker.Overlay;
    using PokerTell.LiveTracker.ViewModels.Overlay;
    using PokerTell.PokerHand.ViewModels;

    using Tools.FunctionalCSharp;
    using Tools.Interfaces;
    using Tools.WPF.Interfaces;
    using Tools.WPF.ViewModels;

    public static class AutoWiring
    {
        public static IUnityContainer ConfigureTableOverlayDependencies()
        {
            var layoutManager = new LayoutManager(new LayoutXDocumentHandler());
            ITableOverlaySettingsViewModel overlaySettings = layoutManager.Load("PokerStars", 6);

            var container = new UnityContainer()
                .RegisterType<IDispatcherTimer, DispatcherTimerAdapter>()
                .RegisterType<IPositionViewModel, PositionViewModel>()
                .RegisterInstance(overlaySettings)
                .RegisterType<IHarringtonMViewModel, HarringtonMViewModel>()
                .RegisterType<IHoleCardsViewModel, HoleCardsViewModel>()
                .RegisterType<IOverlayHoleCardsViewModel, OverlayHoleCardsViewModel>()
                .RegisterType<IBoardViewModel, BoardViewModel>()
                .RegisterType<IOverlayBoardViewModel, OverlayBoardViewModel>()
                .RegisterType<IPlayerStatusViewModel, PlayerStatusViewModel>()
                .RegisterType<IPlayerOverlayViewModel, PlayerOverlayViewModel>()
                .RegisterInstance(new SeatMapper().InitializeWith(6).UpdateWith(3))
                .RegisterInstance<IGameHistoryViewModel>(new GameHistoryDesignModel());

            container
                .RegisterConstructor(() => container.Resolve<IOverlayHoleCardsViewModel>())
                .RegisterType<IOverlaySettingsAidViewModel, OverlaySettingsAidViewModel>();

            IPlayerOverlayViewModel[] playerOverlays = GetPlayerOverlays(container);

            var pokerTableStatisticsViewModel = GetPokerTableStatisticsViewModel(6);

            container.RegisterInstance(pokerTableStatisticsViewModel);

            var tableOverlay = container.Resolve<TableOverlayViewModel>();
            tableOverlay
                .InitializeWith(
                container.Resolve<ISeatMapper>(), 
                container.Resolve<ITableOverlaySettingsViewModel>(), 
                container.Resolve<IGameHistoryViewModel>(), 
                container.Resolve<IPokerTableStatisticsViewModel>(), 
                4)
                .UpdateWith(GetPokerPlayers(), "As Kh Qh");

            container.RegisterInstance<ITableOverlayViewModel>(tableOverlay);

            overlaySettings.SaveChanges += () => layoutManager.Save(overlaySettings, "PokerStars");
            overlaySettings.UndoChanges += revertTo => revertTo(layoutManager.Load("PokerStars", 6));

            return container;
        }

        static IPokerTableStatisticsViewModel GetPokerTableStatisticsViewModel(int numOfSeats)
        {
            var tableStatistics = new Mock<IPokerTableStatisticsViewModel>();
            foreach (int seat in 0.To(numOfSeats - 1))
            {
                var seatIndex = seat;
                tableStatistics
                    .Setup(s => s.GetPlayerStatisticsViewModelFor("player" + (seatIndex + 1)))
                    .Returns(new PlayerStatisticsDesignModel(seatIndex));
            }

            var statisticsAnalyzer = new Mock<IDetailedStatisticsAnalyzerViewModel>();
            statisticsAnalyzer
                .SetupGet(sa => sa.CurrentViewModel)
                .Returns(new DetailedPreFlopStatisticsDesignModel());
            tableStatistics
                .SetupGet(s => s.DetailedStatisticsAnalyzer)
                .Returns(statisticsAnalyzer.Object);
            return tableStatistics.Object;
        }

        static IPlayerOverlayViewModel[] GetPlayerOverlays(IUnityContainer container)
        {
            var po0 = container.Resolve<IPlayerOverlayViewModel>();
            var po1 = container.Resolve<IPlayerOverlayViewModel>();
            var po2 = container.Resolve<IPlayerOverlayViewModel>();
            var po3 = container.Resolve<IPlayerOverlayViewModel>();
            var po4 = container.Resolve<IPlayerOverlayViewModel>();
            var po5 = container.Resolve<IPlayerOverlayViewModel>();

            return new[] { po0, po1, po2, po3, po4, po5 };
        }

        internal static IConvertedPokerPlayer[] GetPokerPlayers()
        {
            return new[]
                {
                    PokerPlayerWith(1, "player1", 4), PokerPlayerWith(2, "player2", 6), PokerPlayerWith(3, "player3", 9), 
                    PokerPlayerWith(4, "player4", 14), PokerPlayerWith(5, "player5", 22), PokerPlayerWith(6, "player6", 40)
                };
        }

        internal static IEnumerable<Mock<IConvertedPokerPlayer>> GetPokerPlayerStubs()
        {
            return new[]
                {
                    PokerPlayerStubWith(1, "player1", 4), PokerPlayerStubWith(2, "player2", 6), PokerPlayerStubWith(3, "player3", 9), 
                    PokerPlayerStubWith(4, "player4", 14), PokerPlayerStubWith(5, "player5", 22), PokerPlayerStubWith(6, "player6", 40)
                };
        }

        internal static IConvertedPokerPlayer PokerPlayerWith(int seat, string name, int mAfter)
        {
            return PokerPlayerStubWith(seat, name, mAfter).Object;
        }

        internal static Mock<IConvertedPokerPlayer> PokerPlayerStubWith(int seat, string name, int mAfter)
        {
            var playerStub = new Mock<IConvertedPokerPlayer>();
            playerStub.SetupGet(p => p.SeatNumber).Returns(seat);
            playerStub.SetupGet(p => p.MAfter).Returns(mAfter);
            playerStub.SetupGet(p => p.Name).Returns(name);
            return playerStub;
        }

        static ITableOverlaySettingsViewModel GetOverlaySettings()
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