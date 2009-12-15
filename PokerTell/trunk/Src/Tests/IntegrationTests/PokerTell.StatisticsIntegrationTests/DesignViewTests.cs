namespace PokerTell.StatisticsIntegrationTests
{
    using DesignWindows;

    using NUnit.Framework;

    using UnitTests;

    public class DesignViewTests : TestWithLog
    {
        [Test]
        public void StatisticsSetSummaryViewTemplate()
        {
            var designWindow = new StatisticsSetSummaryDesignWindow();
            designWindow.ShowDialog();
        }

        [Test]
        public void PostFlopStatisticsSetsViewTemplate()
        {
            var designWindow = new PostFlopStatisticsSetsDesignWindow();
            designWindow.ShowDialog();
        }
    }
}