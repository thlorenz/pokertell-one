namespace PokerTell.PokerHand.Views
{
    using System.Windows.Controls;
    using System.Windows.Input;

    using Infrastructure.Interfaces.PokerHand;

    using Microsoft.Windows.Controls;

    using ViewModels;

    /// <summary>
    /// Interaction logic for HandHistoryView.xaml
    /// </summary>
    public partial class HandHistoryView : UserControl
    {
        #region Constructors and Destructors

        public HandHistoryView() 
        {
            InitializeComponent();
        }

        public HandHistoryView(IHandHistoryViewModel viewModel)
            : this()
        {
            DataContext = viewModel;
        }

        #endregion

        #region Methods
      
        private void DataGrid_MouseEnter(object sender, MouseEventArgs e)
        {
            ((DataGrid)sender).HeadersVisibility = DataGridHeadersVisibility.Column;
        }

        private void DataGrid_MouseLeave(object sender, MouseEventArgs e)
        {
            ((DataGrid)sender).HeadersVisibility = DataGridHeadersVisibility.None;
        }

        #endregion
    }
}