namespace PokerTell.LiveTracker.Tracking
{
    using System.Collections.Generic;
    using System.Linq;

    using Interfaces;

    /// <summary>
    /// Responsible for optimizing the directories being watched
    /// </summary>
    public class WatchedDirectoriesOptimizer : IWatchedDirectoriesOptimizer
    {
        /// <summary>
        /// Assumes that the FileSystemWatcher will include subdirectories.
        /// Assumes that all leading white spaces have been removed from the paths.
        /// Removes redundant directories. Namely it removes a directory if it is a subdirectory of a directory that is already watched and duplicates.
        /// </summary>
        /// <param name="allFullPaths">Unoptimized collection of watched directories</param>
        /// <returns>Optimized collection of watched directories</returns>
        public IEnumerable<string> Optimize(IList<string> allFullPaths)
        {

            var subPaths = allFullPaths
                .Where(subPath => allFullPaths.Any(path => subPath != path && subPath.WithDriveLetterToLower().StartsWith(path.WithDriveLetterToLower())))
                .ToList();

            subPaths.ForEach(subPath => allFullPaths.Remove(subPath));
            return allFullPaths.Distinct();
        }
    }

    public static class PathExtensions
    {
        public static string WithDriveLetterToLower(this string path)
        {
            if (path[1].Equals(':'))
            {
                return path[0].ToString().ToLower() + path.Substring(1);
            }

            return path;
        }
    }
}