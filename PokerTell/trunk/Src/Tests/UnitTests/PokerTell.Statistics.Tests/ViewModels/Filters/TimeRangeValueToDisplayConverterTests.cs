namespace PokerTell.Statistics.Tests.ViewModels.Filters
{
    using NUnit.Framework;

    using Statistics.ViewModels.Filters;

    using UnitTests.Tools;

    public class TimeRangeValueToDisplayConverterTests
    {
        TimeRangeValueToDisplayConverter _sut;

        [SetUp]
        public void _Init()
        {
            _sut = new TimeRangeValueToDisplayConverter();
        }
        
        [Test]
        public void Convert_0_ReturnsNow()
        {
            const int parameter = 0;

            var display = _sut.Convert(parameter);

            display.IsEqualTo("Now");
        }

        [Test]
        public void Convert_Negative20_Returns20MinutesAgo()
        {
            const int parameter = -20;

            var display = _sut.Convert(parameter);

            display.IsEqualTo("20 minutes ago");
        }

        [Test]
        public void Convert_Negative120_Returns2HoursAgo()
        {
            const int parameter = -120;

            var display = _sut.Convert(parameter);

            display.IsEqualTo("2 hours ago");
        }
    }
}