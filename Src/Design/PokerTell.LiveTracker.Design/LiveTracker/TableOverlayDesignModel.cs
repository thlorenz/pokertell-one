namespace PokerTell.LiveTracker.Design.LiveTracker
{
    using PokerTell.LiveTracker.Interfaces;

    public static class TableOverlayDesign
    {
        public static readonly ITableOverlayViewModel Model =
            AutoWirerForTableOverlay
                .ConfigureTableOverlayDependencies()
                .Resolve<ITableOverlayViewModel>();

        
    }
}