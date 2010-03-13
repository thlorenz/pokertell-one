namespace PokerTell.LiveTracker
{
    using System.Reflection;
    using System.Windows.Controls;

    using Interfaces;

    using log4net;

    using Microsoft.Practices.Composite.Modularity;
    using Microsoft.Practices.Composite.Regions;
    using Microsoft.Practices.Unity;

    using Persistence;

    using Tools.WPF;

    using ViewModels;

    using Views;

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

                // LiveTrackerSettings
                .RegisterType<ILiveTrackerSettingsXDocumentHandler, LiveTrackerSettingsXDocumentHandler>()
                .RegisterType<ILiveTrackerSettingsViewModel, LiveTrackerSettingsViewModel>()
                .RegisterType<LiveTrackerSettingsView>();

        }

        void RegisterMenu()
        {
            var liveTrackerSettingsWindow = new WindowManager(_container.Resolve<LiveTrackerSettingsView>);
            MenuItem liveTrackerMenuItem = new LiveTrackerMenuItemFactory(liveTrackerSettingsWindow).Create();
            _regionManager.Regions["Shell.MainMenuRegion"].Add(liveTrackerMenuItem);
        }

    }
}