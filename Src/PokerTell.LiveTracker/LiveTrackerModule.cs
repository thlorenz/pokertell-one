namespace PokerTell.LiveTracker
{
    using System.Reflection;
    using System.Windows.Controls;

    using log4net;

    using Microsoft.Practices.Composite.Modularity;
    using Microsoft.Practices.Composite.Regions;
    using Microsoft.Practices.Unity;

    using PokerRooms;

    using PokerTell.LiveTracker.Interfaces;
    using PokerTell.LiveTracker.Overlay;
    using PokerTell.LiveTracker.Persistence;
    using PokerTell.LiveTracker.ViewModels;
    using PokerTell.LiveTracker.Views;

    using Tools.Interfaces;
    using Tools.WPF;
    using Tools.WPF.Interfaces;
    using Tools.WPF.ViewModels;

    using Tracking;

    using ViewModels.Overlay;

    public class LiveTrackerModule : IModule
    {
        static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        readonly IUnityContainer _container;

        readonly IRegionManager _regionManager;

        public LiveTrackerModule(IUnityContainer container, IRegionManager regionManager)
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
            _container

                .RegisterType<IDispatcherTimer, DispatcherTimerAdapter>()

                // LiveTrackerSettings
                .RegisterType<ILiveTrackerSettingsXDocumentHandler, LiveTrackerSettingsXDocumentHandler>()
                .RegisterType<ILiveTrackerSettingsViewModel, LiveTrackerSettingsViewModel>()
                .RegisterType<LiveTrackerSettingsView>()

                // LiveStats View
                .RegisterType<IPokerTableStatisticsViewModel, PokerTableStatisticsViewModel>()

                // Overlay to Table Attacher
                .RegisterType<IWindowFinder, WindowFinder>()
                .RegisterType<IWindowManipulator, WindowManipulator>()
                .RegisterType<IOverlayToTableAttacher, OverlayToTableAttacher>()

                // Table Overlay
                .RegisterType<IPositionViewModel, PositionViewModel>() // May not be needed
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
                .RegisterType<ITableOverlayManager, TableOverlayManager>()

                // GameController
                .RegisterType<IGameHistoryViewModel, GameHistoryViewModel>()
                .RegisterType<IPlayerStatisticsUpdater, PlayerStatisticsUpdater>()
                .RegisterTypeAndConstructor<IGameController, GameController>(() => _container.Resolve<IGameController>())

                // GamesTracker
                .RegisterType<INewHandsTracker, NewHandsTracker>(new ContainerControlledLifetimeManager())
                .RegisterConstructor<IHandHistoryFilesWatcher, HandHistoryFilesWatcher>()
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
}