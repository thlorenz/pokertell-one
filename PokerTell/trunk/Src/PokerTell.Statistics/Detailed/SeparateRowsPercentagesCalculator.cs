namespace PokerTell.Statistics.Detailed
{
    using System;

    using PokerTell.Statistics.Interfaces;

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

        public int[] SumOfCountsByColumn { get; private set; }

        #endregion

        #region Implemented Interfaces

        #region IPercentagesCalculator

        public IPercentagesCalculator CalculatePercentages(
            Func<int> getNumberOfRows, 
            Func<int, int> getNumberOfColumnsAtRow, 
            Func<int, int, int> getCountAtRowColumn, 
            Action<int, int, int> setPercentageAtRowColumn)
        {
            ValidateAndInitialize(getNumberOfRows, getNumberOfColumnsAtRow, getCountAtRowColumn, setPercentageAtRowColumn);

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
            var numberOfRows = getNumberOfRows();
            if (numberOfRows < 1)
            {
                throw new ArgumentException(
                    "Needs to contain at least one row but contained only " + numberOfRows);
            }

            return numberOfRows;
        }

        void AddCountsOfCurrentRowToSumOfCountsByColumn(int[] countsAtCurrentRow)
        {
            for (int col = 0; col < _numberOfColumns; col++)
            {
                SumOfCountsByColumn[col] += countsAtCurrentRow[col];
            }
        }

        int GetAndValidateNumberOfColumns(Func<int, int> getNumberOfColumnsAtRow)
        {
            var numberOfColumnsOfFirstRow = getNumberOfColumnsAtRow(0);
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

        int GetSumForCurrentRow(int[] countsAtCurrentRow)
        {
            int sumForCurrentRow = 0;
            for (int col = 0; col < _numberOfColumns; col++)
            {
                sumForCurrentRow += countsAtCurrentRow[col];
            }

            return sumForCurrentRow;
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

            SumOfCountsByColumn = new int[_numberOfColumns];
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

                    double roundedPercentage = Math.Round(percentage, MidpointRounding.AwayFromZero);
                    _setPercentageAtRowColumn(currentRow, col, (int)roundedPercentage);
                }
            }
        }

        #endregion
    }
}