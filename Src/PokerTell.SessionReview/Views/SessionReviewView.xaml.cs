namespace PokerTell.SessionReview.Views
{
    using PokerTell.SessionReview.Interfaces;

    /// <summary>
    /// Interaction logic for SessionReviewView.xaml
    /// </summary>
    public partial class SessionReviewView
    {
        public SessionReviewView()
        {
            InitializeComponent();
        }

        public SessionReviewView(ISessionReviewViewModel viewModel)
            : this()
        {
            DataContext = viewModel;
        }

        public ISessionReviewViewModel ViewModel
        {
            get { return (ISessionReviewViewModel)DataContext; }
        }
    }
}