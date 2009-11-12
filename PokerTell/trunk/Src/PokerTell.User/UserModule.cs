namespace PokerTell.User
{
    using System.Reflection;

    using Infrastructure;

    using log4net;

    using Microsoft.Practices.Composite.Modularity;
    using Microsoft.Practices.Composite.Regions;
    using Microsoft.Practices.Unity;

    using PokerTell.Infrastructure.Interfaces;

    using ViewModels;

    using Views;

    public class UserModule : IModule
    {
        #region Constants and Fields

        static readonly ILog Log = LogManager.GetLogger(
            MethodBase.GetCurrentMethod().DeclaringType);

        readonly IUnityContainer _container;

        readonly IRegionManager _regionManager;

        #endregion

        #region Constructors and Destructors

        public UserModule(IUnityContainer container, IRegionManager regionManager)
        {
            _container = container;
            _regionManager = regionManager;
        }

        #endregion

        #region Implemented Interfaces

        #region IModule

        public void Initialize()
        {
            _container
                .RegisterType<IUserConfiguration, UserConfiguration>(new ContainerControlledLifetimeManager())
                .RegisterType<ISettings, Settings>(new ContainerControlledLifetimeManager())
                .RegisterType<StatusBarViewModel>();

            _container
                .Resolve<UserService>();

            _regionManager
                .RegisterViewWithRegion(ApplicationProperties.ShellStatusRegion,
                                        () => _container.Resolve<StatusBarView>());

            Log.Info("got initialized.");
        }

        #endregion

        #endregion
    }
}