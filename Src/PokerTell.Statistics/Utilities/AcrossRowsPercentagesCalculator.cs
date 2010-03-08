namespace PokerTell.Statistics.Utilities
{
    using System;

    using Interfaces;

    using Tools.Extensions;

    /// <summary>
    ///   Used to calculate percentages of number of values in relation o the total count of values in the
    ///   same column.
    /// </summary>
    public class AcrossRowsPercentagesCalculator : IPercentagesCalculator
    {
        #region Constants and Fields

        Func<int, int, int> _getCountAtRowColumn;

        int _numberOfColumns;

        int _numberOfRows;

        Action<int, int, int> _setPercentageAtRowColumn;

        #endregion

        #region Properties

        public int[] SumsOfCountsByColumn { get; private set; }

        #endregion

        #region Implemented Interfaces

        #region IPercentagesCalculator

        /// <summary>
        ///   Manipulates percentage values via the Action (last parameter)
        /// </summary>
        /// <param name="getNumberOfRows">
        ///   Function that returns number of rows in the table
        /// </param>
        /// <param name="getNumberOfColumnsAtRow">
        ///   Function that returns number columns at a given of row in the table
        /// </param>
        /// <param name="getCountAtRowColumn">
        ///   Function that returns number of counts at tablecell given by rows and column
        /// </param>
        /// <param name="setPercentageAtRowColumn">
        ///   Action that will set the calculated percentages for all cells, which are indexed via the given row and column
        /// </param>
        /// <returns>
        ///   Self to allow fluent interface
        /// </returns>
        public IPercentagesCalculator CalculatePercentages(
            Func<int> getNumberOfRows,
            Func<int, int> getNumberOfColumnsAtRow,
            Func<int, int, int> getCountAtRowColumn,
            Action<int, int, int> setPercentageAtRowColumn)
        {
            ValidateAndInitialize(getNumberOfRows, getNumberOfColumnsAtRow, getCountAtRowColumn, setPercentageAtRowColumn);

            for (int col = 0; col < _numberOfColumns; col++)
            {
                SumUpCountsOfAllRowsForColumn(col);

                SetPercentagesForColumn(col);
            }

            return this;
        }

        #endregion

        #endregion

        #region Methods

        static int GetAndValidateNumberOfRows(Func<int> getNumberOfRows)
        {
            int numberOfRows = getNumberOfRows();
            if (numberOfRows < 2)
            {
                throw new ArgumentException(
                    "Needs to contain at least two rows but contained only " + numberOfRows);
            }

            return numberOfRows;
        }

        int GetAndValidateNumberOfColumns(Func<int, int> getNumberOfColumnsAtRow)
        {
            int numberOfColumnsOfFirstRow = getNumberOfColumnsAtRow(0);
            if (numberOfColumnsOfFirstRow < 1)
            {
                throw new ArgumentException("Column count needs to be at least 1");
            }

            for (int row = 1; row < _numberOfRows; row++)
            {
                if (getNumberOfColumnsAtRow(row) != numberOfColumnsOfFirstRow)
                {
                    throw new ArgumentException("Column count needs to be the same for all rows");
                }
            }

            return numberOfColumnsOfFirstRow;
        }

        void SetPercentagesForColumn(int col)
        {
            for (int row = 0; row < _numberOfRows; row++)
            {
                if (SumsOfCountsByColumn[col] == 0)
                {
                    _setPercentageAtRowColumn(row, col, 0);
                }
                else
                {
                    double percentage = (double)_getCountAtRowColumn(row, col) / SumsOfCountsByColumn[col] * 100;

                    _setPercentageAtRowColumn(row, col, percentage.ToInt());
                }
            }
        }

        void SumUpCountsOfAllRowsForColumn(int col)
        {
            for (int row = 0; row < _numberOfRows; row++)
            {
                SumsOfCountsByColumn[col] += _getCountAtRowColumn(row, col);
            }
        }

        void ValidateAndInitialize(
            Func<int> getNumberOfRows,
            Func<int, int> getNumberOfColumnsAtRow,
            Func<int, int, int> getCountAtRowColumn,
            Action<int, int, int> setPercentageAtRowColumn)
        {
            _getCountAtRowColumn = getCountAtRowColumn;
            _setPercentageAtRowColumn = setPercentageAtRowColumn;

            _numberOfRows = GetAndValidateNumberOfRows(getNumberOfRows);

            _numberOfColumns = GetAndValidateNumberOfColumns(getNumberOfColumnsAtRow);

            SumsOfCountsByColumn = new int[_numberOfColumns];
        }

        #endregion
    }
}