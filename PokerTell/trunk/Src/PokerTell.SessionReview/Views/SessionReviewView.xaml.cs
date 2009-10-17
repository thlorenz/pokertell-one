namespace PokerTell.SessionReview.Views
{
    using System;
    using System.Windows.Controls;

    using Microsoft.Practices.Composite;

    using ViewModels;

    /// <summary>
    /// Interaction logic for SessionReviewView.xaml
    /// </summary>
    public partial class SessionReviewView : IActiveAware
    {
        #region Constructors and Destructors

        bool _isActive;

        public SessionReviewView()
        {
            InitializeComponent();
        }

        public SessionReviewView(ISessionReviewViewModel viewModel)
        {
            DataContext = viewModel;
        }

        #endregion

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

        public event EventHandler IsActiveChanged = delegate { };
    }
}