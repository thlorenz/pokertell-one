namespace PokerTell.PokerHand.Views
{
    using Infrastructure.Interfaces.PokerHand;

    /// <summary>
    /// Interaction logic for HandHistoriesView.xaml
    /// </summary>
    public partial class HandHistoriesView : IHandHistoriesView
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
