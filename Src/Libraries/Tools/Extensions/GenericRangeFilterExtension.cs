namespace Tools.Extensions
{
    using System;

    using Tools.GenericRanges;

    public static class GenericRangeFilterExtension
    {
        #region Public Methods

        public static bool PassesThrough<T>(this T value, GenericRangeFilter<T> filter) where T : IComparable
        {
            return filter.DoesNotFilterOut(value);
        }

        #endregion
    }
}