namespace PokerTell.Infrastructure.Interfaces.Statistics
{
    public interface IPreFlopStatisticsSetsViewModel : IFluentInterface
    {
        IStatisticsSetSummaryViewModel PreFlopUnraisedPotStatisticsSet { get; }

        IStatisticsSetSummaryViewModel PreFlopRaisedPotStatisticsSet { get; }

        int TotalCountPreFlopUnraisedPot { get; }

        int TotalCountPreFlopRaisedPot { get; }

        IPreFlopStatisticsSetsViewModel UpdateWith(IPlayerStatistics playerStatistics);
    }
}