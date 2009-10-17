namespace PokerTell.SessionReview.ViewModels
{
    using System.Reflection;
    using System.Windows.Controls;
    using System.Windows.Input;

    using Infrastructure;

    using log4net;

    using Microsoft.Practices.Composite.Regions;
    using Microsoft.Practices.Unity;

    using PokerTell.Infrastructure.Interfaces.PokerHand;

    using Tools.WPF;

    using Views;

    public class SessionReviewMenuItemViewModel : MenuItem
    {
        #region Constants and Fields

        protected IUnityContainer _container;

        protected IRegionManager _regionManager;

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
            var reviewView = _container.Resolve<SessionReviewView>();
            var region = _regionManager.Regions[ApplicationProperties.ShellMainRegion];
            region.Add(reviewView);
            region.Activate(reviewView);
           
        }

        #endregion
    }
}