namespace PokerTell.LiveTracker.IntegrationTests.DesignWindows
{
    using System.Windows;
    using System.Windows.Input;

    using Infrastructure.Enumerations.PokerHand;

    using DesignViewModels;

    using Microsoft.Practices.Composite.Events;

    /// <summary>
    /// Interaction logic for StatisticsSetSummaryDesignWindow.xaml
    /// </summary>
    public partial class TableStatisticsDesignWindow : Window
    {
        #region Constructors and Destructors

        public TableStatisticsDesignWindow(IEventAggregator eventAggregator)
        {
            DataContext = new TableStatisticsDesignModel(eventAggregator);
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