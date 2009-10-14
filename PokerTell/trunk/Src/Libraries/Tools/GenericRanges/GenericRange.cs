//Date: 5/7/2009

using System;

namespace Tools.GenericRanges
{
    public class GenericRange<T> : IComparable, IGenericRange<T> where T : struct, IComparable
    {
        public GenericRange(T minValue, T maxValue)
        {
            MinValue = minValue;
            MaxValue = maxValue;
        }

        #region IComparable Members

        int IComparable.CompareTo(object obj)
        {
            return MinValue.CompareTo(((GenericRange<T>) obj).MinValue);
        }

        #endregion

        #region IGenericRange<T> Members

        public T MinValue { get; protected set; }
        public T MaxValue { get; protected set; }

        public bool IncludesValue(IComparable<T> value)
        {
            bool isGreaterOrEqualToMin = value.CompareTo(MinValue) >= 0;
            bool isSmallerOrEqualToMax = value.CompareTo(MaxValue) <= 0;

            return isGreaterOrEqualToMin && isSmallerOrEqualToMax;
        }

        public override int GetHashCode()
        {
            return MinValue.GetHashCode() ^ MaxValue.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            return GetHashCode().Equals(obj.GetHashCode());
        }

        public override string ToString()
        {
            return string.Format("MinValue={0}, MaxValue={1}", MinValue, MaxValue);
        }

        #endregion
    }
}