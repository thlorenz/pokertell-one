namespace PokerTell.LiveTracker.Tracking
{
    using System.IO;
    using System.Reflection;

    using log4net;

    using PokerTell.LiveTracker.Interfaces;

    using Tools.Interfaces;

    public class HandHistoryFilesWatcher : FileSystemWatcherAdapter, IHandHistoryFilesWatcher
    {
        static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Initializes the underlying FileSystemWatcher to watch for hand histories in files with extension "txt".
        /// It will also watch subdirectories of the given path.
        /// </summary>
        /// <param name="handHistoryFilesPath">It is assumed that the path exists, so check it before passing it in!</param>
        /// <returns></returns>
        public IHandHistoryFilesWatcher InitializeWith(string handHistoryFilesPath)
        {
            return InitializeWith(handHistoryFilesPath, "*.txt");
        }

        /// <summary>
        /// Initializes the underlying FileSystemWatcher to watch for hand histories in files with extension custom extension.
        /// It will also watch subdirectories of the given path.
        /// </summary>
        /// <param name="handHistoryFilesPath">It is assumed that the path exists, so check it before passing it in!</param>
        /// <param name="fileExtension">Custom file extension</param>
        /// <returns></returns>
        public IHandHistoryFilesWatcher InitializeWith(string handHistoryFilesPath, string fileExtension)
        {
            Log.DebugFormat("Creating handhistory watcher for:\n  {0}", handHistoryFilesPath);

            Path = handHistoryFilesPath;

            Filter = fileExtension.StartsWith("*.") ? fileExtension : "*." + fileExtension;

            NotifyFilter = NotifyFilters.Size;

            IncludeSubdirectories = true;

            EnableRaisingEvents = true;

            return this;
        }
    }
}