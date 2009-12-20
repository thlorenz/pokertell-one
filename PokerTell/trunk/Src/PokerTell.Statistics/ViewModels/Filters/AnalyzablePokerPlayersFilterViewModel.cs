namespace PokerTell.Statistics.ViewModels.Filters
{
    using System.Linq;

    using PokerTell.Infrastructure.Enumerations.PokerHand;
    using PokerTell.Infrastructure.Interfaces.Statistics;

    using Tools.GenericRanges;

    public class AnalyzablePokerPlayersFilterViewModel : IAnalyzablePokerPlayersFilterViewModel
    {
        #region Constructors and Destructors

        public AnalyzablePokerPlayersFilterViewModel(IAnalyzablePokerPlayersFilter filters)
        {
            TotalPlayersFilter = new RangeFilterForSelectorsViewModel<int>(
                filters.TotalPlayersFilter, 
                Enumerable.Range(2, 10), 
                "Total Players");

            PlayersInFlopFilter = new RangeFilterForSelectorsViewModel<int>(
                filters.PlayersInFlopFilter, 
                Enumerable.Range(2, 10), 
                "Players in Flop");

            StrategicPositionFilter =
                new RangeFilterForSelectorsViewModel<StrategicPositions>(
                    filters.StrategicPositionFilter, 
                    StrategicPositionsUtility.GetAllPositionsInOrder(), 
                    "Position");

            AnteFilter = new RangeFilterForInputsViewModel<double>(filters.AnteFilter, "Ante");
            BigBlindFilter = new RangeFilterForInputsViewModel<double>(filters.BigBlindFilter, "Big Blind");
            MFilter = new RangeFilterForInputsViewModel<int>(filters.MFilter, "M");

        }

        #endregion

        #region Properties

        public IRangeFilterForSelectorsViewModel<int> PlayersInFlopFilter { get; protected set; }

        public IRangeFilterForSelectorsViewModel<StrategicPositions> StrategicPositionFilter { get; protected set; }

        public IRangeFilterForSelectorsViewModel<int> TotalPlayersFilter { get; protected set; }

        public IRangeFilterForInputsViewModel<double> AnteFilter { get; protected set; }

        public IRangeFilterForInputsViewModel<double> BigBlindFilter { get; protected set; }
      
        public IRangeFilterForInputsViewModel<int> MFilter { get; protected set; }

        public IRangeFilterForSelectorsViewModel<int> TimeRangeFilter { get; protected set; }

        #endregion
    }
}