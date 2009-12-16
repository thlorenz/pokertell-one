namespace PokerTell.Statistics.ViewModels
{
    using System;

    using Infrastructure.Interfaces.Statistics;

    using StatisticsSetSummary;

    public class PreFlopStatisticsSetsViewModel : IPreFlopStatisticsSetsViewModel
    {
        public IStatisticsSetSummaryViewModel PreFlopUnraisedPotStatisticsSet { get; protected set; }

        public IStatisticsSetSummaryViewModel PreFlopRaisedPotStatisticsSet { get; protected set; }

        public int TotalCountPreFlopUnraisedPot { get; protected set; }

        public int TotalCountPreFlopRaisedPot { get; protected set; }

        public PreFlopStatisticsSetsViewModel()
        {
            PreFlopUnraisedPotStatisticsSet = new StatisticsSetSummaryViewModel();
            PreFlopRaisedPotStatisticsSet = new StatisticsSetSummaryViewModel();
        }

        public IPreFlopStatisticsSetsViewModel UpdateWith(IPlayerStatistics playerStatistics)
        {
            PreFlopUnraisedPotStatisticsSet.UpdateWith(playerStatistics.PreFlopUnraisedPot);
            PreFlopRaisedPotStatisticsSet.UpdateWith(playerStatistics.PreFlopRaisedPot);

            TotalCountPreFlopUnraisedPot = playerStatistics.TotalCountPreFlopUnraisedPot;
            TotalCountPreFlopRaisedPot = playerStatistics.TotalCountPreFlopRaisedPot;

            return this;
        }
    }
}