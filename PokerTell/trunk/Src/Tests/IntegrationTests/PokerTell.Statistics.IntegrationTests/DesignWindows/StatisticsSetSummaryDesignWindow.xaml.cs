namespace PokerTell.StatisticsIntegrationTests.DesignWindows
{
    using System.Windows;

    using Statistics.IntegrationTests.DesignViewModels;

    /// <summary>
    /// Interaction logic for StatisticsSetSummaryDesignWindow.xaml
    /// </summary>
    public partial class StatisticsSetSummaryDesignWindow : Window
    {
        #region Constructors and Destructors

        public StatisticsSetSummaryDesignWindow()
        {
            DataContext = StatisticsSetSummaryDesignModel.GetReactionStatisticsSetSummaryDesignModel();
            InitializeComponent();
        }

        #endregion
    }
}