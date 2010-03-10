namespace PokerTell.SessionReview.Views
{
    using System;
    using System.Reflection;

    using Interfaces;

    using log4net;

    using Microsoft.Practices.Composite;

    using Tools.WPF.Interfaces;

    /// <summary>
    /// Interaction logic for SessionReviewView.xaml
    /// </summary>
    public partial class SessionReviewView
    {
        #region Constants and Fields

        static readonly ILog Log =
            LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        #endregion

        #region Constructors and Destructors

        public SessionReviewView()
        {
            InitializeComponent();
        }

        public SessionReviewView(ISessionReviewViewModel viewModel)
            : this()
        {
            DataContext = viewModel;
        }

        #endregion

        #region Events

        #endregion

        #region Properties

        public ISessionReviewViewModel ViewModel
        {
            get { return (ISessionReviewViewModel)DataContext; }
        }

        #endregion
    }
}