namespace PokerTell.Statistics.Tests.ViewModels.Filters
{
    using System;

    using Moq;

    using NUnit.Framework;

    using Statistics.ViewModels.Filters;

    using Tools.GenericRanges;

    using UnitTests.Tools;

    public class RangeFilterViewModelTests
    {
        protected const int MinValue = 1;

        protected const int MaxValue = 3;

        RangeFilterViewModel<int> _sut;

        [SetUp]
        public void _Init()
        {
            var rangeFilter = new GenericRangeFilter<int>().ActivateWith(MinValue, MaxValue);
            _sut = GetRangeFilterViewModel(rangeFilter);
        }

        [Test]
        public void Constructor_WithActiveGnericRangeFilter_SetsIsActiveFromRangeFilterValues()
        {
            _sut.IsActive.IsTrue();
        }

        [Test]
        public void Constructor_WithActiveGnericRangeFilter_SetsMinValueFromRangeFilterValues()
        {
            _sut.MinValue.IsEqualTo(MinValue);
        }

        [Test]
        public void Constructor_WithActiveGnericRangeFilter_SetsMaxValueFromRangeFilterValues()
        {
            _sut.MaxValue.IsEqualTo(MaxValue);
        }

        [Test]
        public void SetMinValue_SmallerThanMaxValue_LeavesMaxValueUnchanged()
        {
            _sut.MinValue = MaxValue - 1;
            _sut.MaxValue.IsEqualTo(MaxValue);
        }

        [Test]
        public void SetMinValue_ToMaxValue_LeavesMaxValueUnchanged()
        {
            _sut.MinValue = MaxValue;
            _sut.MaxValue.IsEqualTo(MaxValue);
        }

        [Test]
        public void SetMinValue_GreaterThanMaxValue_SetsMaxValueToMinValue()
        {
            _sut.MinValue = MaxValue + 1;
            _sut.MaxValue.IsEqualTo(_sut.MinValue);
        }

        [Test]
        public void SetMaxValue_GreaterThanMinValue_LeavesMinValueUnchanged()
        {
            _sut.MaxValue = MinValue + 1;
            _sut.MinValue.IsEqualTo(MinValue);
        }

        [Test]
        public void SetMaxValue_ToMinValue_LeavesMinValueUnchanged()
        {
            _sut.MaxValue = MinValue;
            _sut.MinValue.IsEqualTo(MinValue);
        }

        [Test]
        public void SetMaxValue_SmallerThanMinValue_SetsMinValueToMaxValue()
        {
            _sut.MaxValue = MinValue - 1;
            _sut.MinValue.IsEqualTo(_sut.MaxValue);
        }

        protected virtual RangeFilterViewModel<int> GetRangeFilterViewModel(GenericRangeFilter<int> rangeFilter)
        {
            return new RangeFilterViewModel<int>(rangeFilter, "someName");
        }
    }
}