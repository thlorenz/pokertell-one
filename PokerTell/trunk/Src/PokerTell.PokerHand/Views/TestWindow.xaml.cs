namespace PokerTell.PokerHand.Views
{
    using System.Windows;

    using ViewModels.Design;

    /// <summary>
    /// Interaction logic for TestWindow.xaml
    /// </summary>
    public partial class TestWindow : Window
    {
        #region Constructors and Destructors

        public TestWindow()
        {
            InitializeComponent();
            DataContext = new HandHistoriesViewModel();
        }

        #endregion
    }
}