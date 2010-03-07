namespace PokerTell.LiveTracker.DesignWithDatabase
{
    using DesignWindows;

    using Infrastructure.Interfaces.PokerHand;

    using Microsoft.Practices.Composite.Events;

    using Moq;

    using Statistics.Interfaces;

    public class DesignViewTests 
    {
        static readonly IRepositoryHandBrowserViewModel HandBrowserViewModelStub = new Mock<IRepositoryHandBrowserViewModel>().Object;

        static readonly IPostFlopHeroActsRaiseReactionDescriber RaiseReactionDescriber = new Mock<IPostFlopHeroActsRaiseReactionDescriber>().Object;

        static readonly IRaiseReactionStatisticsBuilder RaiseReactionStatisticsBuilder =
            new Mock<IRaiseReactionStatisticsBuilder>().Object;


        public void TableStatisticsViewTemplate()
        {
            var designWindow = new TableStatisticsDesignWindow(new EventAggregator(), HandBrowserViewModelStub) { Topmost = true };
            designWindow.ShowDialog();
        }

        
    }
}