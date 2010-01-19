namespace PokerTell.Statistics.IntegrationTests.DesignViewModels
{
    using System.Collections.Generic;

    using Interfaces;

    using PokerTell.Statistics.ViewModels.StatisticsSetDetails;

    public class DetailedPostFlopActionStatisticsDesignModel : DetailedPostFlopActionStatisticsViewModel
    {

        public DetailedPostFlopActionStatisticsDesignModel()
        {
            Rows = new List<IDetailedStatisticsRowViewModel>
                {
                    new DetailedStatisticsRowViewModel("Bet", new[] { 20, 30, 12, 40, 30, 12 }, "%"),
                    new DetailedStatisticsRowViewModel("Count", new[] { 1, 4, 30, 34, 33, 30, 12 }, string.Empty)
                };
        }
       
    }
}