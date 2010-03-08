namespace DataGridViewSelection
{
    using Microsoft.Windows.Controls;

    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1
    {
        #region Constants and Fields

        readonly RaiseStatisticsViewModel _viewModel;

        #endregion

        #region Constructors and Destructors

        public Window1()
        {
            InitializeComponent();
            DataContext = _viewModel = new RaiseStatisticsViewModel();
        }

        #endregion

        #region Methods

        protected virtual void DataGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            var grid = (DataGrid)sender;

            LimitSelectionToOneRow(grid);

            UpdateViewModelWithCurrentSelection(grid);
        }

        static bool IsOnAnotherRow(DataGridCellInfo gridCellInfo, RowViewModel firstFoundSelectedRow)
        {
            return gridCellInfo.Item != firstFoundSelectedRow;
        }

        /// <summary>
        /// Prevents User from selecting items from different Rows, as it doesn't make sense to analyse them together
        /// </summary>
        /// <param name="grid"></param>
        static void LimitSelectionToOneRow(DataGrid grid)
        {
            RowViewModel firstSelectedRowFound = null;
            foreach (DataGridCellInfo gridCellInfo in grid.SelectedCells)
            {
                if (firstSelectedRowFound == null)
                {
                    firstSelectedRowFound = (RowViewModel)gridCellInfo.Item;
                }
                else if (IsOnAnotherRow(gridCellInfo, firstSelectedRowFound))
                {
                    grid.SelectedCells.Remove(gridCellInfo);
                }
            }

            ClearSelectionIfRowIsNotBeSelectable(grid, firstSelectedRowFound);
        }

        static void ClearSelectionIfRowIsNotBeSelectable(DataGrid grid, RowViewModel firstSelectedRowFound)
        {
            if (firstSelectedRowFound != null && !firstSelectedRowFound.IsSelectable)
            {
                grid.SelectedCells.Clear();
            }
        }

        void UpdateViewModelWithCurrentSelection(DataGrid grid)
        {
            _viewModel.ClearSelection();

            foreach (DataGridCellInfo gridCellInfo in grid.SelectedCells)
            {
                var row = grid.Items.IndexOf(gridCellInfo.Item);

                var column = gridCellInfo.Column.DisplayIndex;

                _viewModel.AddToSelectionFrom(row, column);
            }
        }

        #endregion
    }
}