namespace PokerTell.Repository
{
    using System;
    using System.Reflection;

    using log4net;

    using Microsoft.Practices.Composite.Modularity;
    using Microsoft.Practices.Composite.Presentation.Commands;
    using Microsoft.Practices.Composite.Regions;
    using Microsoft.Practices.Unity;

    using Moq;

    using PokerTell.Infrastructure;
    using PokerTell.Infrastructure.Interfaces.DatabaseSetup;
    using PokerTell.Infrastructure.Interfaces.Repository;
    using PokerTell.Repository.Database;
    using PokerTell.Repository.Interfaces;
    using PokerTell.Repository.NHibernate;
    using PokerTell.Repository.ViewModels;
    using PokerTell.Repository.Views;

    public class RepositoryModule : IModule
    {
        static readonly ILog Log = LogManager.GetLogger(
            MethodBase.GetCurrentMethod().DeclaringType);

        readonly IUnityContainer _container;

        readonly IRegionManager _regionManager;

        public RepositoryModule(IUnityContainer container, IRegionManager regionManager)
        {
            _regionManager = regionManager;
            _container = container;
        }

        public void Initialize()
        {
            RegisterViewsAndServices();

            GlobalCommands.StartServicesCommand.RegisterCommand(new DelegateCommand<object>(StartServices));

            Log.Info("got initialized.");
        }

        void AttemptToConnectToDatabaseAndAssignResultingDataProviderToRepository()
        {
            var dataProvider = _container.Resolve<IDatabaseConnector>()
                .InitializeFromSettings()
                .ConnectToDatabase()
                .DataProvider;

            if (dataProvider.IsConnectedToDatabase)
                _container.Resolve<ISessionFactoryManager>().Use(dataProvider);

            Log.Info("started services.");
        }

        void RegisterViewsAndServices()
        {
            _container
                .RegisterType<ISessionFactoryManager, SessionFactoryManager>(new ContainerControlledLifetimeManager())
                .RegisterType<ITransactionManager, TransactionManager>()
                .RegisterType<IRepositoryParser, RepositoryParser>(new ContainerControlledLifetimeManager())
                .RegisterType<IRepository, Repository>(new PerThreadLifetimeManager())
                .RegisterType<IHandHistoriesDirectoryImporter, HandHistoriesDirectoryImporter>(new ContainerControlledLifetimeManager())

                // Database Import
                .RegisterType<IPokerTellHandHistoryRetriever, PokerTellHandHistoryRetriever>()
                .RegisterType<IPokerOfficeHandHistoryRetriever, PokerOfficeHandHistoryRetriever>()
                
                // for now no POkerTracker support
                .RegisterInstance(new Mock<IPokerTrackerHandHistoryRetriever>().Object)
                
                .RegisterType<IDatabaseImporter, DatabaseImporter>()

                // ViewModels
                .RegisterType<ImportHandHistoriesViewModel>(new ContainerControlledLifetimeManager())
                .RegisterType<IDatabaseImportViewModel, DatabaseImportViewModel>(new ContainerControlledLifetimeManager());

            _container
                .Resolve<RepositoryMenuItemFactory>()
                .Create()
                .ForEach(menuItem => _regionManager.RegisterViewWithRegion(ApplicationProperties.ShellDatabaseMenuRegion, () => menuItem));
        }

        void StartServices(object ignore)
        {
            AttemptToConnectToDatabaseAndAssignResultingDataProviderToRepository();
        }
    }
}