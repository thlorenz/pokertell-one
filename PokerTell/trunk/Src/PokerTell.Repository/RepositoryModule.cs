namespace PokerTell.Repository
{
    using System.Reflection;

    using Database;

    using Infrastructure;
    using Infrastructure.Interfaces.DatabaseSetup;
    using Infrastructure.Interfaces.Repository;

    using Interfaces;

    using log4net;

    using Microsoft.Practices.Composite.Modularity;
    using Microsoft.Practices.Composite.Regions;
    using Microsoft.Practices.Unity;

    using NHibernate;

    using ViewModels;

    using Views;

    public class RepositoryModule : IModule
    {
        #region Constants and Fields

        static readonly ILog Log = LogManager.GetLogger(
            MethodBase.GetCurrentMethod().DeclaringType);

        readonly IUnityContainer _container;

        readonly IRegionManager _regionManager;

        #endregion

        #region Constructors and Destructors

        public RepositoryModule(IUnityContainer container, IRegionManager regionManager)
        {
            _regionManager = regionManager;
            _container = container;
        }

        #endregion

        #region Implemented Interfaces

        #region IModule

        public void Initialize()
        {
            _container
                .RegisterType<ITransactionManagerFactory, TransactionManagerFactory>(new ContainerControlledLifetimeManager())
                .RegisterType<IRepositoryParser, RepositoryParser>(new ContainerControlledLifetimeManager())
                .RegisterType<IRepository, Repository>(new ContainerControlledLifetimeManager())

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
                .Resolve<IRepository>()
                .Use(dataProvider);
                
        }

        #endregion

        #endregion
    }
}