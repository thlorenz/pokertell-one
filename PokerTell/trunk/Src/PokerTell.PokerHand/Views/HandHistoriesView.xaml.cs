namespace PokerTell.PokerHand.Views
{
    using PokerTell.Infrastructure.Interfaces.PokerHand;

    /// <summary>
    /// Interaction logic for HandHistoriesView.xaml
    /// </summary>
    public partial class HandHistoriesView : IHandHistoriesView
    {
        #region Constructors and Destructors

        public HandHistoriesView()
        {
            InitializeComponent();
        }

        public HandHistoriesView(IHandHistoriesViewModel viewModel)
            : this()
        {
            DataContext = viewModel;
        }

        #endregion

        #region Properties

        public IHandHistoriesViewModel ViewModel
        {
            get { return (IHandHistoriesViewModel)DataContext; }
        }

        #endregion
    }
}