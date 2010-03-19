namespace PokerTell.LiveTracker.Views
{
    using System;
    using System.Windows;
    using System.Windows.Input;

    /// <summary>
    /// Interaction logic for PokerTableStatisticsView.xaml
    /// </summary>
    public partial class PokerTableStatisticsView : Window
    {
        public PokerTableStatisticsView()
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

        void CloseButton_Clicked(object sender, RoutedEventArgs e)
        {
            Hide();
        }
    }
}