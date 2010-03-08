namespace PokerTell.Infrastructure.Interfaces.Statistics
{
    using System;

    using Enumerations.PokerHand;

    using Tools.Interfaces;

    public interface IPreFlopStatisticsSetsViewModel : IFluentInterface
    {
        IStatisticsSetSummaryViewModel PreFlopUnraisedPotStatisticsSet { get; }

        IStatisticsSetSummaryViewModel PreFlopRaisedPotStatisticsSet { get; }

        int TotalCountPreFlopUnraisedPot { get; }

        int TotalCountPreFlopRaisedPot { get; }

        int TotalCounts { get; }

        string Steals { get; set; }

        IPreFlopStatisticsSetsViewModel UpdateWith(IPlayerStatistics playerStatistics);

        event Action<IActionSequenceStatisticsSet> SelectedStatisticsSetEvent;
    }
}