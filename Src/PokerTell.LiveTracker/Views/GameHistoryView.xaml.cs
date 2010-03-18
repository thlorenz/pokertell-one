namespace PokerTell.LiveTracker.Views
{
    using System.Windows;
    using System.Windows.Input;

    /// <summary>
    /// Interaction logic for GameHistoryView.xaml
    /// </summary>
    public partial class GameHistoryView : Window
    {
        public GameHistoryView()
        {
            InitializeComponent();
        }

        protected void WindowBorder_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }
    }
}