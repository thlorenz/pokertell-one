namespace PokerTell.LiveTracker
{
    using Interfaces;

    using Tools.WPF;

    using Views;
    using Views.Overlay;

    public class PokerTableStatisticsWindowManager : WindowManager, IPokerTableStatisticsWindowManager
    {
        public PokerTableStatisticsWindowManager()
            : base(() => new PokerTableStatisticsView())
        {
        }
    }

    public class TableOverlayWindowManager : WindowManager, ITableOverlayWindowManager
    {
        public TableOverlayWindowManager()
            : base(() => new TableOverlayView())
        {
        }
    }
}