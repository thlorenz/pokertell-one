namespace PokerTell.Statistics.IntegrationTests.DesignViewModels
{
    using System.Collections.Generic;

    using Infrastructure.Interfaces.PokerHand;
    using Infrastructure.Interfaces.Statistics;

    using Interfaces;

    using Moq;

    using PokerTell.Statistics.ViewModels.StatisticsSetDetails;

    using System.Linq;

    using ViewModels.Base;

    public class DetailedPostFlopHeroActsStatisticsDesignModel : DetailedPostFlopHeroActsStatisticsViewModel
    {

        public DetailedPostFlopHeroActsStatisticsDesignModel(
           IHandBrowserViewModel handBrowserViewModel,
            IRaiseReactionStatisticsBuilder raiseReactionStatisticsBuilder,
            IPostFlopHeroActsRaiseReactionDescriber raiseReactionDescriber)
            : base(handBrowserViewModel, new StubBuilder().Out<IPostFlopHeroActsRaiseReactionStatisticsViewModel>())
        {
            Rows = new List<IStatisticsTableRowViewModel>
                {
                    new StatisticsTableRowViewModel("Bet", new[] { 20, 30, 12, 40, 30, 12 }, "%"),
                    new StatisticsTableRowViewModel("Count", new[] { 1, 4, 30, 34, 33, 30, 12 }, string.Empty)
                };
        }
       
    }
}