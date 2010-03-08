namespace PokerTell.DatabaseSetup
{
    using System.Reflection;

    using log4net;

    using Microsoft.Practices.Composite.Modularity;
    using Microsoft.Practices.Composite.Regions;
    using Microsoft.Practices.Unity;

    using PokerTell.DatabaseSetup.ViewModels;
    using PokerTell.DatabaseSetup.Views;
    using PokerTell.Infrastructure;
    using PokerTell.Infrastructure.Interfaces.DatabaseSetup;

    public class DatabaseSetupModule : IModule
    {
        #region Constants and Fields

        static readonly ILog Log = LogManager.GetLogger(
            MethodBase.GetCurrentMethod().DeclaringType);

        readonly IUnityContainer _container;

        readonly IRegionManager _regionManager;

        #endregion

        #region Constructors and Destructors

        public DatabaseSetupModule(IUnityContainer container, IRegionManager regionManager)
        {
            _container = container;
            _regionManager = regionManager;
        }

        #endregion

        #region Implemented Interfaces

        #region IModule

        public void Initialize()
        {
            IDataProviderInfos dataProviderInfos = new DataProviderInfos()
                .Support(new MySqlInfo())
                .Support(new SqLiteInfo());

            _container
                .RegisterType<IDataProvider, DataProvider>()
                .RegisterType<IDatabaseConnector, DatabaseConnector>()
                .RegisterInstance(dataProviderInfos)
                .RegisterType<IDatabaseSettings, DatabaseSettings>()
                .RegisterType<ConfigureMySqlDataProviderViewModel>()
                .RegisterType<ConfigureMySqlDataProviderView>();

            _regionManager
                .RegisterViewWithRegion(ApplicationProperties.ShellDatabaseMenuRegion, 
                                        () => _container.Resolve<DatabaseSetupMenuItemFactory>().Create());

            Log.Info("got initialized.");
        }

        #endregion

        #endregion
    }
}