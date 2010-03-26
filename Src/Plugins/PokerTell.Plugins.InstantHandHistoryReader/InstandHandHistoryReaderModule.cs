namespace PokerTell.Plugins.InstantHandHistoryReader
{
    using System;
    using System.Reflection;
    using System.Threading;

    using Infrastructure;

    using Interfaces;

    using log4net;

    using Microsoft.Practices.Composite.Modularity;
    using Microsoft.Practices.Unity;

    [ModuleDependency(ApplicationModules.ToolsModule)]
    [ModuleDependency(ApplicationModules.UserModule)]
    [ModuleDependency(ApplicationModules.PokerHandModule)]
    [ModuleDependency(ApplicationModules.DatabaseSetupModule)]
    [ModuleDependency(ApplicationModules.PokerHandParsersModule)]
    [ModuleDependency(ApplicationModules.RepositoryModule)]
    public class InstandHandHistoryReaderModule : IModule
    {
        static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
       
        // fire every 10 minutes
        const int fireInterval = 10000 * 60; 

        InstantHandHistoryReaderService _readerService;

        readonly IUnityContainer _container;

        public InstandHandHistoryReaderModule(IUnityContainer container)
        {
            _container = container;
        }

        public void Initialize()
        {
            _container
                .RegisterType<IHandHistoryReader, HandHistoryReader>();

            _readerService = _container.Resolve<InstantHandHistoryReaderService>();
            _readerService.Timer = new Timer(_readerService.ReadInstantHandHistoriesFromMemory, null, fireInterval, fireInterval);

            Log.Info("got initialized");
        }
    }
}