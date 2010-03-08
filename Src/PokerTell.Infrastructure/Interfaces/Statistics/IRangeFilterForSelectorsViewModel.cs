namespace PokerTell.Infrastructure.Interfaces.Statistics
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    /// <summary>
    /// Used to allow user to select from a range of available Max and Min values to specify a RangeFilter
    /// using a View binding to it.
    /// </summary>
    /// <typeparam name="T">Type of values making up the min and max of the filter</typeparam>
    public interface IRangeFilterForSelectorsViewModel<T> : IRangeFilterViewModel<T>
        where T : IComparable
    {
        /// <summary>
        /// Values that the user can choose to use as MinValue
        /// </summary>
        IList<IValueViewModel<T>> AvailableMinItems { get; }

        /// <summary>
        /// Values that the user can choose to use as MaxValue
        /// </summary>
        IList<IValueViewModel<T>> AvailableMaxItems { get; }
    }
}