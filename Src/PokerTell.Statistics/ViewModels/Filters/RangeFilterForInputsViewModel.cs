namespace PokerTell.Statistics.ViewModels.Filters
{
    using System;

    using PokerTell.Infrastructure.Interfaces.Statistics;

    using Tools.GenericRanges;

    public class RangeFilterForInputsViewModel<T> : RangeFilterViewModel<T>, IRangeFilterForInputsViewModel<T>
        where T : IComparable
    {
        #region Constants and Fields

        T _maxValue;

        T _minValue;

        #endregion

        #region Constructors and Destructors

        public RangeFilterForInputsViewModel(GenericRangeFilter<T> genericRangeFilter, string filterName)
            : base(genericRangeFilter, filterName)
        {
            MinValue = genericRangeFilter.Range.MinValue;
            MaxValue = genericRangeFilter.Range.MaxValue;
        }

        #endregion

        #region Properties

        public T MaxValue
        {
            get { return _maxValue; }
            set
            {
                _maxValue = value;
                AdjustToNewMaxValue();
                RaisePropertyChanged(() => MaxValue);
            }
        }

        public T MinValue
        {
            get { return _minValue; }
            set
            {
                _minValue = value;
                AdjustToNewMinValue();
                RaisePropertyChanged(() => MinValue);
            }
        }

        #endregion

        #region Methods

        protected void AdjustToNewMaxValue()
        {
            if (MinValue.CompareTo(MaxValue) > 0)
            {
                MinValue = MaxValue;
            }
        }

        protected void AdjustToNewMinValue()
        {
            if (MaxValue.CompareTo(MinValue) < 0)
            {
                MaxValue = MinValue;
            }
        }

        /// <summary>
        /// Returns a filter according to the values currently selected by the user
        /// </summary>
        public override GenericRangeFilter<T> CurrentFilter
        {
            get { return new GenericRangeFilter<T> { Range = new GenericRange<T>(MinValue, MaxValue), IsActive = IsActive }; }
        }

        #endregion
    }
}