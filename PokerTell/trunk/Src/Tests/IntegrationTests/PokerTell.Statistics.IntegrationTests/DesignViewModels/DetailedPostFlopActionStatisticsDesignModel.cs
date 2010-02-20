namespace PokerTell.Statistics.IntegrationTests.DesignViewModels
{
    using System.Collections.Generic;

    using Moq;

    using PokerTell.Infrastructure.Interfaces.PokerHand;
    using PokerTell.Statistics.Interfaces;
    using PokerTell.Statistics.ViewModels.Base;
    using PokerTell.Statistics.ViewModels.StatisticsSetDetails;

    public class DetailedPostFlopHeroActsStatisticsDesignModel : DetailedPostFlopHeroActsStatisticsViewModel
    {
        static readonly StubBuilder Stub = new StubBuilder();

        public DetailedPostFlopHeroActsStatisticsDesignModel(IHandBrowserViewModel handBrowserViewModel)
            : base(
                handBrowserViewModel, 
                Stub.Out<IPostFlopHeroActsRaiseReactionStatisticsViewModel>(), 
                Stub.Out<IDetailedPostFlopHeroActsStatisticsDescriber>())
        {
            Rows = new List<IStatisticsTableRowViewModel>
                {
                    new StatisticsTableRowViewModel("Bet", new[] { 20, 30, 12, 40, 30, 12 }, "%"), 
                    new StatisticsTableRowViewModel("Count", new[] { 1, 4, 30, 34, 33, 30, 12 }, string.Empty)
                };
        }
    }
}