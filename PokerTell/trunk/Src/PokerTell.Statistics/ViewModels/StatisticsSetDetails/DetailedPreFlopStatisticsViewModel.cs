namespace PokerTell.Statistics.ViewModels.StatisticsSetDetails
{
   using System.Collections.Generic;
   using System.Linq;

   using Base;

   using Infrastructure.Interfaces.PokerHand;
   using Infrastructure.Interfaces.Statistics;

   using Interfaces;

   public class DetailedPreFlopStatisticsViewModel : DetailedStatisticsViewModel
   {
      public DetailedPreFlopStatisticsViewModel(
         IHandBrowserViewModel handBrowserViewModel,
         IRaiseReactionStatisticsBuilder raiseReactionStatisticsBuilder,
         IPostFlopHeroActsRaiseReactionDescriber raiseReactionDescriber)
         
         : base(handBrowserViewModel, raiseReactionStatisticsBuilder, raiseReactionDescriber, "Position")
      {
      }

      protected override IDetailedStatisticsViewModel CreateTableAndDescriptionFor(
         IActionSequenceStatisticsSet statisticsSet)
      {
         var foldRow =
            new StatisticsTableRowViewModel("Fold", statisticsSet.ActionSequenceStatistics.First().Percentages, "%");
         var callRow =
            new StatisticsTableRowViewModel("Call", statisticsSet.ActionSequenceStatistics.ElementAt(1).Percentages, "%");
         var raiseRow =
            new StatisticsTableRowViewModel("Raise", statisticsSet.ActionSequenceStatistics.Last().Percentages, "%");
         var countRow =
            new StatisticsTableRowViewModel("Count", statisticsSet.SumOfCountsByColumn, string.Empty);

         Rows = new List<IStatisticsTableRowViewModel>(new[] { foldRow, callRow, raiseRow, countRow });

         StatisticsDescription =
            string.Format(
               "Player {0} {1} on the {2} in {3} pot",
               statisticsSet.PlayerName,
               statisticsSet.ActionSequence,
               statisticsSet.Street,
               statisticsSet.RaisedPot ? "a raised" : "an unraised");

         return this;
      }
   }
}