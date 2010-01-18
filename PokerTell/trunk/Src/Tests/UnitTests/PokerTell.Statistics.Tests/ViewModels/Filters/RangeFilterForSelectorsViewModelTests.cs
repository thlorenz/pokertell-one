namespace PokerTell.Statistics.Tests.ViewModels.Filters
{
    using System.Linq;

    using NUnit.Framework;

    using Statistics.ViewModels.Filters;

    using Tools.GenericRanges;

    using UnitTests.Tools;

    public class RangeFilterForSelectorsViewModelTests
    {
        readonly int[] _availableValues = new[] { 0, 1, 2, 3 };

        RangeFilterForSelectorsViewModel<int> _sut;

        RangeFilterForSelectorsViewModel<int> CreateRangeFilterForSelectorsViewModelWith(int minValue, int maxValue)
        {
            var rangeFilter = new GenericRangeFilter<int>().ActivateWith(minValue, maxValue);
            return new RangeFilterForSelectorsViewModel<int>(rangeFilter, _availableValues, "someName");
        }

        [Test]
        public void Constructor_RangeIncludesAllAvailableValues_AddsValueModelForEachValueToAvailableMinAndMaxItems()
        {
            _sut = CreateRangeFilterForSelectorsViewModelWith(0, 3);
           
            _sut.AvailableMinItems.Select(item => item.Value).IsEqualTo(_availableValues);
            _sut.AvailableMaxItems.Select(item => item.Value).IsEqualTo(_availableValues);
        }

        [Test]
        public void Constructor_AvailableMinValuesContainFilterMinValue_SetsValueOfMinToThatMinValue()
        {
            _sut = CreateRangeFilterForSelectorsViewModelWith(1, 2);
          
            _sut.Min.Value.IsEqualTo(1);
        }

        [Test]
        public void Constructor_AvailableMaxValuesContainFilterMaxValue_SetsValueOfMaxToThatMaxValue()
        {
            _sut = CreateRangeFilterForSelectorsViewModelWith(1, 2);
            
            _sut.Max.Value.IsEqualTo(2);
        }

        [Test]
        public void Constructor_AvailableMinValuesDoesNotContainFilterMinValue_SetsValueOfMinToFirstAvailableMinValue()
        {
            _sut = CreateRangeFilterForSelectorsViewModelWith(-1, 2);
            
            _sut.Min.Value.IsEqualTo(_availableValues.First());
        }

        [Test]
        public void Constructor_AvailableMaxValuesDoesNotContainFilterMaxValue_SetsValueOfMaxToLastAvailableMaxValue()
        {
            _sut = CreateRangeFilterForSelectorsViewModelWith(0, 4);
           
            _sut.Max.Value.IsEqualTo(_availableValues.Last());
        }

        [Test]
        public void SetMinValueToTwo_FirstAvailableMaxValueWasZero_FirstAvailableMaxValueIsTwo()
        {
            _sut = CreateRangeFilterForSelectorsViewModelWith(0, 3);
           
            _sut.Min = _sut.AvailableMinItems.ElementAt(2);

            _sut.AvailableMaxItems.First().Value.IsEqualTo(2);
        }

        [Test]
        public void SetMinValueToOne_FirstAvailableMaxValueWasOne__FirstAvailableMaxValueIsOne()
        {
            _sut = CreateRangeFilterForSelectorsViewModelWith(0, 3);

            _sut.AvailableMaxItems.RemoveAt(0);
            _sut.Min = _sut.AvailableMinItems.ElementAt(1);

            _sut.AvailableMaxItems.First().Value.IsEqualTo(1);
        }

        [Test]
        public void SetMinValueToOne_FirstAvailableMaxValueWasOneAndMaxItemWasTwo_MaxItemValueIsTwo()
        {
            _sut = CreateRangeFilterForSelectorsViewModelWith(0, 3);

            _sut.Max = _sut.AvailableMaxItems.ElementAt(2);
            _sut.AvailableMaxItems.RemoveAt(0);
            _sut.Min = _sut.AvailableMinItems.ElementAt(1);

            _sut.Max.Value.IsEqualTo(2);
        }

        [Test]
        public void SetMaxValueToTwo_LastAvailableMinValueWasThree_LastAvailableMinValueIsTwo()
        {
            _sut = CreateRangeFilterForSelectorsViewModelWith(0, 3);

            _sut.Max = _sut.AvailableMaxItems.ElementAt(2);

            _sut.AvailableMinItems.Last().Value.IsEqualTo(2);
        }

        [Test]
        public void SetMaxValueToTwo_LastAvailableMinValueWasTwo__LastAvailableMinValueIsTwo()
        {
            _sut = CreateRangeFilterForSelectorsViewModelWith(0, 3);

            _sut.AvailableMinItems.RemoveAt(3);
            _sut.Max = _sut.AvailableMaxItems.ElementAt(2);

            _sut.AvailableMinItems.Last().Value.IsEqualTo(2);
        }

        [Test]
        public void SetMaxValueToOne_FirstAvailableMinValueWasOneAndMinItemWasTwo_MinItemValueIsTwo()
        {
            _sut = CreateRangeFilterForSelectorsViewModelWith(0, 3);

            _sut.Min = _sut.AvailableMinItems.ElementAt(2);
            _sut.AvailableMinItems.RemoveAt(3);
            _sut.Max = _sut.AvailableMaxItems.ElementAt(1);

            _sut.Min.Value.IsEqualTo(2);
        }

    }
}