namespace PokerTell.LiveTracker.Interfaces
{
    using System;

    using Infrastructure.Interfaces.PokerHand;

    using Tools.Interfaces;

    public interface IGameController : IFluentInterface
    {
        ILiveTrackerSettings LiveTrackerSettings { get; set; }

        IGameController NewHand(IConvertedPokerHand convertedPokerHand);

        event Action ShuttingDown;
    }
}