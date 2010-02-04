namespace PokerTell.Statistics.IntegrationTests.DesignViewModels
{
    using System.Collections.Generic;

    using Infrastructure.Interfaces.PokerHand;
    using Infrastructure.Interfaces.Statistics;

    using Interfaces;

    using ViewModels.Base;
    using ViewModels.StatisticsSetDetails;

    public class DetailedPostFlopReactionStatisticsDesignModel : DetailedPostFlopReactionStatisticsViewModel
    {
        public DetailedPostFlopReactionStatisticsDesignModel(
           IHandBrowserViewModel handBrowserViewModel,
         IRaiseReactionStatisticsBuilder raiseReactionStatisticsBuilder,
         IRaiseReactionDescriber raiseReactionDescriber)
           : base(handBrowserViewModel, raiseReactionStatisticsBuilder, raiseReactionDescriber)
        {
            Rows = new List<IStatisticsTableRowViewModel>
                {
                    new StatisticsTableRowViewModel("Fold", new[] { 20, 30, 12, 40, 30, 12 }, "%"),
                    new StatisticsTableRowViewModel("Call", new[] { 10, 35, 7, 60, 30, 12 }, "%"),
                    new StatisticsTableRowViewModel("Raise", new[] { 9, 44, 56, 70, 30, 12 },  "%"),
                    new StatisticsTableRowViewModel("Count", new[] { 1, 4, 30, 34, 33, 30, 12 }, string.Empty)
                };
        }
    }
}