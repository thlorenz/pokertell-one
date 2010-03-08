namespace PokerTell.Statistics.ViewModels.Filters
{
    using System;

    using Infrastructure.Interfaces.Statistics;

    using Tools.GenericRanges;
    using Tools.WPF.ViewModels;

    public abstract class RangeFilterViewModel<T> : NotifyPropertyChanged, IRangeFilterViewModel<T>
        where T : IComparable
    {
        bool _isActive;

        public string FilterName { get; set; }

        public RangeFilterViewModel(GenericRangeFilter<T> genericRangeFilter, string filterName)
        {
            IsActive = genericRangeFilter.IsActive;
            
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

        /// <summary>
        /// Returns a filter according to the values currently selected by the user
        /// </summary>
        public abstract GenericRangeFilter<T> CurrentFilter { get; }
    }
}