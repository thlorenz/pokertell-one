namespace PokerTell.LiveTracker.IntegrationTests.DesignWindows
{
    using System.Windows;
    using System.Windows.Input;

    using Infrastructure.Enumerations.PokerHand;

    using DesignViewModels;

    /// <summary>
    /// Interaction logic for StatisticsSetSummaryDesignWindow.xaml
    /// </summary>
    public partial class TableStatisticsDesignWindow : Window
    {
        #region Constructors and Destructors

        public TableStatisticsDesignWindow()
        {
            DataContext = new TableStatisticsDesignModel();
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