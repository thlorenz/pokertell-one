namespace PokerTell.Statistics.Interfaces
{
    using System;
    using System.Collections.Generic;

    using Infrastructure.Enumerations.PokerHand;
    using Infrastructure.Interfaces.PokerHand;

    using Tools.GenericRanges;

    public interface IAnalyzablePokerPlayersFilter
    {
        GenericRangeFilter<double> AnteFilter { get; }

        GenericRangeFilter<double> BigBlindFilter { get; }

        GenericRangeFilter<int> MFilter { get; }

        GenericRangeFilter<int> PlayersInFlopFilter { get; }

        GenericRangeFilter<StrategicPositions> StrategicPositionFilter { get; }

        GenericRangeFilter<DateTime> TimeStampFilter { get; }

        GenericRangeFilter<int> TotalPlayersFilter { get; }

        IEnumerable<IAnalyzablePokerPlayer> Filter(IEnumerable<IAnalyzablePokerPlayer> analyzablePokerPlayers);
    }
}