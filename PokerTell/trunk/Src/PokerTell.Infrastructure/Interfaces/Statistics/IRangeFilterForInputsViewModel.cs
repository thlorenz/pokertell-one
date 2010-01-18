namespace PokerTell.Infrastructure.Interfaces.Statistics
{
    using System;

    public interface IRangeFilterForInputsViewModel<T> : IRangeFilterViewModel<T>
        where T : IComparable
    {
        T MaxValue { get; set; }

        T MinValue { get; set; }
    }
}