namespace PokerTell.LiveTracker.Interfaces
{
    using System;
    using System.Collections.Generic;

    using Infrastructure.Interfaces.PokerHand;
    using Infrastructure.Interfaces.Statistics;

    using Tools.Interfaces;
    using Tools.WPF.Interfaces;

    public interface IGameController : IFluentInterface
    {
        ILiveTrackerSettings LiveTrackerSettings { get; set; }

        string HeroName { get; }

        IDictionary<string, IPlayerStatistics> PlayerStatistics { get; }

        bool IsLaunched { get; }

        string PokerSite { get; }

        IGameController NewHand(IConvertedPokerHand convertedPokerHand);

        event Action ShuttingDown;

        IGameController InitializeWith(IWindowManager liveStatsWindow, IWindowManager tableOverlayWindow);
    }
}