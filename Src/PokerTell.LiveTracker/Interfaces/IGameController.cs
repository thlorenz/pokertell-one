namespace PokerTell.LiveTracker.Interfaces
{
    using System;
    using System.Collections.Generic;

    using Infrastructure.Interfaces;
    using Infrastructure.Interfaces.PokerHand;
    using Infrastructure.Interfaces.Statistics;

    using Tools.WPF.Interfaces;

    public interface IGameController : IFluentInterface
    {
        ILiveTrackerSettingsViewModel LiveTrackerSettings { get; set; }

        IDictionary<string, IPlayerStatistics> PlayerStatistics { get; }

        bool IsLaunched { get; }

        IGameController NewHand(IConvertedPokerHand convertedPokerHand);

        event Action ShuttingDown;
    }
}