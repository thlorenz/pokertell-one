namespace PokerTell.Infrastructure.Interfaces.Statistics
{
    using System;

    using Tools.GenericRanges;

    public interface IRangeFilterViewModel<T> : IRangeFilterViewModel
        where T : IComparable
    {
        /// <summary>
        /// Returns a filter according to the values currently selected by the user
        /// </summary>
        GenericRangeFilter<T> CurrentFilter { get; }
    }

    public interface IRangeFilterViewModel : IFluentInterface
    {
        bool IsActive { get; set; }

        string FilterName { get; set; }
    }
}