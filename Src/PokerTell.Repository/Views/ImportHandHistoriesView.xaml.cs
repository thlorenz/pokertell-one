namespace PokerTell.Repository.Views
{
    using PokerTell.Repository.ViewModels;

    /// <summary>
    /// Interaction logic for ImportHandHistoriesView.xaml
    /// </summary>
    public partial class ImportHandHistoriesView
    {
        public ImportHandHistoriesView()
        {
            InitializeComponent();
        }

        public ImportHandHistoriesView(ImportHandHistoriesViewModel viewModel)
            : this()
        {
            DataContext = viewModel;
        }
    }
}