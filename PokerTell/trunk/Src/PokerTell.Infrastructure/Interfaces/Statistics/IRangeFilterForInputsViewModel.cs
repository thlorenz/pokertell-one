namespace PokerTell.Infrastructure.Interfaces.Statistics
{
    using System;

    public interface IRangeFilterForInputsViewModel<T> : IRangeFilterViewModel<T>
        where T : IComparable
    {
    }
}