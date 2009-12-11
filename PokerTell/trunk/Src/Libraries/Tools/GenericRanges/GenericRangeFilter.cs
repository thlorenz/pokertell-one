namespace Tools.GenericRanges
{
    using System;

    public class GenericRangeFilter<T>
        where T : IComparable
    {
        public bool IsActive { get; set; }

        public bool IsNotActive
        {
            get { return !IsActive; }
        }

        public GenericRange<T> Range { get; set; }

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

        public GenericRangeFilter<T> ActivateWith(T min, T max)
        {
            IsActive = true;
            Range = new GenericRange<T>(min, max);
            return this;
        }
    }
}