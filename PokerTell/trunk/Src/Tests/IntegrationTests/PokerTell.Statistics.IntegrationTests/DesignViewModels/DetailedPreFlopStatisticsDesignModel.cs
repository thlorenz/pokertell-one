namespace PokerTell.Statistics.IntegrationTests.DesignViewModels
{
    using System.Collections.Generic;

    using Infrastructure.Interfaces.PokerHand;
    using Infrastructure.Interfaces.Statistics;

    using Interfaces;

    using ViewModels.Base;
    using ViewModels.StatisticsSetDetails;

    public class DetailedPreFlopStatisticsDesignModel : DetailedPreFlopStatisticsViewModel
    {
        #region Constructors and Destructors

        public DetailedPreFlopStatisticsDesignModel(
           IHandBrowserViewModel handBrowserViewModel,
         IRaiseReactionStatisticsBuilder raiseReactionStatisticsBuilder,
         IPostFlopHeroActsRaiseReactionDescriber raiseReactionDescriber)
           : base(handBrowserViewModel, raiseReactionStatisticsBuilder, raiseReactionDescriber)
        {
            Rows = new List<IStatisticsTableRowViewModel>
                {
                    new StatisticsTableRowViewModel("Fold", new[] { 20, 30, 12, 40, 30, 12, 23 }, "%"),
                    new StatisticsTableRowViewModel("Call", new[] { 10, 35, 7, 60, 30, 12, 90 }, "%"),
                    new StatisticsTableRowViewModel("Raise", new[] { 9, 44, 56, 70, 30, 12, 44 }, "%"),
                    new StatisticsTableRowViewModel("Count", new[] { 1, 4, 30, 34, 33, 30, 12, 45 }, string.Empty)
                };
        }

        #endregion
    }
}