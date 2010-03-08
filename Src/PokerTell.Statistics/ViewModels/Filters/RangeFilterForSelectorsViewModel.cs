namespace PokerTell.Statistics.ViewModels.Filters
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    using PokerTell.Infrastructure.Interfaces.Statistics;

    using Tools.FunctionalCSharp;
    using Tools.GenericRanges;

    public class RangeFilterForSelectorsViewModel<T> : RangeFilterViewModel<T>, IRangeFilterForSelectorsViewModel<T>
        where T : IComparable
    {
        #region Constants and Fields

        IValueViewModel<T> _max;

        IValueViewModel<T> _min;

        /* To temporarily disable the updates while populating ListItems, otherwise recursion occurs due to the 
           Fact that when the ItemsCollection changes, the selected item is updated as well */ 
        bool _updateMaxAndMinEnabled = true;

        #endregion

        #region Constructors and Destructors

        public RangeFilterForSelectorsViewModel(
            GenericRangeFilter<T> genericRangeFilter, IEnumerable<T> availableValues, string filterName)
            : this(genericRangeFilter, availableValues, filterName, val => val.ToString())
        {
        }

        public RangeFilterForSelectorsViewModel(
            GenericRangeFilter<T> genericRangeFilter, 
            IEnumerable<T> availableValues, 
            string filterName, 
            Func<T, string> convertValueToDisplay) : base(genericRangeFilter, filterName)
        {
            CreateAndPopulateAllAvailableItemsFrom(availableValues, convertValueToDisplay);

            CreateAndPopulateAvailableMinAndMaxItemsFromAllAvailableItems();

            DetermineMinAndMaxValuesFrom(genericRangeFilter);

            AddLegalAndRemoveIllegalMinItems();
            AddLegalAndRemoveIllegalMaxItems();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Values that the user can choose to use as MaxValue
        /// </summary>
        public IList<IValueViewModel<T>> AvailableMaxItems { get; private set; }

        /// <summary>
        /// Values that the user can choose to use as MinValue
        /// </summary>
        public IList<IValueViewModel<T>> AvailableMinItems { get; private set; }

        IList<IValueViewModel<T>> _allAvailableItems;

        public IValueViewModel<T> Max
        {
            get { return _max; }
            set
            {
                if (_updateMaxAndMinEnabled)
                {
                    _max = value;

                    AddLegalAndRemoveIllegalMinItems();
                    AdjustMinToAvailableMinValues();

                    RaisePropertyChanged(() => Max);
                }
            }
        }

        public IValueViewModel<T> Min
        {
            get { return _min; }
            set
            {
                if (_updateMaxAndMinEnabled)
                {
                    _min = value;
                    
                    AddLegalAndRemoveIllegalMaxItems();
                    AdjustMaxToAvailableMaxItems();

                    RaisePropertyChanged(() => Min);
                }
            }
        }

        #endregion

        #region Methods

        void CreateAndPopulateAllAvailableItemsFrom(IEnumerable<T> availableValues, Func<T, string> convertValueToDisplay)
        {
            _allAvailableItems = new List<IValueViewModel<T>>();
            availableValues.ForEach(value => _allAvailableItems.Add(new ValueViewModel<T>(value, convertValueToDisplay)));
        }

        void CreateAndPopulateAvailableMinAndMaxItemsFromAllAvailableItems()
        {
            AvailableMaxItems = new ObservableCollection<IValueViewModel<T>>();
            AvailableMinItems = new ObservableCollection<IValueViewModel<T>>();
           
            _allAvailableItems.ForEach(item => {
                AvailableMinItems.Add(item);
                AvailableMaxItems.Add(item);
            });
        }

        void DetermineMinAndMaxValuesFrom(GenericRangeFilter<T> genericRangeFilter)
        {
            _min = AvailableMinItems.FirstOrDefault(item => item.Value.Equals(genericRangeFilter.Range.MinValue)) ??
                  AvailableMinItems.First();

            _max = AvailableMaxItems.FirstOrDefault(item => item.Value.Equals(genericRangeFilter.Range.MaxValue)) ??
                  AvailableMaxItems.Last();
        }

        void AdjustMinToAvailableMinValues()
        {
            // Check if current min is contained in available min values -> if not select largest available
            _min = AvailableMinItems.FirstOrDefault(item => item.Value.Equals(Min.Value)) ??
                  AvailableMinItems.Last();
            RaisePropertyChanged(() => Min);
        }

        void AdjustMaxToAvailableMaxItems()
        {
            // Check if current max is contained in available max values -> if not select smallest available
            _max = AvailableMaxItems.FirstOrDefault(item => item.Value.Equals(Max.Value)) ??
                  AvailableMaxItems.First();
            RaisePropertyChanged(() => Max);
        }

        void AddLegalAndRemoveIllegalMaxItems()
        {
            _updateMaxAndMinEnabled = false;
         
            AvailableMaxItems.Clear();
            _allAvailableItems.ForEach(item => {
                if (item.CompareTo(Min) >= 0)
                {
                    AvailableMaxItems.Add(item);
                }
            });

            _updateMaxAndMinEnabled = true;
        }

        void AddLegalAndRemoveIllegalMinItems()
        {
            _updateMaxAndMinEnabled = false;
          
            AvailableMinItems.Clear();
            _allAvailableItems.ForEach(item =>
            {
                if (item.CompareTo(Max) <= 0)
                {
                    AvailableMinItems.Add(item);
                }
            });

            _updateMaxAndMinEnabled = true;
        }

        #endregion

        /// <summary>
        /// Returns a filter according to the values currently selected by the user
        /// </summary>
        public override GenericRangeFilter<T> CurrentFilter
        {
            get { return new GenericRangeFilter<T> { Range = new GenericRange<T>(Min.Value, Max.Value), IsActive = IsActive }; }
        }
    }
}