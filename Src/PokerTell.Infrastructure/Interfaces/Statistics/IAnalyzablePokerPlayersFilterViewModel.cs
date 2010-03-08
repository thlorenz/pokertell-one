namespace PokerTell.Infrastructure.Interfaces.Statistics
{
    using PokerTell.Infrastructure.Enumerations.PokerHand;

    using Tools.Interfaces;

    public interface IAnalyzablePokerPlayersFilterViewModel : IFluentInterface
    {
        #region Properties

        IRangeFilterForSelectorsViewModel<int> PlayersInFlopFilter { get; }

        IRangeFilterForSelectorsViewModel<StrategicPositions> StrategicPositionFilter { get; }

        IRangeFilterForSelectorsViewModel<int> TotalPlayersFilter { get; }

        IRangeFilterForInputsViewModel<double> AnteFilter { get; }

        IRangeFilterForInputsViewModel<double> BigBlindFilter { get;  }

        IRangeFilterForInputsViewModel<int> MFilter { get;  }

        IRangeFilterForSelectorsViewModel<int> TimeRangeFilter { get; }

        IRangeFilterForSelectorsViewModel<string> TimeRangeFilterDisplay { get; }

        IAnalyzablePokerPlayersFilter CurrentFilter { get; }

        #endregion
    }
}