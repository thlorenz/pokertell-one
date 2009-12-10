namespace PokerTell.Statistics.Detailed
{
    using System;

    using PokerTell.Statistics.Interfaces;

    public class AcrossRowsPercentagesCalculator : IPercentagesCalculator
    {
        #region Constants and Fields

        Func<int, int, int> _getCountAtRowColumn;

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
            var numberOfRows = getNumberOfRows();
            if (numberOfRows < 2)
            {
                throw new ArgumentException(
                    "Needs to contain at least two rows but contained only " + numberOfRows);
            }

            return numberOfRows;
        }

        int GetAndValidateNumberOfColumns(Func<int, int> getNumberOfColumnsAtRow)
        {
            var numberOfColumnsOfFirstRow = getNumberOfColumnsAtRow(0);
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

            SumOfCountsByColumn = new int[_numberOfColumns];
        }

        void SetPercentagesForColumn(int col)
        {
            for (int row = 0; row < _numberOfRows; row++)
            {
                if (SumOfCountsByColumn[col] == 0)
                {
                    _setPercentageAtRowColumn(row, col, 0);
                }
                else
                {
                    double percentage = (double)_getCountAtRowColumn(row, col) / SumOfCountsByColumn[col] * 100;
                    double roundedPercentage = Math.Round(percentage, MidpointRounding.AwayFromZero);

                    _setPercentageAtRowColumn(row, col, (int)roundedPercentage);
                }
            }
        }

        void SumUpCountsOfAllRowsForColumn(int col)
        {
            for (int row = 0; row < _numberOfRows; row++)
            {
                SumOfCountsByColumn[col] += _getCountAtRowColumn(row, col);
            }
        }

        #endregion
    }
}