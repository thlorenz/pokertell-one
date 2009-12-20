namespace PokerTell.Infrastructure.Interfaces.Statistics
{
    using PokerTell.Infrastructure.Enumerations.PokerHand;

    public interface IAnalyzablePokerPlayersFilterViewModel : IFluentInterface
    {
        #region Properties

        IRangeFilterForSelectorsViewModel<int> PlayersInFlopFilter { get; }

        IRangeFilterForSelectorsViewModel<StrategicPositions> StrategicPositionFilter { get; }

        IRangeFilterForSelectorsViewModel<int> TotalPlayersFilter { get; }

        IRangeFilterForInputsViewModel<double> AnteFilter { get; }

        IRangeFilterForInputsViewModel<double> BigBlindFilter { get;  }

        IRangeFilterForInputsViewModel<int> MFilter { get;  }

        #endregion
    }
}