namespace PokerTell.LiveTracker.Interfaces
{
    using System.Collections.Generic;

    using Tools.Interfaces;

    public interface IHandHistoryFolderAutoDetector : IFluentInterface
    {
        IHandHistoryFolderAutoDetector Detect();

        IList<ITuple<string, string>> PokerRoomsWithDetectedHandHistoryDirectories { get; }

        IList<ITuple<string, string>> PokerRoomsWithoutDetectedHandHistoryDirectories { get; }

        IHandHistoryFolderAutoDetector InitializeWith(IEnumerable<IPokerRoomInfo> pokerRoomInfos);
    }
}