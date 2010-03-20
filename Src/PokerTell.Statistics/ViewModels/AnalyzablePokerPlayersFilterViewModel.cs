namespace PokerTell.Statistics.ViewModels
{
    using PokerTell.Infrastructure.Enumerations.PokerHand;
    using PokerTell.Infrastructure.Interfaces.Statistics;
    using PokerTell.Statistics.Filters;
    using PokerTell.Statistics.ViewModels.Filters;

    using Tools.FunctionalCSharp;

    public class AnalyzablePokerPlayersFilterViewModel : IAnalyzablePokerPlayersFilterViewModel
    {
        public AnalyzablePokerPlayersFilterViewModel(IAnalyzablePokerPlayersFilter filter)
        {
            InitializePropertiesFrom(filter);
        }

        public IRangeFilterForInputsViewModel<double> AnteFilter { get; protected set; }

        public IRangeFilterForInputsViewModel<double> BigBlindFilter { get; protected set; }

        public IRangeFilterForInputsViewModel<int> MFilter { get; protected set; }

        public IRangeFilterForSelectorsViewModel<int> PlayersInFlopFilter { get; protected set; }

        public IRangeFilterForSelectorsViewModel<StrategicPositions> StrategicPositionFilter { get; protected set; }

        public IRangeFilterForSelectorsViewModel<int> TimeRangeFilter { get; protected set; }

        public IRangeFilterForSelectorsViewModel<string> TimeRangeFilterDisplay { get; protected set; }

        public IRangeFilterForSelectorsViewModel<int> TotalPlayersFilter { get; protected set; }

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