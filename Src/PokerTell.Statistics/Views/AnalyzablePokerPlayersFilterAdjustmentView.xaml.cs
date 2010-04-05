namespace PokerTell.Statistics.Views
{
    using System.Windows;
    using System.Windows.Input;

    using Infrastructure.Interfaces.Statistics;

    using ViewModels;

    /// <summary>
    /// Interaction logic for AnalyzablePokerPlayersFilterAdjustmentView.xaml
    /// </summary>
    public partial class AnalyzablePokerPlayersFilterAdjustmentView 
    {
        #region Constructors and Destructors

        public AnalyzablePokerPlayersFilterAdjustmentView(IAnalyzablePokerPlayersFilterAdjustmentViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        #endregion

        void WindowBorder_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }
        
        void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}