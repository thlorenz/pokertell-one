namespace PokerTell.LiveTracker.ManualTests.NewHandCreator
{
    using System.Reflection;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    using Interfaces;

    using log4net;

    using Microsoft.Practices.Composite.Modularity;
    using Microsoft.Practices.Composite.Regions;
    using Microsoft.Practices.Unity;

    using Moq;

    using Overlay;

    using Persistence;

    using PokerRooms;

    using Tools.Interfaces;
    using Tools.Validation;
    using Tools.WPF;
    using Tools.WPF.Interfaces;
    using Tools.WPF.ViewModels;

    using Tracking;

    using ViewModels;
    using ViewModels.Overlay;

    using Views;
    using Views.Overlay;

    // Resharper disable InconsistentNaming
    public class LiveTrackerModuleMock : IModule
    {
        static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        readonly IUnityContainer _container;

        readonly IRegionManager _regionManager;

        public LiveTrackerModuleMock(IUnityContainer container, IRegionManager regionManager)
        {
            _container = container;
            _regionManager = regionManager;
        }

        public void Initialize()
        {
            RegisterViewsAndServices();
            RegisterMenu();

            Log.Info("got initialized.");
        }

        void RegisterViewsAndServices()
        {
            var overlayToTableAttacher_Mock = new Mock<IOverlayToTableAttacher>();

            overlayToTableAttacher_Mock.Setup(ta => ta.InitializeWith(
                It.IsAny<IWindowManager>(),
                It.IsAny<IDispatcherTimer>(),
                It.IsAny<IDispatcherTimer>(),
                It.IsAny<IPokerRoomInfo>(),
                It.IsAny<string>())).Returns(overlayToTableAttacher_Mock.Object);

            _container

                // Tools
                .RegisterType<IDispatcherTimer, DispatcherTimerAdapter>()
                .RegisterType<ICollectionValidator, CollectionValidator>()
               
                // Tools.WPF
                .RegisterType<IPositionViewModel, PositionViewModel>() 

                // LiveTrackerSettings
                .RegisterType<ILiveTrackerSettingsXDocumentHandler, LiveTrackerSettingsXDocumentHandler>()
                .RegisterType<ILiveTrackerSettingsViewModel, LiveTrackerSettingsViewModel>()
                .RegisterType<LiveTrackerSettingsView>()

                // LiveStats View
                .RegisterType<IPokerTableStatisticsViewModel, PokerTableStatisticsViewModel>()

                // Overlay to Table Attacher
                .RegisterType<IWindowFinder, WindowFinder>()
                .RegisterType<IWindowManipulator, WindowManipulator>()

                // Mock
                // .RegisterType<IOverlayToTableAttacher, OverlayToTableAttacher>()
                .RegisterInstance(overlayToTableAttacher_Mock.Object)

                // Table Overlay
                .RegisterType<ITableOverlaySettingsViewModel, TableOverlaySettingsViewModel>()

                .RegisterType<IHarringtonMViewModel, HarringtonMViewModel>()
                .RegisterTypeAndConstructor<IOverlayHoleCardsViewModel, OverlayHoleCardsViewModel>(() => _container.Resolve<IOverlayHoleCardsViewModel>())
                .RegisterType<IOverlayBoardViewModel, OverlayBoardViewModel>()
                .RegisterType<IOverlaySettingsAidViewModel, OverlaySettingsAidViewModel>()
                .RegisterType<IPlayerStatusViewModel, PlayerStatusViewModel>()
                .RegisterTypeAndConstructor<IPlayerOverlayViewModel, PlayerOverlayViewModel>(() => _container.Resolve<IPlayerOverlayViewModel>())
                .RegisterType<ITableOverlayViewModel, TableOverlayViewModel>()

                // Table Overlay Manager
                .RegisterType<IPokerRoomInfoLocator, PokerRoomInfoLocator>()
                .RegisterType<ILayoutXDocumentHandler, LayoutXDocumentHandler>()
                .RegisterType<ILayoutManager, LayoutManager>()
                .RegisterType<ISeatMapper, SeatMapper>()
                
                // Mock
                // .RegisterType<ITableOverlayWindowManager, TableOverlayWindowManager>()
                .RegisterType<ITableOverlayWindowManager, TableOverlayWindowManagerMock>()
                .RegisterType<ITableOverlayManager, TableOverlayManager>()

                // GameHistory
                .RegisterType<IGameHistoryViewModel, GameHistoryViewModel>()
                .RegisterType<IGameHistoryWindowManager, GameHistoryWindowManager>()

                // GameController
                .RegisterType<IPlayerStatisticsUpdater, PlayerStatisticsUpdater>()
                .RegisterType<IPokerTableStatisticsWindowManager, PokerTableStatisticsWindowManager>()
                .RegisterTypeAndConstructor<IGameController, GameController>(() => _container.Resolve<IGameController>())

                // GamesTracker
                .RegisterType<IWatchedDirectoriesOptimizer, WatchedDirectoriesOptimizer>()
               
                // Mock
                // .RegisterType<INewHandsTracker, NewHandsTracker>(new ContainerControlledLifetimeManager())
                // .RegisterConstructor<IHandHistoryFilesWatcher, HandHistoryFilesWatcher>()
                .RegisterInstance(new Mock<INewHandsTracker>().Object)
                .RegisterType<IGamesTracker, GamesTracker>(new ContainerControlledLifetimeManager());
        }

        void RegisterMenu()
        {
            var liveTrackerSettingsWindow = new WindowManager(_container.Resolve<LiveTrackerSettingsView>);

            var liveTrackerSettings = _container
                .Resolve<ILiveTrackerSettingsViewModel>()
                .LoadSettings();

            var gamesTracker = _container
                .Resolve<IGamesTracker>()
                .InitializeWith(liveTrackerSettings);

            MenuItem liveTrackerMenuItem = new LiveTrackerMenuItemFactory(liveTrackerSettingsWindow, gamesTracker).Create();
            _regionManager.Regions["Shell.MainMenuRegion"].Add(liveTrackerMenuItem);
        }
    }

    public class TableOverlayWindowManagerMock : WindowManager, ITableOverlayWindowManager
    {
        public TableOverlayWindowManagerMock()
            : base(() => new TableOverlayView { Background = Brushes.Black, AllowsTransparency = false, WindowStyle = WindowStyle.ToolWindow })
        {
        }
    }
}