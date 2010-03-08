namespace PokerTell.Statistics.ViewModels
{
    using PokerTell.Infrastructure.Enumerations.PokerHand;
    using PokerTell.Infrastructure.Interfaces.Statistics;
    using PokerTell.Statistics.ViewModels.Filters;

    using Statistics.Filters;

    using Tools.Extensions;
    using Tools.FunctionalCSharp;
    using Tools.GenericRanges;

    public class AnalyzablePokerPlayersFilterViewModel : IAnalyzablePokerPlayersFilterViewModel
    {
        #region Constructors and Destructors

        public AnalyzablePokerPlayersFilterViewModel(IAnalyzablePokerPlayersFilter filter)
        {
            InitializePropertiesFrom(filter);
        }

        #endregion

        #region Properties

        public IRangeFilterForInputsViewModel<double> AnteFilter { get; protected set; }

        public IRangeFilterForInputsViewModel<double> BigBlindFilter { get; protected set; }

        public IRangeFilterForInputsViewModel<int> MFilter { get; protected set; }

        public IRangeFilterForSelectorsViewModel<int> PlayersInFlopFilter { get; protected set; }

        public IRangeFilterForSelectorsViewModel<StrategicPositions> StrategicPositionFilter { get; protected set; }

        public IRangeFilterForSelectorsViewModel<int> TimeRangeFilter { get; protected set; }

        public IRangeFilterForSelectorsViewModel<string> TimeRangeFilterDisplay { get; protected set; }

        public IRangeFilterForSelectorsViewModel<int> TotalPlayersFilter { get; protected set; }

        #endregion

        #region Methods

        protected void InitializePropertiesFrom(IAnalyzablePokerPlayersFilter filter)
        {
            TotalPlayersFilter = new RangeFilterForSelectorsViewModel<int>(
                filter.TotalPlayersFilter, 
                2.To(10), 
                "Total Players");

            PlayersInFlopFilter = new RangeFilterForSelectorsViewModel<int>(
                filter.PlayersInFlopFilter, 
                2.To(10), 
                "Players in Flop");

            StrategicPositionFilter =
                new RangeFilterForSelectorsViewModel<StrategicPositions>(
                    filter.StrategicPositionFilter, 
                    StrategicPositions.SB.To(StrategicPositions.BU), 
                    "Position");

            TimeRangeFilter =
                new RangeFilterForSelectorsViewModel<int>(
                    filter.TimeRangeFilter, 
                    new[] { 0, -10, -20, -30, -45, -60, -90, -120, -240, -480, -720 }, 
                    "Time Range", 
                    new TimeRangeValueToDisplayConverter().Convert);

            AnteFilter = new RangeFilterForInputsViewModel<double>(filter.AnteFilter, "Ante");
            BigBlindFilter = new RangeFilterForInputsViewModel<double>(filter.BigBlindFilter, "Big Blind");
            MFilter = new RangeFilterForInputsViewModel<int>(filter.MFilter, "M");
        }
        #endregion

        public IAnalyzablePokerPlayersFilter CurrentFilter
        {
            get
            {
                return new AnalyzablePokerPlayersFilter
                    {
                        AnteFilter = AnteFilter.CurrentFilter,
                        BigBlindFilter = BigBlindFilter.CurrentFilter,
                        MFilter = MFilter.CurrentFilter,
                        PlayersInFlopFilter = PlayersInFlopFilter.CurrentFilter,
                        StrategicPositionFilter = StrategicPositionFilter.CurrentFilter,
                        TimeRangeFilter = TimeRangeFilter.CurrentFilter,
                        TotalPlayersFilter = TotalPlayersFilter.CurrentFilter,
                    };
            }
        }
    }
}