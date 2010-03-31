namespace PokerTell.LiveTracker.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Input;

    using Infrastructure.Interfaces.Statistics;

    using Tools.WPF.Interfaces;

    public interface IPokerTableStatisticsViewModel
    {
        IDetailedStatisticsAnalyzerViewModel DetailedStatisticsAnalyzer { get; }

        ICommand FilterAdjustmentRequestedCommand { get; }

        IPlayerStatisticsViewModel SelectedPlayer { get; set; }

        IList<IPlayerStatisticsViewModel> Players { get; }

        string TableName { get; set; }

        IDimensionsViewModel Dimensions { get; }

        void UpdateWith(IEnumerable<IPlayerStatistics> playersStatistics);

        IPlayerStatisticsViewModel GetPlayerStatisticsViewModelFor(string playerName);

        event Action PlayersStatisticsWereUpdated;

        event Action<IActionSequenceStatisticsSet> UserSelectedStatisticsSet;

        event Action<IPlayerStatisticsViewModel> UserBrowsedAllHands;

        IPokerTableStatisticsViewModel SaveDimensions();
    }
}