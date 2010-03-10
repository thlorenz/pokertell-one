namespace PokerTell.User
{
    using System.Reflection;

    using log4net;

    using Microsoft.Practices.Composite.Modularity;
    using Microsoft.Practices.Composite.Regions;
    using Microsoft.Practices.Unity;

    using PokerTell.Infrastructure;
    using PokerTell.Infrastructure.Interfaces;
    using PokerTell.User.ViewModels;
    using PokerTell.User.Views;

    public class UserModule : IModule
    {
        static readonly ILog Log = LogManager.GetLogger(
            MethodBase.GetCurrentMethod().DeclaringType);

        readonly IUnityContainer _container;

        readonly IRegionManager _regionManager;

        public UserModule(IUnityContainer container, IRegionManager regionManager)
        {
            _container = container;
            _regionManager = regionManager;
        }

        public void Initialize()
        {
            _container
                .RegisterType<IUserConfiguration, UserConfiguration>(new ContainerControlledLifetimeManager())
                .RegisterType<ISettings, Settings>(new ContainerControlledLifetimeManager())
                .RegisterType<IProgressViewModel, ProgressViewModel>()
                .RegisterType<StatusBarViewModel>();

            // TODO: Is this necessary?
            _container
                .Resolve<UserService>();

            _regionManager
                .RegisterViewWithRegion(ApplicationProperties.ShellStatusRegion, 
                                        () => _container.Resolve<StatusBarView>());

            Log.Info("got initialized.");
        }
    }
}