namespace PokerTell
{
    using System.Windows;
    using System.Windows.Input;

    /// <summary>
    /// Interaction logic for Shell.xaml
    /// </summary>
    public partial class Shell : Window
    {
        #region Constructors and Destructors

        public Shell()
        {
            InitializeComponent();
        }

        #endregion

        #region Methods

        void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        void MaiximizeButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;

        }

        void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
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