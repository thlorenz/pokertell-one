namespace PokerTell.SessionReview
{
    using System;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Controls;

    using Infrastructure;

    using log4net;

    using Microsoft.Practices.Composite.Modularity;
    using Microsoft.Practices.Composite.Regions;
    using Microsoft.Practices.Unity;

    using PokerTell.SessionReview.Interfaces;
    using PokerTell.SessionReview.ViewModels;
    using PokerTell.SessionReview.Views;

    public class SessionReviewModule : IModule
    {
        static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        readonly IUnityContainer _container;

        readonly IRegionManager _regionManager;

        public SessionReviewModule(IUnityContainer container, IRegionManager regionManager)
        {
            _container = container;
            _regionManager = regionManager;
        }

        public void Initialize()
        {
          _container.RegisterType<ISessionReviewViewModel, SessionReviewViewModel>();

           MenuItem sessionReviewMenuItem = _container.Resolve<SessionReviewMenuItemFactory>().Create();
          
            _regionManager.Regions[ApplicationProperties.ShellMainMenuRegion].Add(sessionReviewMenuItem);

            Log.Info("got initialized.");
        }


    }
}