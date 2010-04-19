namespace PokerTell.User.Views
{
    using ViewModels;

    /// <summary>
    /// Interaction logic for AboutView.xaml
    /// </summary>
    public partial class AboutView
    {
        public AboutView()
        {
            InitializeComponent();

            DataContext = new AboutViewModel();
        }
    }
}