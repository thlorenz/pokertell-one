namespace PokerTell.LiveTracker.Interfaces
{
    using Tools.Interfaces;

    public interface ILayoutManager : IFluentInterface
    {
        ITableOverlaySettingsViewModel Load(string pokerSite, int seats);

        ILayoutManager Save(ITableOverlaySettingsViewModel overlaySettings, string pokerSite);
    }
}