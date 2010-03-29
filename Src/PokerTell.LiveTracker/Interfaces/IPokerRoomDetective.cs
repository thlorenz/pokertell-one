namespace PokerTell.LiveTracker.Interfaces
{
    using System.Collections.Generic;

    using Infrastructure.Interfaces;

    public interface IPokerRoomDetective : IFluentInterface
    {
        bool PokerRoomIsInstalled { get; }

        bool PokerRoomSavesPreferredSeats { get; }

        bool DetectedPreferredSeats { get; }

        bool DetectedHandHistoryDirectory { get; }

        string HandHistoryDirectory { get; }

        IDictionary<int, int> PreferredSeats { get; }

        IPokerRoomDetective Investigate();
    }
}