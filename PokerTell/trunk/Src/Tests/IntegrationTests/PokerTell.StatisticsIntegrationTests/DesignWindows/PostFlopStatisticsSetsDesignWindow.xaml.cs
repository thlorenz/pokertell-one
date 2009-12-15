namespace PokerTell.StatisticsIntegrationTests.DesignWindows
{
    using System.Windows;

    using DesignViewModels;

    using Infrastructure.Enumerations.PokerHand;

    using Statistics.ViewModels;

    /// <summary>
    /// Interaction logic for StatisticsSetSummaryDesignWindow.xaml
    /// </summary>
    public partial class PostFlopStatisticsSetsDesignWindow : Window
    {
        #region Constructors and Destructors

        public PostFlopStatisticsSetsDesignWindow()
        {
            DataContext = new PostFlopStatisticsSetsDesignModel(Streets.Flop);
            InitializeComponent();
        }

        #endregion
    }
}