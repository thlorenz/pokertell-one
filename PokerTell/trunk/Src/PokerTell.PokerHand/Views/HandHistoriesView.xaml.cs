namespace PokerTell.PokerHand.Views
{
    using System.Windows.Controls;

    using Infrastructure.Interfaces.PokerHand;

    /// <summary>
    /// Interaction logic for HandHistoriesView.xaml
    /// </summary>
    public partial class HandHistoriesView : UserControl, IHandHistoriesView
    {
        public HandHistoriesView()
        {
            InitializeComponent();
        }

        public HandHistoriesView(IHandHistoriesViewModel viewModel)
            : this()
        {
            DataContext = viewModel;
        }

        public IHandHistoriesViewModel ViewModel
        {
            get { return (IHandHistoriesViewModel)DataContext; }
        }
    }
}
