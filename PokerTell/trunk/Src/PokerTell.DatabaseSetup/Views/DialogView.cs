namespace PokerTell.DatabaseSetup.Views
{
    using System.Windows;
    using System.Windows.Input;

    public class DialogView : Window
    {
        public DialogView()
        {
            Owner = Application.Current.MainWindow;
        }

        protected void WindowBorder_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }

        protected void Close_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Close();
        }
    }
}