namespace PokerTell.Statistics.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Interfaces;

    using Tools.Extensions;

    /// <summary>
    ///   Used to calculate percentages of number of values in relation o the total count of values in the
    ///   same row. As a result each row gets treated separately since rows don't influence each others results.
    /// </summary>
    public class SeparateRowsPercentagesCalculator : IPercentagesCalculator
    {
        #region Constants and Fields

        Func<int, int, int> _getCountAtRowColumn;

        Func<int, int> _getNumberOfColumnsAtRow;

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
        /// <exception cref="System.ArgumentException">
        ///   If rows have different lenghts or less than one row and/or one column was found
        /// </exception>
        public IPercentagesCalculator CalculatePercentages(
            Func<int> getNumberOfRows,
            Func<int, int> getNumberOfColumnsAtRow,
            Func<int, int, int> getCountAtRowColumn,
            Action<int, int, int> setPercentageAtRowColumn)
        {
            ValidateAndInitialize(
                getNumberOfRows,
                getNumberOfColumnsAtRow,
                getCountAtRowColumn,
                setPercentageAtRowColumn);

            for (int row = 0; row < _numberOfRows; row++)
            {
                int[] countsAtCurrentRow = GetCountsForRow(row);

                AddCountsOfCurrentRowToSumOfCountsByColumn(countsAtCurrentRow);

                int sumOfCountsForCurrentRow = GetSumForCurrentRow(countsAtCurrentRow);

                SetPercentagesForRow(row, countsAtCurrentRow, sumOfCountsForCurrentRow);
            }

            return this;
        }

        #endregion

        #endregion

        #region Methods

        static int GetAndValidateNumberOfRows(Func<int> getNumberOfRows)
        {
            int numberOfRows = getNumberOfRows();
            if (numberOfRows < 1)
            {
                throw new ArgumentException(
                    "Needs to contain at least one row but contained only " + numberOfRows);
            }

            return numberOfRows;
        }

        static int GetSumForCurrentRow(IEnumerable<int> countsAtCurrentRow)
        {
            return (from count in countsAtCurrentRow select count).Sum();
        }

        void AddCountsOfCurrentRowToSumOfCountsByColumn(int[] countsAtCurrentRow)
        {
            for (int col = 0; col < _numberOfColumns; col++)
            {
                SumsOfCountsByColumn[col] += countsAtCurrentRow[col];
            }
        }

        int GetAndValidateNumberOfColumns(Func<int, int> getNumberOfColumnsAtRow)
        {
            int numberOfColumnsOfFirstRow = getNumberOfColumnsAtRow(0);
            if (numberOfColumnsOfFirstRow < 1)
            {
                throw new ArgumentException("Column count needs to be at least 2");
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

        int[] GetCountsForRow(int row)
        {
            var countsAtCurrentRow = new int[_numberOfColumns];

            for (int col = 0; col < _numberOfColumns; col++)
            {
                countsAtCurrentRow[col] = _getCountAtRowColumn(row, col);
            }

            return countsAtCurrentRow;
        }

        void SetPercentagesForRow(int currentRow, int[] countsAtCurrentRow, int sumOfCountsForCurrentRow)
        {
            for (int col = 0; col < _numberOfColumns; col++)
            {
                if (sumOfCountsForCurrentRow == 0)
                {
                    _setPercentageAtRowColumn(currentRow, col, 0);
                }
                else
                {
                    double percentage = (double)countsAtCurrentRow[col] / sumOfCountsForCurrentRow * 100;
                    
                    _setPercentageAtRowColumn(currentRow, col, percentage.ToInt());
                }
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
            _getNumberOfColumnsAtRow = getNumberOfColumnsAtRow;

            _numberOfRows = GetAndValidateNumberOfRows(getNumberOfRows);
            _numberOfColumns = GetAndValidateNumberOfColumns(getNumberOfColumnsAtRow);

            SumsOfCountsByColumn = new int[_numberOfColumns];
        }

        #endregion
    }
}