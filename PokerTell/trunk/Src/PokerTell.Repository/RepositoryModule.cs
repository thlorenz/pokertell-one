namespace PokerTell.Repository
{
    using System.Reflection;

    using Infrastructure.Interfaces.Repository;

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
                .RegisterType<RepositoryParser>(new ContainerControlledLifetimeManager())
                .RegisterType<IRepository, Repository>(new ContainerControlledLifetimeManager());
            Log.Info("got initialized.");
        }

        #endregion

        #endregion
    }
}