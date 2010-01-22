namespace PokerTell.Infrastructure.Interfaces.Statistics
{
    using System;

    using Enumerations.PokerHand;

    public interface IPlayerStatisticsViewModel
    {
        #region Properties

        IPostFlopStatisticsSetsViewModel FlopStatisticsSets { get; }

        string PlayerName { get; }

        IPreFlopStatisticsSetsViewModel PreFlopStatisticsSets { get; }

        IPostFlopStatisticsSetsViewModel RiverStatisticsSets { get; }

        IPostFlopStatisticsSetsViewModel TurnStatisticsSets { get; }

        IPlayerStatistics PlayerStatistics { get; }

        IAnalyzablePokerPlayersFilter Filter { get; set; }

        #endregion

        #region Public Methods

        IPlayerStatisticsViewModel UpdateWith(IPlayerStatistics playerStatistics);

        #endregion

        event Action<IActionSequenceStatisticsSet> SelectedStatisticsSetEvent;
    }
}