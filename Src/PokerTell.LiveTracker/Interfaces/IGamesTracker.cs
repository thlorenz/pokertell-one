namespace PokerTell.LiveTracker.Interfaces
{
    using System.Collections.Generic;

    using Microsoft.Practices.Composite.Presentation.Events;

    using Tools.Interfaces;

    public interface IGamesTracker : IFluentInterface
    {
        IGamesTracker StartTracking(string fullPath);

        IDictionary<string, IGameController> GameControllers { get; }

        ThreadOption ThreadOption { get; set; }

        IDictionary<string, IHandHistoryFilesWatcher> HandHistoryFilesWatchers { get; }

        IGamesTracker InitializeWith(ILiveTrackerSettingsViewModel liveTrackerSettings);
    }
}