namespace PokerTell.StatisticsIntegrationTests.DesignWindows
{
    using System.Windows;

    using DesignViewModels;

    /// <summary>
    /// Interaction logic for StatisticsSetSummaryDesignWindow.xaml
    /// </summary>
    public partial class PreFlopStatisticsSetsDesignWindow : Window
    {
        #region Constructors and Destructors

        public PreFlopStatisticsSetsDesignWindow()
        {
            DataContext = new PreFlopStatisticsSetsDesignModel();
            InitializeComponent();
        }

        #endregion
    }
}