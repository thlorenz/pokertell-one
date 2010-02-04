namespace PokerTell.LiveTracker.IntegrationTests
{
    using DesignWindows;

    using Infrastructure.Interfaces.PokerHand;

    using Microsoft.Practices.Composite.Events;

    using Moq;

    using NUnit.Framework;

    using Statistics.Interfaces;

    using UnitTests;

    public class DesignViewTests : TestWithLog
    {
       static readonly IHandBrowserViewModel HandBrowserViewModelStub = new Mock<IHandBrowserViewModel>().Object;

      static readonly IRaiseReactionDescriber RaiseReactionDescriber = new Mock<IRaiseReactionDescriber>().Object;

      static readonly IRaiseReactionStatisticsBuilder RaiseReactionStatisticsBuilder =
         new Mock<IRaiseReactionStatisticsBuilder>().Object;


        public void TableStatisticsViewTemplate()
        {
            var designWindow = new TableStatisticsDesignWindow(new EventAggregator(),HandBrowserViewModelStub, RaiseReactionStatisticsBuilder, RaiseReactionDescriber) { Topmost = true };
            designWindow.ShowDialog();
        }

        
    }
}