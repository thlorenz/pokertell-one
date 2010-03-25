namespace PokerTell.LiveTracker.Interfaces
{
    using System.Collections.Generic;

    using Tools.Interfaces;

    public interface IHandHistoryFolderAutoDetectResultsViewModel
    {
        IEnumerable<string> PokerRoomsWithDetectedHandHistoryDirectories { get; }

        IList<ITuple<string, string>> PokerRoomsWithoutDetectedHandHistoryDirectories { get; }

        ITuple<string, string> SelectedUndetectedPokerRoom { get; set; }

        string SelectedUndetectedPokerRoomInformation { get; set; }

        bool SomeDetectionsFailed { get; }

        IHandHistoryFolderAutoDetectResultsViewModel InitializeWith(IHandHistoryFolderAutoDetector handHistoryFolderAutoDetector);
    }
}