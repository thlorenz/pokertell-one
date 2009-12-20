namespace PokerTell.Infrastructure.Interfaces.Statistics
{
    using System;

    public interface IRangeFilterViewModel<T>
        where T : IComparable
    {
        bool IsActive { get; }

        T MinValue { get; }

        T MaxValue { get; }
    }
}