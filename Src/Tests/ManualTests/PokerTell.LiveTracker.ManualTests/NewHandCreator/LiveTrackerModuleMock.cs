namespace PokerTell.LiveTracker.ManualTests.NewHandCreator
{
    using System.Reflection;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    using Infrastructure.Interfaces.LiveTracker;

    using log4net;

    using Microsoft.Practices.Composite.Modularity;
    using Microsoft.Practices.Composite.Regions;
    using Microsoft.Practices.Unity;

    using Moq;

    using PokerTell.Infrastructure;
    using PokerTell.LiveTracker.Interfaces;
    using PokerTell.LiveTracker.Overlay;
    using PokerTell.LiveTracker.Persistence;
    using PokerTell.LiveTracker.PokerRooms;
    using PokerTell.LiveTracker.Tracking;
    using PokerTell.LiveTracker.ViewModels;
    using PokerTell.LiveTracker.ViewModels.Overlay;
    using PokerTell.LiveTracker.Views;
    using PokerTell.LiveTracker.Views.Overlay;

    using Tools.Interfaces;
    using Tools.WPF;
    using Tools.WPF.Interfaces;

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

                // Mock
                // .RegisterType<IOverlayToTableAttacher, OverlayToTableAttacher>()
                .RegisterInstance(overlayToTableAttacher_Mock.Object)

                // Table Overlay
                .RegisterType<ITableOverlaySettingsViewModel, TableOverlaySettingsViewModel>()
                .RegisterType<IHarringtonMViewModel, HarringtonMViewModel>()
                .RegisterTypeAndConstructor<IOverlayHoleCardsViewModel, OverlayHoleCardsViewModel>(
                () => _container.Resolve<IOverlayHoleCardsViewModel>())
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
    }

    public class TableOverlayWindowManagerMock : WindowManager, ITableOverlayWindowManager
    {
        const string DesignerPath = @"C:\SD\PokerTell\Src\Design\Designer\";

        const string PokerStars_10max_Background = "PokerTables/PokerStars/PokerStars.10-max.jpg";

        const string PokerStars_2max_Background = "PokerTables/PokerStars/PokerStars.2-max.jpg";

        const string PokerStars_4max_Background = "PokerTables/PokerStars/PokerStars.4-max.jpg";

        const string PokerStars_6max_Background = "PokerTables/PokerStars/PokerStars.6-max.jpg";

        const string PokerStars_7max_Background = "PokerTables/PokerStars/PokerStars.7-max.jpg";

        const string PokerStars_8max_Background = "PokerTables/PokerStars/PokerStars.8-max.jpg";

        const string PokerStars_9max_Background = "PokerTables/PokerStars/PokerStars.9-max.jpg";

        static readonly object img = new ImageSourceConverter().ConvertFromString(DesignerPath + PokerStars_9max_Background);

        public TableOverlayWindowManagerMock()
            : base(() => new TableOverlayView
                {
                    Background = new ImageBrush((ImageSource)img) { Stretch = Stretch.UniformToFill }, 
                    AllowsTransparency = false, 
                    WindowStyle = WindowStyle.ToolWindow, 
                    SizeToContent = SizeToContent.WidthAndHeight
                })
        {
        }
    }
}