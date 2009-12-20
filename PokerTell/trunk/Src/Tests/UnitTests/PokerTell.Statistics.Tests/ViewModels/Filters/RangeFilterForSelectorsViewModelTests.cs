namespace PokerTell.Statistics.Tests.ViewModels.Filters
{
    using NUnit.Framework;

    using Statistics.ViewModels.Filters;

    using Tools.GenericRanges;

    using UnitTests.Tools;

    public class RangeFilterForSelectorsViewModelTests : RangeFilterViewModelTests
    {
        readonly int[] _availableValues = new[] { 1, 2, 3 };

        RangeFilterForSelectorsViewModel<int> _sut;

        protected override RangeFilterViewModel<int> GetRangeFilterViewModel(Tools.GenericRanges.GenericRangeFilter<int> rangeFilter)
        {
            return new RangeFilterForSelectorsViewModel<int>(rangeFilter, _availableValues, "someName");
        }

        RangeFilterForSelectorsViewModel<int> CreateRangeFilterForSelectorsViewModelWith(int minValue, int maxValue)
        {
            var rangeFilter = new GenericRangeFilter<int>().ActivateWith(minValue, maxValue);
            return new RangeFilterForSelectorsViewModel<int>(rangeFilter, _availableValues, "someName");
        }

        [Test]
        public void Constructor_AvailableValuesContainValuesSmallerThanMinValue_AvailableMaxValuesOnlyContainThoseGreaterOrEqualToMinValue()
        {
            _sut = CreateRangeFilterForSelectorsViewModelWith(2, 3);
            _sut.AvailableMaxValues
                .DoesNotContain(1)
                .DoesContain(2)
                .DoesContain(3);
        }

        [Test]
        public void Constructor_AvailableValuesContainValuesGreaterThanMaxValue_AvailableMinValuesOnlyContainThoseSmallererOrEqualToMaxValue()
        {
            _sut = CreateRangeFilterForSelectorsViewModelWith(1, 2);
            _sut.AvailableMinValues
                .DoesContain(1)
                .DoesContain(2)
                .DoesNotContain(3);
        }

        [Test]
        public void SetMinValue_AvailableValuesContainValuesSmallerThanMinValue_AvailableMaxValuesOnlyContainThoseGreaterOrEqualToMinValue()
        {
            _sut = CreateRangeFilterForSelectorsViewModelWith(1, 3);
            _sut.MinValue = 2;
            _sut.AvailableMaxValues
               .DoesNotContain(1)
               .DoesContain(2)
               .DoesContain(3);
        }

        [Test]
        public void SetMaxValue_AvailableValuesContainValuesGreaterThanMaxValue_AvailablMinValuesOnlyContainThoseSmallerOrEqualToMaxValue()
        {
            _sut = CreateRangeFilterForSelectorsViewModelWith(1, 3);
            _sut.MaxValue = 2;
            _sut.AvailableMinValues
                .DoesContain(1)
                .DoesContain(2)
                .DoesNotContain(3);
        }
    }
}