namespace PokerTell.SessionReview.ViewModels
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Windows.Controls;
    using System.Windows.Input;

    using Infrastructure;
    using Infrastructure.Interfaces;
    using Infrastructure.Interfaces.PokerHand;
    using Infrastructure.Interfaces.Repository;

    using log4net;

    using Microsoft.Practices.Composite.Regions;
    using Microsoft.Practices.Unity;
    using Microsoft.Win32;

    using Tools.Serialization;
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

        readonly IRepository _repository;

        readonly IConstructor<IHandHistoriesViewModel> _handHistoriesViewModelMake;

        ICommand _importHandHistoriesCommand;

        #endregion

        #region Constructors and Destructors

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

        void ImportHandHistories(object arg)
        {
            var handHistoriesViewModel = ImportHandHistoriesViewModelFromFile();

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

        #endregion

        #region Public Methods

        public void OpenReview(object arg)
        {
            var handHistoriesViewModel = LoadHandHistoriesViewModelFromFile();

            ShowSessionReviewFor(handHistoriesViewModel);
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

        #endregion

        IHandHistoriesViewModel LoadHandHistoriesViewModelFromFile()
        {
            var openFileDialog = new OpenFileDialog
            {
                AddExtension = true,
                DefaultExt = "pthh",
                Filter = "PokerTell HandHistories (*.pthh)|*.pthh|All files (*.*)|*.*",
                Title = "Open PokerTell Session Review"
            };

            IHandHistoriesViewModel handHistoriesViewModel;

            if ((bool)openFileDialog.ShowDialog())
            {
                handHistoriesViewModel =
                    (IHandHistoriesViewModel)BinarySerializer.Deserialize(openFileDialog.FileName);
               
                return handHistoriesViewModel;
            }
            
            handHistoriesViewModel = _container.Resolve<IHandHistoriesViewModel>();
            handHistoriesViewModel.HandHistoriesFilter.HeroName = "hero";
            return handHistoriesViewModel;
        }
    }
    
}