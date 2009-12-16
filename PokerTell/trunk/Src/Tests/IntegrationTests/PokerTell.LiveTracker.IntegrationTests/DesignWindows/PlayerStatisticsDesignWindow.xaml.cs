namespace PokerTell.LiveTracker.IntegrationTests.DesignWindows
{
    using System.Windows;
    using System.Windows.Input;

    using Infrastructure.Enumerations.PokerHand;

    using DesignViewModels;

    /// <summary>
    /// Interaction logic for StatisticsSetSummaryDesignWindow.xaml
    /// </summary>
    public partial class PlayerStatisticsDesignWindow : Window
    {
        #region Constructors and Destructors

        public PlayerStatisticsDesignWindow()
        {
            DataContext = new PlayerStatisticsDesignModel();
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