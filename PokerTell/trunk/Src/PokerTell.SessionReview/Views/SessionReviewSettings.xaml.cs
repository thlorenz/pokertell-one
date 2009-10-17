namespace PokerTell.SessionReview.Views
{
    using System.Windows.Controls;

    using ViewModels;

    /// <summary>
    /// Interaction logic for SessionReviewSettingsView.xaml
    /// </summary>
    public partial class SessionReviewSettingsView 
    {
        #region Constructors and Destructors

        public SessionReviewSettingsView(SessionReviewSettingsViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        #endregion
    }
}