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

        IPreFlopStatisticsSetsViewModel UpdateWith(IPlayerStatistics playerStatistics);

        event Action<IActionSequenceStatisticsSet, Streets> SelectedStatisticsSetEvent;
    }
}