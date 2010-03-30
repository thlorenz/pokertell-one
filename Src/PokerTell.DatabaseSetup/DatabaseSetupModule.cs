namespace PokerTell.DatabaseSetup
{
    using System.Reflection;

    using DatabaseVersioning;

    using Interfaces;

    using log4net;

    using Microsoft.Practices.Composite.Modularity;
    using Microsoft.Practices.Composite.Presentation.Commands;
    using Microsoft.Practices.Composite.Regions;
    using Microsoft.Practices.Unity;

    using PokerTell.DatabaseSetup.ViewModels;
    using PokerTell.DatabaseSetup.Views;
    using PokerTell.Infrastructure;
    using PokerTell.Infrastructure.Interfaces.DatabaseSetup;

    public class DatabaseSetupModule : IModule
    {
        static readonly ILog Log = LogManager.GetLogger(
            MethodBase.GetCurrentMethod().DeclaringType);

        readonly IUnityContainer _container;

        readonly IRegionManager _regionManager;

        public DatabaseSetupModule(IUnityContainer container, IRegionManager regionManager)
        {
            _container = container;
            _regionManager = regionManager;
        }

        public void Initialize()
        {
            RegisterViewsAndServices();

            GlobalCommands.ConfigureServicesForFirstTimeCommand.RegisterCommand(new DelegateCommand<object>(ConfigureServicesForFirstTime));

            Log.Info("got initialized.");
        }

        void RegisterViewsAndServices()
        {
            IDataProviderInfos dataProviderInfos = new DataProviderInfos()
                .Support(new MySqlInfo())
                .Support(new SqLiteInfo());

            _container
                .RegisterType<IDataProvider, DataProvider>()
                .RegisterType<IDatabaseConnector, DatabaseConnector>()
                .RegisterInstance(dataProviderInfos)
                .RegisterType<IDatabaseSettings, DatabaseSettings>()
                .RegisterType<IDatabaseManager, DatabaseManager>()
                
                .RegisterType<ConfigureMySqlDataProviderViewModel>()
                .RegisterType<ConfigureMySqlDataProviderView>()

                // Database Version
                .RegisterType<IDatabaseVersion, DatabaseVersion>()
                ;

            _regionManager
                .RegisterViewWithRegion(ApplicationProperties.ShellDatabaseMenuRegion, 
                                        () => _container.Resolve<DatabaseSetupMenuItemFactory>().Create());
        }

        void ConfigureServicesForFirstTime(object ignore)
        {
            const string firstDatabase = "pokertell";

            _container
                .Resolve<IDatabaseSettings>()
                .SetCurrentDataProviderTo(new SqLiteInfo());

            var databaseManager =
                _container
                    .RegisterType<IDataProviderInfo, SqLiteInfo>()
                    .RegisterType<IManagedDatabase, EmbeddedManagedDatabase>()
                    .Resolve<IDatabaseManager>();

            if (!databaseManager.DatabaseExists(firstDatabase))
                databaseManager.CreateDatabase(firstDatabase);

            databaseManager.ChooseDatabase(firstDatabase);

            Log.Info("configured services for the first time.");
        }
    }
}