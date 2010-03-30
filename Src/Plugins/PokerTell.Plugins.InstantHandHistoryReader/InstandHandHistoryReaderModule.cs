namespace PokerTell.Plugins.InstantHandHistoryReader
{
    using System.Reflection;
    using System.Threading;

    using log4net;

    using Microsoft.Practices.Composite.Modularity;
    using Microsoft.Practices.Composite.Presentation.Commands;
    using Microsoft.Practices.Unity;

    using PokerTell.Infrastructure;
    using PokerTell.Plugins.InstantHandHistoryReader.Interfaces;

    [ModuleDependency(ApplicationModules.ToolsModule)]
    [ModuleDependency(ApplicationModules.UserModule)]
    [ModuleDependency(ApplicationModules.PokerHandModule)]
    [ModuleDependency(ApplicationModules.DatabaseSetupModule)]
    [ModuleDependency(ApplicationModules.PokerHandParsersModule)]
    [ModuleDependency(ApplicationModules.RepositoryModule)]
    public class InstandHandHistoryReaderModule : IModule
    {
        // fire every 10 minutes
        const int fireInterval = 10000 * 60;

        static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        readonly IUnityContainer _container;

        InstantHandHistoryReaderService _readerService;

        public InstandHandHistoryReaderModule(IUnityContainer container)
        {
            _container = container;
        }

        public void Initialize()
        {
            RegisterViewsAndServices();

            GlobalCommands.StartServicesCommand.RegisterCommand(new DelegateCommand<object>(StartServices));

            Log.Info("got initialized");
        }

        void RegisterViewsAndServices()
        {
            _container
                .RegisterType<IHandHistoryReader, HandHistoryReader>();
        }

        void StartServices(object ignore)
        {
            _readerService = _container.Resolve<InstantHandHistoryReaderService>();
            _readerService.Timer = new Timer(_readerService.ReadInstantHandHistoriesFromMemory, null, fireInterval, fireInterval);

            Log.Info("started services.");
        }
    }
}