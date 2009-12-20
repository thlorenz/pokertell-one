namespace PokerTell.LiveTracker.IntegrationTests
{
    using DesignWindows;

    using NUnit.Framework;

    using UnitTests;

    public class DesignViewTests : TestWithLog
    {
        [Test]
        public void PlayerStatisticsViewTemplate()
        {
            var designWindow = new TableStatisticsDesignWindow();
            designWindow.ShowDialog();
        }
    }
}