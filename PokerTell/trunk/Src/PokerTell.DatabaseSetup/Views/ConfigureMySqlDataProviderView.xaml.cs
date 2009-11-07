namespace PokerTell.DatabaseSetup.Views
{
    using System.Windows.Input;

    using ViewModels;

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

        void WindowBorder_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }

        private void Close_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Close();
        }
    }
}
