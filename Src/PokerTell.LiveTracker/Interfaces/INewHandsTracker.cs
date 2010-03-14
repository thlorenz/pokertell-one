namespace PokerTell.LiveTracker.Interfaces
{
    using System.Collections.Generic;

    using Tools.Interfaces;

    /// <summary>
    /// Is responsible for picking up file changes from the file system watchers it is initialized with.
    /// It then attempts to parse for hands in the file and raises a global new hand found event if it is successful.
    /// It also takes care of updating the repository with the new hand(s).
    /// There should only be one in the entire application.
    /// It is initialized from the GamesTracker.
    /// </summary>
    public interface INewHandsTracker : IFluentInterface
    {
        INewHandsTracker InitializeWith(IEnumerable<IHandHistoryFilesWatcher> handHistoryFilesWatchers);

        void ProcessHandHistoriesInFile(string fullPath);
    }
}