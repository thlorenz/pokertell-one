namespace PokerTell.LiveTracker.Events
{
    using Infrastructure.Interfaces.LiveTracker;

    using Microsoft.Practices.Composite.Presentation.Events;

    public class LiveTrackerSettingsChangedEvent : CompositePresentationEvent<ILiveTrackerSettingsViewModel>
    {
    }

}