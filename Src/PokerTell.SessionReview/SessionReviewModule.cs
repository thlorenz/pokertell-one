namespace PokerTell.SessionReview
{
    using System.Reflection;
    using System.Windows.Controls;

    using log4net;

    using Microsoft.Practices.Composite.Modularity;
    using Microsoft.Practices.Composite.Regions;
    using Microsoft.Practices.Unity;

    using PokerTell.SessionReview.ViewModels;
    using PokerTell.SessionReview.Views;

    public class SessionReviewModule : IModule
    {
        #region Constants and Fields

        static readonly ILog Log = LogManager.GetLogger(
            MethodBase.GetCurrentMethod().DeclaringType);

        readonly IUnityContainer _container;

        readonly IRegionManager _regionManager;

        #endregion

        #region Constructors and Destructors

        public SessionReviewModule(IUnityContainer container, IRegionManager regionManager)
        {
            _container = container;
            _regionManager = regionManager;
        }

        #endregion

        #region Implemented Interfaces

        #region IModule

        public void Initialize()
        {
            _container.RegisterType<ISessionReviewViewModel, SessionReviewViewModel>();

            MenuItem sessionReviewMenuItem = _container.Resolve<SessionReviewMenuItemFactory>().Create();
            _regionManager.Regions["Shell.MainMenuRegion"].Add(sessionReviewMenuItem);

            Log.Info("got initialized.");
        }

        #endregion

        #endregion
    }
}