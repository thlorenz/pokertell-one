namespace PokerTell.PokerHand.Views
{
    using System;
    using System.Windows.Input;

    using Infrastructure.Interfaces.PokerHand;

    using Microsoft.Windows.Controls;

    using Tools.WPF.Interfaces;

    /// <summary>
    /// Interaction logic for HandHistoryView.xaml
    /// </summary>
    public partial class HandHistoryView : IItemsRegionView
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

        public bool IsActive
        {
            get { return ((IHandHistoryViewModel) DataContext).IsActive; }
            set { ((IHandHistoryViewModel)DataContext).IsActive = value; }
        }

        public event EventHandler IsActiveChanged;

        public IItemsRegionViewModel ActiveAwareViewModel
        {
            get { return (IItemsRegionViewModel) DataContext; }
        }
    }
}