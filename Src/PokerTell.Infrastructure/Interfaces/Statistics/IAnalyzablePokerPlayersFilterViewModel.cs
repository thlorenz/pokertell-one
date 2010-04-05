namespace PokerTell.Infrastructure.Interfaces.Statistics
{
    using PokerTell.Infrastructure.Enumerations.PokerHand;

    public interface IAnalyzablePokerPlayersFilterViewModel : IFluentInterface
    {
        IRangeFilterForInputsViewModel<double> AnteFilter { get; }

        IRangeFilterForInputsViewModel<double> BigBlindFilter { get; }

        IAnalyzablePokerPlayersFilter CurrentFilter { get; }

        IRangeFilterForInputsViewModel<int> MFilter { get; }

        IRangeFilterForSelectorsViewModel<int> PlayersInFlopFilter { get; }

        IRangeFilterForSelectorsViewModel<StrategicPositions> StrategicPositionFilter { get; }

        IRangeFilterForSelectorsViewModel<int> TimeRangeFilter { get; }

        IRangeFilterForSelectorsViewModel<string> TimeRangeFilterDisplay { get; }

        IRangeFilterForSelectorsViewModel<int> TotalPlayersFilter { get; }
    }
}