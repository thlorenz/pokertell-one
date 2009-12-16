namespace PokerTell.StatisticsIntegrationTests
{
    using DesignWindows;

    using NUnit.Framework;

    using UnitTests;

    public class DesignViewTests : TestWithLog
    {
        public void StatisticsSetSummaryViewTemplate()
        {
            var designWindow = new StatisticsSetSummaryDesignWindow();
            designWindow.ShowDialog();
        }

        public void PostFlopStatisticsSetsViewTemplate()
        {
            var designWindow = new PostFlopStatisticsSetsDesignWindow();
            designWindow.ShowDialog();
        }

        public void PreFlopStatisticsSetsViewTemplate()
        {
            var designWindow = new PreFlopStatisticsSetsDesignWindow();
            designWindow.ShowDialog();
        }
    }
}