namespace PokerTell.LiveTracker.Interfaces
{
    using System.Collections.Generic;

    using Infrastructure.Interfaces;

    public interface ILayoutManager : IFluentInterface
    {
        ITableOverlaySettingsViewModel Load(string pokerSite, int seats);

        ILayoutManager Save(ITableOverlaySettingsViewModel overlaySettings, string pokerSite);
    }
}