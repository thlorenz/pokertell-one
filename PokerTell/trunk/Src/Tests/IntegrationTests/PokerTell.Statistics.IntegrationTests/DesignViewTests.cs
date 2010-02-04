namespace PokerTell.Statistics.IntegrationTests
{
   using System;

   using DesignViewModels;

   using DesignWindows;

   using Filters;

   using Infrastructure.Enumerations.PokerHand;
   using Infrastructure.Events;
   using Infrastructure.Interfaces.PokerHand;

   using Interfaces;

   using Microsoft.Practices.Composite.Events;

   using Moq;

   using StatisticsIntegrationTests.DesignWindows;

   using Tools.GenericRanges;

   using UnitTests;

   using ViewModels;
   using ViewModels.Filters;

   public class DesignViewTests : TestWithLog
   {
      static readonly IHandBrowserViewModel HandBrowserViewModelStub = new Mock<IHandBrowserViewModel>().Object;

      static readonly IRaiseReactionDescriber RaiseReactionDescriber = new Mock<IRaiseReactionDescriber>().Object;

      static readonly IRaiseReactionStatisticsBuilder RaiseReactionStatisticsBuilder =
         new Mock<IRaiseReactionStatisticsBuilder>().Object;

      public void AnalyzablePokerPlayersFilterAdjustmentView()
      {
         var eventAggregator = new EventAggregator();
         new PlayerStatisticsService(eventAggregator);

         var adjustFilterEventArgs = new AdjustAnalyzablePokerPlayersFilterEventArgs(
            "renniweg",
            AnalyzablePokerPlayersFilter.InactiveFilter,
            (name, filter) => Console.WriteLine("Apply filter: {0}\n to {1}.", filter, name),
            filter => Console.WriteLine("Apply filter: {0}\n to all Players.", filter));

         eventAggregator
            .GetEvent<AdjustAnalyzablePokerPlayersFilterEvent>()
            .Publish(adjustFilterEventArgs);
      }

      public void AnalyzablePokerPlayersFilterViewTemplate()
      {
         var filters = new AnalyzablePokerPlayersFilter();
         filters.TotalPlayersFilter.ActivateWith(2, 10);
         filters.PlayersInFlopFilter.ActivateWith(2, 10);
         filters.StrategicPositionFilter.ActivateWith(StrategicPositions.SB, StrategicPositions.BU);
         filters.AnteFilter.ActivateWith(50, 200);
         filters.BigBlindFilter.ActivateWith(1000, 5000);
         filters.MFilter.ActivateWith(10, 15);
         filters.TimeRangeFilter.ActivateWith(-120, 0);

         var designWindow = new AnalyzablePokerPlayersFilterDesignWindow {
            Topmost = true,
            DataContext = new AnalyzablePokerPlayersFilterViewModel(filters)
         };
         designWindow.ShowDialog();
      }

      public void DetailedStatisticsViewTemplate_DetailedPostFlopActionStatisticsViewModel()
      {
         var designWindow = new DetailedStatisticsDesignWindow
         { Topmost = true, DataContext = new DetailedPostFlopActionStatisticsDesignModel(HandBrowserViewModelStub, RaiseReactionStatisticsBuilder, RaiseReactionDescriber) };
         designWindow.ShowDialog();
      }

      public void DetailedStatisticsViewTemplate_DetailedPostFlopReactionStatisticsViewModel()
      {
         var designWindow = new DetailedStatisticsDesignWindow
         { Topmost = true, DataContext = new DetailedPostFlopReactionStatisticsDesignModel(HandBrowserViewModelStub, RaiseReactionStatisticsBuilder, RaiseReactionDescriber) };
         designWindow.ShowDialog();
      }

      public void DetailedStatisticsViewTemplate_DetailedPreFlopStatisticsViewModel()
      {
         var designWindow = new DetailedStatisticsDesignWindow
         { Topmost = true, DataContext = new DetailedPreFlopStatisticsDesignModel(HandBrowserViewModelStub, RaiseReactionStatisticsBuilder, RaiseReactionDescriber) };
         designWindow.ShowDialog();
      }

      public void DetailedStatisticsViewTemplate_RaiseReactionStatisticsViewModel()
      {
         //            var designWindow = new DetailedStatisticsDesignWindow
         //                { Topmost = true, DataContext = new DetailedRaiseReactionStatisticsDesignModel() };
         //            designWindow.ShowDialog();
      }

      public void PostFlopStatisticsSetsViewTemplate()
      {
         var designWindow = new PostFlopStatisticsSetsDesignWindow { Topmost = true };
         designWindow.ShowDialog();
      }

      public void PreFlopStatisticsSetsViewTemplate()
      {
         var designWindow = new PreFlopStatisticsSetsDesignWindow { Topmost = true };
         designWindow.ShowDialog();
      }

      public void RangeFilterForSelectorsViewTemplate()
      {
         var designWindow = new RangeFilterForSelectorsDesignWindow {
            Topmost = true,
            DataContext =
               new RangeFilterForSelectorsViewModel<int>(
                  new GenericRangeFilter<int>().ActivateWith(-120, 0),
                  new[] { 0, -10, -20, -30, -60, -90, -120, -240 },
                  "Time Range")
         };
         designWindow.ShowDialog();
      }

      public void StatisticsSetSummaryViewTemplate()
      {
         var designWindow = new StatisticsSetSummaryDesignWindow { Topmost = true };
         designWindow.ShowDialog();
      }
   }
}