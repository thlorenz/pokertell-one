namespace PokerTell.DatabaseSetup.Views
{
    using System.Windows.Media;

    using Infrastructure;

    using Tools;

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

            if (Static.OperatingSystemIsWindowsXPOrOlder())
            {
                Background = ApplicationProperties.BorderedWindowBackgoundBrush;
                AllowsTransparency = false;
            }
        }

        public ConfigureMySqlDataProviderView(ConfigureMySqlDataProviderViewModel viewModel)
            : this()
        {
            DataContext = viewModel;
        }
    }
}
