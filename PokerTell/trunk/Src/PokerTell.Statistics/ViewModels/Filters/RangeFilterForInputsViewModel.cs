namespace PokerTell.Statistics.ViewModels.Filters
{
    using System;

    using Infrastructure.Interfaces.Statistics;

    using Tools.GenericRanges;

    public class RangeFilterForInputsViewModel<T> : RangeFilterViewModel<T>, IRangeFilterForInputsViewModel<T>
        where T : IComparable
    {
        public RangeFilterForInputsViewModel(GenericRangeFilter<T> genericRangeFilter, string filterName)
            : base(genericRangeFilter, filterName)
        {
        }
    }
}