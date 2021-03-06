namespace PokerTell.LiveTracker.Design.Statistics
{
    using System.Windows;
    using System.Windows.Input;

    using Infrastructure.Interfaces.PokerHand;

    using Microsoft.Practices.Composite.Events;

    /// <summary>
    /// Interaction logic for StatisticsSetSummaryDesignWindow.xaml
    /// </summary>
    public partial class TableStatisticsDesignWindow : Window
    {
        #region Constructors and Destructors

        public TableStatisticsDesignWindow(IEventAggregator eventAggregator, IRepositoryHandBrowserViewModel handBrowserViewModel)
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

        #endregion
    }
}