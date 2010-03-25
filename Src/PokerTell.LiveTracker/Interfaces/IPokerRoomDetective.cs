namespace PokerTell.LiveTracker.Interfaces
{
    using System.Collections.Generic;

    using Tools.Interfaces;

    public interface IPokerRoomDetective : IFluentInterface
    {
        bool PokerRoomIsInstalled { get; }

        bool SavesPreferredSeats { get; }

        bool DetectedPreferredSeats { get; }

        bool DetectedHandHistoryDirectory { get; }

        string HandHistoryDirectory { get; }

        IDictionary<int, int> PreferredSeats { get; }

        IPokerRoomDetective Investigate();
    }
}