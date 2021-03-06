namespace PokerTell.Statistics.Tests.ViewModels.Filters
{
    using System;

    using Moq;

    using NUnit.Framework;

    using Statistics.ViewModels.Filters;

    using Tools.GenericRanges;

    using UnitTests.Tools;

    public class RangeFilterForInputsViewModelTests
    {
        protected const int MinValue = 1;

        protected const int MaxValue = 3;

        RangeFilterForInputsViewModel<int> _sut;

        [SetUp]
        public void _Init()
        {
            var rangeFilter = new GenericRangeFilter<int>().ActivateWith(MinValue, MaxValue);
            _sut = new RangeFilterForInputsViewModel<int>(rangeFilter, "someName");
        }

        [Test]
        public void Constructor_WithActiveGnericRangeFilter_SetsIsActiveFromRangeFilterValues()
        {
            _sut.IsActive.ShouldBeTrue();
        }

        [Test]
        public void Constructor_WithActiveGnericRangeFilter_SetsMinValueFromRangeFilterValues()
        {
            _sut.MinValue.ShouldBeEqualTo(MinValue);
        }

        [Test]
        public void Constructor_WithActiveGnericRangeFilter_SetsMaxValueFromRangeFilterValues()
        {
            _sut.MaxValue.ShouldBeEqualTo(MaxValue);
        }

        [Test]
        public void SetMinValue_SmallerThanMaxValue_LeavesMaxValueUnchanged()
        {
            _sut.MinValue = MaxValue - 1;
            _sut.MaxValue.ShouldBeEqualTo(MaxValue);
        }

        [Test]
        public void SetMinValue_ToMaxValue_LeavesMaxValueUnchanged()
        {
            _sut.MinValue = MaxValue;
            _sut.MaxValue.ShouldBeEqualTo(MaxValue);
        }

        [Test]
        public void SetMinValue_GreaterThanMaxValue_SetsMaxValueToMinValue()
        {
            _sut.MinValue = MaxValue + 1;
            _sut.MaxValue.ShouldBeEqualTo(_sut.MinValue);
        }

        [Test]
        public void SetMaxValue_GreaterThanMinValue_LeavesMinValueUnchanged()
        {
            _sut.MaxValue = MinValue + 1;
            _sut.MinValue.ShouldBeEqualTo(MinValue);
        }

        [Test]
        public void SetMaxValue_ToMinValue_LeavesMinValueUnchanged()
        {
            _sut.MaxValue = MinValue;
            _sut.MinValue.ShouldBeEqualTo(MinValue);
        }

        [Test]
        public void SetMaxValue_SmallerThanMinValue_SetsMinValueToMaxValue()
        {
            _sut.MaxValue = MinValue - 1;
            _sut.MinValue.ShouldBeEqualTo(_sut.MaxValue);
        }
    }
}