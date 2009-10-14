//Date: 5/7/2009

using System;

namespace Tools.GenericRanges
{
    public interface IGenericRange<T> : IComparable where T : IComparable
    {
        bool IncludesValue(IComparable<T> value);
        int GetHashCode();
        bool Equals(object obj);
        string ToString();
        T MinValue {get; }
			
        T MaxValue {get; }
    }
}