namespace PokerTell.LiveTracker.Interfaces
{
    using Tools.Interfaces;

    public interface ILiveTrackerSettings : IFluentInterface
    {
        bool AutoTrack { get; }

        bool ShowTableOverlay { get; }

        int ShowHoleCardsDuration { get; }

        bool ShowLiveStatsWindowOnStartup { get; }
    }
}