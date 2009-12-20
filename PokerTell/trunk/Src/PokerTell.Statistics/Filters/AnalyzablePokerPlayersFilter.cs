namespace PokerTell.Statistics.Filters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Infrastructure.Interfaces.Statistics;

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
            TimeRangeFilter = new GenericRangeFilter<int>();

            _timeStampFilter = new GenericRangeFilter<DateTime>();
        }

        #endregion

        #region Properties

        public GenericRangeFilter<double> AnteFilter { get; private set; }

        public GenericRangeFilter<double> BigBlindFilter { get; private set; }

        public GenericRangeFilter<int> MFilter { get; private set; }

        public GenericRangeFilter<int> PlayersInFlopFilter { get; private set; }

        public GenericRangeFilter<StrategicPositions> StrategicPositionFilter { get; private set; }

        readonly GenericRangeFilter<DateTime> _timeStampFilter;
       
        public GenericRangeFilter<int> TimeRangeFilter { get; private set; }

        public GenericRangeFilter<int> TotalPlayersFilter { get; private set; }

        #endregion

        #region Public Methods

        public IEnumerable<IAnalyzablePokerPlayer> Filter(IEnumerable<IAnalyzablePokerPlayer> analyzablePokerPlayers)
        {
            if (NoFilterIsActive())
            {
                return analyzablePokerPlayers;
            }
            
            ConvertTimeRangeFilterToTimeStampFilter();

            return
                (from player in analyzablePokerPlayers
                 where
                     player.MBefore.PassesThrough(MFilter) &&
                     player.Ante.PassesThrough(AnteFilter) &&
                     player.BB.PassesThrough(BigBlindFilter) &&
                     player.TotalPlayers.PassesThrough(TotalPlayersFilter) &&
                     player.PlayersInFlop.PassesThrough(PlayersInFlopFilter) &&
                     player.StrategicPosition.PassesThrough(StrategicPositionFilter) &&
                     player.TimeStamp.PassesThrough(_timeStampFilter)
                 select player).ToList();
        }

        void ConvertTimeRangeFilterToTimeStampFilter()
        {
            _timeStampFilter.Range = new GenericRange<DateTime>(
                DateTime.Now.AddMinutes(-TimeRangeFilter.Range.MinValue), 
                DateTime.Now.AddMinutes(-TimeRangeFilter.Range.MaxValue));
            _timeStampFilter.IsActive = TimeRangeFilter.IsActive;
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
                TimeRangeFilter.IsNotActive;
        }

        #endregion
    }
}