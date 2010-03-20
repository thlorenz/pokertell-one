namespace PokerTell.LiveTracker.Views
{
    using System;
    using System.Windows;
    using System.Windows.Input;

    using Interfaces;

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

        void Window_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
           const int rollSize = 120;

           int change = 0 - (e.Delta / rollSize);

           var pokerTableStatisticsViewModel = (IPokerTableStatisticsViewModel) DataContext;

           pokerTableStatisticsViewModel.DetailedStatisticsAnalyzer.CurrentViewModel.Scroll(change);

            e.Handled = true;
        }
    }
}