using System.Reflection;

using log4net;

using Microsoft.Practices.Unity;

namespace PokerTell.User
{
    using Infrastructure.Interfaces;
    using Infrastructure.Interfaces.User;

    using Microsoft.Practices.Composite.Modularity;
    using Microsoft.Practices.Composite.Regions;

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
                .RegisterType<IUserMessageViewFactory, UserMessageViewFactory>();

            _container
                .Resolve<UserService>();

            Log.Info("got initialized.");
        }

        #endregion

        #endregion
    }
}