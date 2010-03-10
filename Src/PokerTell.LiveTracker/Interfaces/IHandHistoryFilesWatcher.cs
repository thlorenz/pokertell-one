namespace PokerTell.LiveTracker.Interfaces
{
    using System.IO;

    using Tools.Interfaces;

    public interface IHandHistoryFilesWatcher : IFileSystemWatcher
    {

        /// <summary>
        /// Initializes the underlying FileSystemWatcher to watch for hand histories in files with extension custom extension.
        /// It will also watch subdirectories of the given path.
        /// </summary>
        /// <param name="handHistoryFilesPath">It is assumed that the path exists, so check it before passing it in!</param>
        /// <returns></returns>
        IHandHistoryFilesWatcher InitializeWith(string handHistoryFilesPath);


        /// <summary>
        /// Initializes the underlying FileSystemWatcher to watch for hand histories in files with extension custom extension.
        /// It will also watch subdirectories of the given path.
        /// </summary>
        /// <param name="handHistoryFilesPath">It is assumed that the path exists, so check it before passing it in!</param>
        /// <param name="fileExtension">Custom file extension</param>
        /// <returns></returns>
        IHandHistoryFilesWatcher InitializeWith(string handHistoryFilesPath, string fileExtension);
    }
}