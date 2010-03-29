namespace PokerTell.LiveTracker.Interfaces
{
    using System.Collections.Generic;

    using Infrastructure.Interfaces;

    using Tools.Interfaces;

    public interface IPokerRoomSettingsDetector : IFluentInterface
    {
        IPokerRoomSettingsDetector DetectHandHistoryFolders();

        IList<ITuple<string, string>> PokerRoomsWithDetectedHandHistoryDirectories { get; }

        IList<ITuple<string, string>> PokerRoomsWithoutDetectedHandHistoryDirectories { get; }

        IList<ITuple<string, IDictionary<int, int>>> PokerRoomsWithDetectedPreferredSeats { get; }

        IPokerRoomSettingsDetector InitializeWith(IEnumerable<IPokerRoomInfo> pokerRoomInfos);

        IPokerRoomSettingsDetector DetectPreferredSeats();
    }
}