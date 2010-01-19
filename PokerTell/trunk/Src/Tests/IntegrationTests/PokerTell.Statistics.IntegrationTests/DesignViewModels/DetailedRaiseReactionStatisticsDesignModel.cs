namespace PokerTell.Statistics.IntegrationTests.DesignViewModels
{
    using System.Collections.Generic;

    using Interfaces;

    using ViewModels.StatisticsSetDetails;

    public class DetailedRaiseReactionStatisticsDesignModel : DetailedRaiseReactionStatisticsViewModel
    {
        #region Constructors and Destructors

        public DetailedRaiseReactionStatisticsDesignModel()
        {
            Rows = new List<IDetailedStatisticsRowViewModel>
                {
                    new DetailedStatisticsRowViewModel("Fold", new[] { 20, 30, 12, 40 }, "%"),
                    new DetailedStatisticsRowViewModel("Call", new[] { 10, 35, 7, 60 }, "%"),
                    new DetailedStatisticsRowViewModel("Raise", new[] { 9, 44, 56, 70 }, "%"),
                    new DetailedStatisticsRowViewModel("Count", new[] { 1, 4, 30, 34, 33 }, string.Empty)
                };
        }

        #endregion
    }
}