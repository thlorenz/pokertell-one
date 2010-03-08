namespace Tools.GenericRanges
{
    using System;

    public class GenericRangeFilter<T>
        where T : IComparable
    {
        #region Properties

        public bool IsActive { get; set; }

        public bool IsNotActive
        {
            get { return !IsActive; }
        }

        public GenericRange<T> Range { get; set; }

        #endregion

        #region Public Methods

        public GenericRangeFilter<T> ActivateWith(T min, T max)
        {
            IsActive = true;
            Range = new GenericRange<T>(min, max);

            return this;
        }

        public bool DoesNotFilterOut(T value)
        {
            if (IsNotActive)
            {
                return true;
            }

            if (Range == null)
            {
                throw new NullReferenceException("Need to initialize Range first.");
            }

            return Range.IncludesValue(value);
        }

        public bool Equals(GenericRangeFilter<T> other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return other.IsActive.Equals(IsActive) && Equals(other.Range, Range);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return obj.GetType() == GetType() && Equals((GenericRangeFilter<T>)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (IsActive.GetHashCode() * 397) ^ (Range != null ? Range.GetHashCode() : 0);
            }
        }

        public override string ToString()
        {
            return string.Format("{0}, Range: {1}", IsActive ? "Active" : "Inactive", Range);
        }

        #endregion
    }
}