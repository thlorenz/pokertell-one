namespace PokerTell.LiveTracker.Design.Statistics
{
    using System.Collections.Generic;

    using Infrastructure.Interfaces.Statistics;

    using PokerTell.Infrastructure.Interfaces.PokerHand;
    using PokerTell.Statistics.Interfaces;
    using PokerTell.Statistics.ViewModels.Base;
    using PokerTell.Statistics.ViewModels.StatisticsSetDetails;

    public class DetailedPreFlopStatisticsDesignModel : DetailedPreFlopStatisticsViewModel
    {
        public DetailedPreFlopStatisticsDesignModel()
            : base(null, null, null, null, null, null)
        {
            var foldRow =
                new StatisticsTableRowViewModel("Fold", new[] { 10, 20, 12, 23, 66, 34, 54 }, "%");
            var callRow =
                new StatisticsTableRowViewModel("Call", new[] { 10, 20, 12, 23, 66, 34, 54 }, "%");
            var raiseRow =
                new StatisticsTableRowViewModel("Raise", new[] { 10, 20, 12, 23, 66, 34, 54 }, "%");
            var countRow =
                new StatisticsTableRowViewModel("Count", new[] { 10, 20, 12, 23, 66, 34, 54 }, string.Empty);

            Rows = new List<IStatisticsTableRowViewModel>(new[] { foldRow, callRow, raiseRow, countRow });
        }
    }
}