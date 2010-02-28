namespace PokerTell.LiveTracker.Interfaces
{
    public interface ILayoutManager
    {
        ITableOverlaySettingsViewModel Load(string pokerSite, int seats);

        ILayoutManager Save(ITableOverlaySettingsViewModel overlaySettings, string pokerSite);
    }
}