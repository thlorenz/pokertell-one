namespace PokerTell.Statistics.ViewModels.Filters
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    using PokerTell.Infrastructure.Interfaces.Statistics;

    using Tools.GenericRanges;

    public class RangeFilterForSelectorsViewModel<T> : RangeFilterViewModel<T>, IRangeFilterForSelectorsViewModel<T>
        where T : IComparable
    {
        #region Constants and Fields

        readonly ObservableCollection<T> _availableMaxValues = new ObservableCollection<T>();

        readonly ObservableCollection<T> _availableMinValues = new ObservableCollection<T>();

        readonly IEnumerable<T> _availableValues;

        #endregion

        #region Constructors and Destructors

        public RangeFilterForSelectorsViewModel(
            GenericRangeFilter<T> genericRangeFilter, IEnumerable<T> availableValues, string filterName)
            : base(genericRangeFilter, filterName)
        {
            _availableValues = availableValues;
          
            SelectAvailableMinValuesFromAvailableValues();

            SelectAvailableMaxValuesFromAvailableValues();
        }

        void SelectAvailableMinValuesFromAvailableValues()
        {
            if (_availableValues != null)
            {
                _availableMinValues.Clear();
                _availableValues
                    .Where(value => value.CompareTo(MaxValue) <= 0)
                    .ToList()
                    .ForEach(value => _availableMinValues.Add(value));
            }
        }

        void SelectAvailableMaxValuesFromAvailableValues()
        {
            if (_availableValues != null)
            {
                _availableMaxValues.Clear();
                _availableValues
                    .Where(value => value.CompareTo(MinValue) >= 0)
                    .ToList()
                    .ForEach(value => _availableMaxValues.Add(value));
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Values that the user can choose to use as MaxValue
        /// </summary>
        public ObservableCollection<T> AvailableMaxValues
        {
            get { return _availableMaxValues; }
        }

        /// <summary>
        /// Values that the user can choose to use as MinValue
        /// </summary>
        public ObservableCollection<T> AvailableMinValues
        {
            get { return _availableMinValues; }
        }

        #endregion

        #region Methods

        protected override void AdjustToNewMaxValue()
        {
            base.AdjustToNewMaxValue();
            SelectAvailableMinValuesFromAvailableValues();
            
            // When ItemsSource of Listbox is updated, the selected item becomes empty, so we need to cause an update
            RaisePropertyChanged(() => MinValue);
        }

        protected override void AdjustToNewMinValue()
        {
            base.AdjustToNewMinValue();
            SelectAvailableMaxValuesFromAvailableValues();

            // When ItemsSource of Listbox is updated, the selected item becomes empty, so we need to cause an update
            RaisePropertyChanged(() => MaxValue);
        }

        #endregion
    }
}