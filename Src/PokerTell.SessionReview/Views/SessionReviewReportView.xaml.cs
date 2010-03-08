namespace PokerTell.SessionReview.Views
{
    using System.Windows.Controls;

    using PokerTell.SessionReview.ViewModels;

    /// <summary>
    /// Interaction logic for SessionReviewReportView.xaml
    /// </summary>
    public partial class SessionReviewReportView
    {
        #region Constructors and Destructors

        public SessionReviewReportView()
        {
            InitializeComponent();
        }

        public SessionReviewReportView(SessionReviewReportViewModel viewModel)
            : this()
        {
            DataContext = viewModel;

            viewModel.PrintRequested += () => {
                var printDialog = new PrintDialog();
                printDialog.PrintVisual(WebBrowser, "Report");
            };

            WebBrowser.NavigateToString(viewModel.HtmlDocumentText);
        }

        #endregion
    }
}