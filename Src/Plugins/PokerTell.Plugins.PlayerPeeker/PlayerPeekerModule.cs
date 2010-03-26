namespace PokerTell.Plugins.PlayerPeeker
{
    using System.Reflection;

    using log4net;

    using Microsoft.Practices.Composite.Modularity;
    using Microsoft.Practices.Unity;

    using PokerTell.Infrastructure;
    using PokerTell.Plugins.PlayerPeeker.Interfaces;

    [ModuleDependency(ApplicationModules.UserModule)]
    public class PlayerPeekerModule : IModule
    {
        static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        readonly IUnityContainer _container;

        public PlayerPeekerModule(IUnityContainer container)
        {
            _container = container;
        }

        public void Initialize()
        {
            _container
                .RegisterTypeAndConstructor<IPlayerPeekerForm, PlayerPeekerForm>(() => _container.Resolve<IPlayerPeekerForm>())
                .Resolve<PlayerPeekerService>();

            Log.Info("got initialized");
        }
    }
}