namespace PokerTell.Infrastructure.Interfaces.Statistics
{
    public interface IPlayerStatisticsViewModel
    {
        #region Properties

        IPostFlopStatisticsSetsViewModel FlopStatisticsSets { get; }

        IPreFlopStatisticsSetsViewModel PreFlopStatisticsSets { get; }

        IPostFlopStatisticsSetsViewModel RiverStatisticsSets { get; }

        IPostFlopStatisticsSetsViewModel TurnStatisticsSets { get; }

        IPlayerStatisticsViewModel UpdateWith(IPlayerStatistics playerStatistics);

        #endregion
    }
}