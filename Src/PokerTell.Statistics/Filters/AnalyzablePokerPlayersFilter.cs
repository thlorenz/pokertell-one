namespace PokerTell.Statistics.Filters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using PokerTell.Infrastructure.Enumerations.PokerHand;
    using PokerTell.Infrastructure.Interfaces.PokerHand;
    using PokerTell.Infrastructure.Interfaces.Statistics;

    using Tools.Extensions;
    using Tools.GenericRanges;

    public class AnalyzablePokerPlayersFilter : IAnalyzablePokerPlayersFilter
    {
        #region Constants and Fields

        readonly GenericRangeFilter<DateTime> _timeStampFilter;

        #endregion

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

        public static IAnalyzablePokerPlayersFilter InactiveFilter
        {
            get
            {
                return new AnalyzablePokerPlayersFilter
                    {
                        AnteFilter =
                            new GenericRangeFilter<double>
                                {
                                    Range = new GenericRange<double>(0, 10000), 
                                    IsActive = false
                                }, 
                        BigBlindFilter =
                            new GenericRangeFilter<double>
                                {
                                    Range = new GenericRange<double>(0, 10000), 
                                    IsActive = false
                                }, 
                        MFilter =
                            new GenericRangeFilter<int> { Range = new GenericRange<int>(0, 100), IsActive = false }, 
                        PlayersInFlopFilter =
                            new GenericRangeFilter<int> { Range = new GenericRange<int>(2, 10), IsActive = false }, 
                        StrategicPositionFilter =
                            new GenericRangeFilter<StrategicPositions>
                                {
                                    Range =
                                        new GenericRange<StrategicPositions>(StrategicPositions.SB, 
                                                                             StrategicPositions.BU), 
                                    IsActive = false
                                }, 
                        TimeRangeFilter =
                            new GenericRangeFilter<int> { Range = new GenericRange<int>(-720, 0), IsActive = false }, 
                        TotalPlayersFilter =
                            new GenericRangeFilter<int> { Range = new GenericRange<int>(2, 10), IsActive = false }
                    };
            }
        }

        public GenericRangeFilter<double> AnteFilter { get; internal set; }

        public GenericRangeFilter<double> BigBlindFilter { get; internal set; }

        public GenericRangeFilter<int> MFilter { get; internal set; }

        public GenericRangeFilter<int> PlayersInFlopFilter { get; internal set; }

        public GenericRangeFilter<StrategicPositions> StrategicPositionFilter { get; internal set; }

        public GenericRangeFilter<int> TimeRangeFilter { get; internal set; }

        public GenericRangeFilter<int> TotalPlayersFilter { get; internal set; }

        protected virtual DateTime CurrentTime
        {
            get { return DateTime.Now; }
        }

        #endregion

        #region Public Methods

        public bool Equals(AnalyzablePokerPlayersFilter other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Equals(other.AnteFilter, AnteFilter) && Equals(other.BigBlindFilter, BigBlindFilter) &&
                   Equals(other.MFilter, MFilter) && Equals(other.PlayersInFlopFilter, PlayersInFlopFilter) &&
                   Equals(other.StrategicPositionFilter, StrategicPositionFilter) &&
                   Equals(other.TimeRangeFilter, TimeRangeFilter) &&
                   Equals(other.TotalPlayersFilter, TotalPlayersFilter);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return obj.GetType() == GetType() && Equals((AnalyzablePokerPlayersFilter)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = AnteFilter != null ? AnteFilter.GetHashCode() : 0;
                result = (result * 397) ^ (BigBlindFilter != null ? BigBlindFilter.GetHashCode() : 0);
                result = (result * 397) ^ (MFilter != null ? MFilter.GetHashCode() : 0);
                result = (result * 397) ^ (PlayersInFlopFilter != null ? PlayersInFlopFilter.GetHashCode() : 0);
                result = (result * 397) ^ (StrategicPositionFilter != null ? StrategicPositionFilter.GetHashCode() : 0);
                result = (result * 397) ^ (TimeRangeFilter != null ? TimeRangeFilter.GetHashCode() : 0);
                result = (result * 397) ^ (TotalPlayersFilter != null ? TotalPlayersFilter.GetHashCode() : 0);
                return result;
            }
        }

        public override string ToString()
        {
            return
                string.Format(
                    "AnalyzablePokerPlayersFilter:\n AnteFilter: {0}\n BigBlindFilter: {1}\n MFilter: {2}\n PlayersInFlopFilter: {3}\n StrategicPositionFilter: {4}\n TimeRangeFilter: {5}\n TotalPlayersFilter: {6}", 
                    AnteFilter, 
                    BigBlindFilter, 
                    MFilter, 
                    PlayersInFlopFilter, 
                    StrategicPositionFilter, 
                    TimeRangeFilter, 
                    TotalPlayersFilter);
        }

        #endregion

        #region Implemented Interfaces

        #region IAnalyzablePokerPlayersFilter

        public IEnumerable<IAnalyzablePokerPlayer> Filter(IEnumerable<IAnalyzablePokerPlayer> analyzablePokerPlayers)
        {
            if (NoFilterIsActive())
            {
                return analyzablePokerPlayers;
            }

            SetTimeStampFilterFromTimeRangeFilterAndCurrentTime();

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

        #endregion

        #endregion

        #region Methods

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

        void SetTimeStampFilterFromTimeRangeFilterAndCurrentTime()
        {
            if (TimeRangeFilter.IsActive)
            {
                _timeStampFilter.Range = new GenericRange<DateTime>(
                    CurrentTime.AddMinutes(TimeRangeFilter.Range.MinValue), 
                    CurrentTime.AddMinutes(TimeRangeFilter.Range.MaxValue));
            }

            _timeStampFilter.IsActive = TimeRangeFilter.IsActive;
        }

        #endregion
    }
}