namespace PokerTell.LiveTracker.Interfaces
{
    using System.Collections.Generic;
    using System.Windows.Input;

    using Infrastructure.Interfaces.Statistics;

    public interface IPokerTableStatisticsViewModel
    {
        IDetailedStatisticsAnalyzerViewModel DetailedStatisticsAnalyzer { get; }

        ICommand FilterAdjustmentRequestedCommand { get; }

        IPlayerStatisticsViewModel SelectedPlayer { get; set; }

        IList<IPlayerStatisticsViewModel> Players { get; }

        IPokerTableStatisticsViewModel UpdateWith(IEnumerable<IPlayerStatistics> playersStatistics);
    }
}