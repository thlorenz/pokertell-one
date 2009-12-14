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
            sut.Bars.IsNotNull();
        }

        [Test]
        public void UpdateWith_PercentagesContainOnlyOneElement_DoesNotAddAnyBars()
        {
            var percentages = new[] { 1 };

            var sut = new BarGraphViewModel()
                .UpdateWith(percentages);

            sut.Bars.HasCount(0);
        }

        [Test]
        public void UpdateWith_PercentagesContainOnlyOneElementVisibleIsTrue_VisibleBecomesFalse()
        {
            var percentages = new[] { 1 };

            var sut = new BarGraphViewModel()
                .UpdateWith(new[] { 1, 2 });

            sut.Visible.IsTrue();

            sut.UpdateWith(percentages);

            sut.Visible.IsFalse();
        }

        [Test]
        public void UpdateWith_PercentagesContainTwoElements_VisibleIsTrue()
        {
            var percentages = new[] { 1, 2 };

            var sut = new BarGraphViewModel()
                .UpdateWith(percentages);

            sut.Visible.IsTrue();
        }

        [Test]
        public void UpdateWith_PercentagesFirstTime_AddsABarForEachPercentage()
        {
            var percentages = new[] { 1, 2 };
           
            var sut = new BarGraphViewModel()
                .UpdateWith(percentages);

            sut.Bars.HasCount(percentages.Length);
        }

        [Test]
        public void UpdateWith_PercentagesSecondTime_ClearsBarsBeforeAddingBarForEachPercentage()
        {
            var percentages = new[] { 1, 2 };

            var sut = new BarGraphViewModel()
                .UpdateWith(percentages)
                .UpdateWith(percentages);

            sut.Bars.Count.IsEqualTo(percentages.Length);
        }

        [Test]
        public void UpdateWith_Percentages_BarsPercentageEqualPercentages()
        {
            var percentages = new[] { 1, 2 };

            var sut = new BarGraphViewModel()
                .UpdateWith(percentages);

            sut.Bars[0].Percentage.IsEqualTo(percentages[0]);
            sut.Bars[1].Percentage.IsEqualTo(percentages[1]);
        }

        [Test]
        public void UpdateWith_Always_AssignsColorsToBarsStroke()
        {
            var percentages = new[] { 1, 2 };
            
            var sut = new BarGraphViewModel()
                .UpdateWith(percentages);

            sut.Bars[0].Stroke.Color.IsEqualTo(sut.BarColors[0]);
            sut.Bars[1].Stroke.Color.IsEqualTo(sut.BarColors[1]);
        }
    }
}