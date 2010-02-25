namespace PokerTell.LiveTracker.Design.LiveTracker
{
    using System.Windows;

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

    public static class AutoWiring
    {
        public static IUnityContainer ConfigureTableOverlayDependencies()
        {
            TableOverlaySettingsDesignModel overlaySettings = GetOverlaySettings();

            var container = new UnityContainer()
                .RegisterType<IDispatcherTimer, DispatcherTimerAdapter>()
                .RegisterInstance<ITableOverlaySettingsViewModel>(overlaySettings)
                .RegisterType<IHarringtonMViewModel, HarringtonMViewModel>()
                .RegisterType<IHoleCardsViewModel, HoleCardsViewModel>()
                .RegisterType<IOverlayHoleCardsViewModel, OverlayHoleCardsViewModel>()
                .RegisterType<IBoardViewModel, BoardViewModel>()
                .RegisterType<IOverlayBoardViewModel, OverlayBoardViewModel>()
                .RegisterType<IPlayerStatusViewModel, PlayerStatusViewModel>()
                .RegisterType<IPlayerOverlayViewModel, PlayerOverlayViewModel>()
                .RegisterInstance(new SeatMapper().InitializeWith(6).UpdateWith(3))
                .RegisterInstance(new Mock<IGameHistoryViewModel>().Object);

            IPlayerOverlayViewModel[] playerOverlays = GetPlayerOverlays(overlaySettings, container);

            var pokerTableStatisticsViewModel = GetPokerTableStatisticsViewModel(6);

            container.RegisterInstance(pokerTableStatisticsViewModel);

            var tableOverlay = container.Resolve<TableOverlayViewModel>();
            tableOverlay
                .InitializeWith(
                container.Resolve<ISeatMapper>(),
                container.Resolve<ITableOverlaySettingsViewModel>(),
                container.Resolve<IGameHistoryViewModel>(),
                container.Resolve<IPokerTableStatisticsViewModel>(),
                playerOverlays,
                4)
                .UpdateWith(GetPokerPlayers(), string.Empty);

            container.RegisterInstance<ITableOverlayViewModel>(tableOverlay);

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

            return tableStatistics.Object;
        }

        static IPlayerOverlayViewModel[] GetPlayerOverlays(TableOverlaySettingsDesignModel overlaySettings, IUnityContainer container)
        {
            var po0 = container.Resolve<IPlayerOverlayViewModel>()
                .InitializeWith(overlaySettings, 0);
            var po1 = container.Resolve<IPlayerOverlayViewModel>()
                .InitializeWith(overlaySettings, 1);
            var po2 = container.Resolve<IPlayerOverlayViewModel>()
                .InitializeWith(overlaySettings, 2);
            var po3 = container.Resolve<IPlayerOverlayViewModel>()
                .InitializeWith(overlaySettings, 3);
            var po4 = container.Resolve<IPlayerOverlayViewModel>()
                .InitializeWith(overlaySettings, 4);
            var po5 = container.Resolve<IPlayerOverlayViewModel>()
                .InitializeWith(overlaySettings, 5);

            return new[] { po0, po1, po2, po3, po4, po5 };
        }

        static IConvertedPokerPlayer[] GetPokerPlayers()
        {
            return new[]
                {
                    PokerPlayerWithM(1, "player1", 4), PokerPlayerWithM(2, "player2", 6), PokerPlayerWithM(3, "player3", 9), 
                    PokerPlayerWithM(4, "player4", 14), PokerPlayerWithM(5, "player5", 22), PokerPlayerWithM(6, "player6", 40)
                };
        }

        static IConvertedPokerPlayer PokerPlayerWithM(int seat, string name, int mAfter)
        {
            var player = new Mock<IConvertedPokerPlayer>();
            player.SetupGet(p => p.SeatNumber).Returns(seat);
            player.SetupGet(p => p.MAfter).Returns(mAfter);
            player.SetupGet(p => p.Name).Returns(name);
            return player.Object;
        }

        static TableOverlaySettingsDesignModel GetOverlaySettings()
        {
            var statisticsPositions = new[]
                {
                    new Point(600, 20), 
                    new Point(650, 120), 
                    new Point(600, 220), 
                    new Point(300, 220), 
                    new Point(100, 120), 
                    new Point(300, 20), 
                };

            var harringtonMPositions = new[]
                {
                    new Point(550, 20), 
                    new Point(600, 120), 
                    new Point(550, 220), 
                    new Point(250, 220), 
                    new Point(50, 120), 
                    new Point(250, 20), 
                };

            var holeCardsPositions = new[]
                {
                    new Point(550, 10), 
                    new Point(600, 110), 
                    new Point(550, 210), 
                    new Point(250, 210), 
                    new Point(50, 110), 
                    new Point(250, 10), 
                };

            return new TableOverlaySettingsDesignModel(
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
                new Point(300, 300));
        }
    }
}