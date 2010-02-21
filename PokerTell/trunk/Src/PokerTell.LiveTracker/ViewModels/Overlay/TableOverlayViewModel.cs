namespace PokerTell.LiveTracker.ViewModels.Overlay
{
    using System.Collections.Generic;

    using Interfaces;

    public class TableOverlayViewModel
    {
        // Need to sync PlayerStatistics of PokerTableStatisticsViewModel with OverlayStatisticsPanelViewModels
        
        public IPokerTableStatisticsViewModel PokerTableStatisticsViewModel { get; protected set; }

        public IList<IPlayerOverlayViewModel> PlayerOverlays { get; protected set; }
    }
}