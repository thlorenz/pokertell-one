namespace PokerTell.Statistics.Filters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Interfaces;

    using PokerTell.Infrastructure.Enumerations.PokerHand;
    using PokerTell.Infrastructure.Interfaces.PokerHand;

    using Tools.Extensions;
    using Tools.GenericRanges;

    public class AnalyzablePokerPlayersFilter : IAnalyzablePokerPlayersFilter
    {
        #region Constructors and Destructors

        public AnalyzablePokerPlayersFilter()
        {
            MFilter = new GenericRangeFilter<int>();
            AnteFilter = new GenericRangeFilter<double>();
            BigBlindFilter = new GenericRangeFilter<double>();
            TotalPlayersFilter = new GenericRangeFilter<int>();
            PlayersInFlopFilter = new GenericRangeFilter<int>();
            StrategicPositionFilter = new GenericRangeFilter<StrategicPositions>();
            TimeStampFilter = new GenericRangeFilter<DateTime>();
        }

        #endregion

        #region Properties

        public GenericRangeFilter<double> AnteFilter { get; private set; }

        public GenericRangeFilter<double> BigBlindFilter { get; private set; }

        public GenericRangeFilter<int> MFilter { get; private set; }

        public GenericRangeFilter<int> PlayersInFlopFilter { get; private set; }

        public GenericRangeFilter<StrategicPositions> StrategicPositionFilter { get; private set; }

        public GenericRangeFilter<DateTime> TimeStampFilter { get; private set; }

        public GenericRangeFilter<int> TotalPlayersFilter { get; private set; }

        #endregion

        #region Public Methods

        public IEnumerable<IAnalyzablePokerPlayer> Filter(IEnumerable<IAnalyzablePokerPlayer> analyzablePokerPlayers)
        {
            if (NoFilterIsActive())
            {
                return analyzablePokerPlayers;
            }
            
            return
                (from player in analyzablePokerPlayers
                 where
                     player.MBefore.PassesThrough(MFilter) &&
                     player.Ante.PassesThrough(AnteFilter) &&
                     player.BB.PassesThrough(BigBlindFilter) &&
                     player.TotalPlayers.PassesThrough(TotalPlayersFilter) &&
                     player.PlayersInFlop.PassesThrough(PlayersInFlopFilter) &&
                     player.StrategicPosition.PassesThrough(StrategicPositionFilter) &&
                     player.TimeStamp.PassesThrough(TimeStampFilter)
                 select player).ToList();
        }

        bool NoFilterIsActive()
        {
            return
                MFilter.IsNotActive &&
                AnteFilter.IsNotActive &&
                BigBlindFilter.IsNotActive &&
                TotalPlayersFilter.IsNotActive &&
                PlayersInFlopFilter.IsNotActive &&
                StrategicPositionFilter.IsNotActive &&
                TimeStampFilter.IsNotActive;
        }

        #endregion
    }
}