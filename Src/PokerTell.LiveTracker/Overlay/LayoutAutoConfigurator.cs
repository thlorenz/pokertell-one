namespace PokerTell.LiveTracker.Overlay
{
    using System.Collections.Generic;

    using Interfaces;

    using Tools.FunctionalCSharp;

    public class LayoutAutoConfigurator : ILayoutAutoConfigurator
    {
        readonly ILayoutManager _layoutManager;

        public LayoutAutoConfigurator(ILayoutManager layoutManager)
        {
            _layoutManager = layoutManager;
        }

        public ILayoutAutoConfigurator ConfigurePreferredSeats(string pokerSite, IDictionary<int, int> preferredSeats)
        {
            preferredSeats.Keys.ForEach(totalSeats => {
                var tableOverlaySettings = _layoutManager.Load(pokerSite, totalSeats);
                tableOverlaySettings.PreferredSeat = preferredSeats[totalSeats];
                _layoutManager.Save(tableOverlaySettings, pokerSite);
            });
            return this;
        }

    }
}