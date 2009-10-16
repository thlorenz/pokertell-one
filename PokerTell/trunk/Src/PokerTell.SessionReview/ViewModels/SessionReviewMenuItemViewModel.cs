namespace PokerTell.SessionReview.ViewModels
{
    using System.Reflection;
    using System.Windows.Controls;

    using Infrastructure.Interfaces.PokerHand;

    using log4net;

    using Microsoft.Practices.Composite.Presentation.Commands;
    using Microsoft.Practices.Composite.Regions;
    using Microsoft.Practices.Unity;

    public class SessionReviewMenuItemViewModel : MenuItem
    {
        static readonly ILog Log = LogManager.GetLogger(
            MethodBase.GetCurrentMethod().DeclaringType);


        public SessionReviewMenuItemViewModel(IUnityContainer container, IRegionManager regionManager)
        {
            _regionManager = regionManager;
            _container = container;
        }

        protected IUnityContainer _container;

        protected IRegionManager _regionManager;

        DelegateCommand<string> _openReviewCommand;

        DelegateCommand<string> _saveReviewCommand;

        public DelegateCommand<string> OpenReviewCommand
        {
            get
            {
                if (_openReviewCommand == null)
                {
                    _openReviewCommand = new DelegateCommand<string>(OpenReview, CanOpenReview);
                }

                return _openReviewCommand;
            }
        }

        public DelegateCommand<string> SaveReviewCommand
        {
            get
            {
                if (_saveReviewCommand == null)
                {
                    _saveReviewCommand = new DelegateCommand<string>(SaveReview, CanSaveReview);
                }

                return _saveReviewCommand;
            }
        }

        public bool CanOpenReview(string arg)
        {
            return true;
        }

        public bool CanSaveReview(string arg)
        {
            return true;
        }

        public void OpenReview(string arg)
        {
            var historiesView = _container.Resolve<IHandHistoriesView>();
            _regionManager
                .AddToRegion("Shell.MainRegion", historiesView)
                .Regions["Shell.MainRegion"].Activate(historiesView);

            Log.Info("Opening new Review");
        }

        public void SaveReview(string arg)
        {
            Log.Info("Saving Review");
        }
    }
}