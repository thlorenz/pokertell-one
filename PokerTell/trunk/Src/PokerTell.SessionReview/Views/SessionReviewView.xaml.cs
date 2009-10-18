namespace PokerTell.SessionReview.Views
{
    using System;
    using System.Reflection;

    using log4net;

    using Microsoft.Practices.Composite;

    using PokerTell.SessionReview.ViewModels;

    using Tools.WPF.Interfaces;

    /// <summary>
    /// Interaction logic for SessionReviewView.xaml
    /// </summary>
    public partial class SessionReviewView : IItemsRegionView
    {
        #region Constants and Fields

        static readonly ILog Log =
            LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        readonly ISessionReviewViewModel _viewModel;

        bool _isActive;

        #endregion

        #region Constructors and Destructors

        public SessionReviewView()
        {
            InitializeComponent();
        }

        public SessionReviewView(ISessionReviewViewModel viewModel)
            : this()
        {
            _viewModel = viewModel;

            DataContext = _viewModel;
        }

        #endregion

        #region Events

        public event EventHandler IsActiveChanged = delegate { };

        #endregion

        #region Properties

        public IItemsRegionViewModel ActiveAwareViewModel
        {
            get { return _viewModel; }
        }

        public bool IsActive
        {
            get { return _isActive; }

            set
            {
                _isActive = value;
                var vmAware = DataContext as IActiveAware;
                if (vmAware != null)
                {
                    vmAware.IsActive = value;
                }
            }
        }

        public ISessionReviewViewModel ViewModel
        {
            get { return _viewModel; }
        }

        #endregion
    }
}