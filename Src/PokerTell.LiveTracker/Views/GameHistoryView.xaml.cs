namespace PokerTell.LiveTracker.Views
{
    using System;
    using System.Windows;
    using System.Windows.Input;

    using Interfaces;

    /// <summary>
    /// Interaction logic for GameHistoryView.xaml
    /// </summary>
    public partial class GameHistoryView : Window
    {
        public GameHistoryView()
        {
            InitializeComponent();

            Closing += (s, e) => {
                if (ViewModel != null) ViewModel.SaveDimensions();
            };
        }

        protected void WindowBorder_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }

        void CloseButton_Clicked(object sender, RoutedEventArgs e)
        {
            Hide();
        }

        void Window_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
           const int rollSize = 120;

           var gameHistoryViewModel = (IGameHistoryViewModel) DataContext;

           int change = 0 - (e.Delta / rollSize);

           gameHistoryViewModel.CurrentHandIndex += change;
        }

        IGameHistoryViewModel ViewModel
        {
            get { return (IGameHistoryViewModel)DataContext; }
        }
    }
}