namespace PokerTell.Repository
{
    using System.Reflection;

    using Database;

    using Infrastructure.Interfaces.DatabaseSetup;
    using Infrastructure.Interfaces.Repository;

    using Interfaces;

    using log4net;

    using Microsoft.Practices.Composite.Modularity;
    using Microsoft.Practices.Unity;

    public class RepositoryModule : IModule
    {
        #region Constants and Fields

        static readonly ILog Log = LogManager.GetLogger(
            MethodBase.GetCurrentMethod().DeclaringType);

        readonly IUnityContainer _container;

        #endregion

        #region Constructors and Destructors

        public RepositoryModule(IUnityContainer container)
        {
            _container = container;
        }

        #endregion

        #region Implemented Interfaces

        #region IModule

        public void Initialize()
        {
            _container
                .RegisterType<IRepositoryParser, RepositoryParser>(new ContainerControlledLifetimeManager())
                .RegisterType<IRepository, Repository>(new ContainerControlledLifetimeManager())
                .RegisterType<IRepositoryDatabase, RepositoryDatabase>()
                .RegisterType<IDatabaseUtility, DatabaseUtility>()
                .RegisterType<IConvertedPokerHandInserter, ConvertedPokerHandInserter>()
                .RegisterType<IConvertedPokerHandRetriever, ConvertedPokerHandRetriever>();

            AttemptToConnectToDatabaseAndAssignResultingDataProviderToRepository();

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