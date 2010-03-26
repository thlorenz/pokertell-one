namespace PokerTell.Infrastructure.Interfaces.Statistics
{
    using System;
    using System.Windows.Input;

    using Enumerations.PokerHand;

    public interface IPreFlopStatisticsSetsViewModel : IFluentInterface
    {
        IStatisticsSetSummaryViewModel PreFlopUnraisedPotStatisticsSet { get; }

        IStatisticsSetSummaryViewModel PreFlopRaisedPotStatisticsSet { get; }

        int TotalCountPreFlopUnraisedPot { get; }

        int TotalCountPreFlopRaisedPot { get; }

        int TotalCounts { get; }

        string Steals { get; set; }

        ICommand BrowseAllHandsCommand { get; }

        IPreFlopStatisticsSetsViewModel UpdateWith(IPlayerStatistics playerStatistics);

        event Action<IActionSequenceStatisticsSet> SelectedStatisticsSetEvent;

        event Action BrowseAllMyHandsRequested;
    }
}