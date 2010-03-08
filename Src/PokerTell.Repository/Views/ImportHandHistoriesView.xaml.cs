namespace PokerTell.Repository.Views
{
    using Infrastructure;

    using Tools;

    using ViewModels;

    /// <summary>
    /// Interaction logic for ImportHandHistoriesView.xaml
    /// </summary>
    public partial class ImportHandHistoriesView 
    {
        public ImportHandHistoriesView()
        {
            InitializeComponent();
            if (Static.OperatingSystemIsWindowsXPOrOlder())
            {
                Background = ApplicationProperties.BorderedWindowBackgoundBrush;
                AllowsTransparency = false;
            }
        }

        public ImportHandHistoriesView(ImportHandHistoriesViewModel viewModel)
            : this()
        {
            DataContext = viewModel;
        }
    }
}
