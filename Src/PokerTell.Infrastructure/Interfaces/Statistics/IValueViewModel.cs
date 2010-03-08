namespace PokerTell.Infrastructure.Interfaces.Statistics
{
    using System;

    public interface IValueViewModel<T> : IComparable<IValueViewModel<T>>
        where T : IComparable
    {
        T Value { get; set; }

        bool Visible { get; set; }
    }
}