namespace PokerTell.LiveTracker.Interfaces
{
    using System.Collections.Generic;

    public interface IWatchedDirectoriesOptimizer
    {
        /// <summary>
        /// Assumes that the FileSystemWatcher will include subdirectories
        /// Assumes that all leading white spaces have been removed from the paths
        /// Removes redundant directories. Namely it removes a directory if it is a subdirectory of a directory that is already watched and duplicates.
        /// </summary>
        /// <param name="allFullPaths">Unoptimized collection of watched directories</param>
        /// <returns>Optimized collection of watched directories</returns>
        IEnumerable<string> Optimize(IList<string> allFullPaths);
    }
}