namespace PokerTell.LiveTracker.IntegrationTests
{
    using DesignWindows;

    using Microsoft.Practices.Composite.Events;

    using NUnit.Framework;

    using UnitTests;

    public class DesignViewTests : TestWithLog
    {
       
        public void TableStatisticsViewTemplate()
        {
            var designWindow = new TableStatisticsDesignWindow(new EventAggregator()) { Topmost = true };
            designWindow.ShowDialog();
        }

        
    }
}