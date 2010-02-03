namespace PokerTell.Statistics.Views.StatisticsSetDetails
{
    using Infrastructure.Interfaces.Statistics;

    using Interfaces;

    using ViewModels.Base;
    using ViewModels.StatisticsSetDetails;

    using Microsoft.Windows.Controls;

    public partial class DetailedStatisticsViewTemplate
    {
        #region Methods

        protected virtual void DataGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            var grid = (DataGrid)sender;

            LimitSelectionToOneRow(grid);

            UpdateViewModelWithCurrentSelection(grid);
        }

        static bool IsOnAnotherRow(DataGridCellInfo gridCellInfo, StatisticsTableRowViewModel firstFoundSelectedRow)
        {
            return gridCellInfo.Item != firstFoundSelectedRow;
        }

        /// <summary>
        /// Prevents User from selecting items from different Rows, as it doesn't make sense to analyse them together
        /// </summary>
        /// <param name="grid"></param>
        static void LimitSelectionToOneRow(DataGrid grid)
        {
            StatisticsTableRowViewModel firstSelectedRowFound = null;
            foreach (DataGridCellInfo gridCellInfo in grid.SelectedCells)
            {
                if (firstSelectedRowFound == null)
                {
                    firstSelectedRowFound = (StatisticsTableRowViewModel)gridCellInfo.Item;
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
        static void ClearSelectionIfRowIsNotSelectable(DataGrid grid, StatisticsTableRowViewModel firstSelectedRowFound)
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

                viewModel.AddToSelection(row, column);
            }
        }

        #endregion
    }
}