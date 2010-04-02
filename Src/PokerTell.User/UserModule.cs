namespace PokerTell.User
{
    using System;
    using System.Reflection;
    using System.Windows.Controls;

    using Interfaces;

    using log4net;

    using Microsoft.Practices.Composite.Modularity;
    using Microsoft.Practices.Composite.Presentation.Commands;
    using Microsoft.Practices.Composite.Regions;
    using Microsoft.Practices.Unity;

    using PokerTell.Infrastructure;
    using PokerTell.Infrastructure.Interfaces;
    using PokerTell.User.ViewModels;
    using PokerTell.User.Views;

    using Reporting;

    public class UserModule : IModule
    {
        static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        readonly IUnityContainer _container;

        readonly IRegionManager _regionManager;

        public UserModule(IUnityContainer container, IRegionManager regionManager)
        {
            _container = container;
            _regionManager = regionManager;
        }

        public void Initialize()
        {
            RegisterViewsAndServices();
            CleanupReporterTempFolder();

            GlobalCommands.StartServicesCommand.RegisterCommand(new DelegateCommand<object>(StartServices));

            Log.Info("got initialized.");
        }

        void StartServices(object ignore)
        {
            // Register menu here in order to make it show as last item
            // A cleaner but more involved solution is here: http://blogs.southworks.net/dschenkelman/2009/03/
            RegisterMenu();

            Log.Info("started services.");
        }

        void CleanupReporterTempFolder()
        {
            _container
                .Resolve<IReporter>()
                .DeleteReportingTempFolder();
        }

        void RegisterViewsAndServices()
        {
            _container

                // User Configuration
                .RegisterType<IUserConfiguration, UserConfiguration>(new ContainerControlledLifetimeManager())
                .RegisterType<ISettings, Settings>(new ContainerControlledLifetimeManager())
                
                // User Reports
                .RegisterType<IEmailer, Emailer>()
                .RegisterType<IReporter, Reporter>(new ContainerControlledLifetimeManager())
                .RegisterType<IReportViewModel, ReportViewModel>()
                .RegisterTypeAndConstructor<IReportWindowManager, ReportWindowManager>(() => _container.Resolve<IReportWindowManager>())
                
                // Progress ViewModel
                .RegisterType<IProgressViewModel, ProgressViewModel>()
                .RegisterType<StatusBarViewModel>();

            _container
                .Resolve<UserService>();

            _regionManager
                .RegisterViewWithRegion(ApplicationProperties.ShellStatusRegion, () => _container.Resolve<StatusBarView>());
        }

        void RegisterMenu()
        {
            MenuItem userMenuItem = _container.Resolve<UserMenuItemFactory>().Create();
            _regionManager.Regions[ApplicationProperties.ShellMainMenuRegion].Add(userMenuItem);
        }
    }
}