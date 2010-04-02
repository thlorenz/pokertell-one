namespace PokerTell.SessionReview.ViewModels
{
    using System.Reflection;
    using System.Windows.Controls;
    using System.Windows.Input;

    using log4net;

    using Microsoft.Practices.Composite.Regions;
    using Microsoft.Practices.Unity;
    using Microsoft.Win32;

    using PokerTell.Infrastructure;
    using PokerTell.Infrastructure.Interfaces;
    using PokerTell.Infrastructure.Interfaces.PokerHand;
    using PokerTell.Infrastructure.Interfaces.Repository;
    using PokerTell.SessionReview.Views;

    using Tools.Serialization;
    using Tools.WPF;

    public class SessionReviewMenuItemViewModel : MenuItem
    {
        static readonly ILog Log = LogManager.GetLogger(
            MethodBase.GetCurrentMethod().DeclaringType);

        readonly IUnityContainer _container;

        readonly IConstructor<IHandHistoriesViewModel> _handHistoriesViewModelMake;

        readonly IRegionManager _regionManager;

        readonly IRepository _repository;

        ICommand _importHandHistoriesCommand;

        ICommand _openReviewCommand;

        public SessionReviewMenuItemViewModel(
            IUnityContainer container, 
            IRegionManager regionManager, 
            IRepository repository, 
            IConstructor<IHandHistoriesViewModel> handHistoriesViewModelMake)
        {
            _regionManager = regionManager;
            _container = container;
            _repository = repository;
            _handHistoriesViewModelMake = handHistoriesViewModelMake;
        }

        public ICommand ImportHandHistoriesCommand
        {
            get
            {
                return _importHandHistoriesCommand ?? (_importHandHistoriesCommand = new SimpleCommand
                    {
                        ExecuteDelegate = ImportHandHistories
                    });
            }
        }

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
            get { return Commands.SaveSessionReviewCommand; }
        }

        public void OpenReview(object arg)
        {
            var handHistoriesViewModel = LoadHandHistoriesViewModelFromFile();

            if (handHistoriesViewModel != null)
                ShowSessionReviewFor(handHistoriesViewModel);
        }

        static IHandHistoriesViewModel LoadHandHistoriesViewModelFromFile()
        {
            var openFileDialog = new OpenFileDialog
                {
                    AddExtension = true, 
                    DefaultExt = "pthh", 
                    Filter = "PokerTell HandHistories (*.pthh)|*.pthh|All files (*.*)|*.*", 
                    Title = "Open PokerTell Session Review"
                };

            if ((bool)openFileDialog.ShowDialog())
            {
                var handHistoriesViewModel = (IHandHistoriesViewModel)BinarySerializer.Deserialize(openFileDialog.FileName);

                return handHistoriesViewModel;
            }

            return null;
        }

        void ImportHandHistories(object arg)
        {
            var handHistoriesViewModel = ImportHandHistoriesViewModelFromFile();

            if (handHistoriesViewModel != null)
                ShowSessionReviewFor(handHistoriesViewModel);
        }

        IHandHistoriesViewModel ImportHandHistoriesViewModelFromFile()
        {
            var openFileDialog = new OpenFileDialog
                {
                    AddExtension = true, 
                    DefaultExt = "txt", 
                    Filter = "HandHistories (*.txt)|*.txt|All files (*.*)|*.*", 
                    Title = "Import Hand Histories"
                };

            if ((bool)openFileDialog.ShowDialog())
            {
                var convertedHands = _repository.RetrieveHandsFromFile(openFileDialog.FileName);
                var handHistoriesViewModel = _handHistoriesViewModelMake.New.InitializeWith(convertedHands);

                return handHistoriesViewModel;
            }

            return null;
        }

        void ShowSessionReviewFor(IHandHistoriesViewModel handHistoriesViewModel)
        {
            if (handHistoriesViewModel == null)
            {
                return;
            }

            // All need to get same HandHistoriesViewModel
            var childContainer = _container.CreateChildContainer();
            childContainer.RegisterInstance(handHistoriesViewModel);

            var reviewView = childContainer.Resolve<SessionReviewView>();
            var handHistoriesView = childContainer.Resolve<IHandHistoriesView>();
            var settingsView = childContainer.Resolve<SessionReviewSettingsView>();

            var region = _regionManager.Regions[ApplicationProperties.ShellMainRegion];

            var scopedRegionManager = region.Add(reviewView, null, true);
            scopedRegionManager.Regions[ApplicationProperties.HandHistoriesRegion].Add(handHistoriesView);
            scopedRegionManager.Regions["ReviewSettingsRegion"].Add(settingsView);

            region.Activate(reviewView);
        }
    }
}