//Date: 5/7/2009

namespace Tools.GenericRanges
{
    using System;

    public interface IGenericRange<T> : IComparable
        where T : IComparable
    {
        #region Properties

        T MaxValue { get; }

        T MinValue { get; }

        #endregion

        #region Public Methods

        bool Equals(object obj);

        int GetHashCode();

        bool IncludesValue(IComparable value);

        string ToString();

        #endregion
    }
}