namespace PokerTell.LiveTracker.IntegrationTests.DesignWindows
{
    using System.Windows;
    using System.Windows.Input;

    using Infrastructure.Enumerations.PokerHand;

    using DesignViewModels;

    using Infrastructure.Interfaces.PokerHand;

    using Microsoft.Practices.Composite.Events;

    using Statistics.Interfaces;

   /// <summary>
    /// Interaction logic for StatisticsSetSummaryDesignWindow.xaml
    /// </summary>
    public partial class TableStatisticsDesignWindow : Window
    {
        #region Constructors and Destructors



        public TableStatisticsDesignWindow(
           IEventAggregator eventAggregator,
            IHandBrowserViewModel handBrowserViewModel,
         IRaiseReactionStatisticsBuilder raiseReactionStatisticsBuilder,
         IPostFlopHeroActsRaiseReactionDescriber raiseReactionDescriber)
        {
            DataContext = new PokerTableStatisticsDesignModel(eventAggregator, handBrowserViewModel, raiseReactionStatisticsBuilder, raiseReactionDescriber);
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