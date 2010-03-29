namespace PokerTell.LiveTracker.Interfaces
{
    using System.Collections.Generic;

    public interface ILayoutAutoConfigurator
    {
        ILayoutAutoConfigurator ConfigurePreferredSeats(string pokerSite, IDictionary<int, int> preferredSeats);
    }
}