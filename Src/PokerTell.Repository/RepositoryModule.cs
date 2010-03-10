namespace PokerTell.Repository
{
    using System.Reflection;

    using log4net;

    using Microsoft.Practices.Composite.Modularity;
    using Microsoft.Practices.Composite.Regions;
    using Microsoft.Practices.Unity;

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
            _container
                .RegisterType<ISessionFactoryManager, SessionFactoryManager>(new ContainerControlledLifetimeManager())
                .RegisterType<ITransactionManager, TransactionManager>()
                .RegisterType<IRepositoryParser, RepositoryParser>(new ContainerControlledLifetimeManager())
                .RegisterType<IRepository, Repository>(new PerThreadLifetimeManager())

                /* .RegisterType<IDatabaseUtility, DatabaseUtility>()
               * .RegisterType<IRepositoryDatabase, RepositoryDatabase>()
               * .RegisterType<IConvertedPokerHandInserter, ConvertedPokerHandInserter>()
                .RegisterType<IConvertedPokerHandRetriever, ConvertedPokerHandRetriever>() */
                .RegisterType<IHandHistoriesDirectoryImporter, HandHistoriesDirectoryImporter>(new ContainerControlledLifetimeManager())

                // ViewModels
                .RegisterType<ImportHandHistoriesViewModel>(new ContainerControlledLifetimeManager());

            AttemptToConnectToDatabaseAndAssignResultingDataProviderToRepository();

            _container
                .Resolve<RepositoryMenuItemFactory>()
                .Create()
                .ForEach(menuItem =>
                         _regionManager.RegisterViewWithRegion(ApplicationProperties.ShellDatabaseMenuRegion, 
                                                               () => menuItem));

            Log.Info("got initialized.");
        }

        void AttemptToConnectToDatabaseAndAssignResultingDataProviderToRepository()
        {
            var databaseConnector = _container.Resolve<IDatabaseConnector>();

            var dataProvider =
                databaseConnector
                    .InitializeFromSettings()
                    .ConnectToDatabase()
                    .DataProvider;

            _container
                .Resolve<ISessionFactoryManager>()
                .Use(dataProvider);
        }
    }
}