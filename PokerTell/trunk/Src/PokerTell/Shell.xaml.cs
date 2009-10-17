namespace PokerTell
{
    using System.Windows;
    using System.Windows.Input;

    /// <summary>
    /// Interaction logic for Shell.xaml
    /// </summary>
    public partial class Shell : Window
    {
        readonly IShellViewModel _viewModel;

        #region Constructors and Destructors
        
        public Shell()
        {
            InitializeComponent();
        }

        public Shell(IShellViewModel shellViewModel)
            : this()
        {
            _viewModel = shellViewModel;
            DataContext = _viewModel;
        }

        #endregion

        #region Methods

        void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        void WindowBorder_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }

        #endregion
    }
}