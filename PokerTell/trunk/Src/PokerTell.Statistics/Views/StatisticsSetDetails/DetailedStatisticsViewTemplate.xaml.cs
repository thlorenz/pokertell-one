namespace PokerTell.Statistics.Views.StatisticsSetDetails
{
    using System.Linq;
    using System.Windows;

    using Microsoft.Windows.Controls;

    using PokerTell.Statistics.Interfaces;

    using Tools.FunctionalCSharp;

    public partial class DetailedStatisticsViewTemplate
    {
        #region Methods

        protected virtual void DataGrid_SelectedCellsChanged_LimitToCellsOnOneRow(object sender, SelectedCellsChangedEventArgs e)
        {
            var grid = (DataGrid)sender;

            LimitSelectionToOneRow(grid);

            UpdateViewModelWithCurrentSelection(grid);
        }

        static bool IsOnAnotherRow(DataGridCellInfo gridCellInfo, IStatisticsTableRowViewModel firstFoundSelectedRow)
        {
            return gridCellInfo.Item != firstFoundSelectedRow;
        }

        /// <summary>
        /// Prevents User from selecting items from different Rows, as it doesn't make sense to analyse them together
        /// </summary>
        /// <param name="grid"></param>
        static void LimitSelectionToOneRow(DataGrid grid)
        {
            IStatisticsTableRowViewModel firstSelectedRowFound = null;
            foreach (DataGridCellInfo gridCellInfo in grid.SelectedCells)
            {
                if (firstSelectedRowFound == null)
                {
                    firstSelectedRowFound = (IStatisticsTableRowViewModel)gridCellInfo.Item;
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
        static void ClearSelectionIfRowIsNotSelectable(DataGrid grid, IStatisticsTableRowViewModel firstSelectedRowFound)
        {
            if (firstSelectedRowFound != null && !firstSelectedRowFound.IsSelectable)
            {
                grid.SelectedCells.Clear();
            }
        }

        static void UpdateViewModelWithCurrentSelection(DataGrid grid)
        {
            var viewModel = (IStatisticsTableViewModel)grid.DataContext;
            viewModel.ClearSelection();

            foreach (DataGridCellInfo gridCellInfo in grid.SelectedCells)
            {
                var row = grid.Items.IndexOf(gridCellInfo.Item);

                var column = gridCellInfo.Column.DisplayIndex;

                viewModel.AddToSelection(row, column);
            }
        }

        #endregion

        /// <summary>
        /// Datagrid Loaded makes sure that the selected cells are restored again when the user returns to it
        /// The ViewModel itself is responsible for saving them e.g. when a command is executes that causes
        /// navigation to a new DataGrid (e.g. Investigate Raise)
        /// </summary>
        /// <param name="sender">DataGrid</param>
        /// <param name="e">Ignored</param>
        void DataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            var grid = (DataGrid)sender;
            var viewModel = (IStatisticsTableViewModel)grid.DataContext;
            grid.SelectedCellsChanged -= DataGrid_SelectedCellsChanged_LimitToCellsOnOneRow;

            viewModel.SavedSelectedCells.ForEach(selectedCell => grid.SelectedCells.Add(
                                                                     new DataGridCellInfo(viewModel.Rows.ElementAt(selectedCell.First), 
                                                                                          grid.Columns[selectedCell.Second])));

            grid.SelectedCellsChanged += DataGrid_SelectedCellsChanged_LimitToCellsOnOneRow;

            UpdateViewModelWithCurrentSelection(grid);
        }

        void DataGrid_SelectedCellsChanged_SelectEntireColumn(object sender, SelectedCellsChangedEventArgs e)
        {
            var grid = (DataGrid)sender;

            UpdateViewModelWithCurrentSelection(grid);

            var viewModel = (IStatisticsTableViewModel)grid.DataContext;
            var selectedColumns = viewModel.SelectedCells.Select(c => c.Second);

            grid.SelectedCellsChanged -= DataGrid_SelectedCellsChanged_SelectEntireColumn;
            foreach (int row in 0.To(grid.Items.Count - 1))
            {
                int rowIndex = row;
                selectedColumns.ForEach(col => {
                    var tobeAddedCell = new DataGridCellInfo(viewModel.Rows.ElementAt(rowIndex), grid.Columns[col]);
                    if (! grid.SelectedCells.Contains(tobeAddedCell))
                    {
                        grid.SelectedCells.Add(tobeAddedCell);
                    }
                });
            }

            grid.SelectedCellsChanged += DataGrid_SelectedCellsChanged_SelectEntireColumn;
            UpdateViewModelWithCurrentSelection(grid);
        }
    }
}