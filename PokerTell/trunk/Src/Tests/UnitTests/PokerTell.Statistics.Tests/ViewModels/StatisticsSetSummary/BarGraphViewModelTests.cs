namespace PokerTell.Statistics.Tests.ViewModels.StatisticsSetSummary
{
    using System.Windows.Media;

    using NUnit.Framework;

    using Statistics.ViewModels.StatisticsSetSummary;

    using UnitTests.Tools;

    public class BarGraphViewModelTests
    {
        [Test]
        public void Constructor_Always_InitializesBars()
        {
            var sut = new BarGraphViewModel();
            sut.Bars.ShouldNotBeNull();
        }

        [Test]
        public void UpdateWith_PercentagesContainOnlyOneElement_DoesNotAddAnyBars()
        {
            var percentages = new[] { 1 };

            var sut = new BarGraphViewModel()
                .UpdateWith(percentages);

            sut.Bars.ShouldHaveCount(0);
        }

        [Test]
        public void UpdateWith_PercentagesContainOnlyOneElementVisibleIsTrue_VisibleBecomesFalse()
        {
            var percentages = new[] { 1 };

            var sut = new BarGraphViewModel()
                .UpdateWith(new[] { 1, 2 });

            sut.Visible.ShouldBeTrue();

            sut.UpdateWith(percentages);

            sut.Visible.ShouldBeFalse();
        }

        [Test]
        public void UpdateWith_PercentagesContainTwoElements_VisibleIsTrue()
        {
            var percentages = new[] { 1, 2 };

            var sut = new BarGraphViewModel()
                .UpdateWith(percentages);

            sut.Visible.ShouldBeTrue();
        }

        [Test]
        public void UpdateWith_PercentagesFirstTime_AddsABarForEachPercentage()
        {
            var percentages = new[] { 1, 2 };
           
            var sut = new BarGraphViewModel()
                .UpdateWith(percentages);

            sut.Bars.ShouldHaveCount(percentages.Length);
        }

        [Test]
        public void UpdateWith_PercentagesSecondTime_ClearsBarsBeforeAddingBarForEachPercentage()
        {
            var percentages = new[] { 1, 2 };

            var sut = new BarGraphViewModel()
                .UpdateWith(percentages)
                .UpdateWith(percentages);

            sut.Bars.Count.ShouldBeEqualTo(percentages.Length);
        }

        [Test]
        public void UpdateWith_Percentages_BarsPercentageEqualPercentages()
        {
            var percentages = new[] { 1, 2 };

            var sut = new BarGraphViewModel()
                .UpdateWith(percentages);

            sut.Bars[0].Percentage.ShouldBeEqualTo(percentages[0]);
            sut.Bars[1].Percentage.ShouldBeEqualTo(percentages[1]);
        }

        [Test]
        public void UpdateWith_Always_AssignsColorsToBarsStroke()
        {
            var percentages = new[] { 1, 2 };
            
            var sut = new BarGraphViewModel()
                .UpdateWith(percentages);

            sut.Bars[0].Stroke.Color.ShouldBeEqualTo(sut.BarColors[0]);
            sut.Bars[1].Stroke.Color.ShouldBeEqualTo(sut.BarColors[1]);
        }
    }
}