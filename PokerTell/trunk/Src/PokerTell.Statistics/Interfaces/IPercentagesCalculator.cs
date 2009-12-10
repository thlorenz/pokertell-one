namespace PokerTell.Statistics.Interfaces
{
    using System;

    public interface IPercentagesCalculator
    {
        IPercentagesCalculator CalculatePercentages(
            Func<int> getNumberOfRows,
            Func<int, int> getNumberOfColumnsAtRow,
            Func<int, int, int> getCountAtRowColumn,
            Action<int, int, int> setPercentageAtRowColumn);

        int[] SumOfCountsByColumn { get; }
    }
}