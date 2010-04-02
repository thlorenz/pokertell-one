namespace PokerTell.LiveTracker
{
    using System;
    using System.Reflection;
    using System.Windows.Controls;

    using Infrastructure;

    using log4net;

    using Microsoft.Practices.Composite.Modularity;
    using Microsoft.Practices.Composite.Presentation.Commands;
    using Microsoft.Practices.Composite.Regions;
    using Microsoft.Practices.Unity;

    using PokerRooms;

    using PokerTell.LiveTracker.Interfaces;
    using PokerTell.LiveTracker.Overlay;
    using PokerTell.LiveTracker.Persistence;
    using PokerTell.LiveTracker.ViewModels;
    using PokerTell.LiveTracker.Views;

    using Properties;

    using Tools.WPF;

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

            GlobalCommands.ConfigureServicesForFirstTimeCommand.RegisterCommand(new DelegateCommand<object>(ConfigureServicesForTheFirstTime));
            GlobalCommands.StartServicesCommand.RegisterCommand(new DelegateCommand<object>(StartServices));

            Log.Info("got initialized.");
        }

        void RegisterViewsAndServices()
        {
            _container
                
                // LiveTrackerSettings
                .RegisterType<ILayoutAutoConfigurator, LayoutAutoConfigurator>()
                .RegisterType<ILiveTrackerSettingsXDocumentHandler, LiveTrackerSettingsXDocumentHandler>()
                .RegisterType<IPokerRoomSettingsDetector, PokerRoomSettingsDetector>()
                .RegisterType<IHandHistoryFolderAutoDetectResultsViewModel, HandHistoryFolderAutoDetectResultsViewModel>()
                .RegisterType<IHandHistoryFolderAutoDetectResultsWindowManager, HandHistoryFolderAutoDetectResultsWindowManager>()
                .RegisterType<ILiveTrackerSettingsViewModel, LiveTrackerSettingsViewModel>()
                .RegisterType<LiveTrackerSettingsView>()

                // LiveStats View
                .RegisterType<IPokerTableStatisticsViewModel, PokerTableStatisticsViewModel>()

                // Overlay to Table Attacher
                .RegisterType<IWindowFinder, WindowFinder>()
                .RegisterType<IWindowManipulator, WindowManipulator>()
                .RegisterType<IOverlayToTableAttacher, OverlayToTableAttacher>()

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
                .RegisterType<ITableOverlayWindowManager, TableOverlayWindowManager>()
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

            MenuItem liveTrackerMenuItem = new LiveTrackerMenuItemFactory(liveTrackerSettingsWindow, gamesTracker, liveTrackerSettings).Create();
            _regionManager.Regions[ApplicationProperties.ShellMainMenuRegion].Add(liveTrackerMenuItem);
        }

        void ConfigureServicesForTheFirstTime(object ignore)
        {
            var liveTrackerSettings = _container
                .Resolve<ILiveTrackerSettingsViewModel>()
                .LoadSettings();

            liveTrackerSettings
                .AutoDetectHandHistoryFoldersCommand.Execute(null);
            
            liveTrackerSettings
                .DetectAndSavePreferredSeats();

            liveTrackerSettings
                .SaveSettingsCommand.Execute(null);
        }

        void StartServices(object ignore)
        {
            var liveTrackerSettings = _container
                .Resolve<ILiveTrackerSettingsViewModel>()
                .LoadSettings();

            _container
                .Resolve<IGamesTracker>()
                .InitializeWith(liveTrackerSettings);

            Log.Info("started services.");
        }
    }
}