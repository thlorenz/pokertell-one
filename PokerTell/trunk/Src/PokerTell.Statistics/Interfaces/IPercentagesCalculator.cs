namespace PokerTell.Statistics.Interfaces
{
    using System;

    public interface IPercentagesCalculator
    {
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
        ///   If rows have different lenghts or less than - one (if separate rows), two (if across rows) - rows and/or less than one column was found
        /// </exception>
        IPercentagesCalculator CalculatePercentages(
            Func<int> getNumberOfRows,
            Func<int, int> getNumberOfColumnsAtRow,
            Func<int, int, int> getCountAtRowColumn,
            Action<int, int, int> setPercentageAtRowColumn);

        int[] SumsOfCountsByColumn { get; }
    }
}