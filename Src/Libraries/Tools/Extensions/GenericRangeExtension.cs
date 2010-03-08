namespace Tools.Extensions
{
    using System;

    using GenericRanges;

    public static class GenericRangeExtension
    {
        public static bool IsIn<T>(this T value, GenericRange<T> range) where T : IComparable
        {
            return range.IncludesValue(value);
        }
    }
}