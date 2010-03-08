namespace DetailedStatisticsViewer.Views
{
    using Microsoft.Windows.Controls;

    using ViewModels;

    public partial class DetailedStatisticsViewTemplate
    {
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

            ClearSelectionIfRowIsNotSelectable(grid, firstSelectedRowFound);
        }

        /// <summary>
        /// Prevents user from selecting a row that shouldn't be selected e.g. the Counts row
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="firstSelectedRowFound">Row that user is trying to select</param>
        static void ClearSelectionIfRowIsNotSelectable(DataGrid grid, RowViewModel firstSelectedRowFound)
        {
            if (firstSelectedRowFound != null && !firstSelectedRowFound.IsSelectable)
            {
                grid.SelectedCells.Clear();
            }
        }

        static void UpdateViewModelWithCurrentSelection(DataGrid grid)
        {
            var viewModel = (IDetailedStatisticsViewModel)grid.DataContext;
            viewModel.ClearSelection();

            foreach (DataGridCellInfo gridCellInfo in grid.SelectedCells)
            {
                var row = grid.Items.IndexOf(gridCellInfo.Item);

                var column = gridCellInfo.Column.DisplayIndex;

                viewModel.AddToSelectionFrom(row, column);
            }
        }

        #endregion
    }
}