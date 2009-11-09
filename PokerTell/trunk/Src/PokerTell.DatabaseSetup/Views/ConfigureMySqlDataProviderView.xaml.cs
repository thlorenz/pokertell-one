namespace PokerTell.DatabaseSetup.Views
{
    using System.Windows;
    using System.Windows.Input;

    using ViewModels;

    /// <summary>
    /// Interaction logic for ConfigureMySqlDataProviderView.xaml
    /// </summary>
    public partial class ConfigureMySqlDataProviderView
    {
        public ConfigureMySqlDataProviderView()
            : base()
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
