namespace PokerTell.Statistics.IntegrationTests.DesignViewModels
{
    using System.Collections.Generic;

    using Infrastructure.Interfaces.Statistics;

    using Interfaces;

    using ViewModels.StatisticsSetDetails;

    public class DetailedPostFlopReactionStatisticsDesignModel : DetailedPostFlopReactionStatisticsViewModel
    {
        public DetailedPostFlopReactionStatisticsDesignModel()
        {
            Rows = new List<IDetailedStatisticsRowViewModel>
                {
                    new DetailedStatisticsRowViewModel("Fold", new[] { 20, 30, 12, 40, 30, 12 }, "%"),
                    new DetailedStatisticsRowViewModel("Call", new[] { 10, 35, 7, 60, 30, 12 }, "%"),
                    new DetailedStatisticsRowViewModel("Raise", new[] { 9, 44, 56, 70, 30, 12 },  "%"),
                    new DetailedStatisticsRowViewModel("Count", new[] { 1, 4, 30, 34, 33, 30, 12 }, string.Empty)
                };
        }
    }
}