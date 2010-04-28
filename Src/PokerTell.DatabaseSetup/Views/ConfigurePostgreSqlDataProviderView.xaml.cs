namespace PokerTell.DatabaseSetup.Views
{
    using System.Windows.Controls;

    using ViewModels;

    /// <summary>
    /// Interaction logic for ConfigurePostgreSqlDataProviderView.xaml
    /// </summary>
    public partial class ConfigurePostgreSqlDataProviderView
    {
        public ConfigurePostgreSqlDataProviderView()
        {
            InitializeComponent();
        }

        public ConfigurePostgreSqlDataProviderView(ConfigurePostgreSqlDataProviderViewModel viewModel)
            : this()
        {
            DataContext = viewModel;
        }
    }
}