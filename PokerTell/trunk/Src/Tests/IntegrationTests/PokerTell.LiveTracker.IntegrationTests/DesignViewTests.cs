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

      static readonly IPostFlopHeroActsRaiseReactionDescriber RaiseReactionDescriber = new Mock<IPostFlopHeroActsRaiseReactionDescriber>().Object;

      static readonly IRaiseReactionStatisticsBuilder RaiseReactionStatisticsBuilder =
         new Mock<IRaiseReactionStatisticsBuilder>().Object;


        public void TableStatisticsViewTemplate()
        {
            var designWindow = new TableStatisticsDesignWindow(new EventAggregator(),HandBrowserViewModelStub) { Topmost = true };
            designWindow.ShowDialog();
        }

        
    }
}