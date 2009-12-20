namespace PokerTell.Statistics.ViewModels.Filters
{
    using System;

    using Infrastructure.Interfaces.Statistics;

    using Tools.GenericRanges;
    using Tools.WPF.ViewModels;

    public class RangeFilterViewModel<T> : NotifyPropertyChanged, IRangeFilterViewModel<T>
        where T : IComparable
    {
        bool _isActive;

        T _minValue;

        T _maxValue;

        public string FilterName { get; set; }

        public RangeFilterViewModel(GenericRangeFilter<T> genericRangeFilter, string filterName)
        {
            IsActive = genericRangeFilter.IsActive;
            MinValue = genericRangeFilter.Range.MinValue;
            MaxValue = genericRangeFilter.Range.MaxValue;
            FilterName = filterName;
        }

        public bool IsActive
        {
            get { return _isActive; }
            set
            {
                _isActive = value;
                RaisePropertyChanged(() => IsActive);
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

        protected virtual void AdjustToNewMinValue()
        {
            if (MaxValue.CompareTo(MinValue) < 0)
            {
                MaxValue = MinValue;
            }
        }

        protected virtual void AdjustToNewMaxValue()
        {
            if (MinValue.CompareTo(MaxValue) > 0)
            {
                MinValue = MaxValue;
            }
        }
       
    }
}