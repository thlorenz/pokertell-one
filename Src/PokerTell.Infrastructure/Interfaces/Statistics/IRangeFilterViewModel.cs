namespace PokerTell.Infrastructure.Interfaces.Statistics
{
    using System;

    using Tools.GenericRanges;

    public interface IRangeFilterViewModel<T>
        where T : IComparable
    {
        bool IsActive { get; }

        string FilterName { get; set; }

        /// <summary>
        /// Returns a filter according to the values currently selected by the user
        /// </summary>
        GenericRangeFilter<T> CurrentFilter { get; }
    }
}