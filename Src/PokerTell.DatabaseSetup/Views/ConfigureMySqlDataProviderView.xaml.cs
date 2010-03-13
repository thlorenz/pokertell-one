namespace PokerTell.DatabaseSetup.Views
{
    using PokerTell.DatabaseSetup.ViewModels;

    /// <summary>
    /// Interaction logic for ConfigureMySqlDataProviderView.xaml
    /// </summary>
    public partial class ConfigureMySqlDataProviderView
    {
        public ConfigureMySqlDataProviderView()
        {
            InitializeComponent();
        }

        public ConfigureMySqlDataProviderView(ConfigureMySqlDataProviderViewModel viewModel)
            : this()
        {
            DataContext = viewModel;
        }
    }
}