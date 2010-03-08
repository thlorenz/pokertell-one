namespace PokerTell.Infrastructure.Interfaces.Statistics
{
    using System;
    using System.Collections.Generic;

    using Enumerations.PokerHand;

    using PokerHand;

    using Tools.GenericRanges;

    public interface IAnalyzablePokerPlayersFilter
    {
        GenericRangeFilter<double> AnteFilter { get; }

        GenericRangeFilter<double> BigBlindFilter { get; }

        GenericRangeFilter<int> MFilter { get; }

        GenericRangeFilter<int> PlayersInFlopFilter { get; }

        GenericRangeFilter<StrategicPositions> StrategicPositionFilter { get; }

        GenericRangeFilter<int> TimeRangeFilter { get; }

        GenericRangeFilter<int> TotalPlayersFilter { get; }

        IEnumerable<IAnalyzablePokerPlayer> Filter(IEnumerable<IAnalyzablePokerPlayer> analyzablePokerPlayers);
    }
}