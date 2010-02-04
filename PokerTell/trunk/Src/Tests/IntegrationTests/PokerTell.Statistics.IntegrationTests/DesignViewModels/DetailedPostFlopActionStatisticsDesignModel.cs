namespace PokerTell.Statistics.IntegrationTests.DesignViewModels
{
    using System.Collections.Generic;

    using Infrastructure.Interfaces.PokerHand;
    using Infrastructure.Interfaces.Statistics;

    using Interfaces;

    using PokerTell.Statistics.ViewModels.StatisticsSetDetails;

    using System.Linq;

    using ViewModels.Base;

    public class DetailedPostFlopActionStatisticsDesignModel : DetailedPostFlopActionStatisticsViewModel
    {

        public DetailedPostFlopActionStatisticsDesignModel(
           IHandBrowserViewModel handBrowserViewModel,
            IRaiseReactionStatisticsBuilder raiseReactionStatisticsBuilder,
            IRaiseReactionDescriber raiseReactionDescriber)
           : base(handBrowserViewModel, raiseReactionStatisticsBuilder, raiseReactionDescriber)
        {
            Rows = new List<IStatisticsTableRowViewModel>
                {
                    new StatisticsTableRowViewModel("Bet", new[] { 20, 30, 12, 40, 30, 12 }, "%"),
                    new StatisticsTableRowViewModel("Count", new[] { 1, 4, 30, 34, 33, 30, 12 }, string.Empty)
                };
        }
       
    }
}