namespace PokerTell.LiveTracker.Events
{
    using Interfaces;

    using Microsoft.Practices.Composite.Presentation.Events;

    public class LiveTrackerSettingsChangedEvent : CompositePresentationEvent<ILiveTrackerSettings>
    {
    }

}