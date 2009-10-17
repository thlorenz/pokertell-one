namespace PokerTell.SessionReview.ViewModels
{
    using System.Reflection;
    using System.Windows.Controls;
    using System.Windows.Input;

    using Infrastructure;
    using Infrastructure.Interfaces.PokerHand;

    using log4net;

    using Microsoft.Practices.Composite.Regions;
    using Microsoft.Practices.Unity;

    using Tools.WPF;

    using Views;

    public class SessionReviewMenuItemViewModel : MenuItem
    {
        #region Constants and Fields

        readonly IUnityContainer _container;

        readonly IRegionManager _regionManager;

        static readonly ILog Log = LogManager.GetLogger(
            MethodBase.GetCurrentMethod().DeclaringType);

        ICommand _openReviewCommand;

        #endregion

        #region Constructors and Destructors

        public SessionReviewMenuItemViewModel(IUnityContainer container, IRegionManager regionManager)
        {
            _regionManager = regionManager;
            _container = container;
        }

        #endregion

        #region Properties

        public ICommand OpenReviewCommand
        {
            get
            {
                return _openReviewCommand ?? (_openReviewCommand = new SimpleCommand
                    {
                        ExecuteDelegate = OpenReview
                    });
            }
        }

        public ICommand SaveReviewCommand
        {
            get
            {
                return Commands.SaveSessionReviewCommand;
            }
        }
       
        #endregion

        #region Public Methods

        public void OpenReview(object arg)
        {
            // SessionReviewViewModel and SessionReviewSettingsViewModel need to get the same HandHistoriesView
            var childContainer = _container.CreateChildContainer()
                .RegisterInstance(_container.Resolve<IHandHistoriesViewModel>());

            var reviewView = childContainer.Resolve<SessionReviewView>();
            var settingsView = childContainer.Resolve<SessionReviewSettingsView>();

            var region = _regionManager.Regions[ApplicationProperties.ShellMainRegion];
            const bool createRegionManagerScope = true;
            var scopedRegion = region.Add(reviewView, null, createRegionManagerScope);
            scopedRegion.Regions["ReviewSettingsRegion"].Add(settingsView);

            region.Activate(reviewView);
        }

        #endregion
    }
}