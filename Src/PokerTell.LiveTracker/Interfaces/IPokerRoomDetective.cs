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

        /// <summary>
        /// When AutoRotate is selcted, Full Tilt will seat the player in the bottom Center which corresponds to the following seats for the different max players:
        /// Max     CenterSeat
        ///  2      1
        ///  5      3
        ///  6      3
        ///  7      4
        ///  8      4
        ///  9      5
        /// </summary>
        /// <param name="settings"></param>
        void DetectPreferredSeats(string settings);
    }
}