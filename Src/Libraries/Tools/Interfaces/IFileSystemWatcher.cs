namespace Tools.Interfaces
{
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Runtime.Remoting;

    public interface IFileSystemWatcher : ISupportInitialize
    {
        WaitForChangedResult WaitForChanged(WatcherChangeTypes changeType);

        WaitForChangedResult WaitForChanged(WatcherChangeTypes changeType, int timeout);

        NotifyFilters NotifyFilter { get; set; }

        bool EnableRaisingEvents { get; set; }

        string Filter { get; set; }

        bool IncludeSubdirectories { get; set; }

        int InternalBufferSize { get; set; }

        string Path { get; set; }

        ISite Site { get; set; }

        ISynchronizeInvoke SynchronizingObject { get; set; }

        IContainer Container { get; }

        event FileSystemEventHandler Changed;

        event FileSystemEventHandler Created;

        event FileSystemEventHandler Deleted;

        event ErrorEventHandler Error;

        event RenamedEventHandler Renamed;

        void Dispose();

        event EventHandler Disposed;
    }

    /// <summary>
    /// Adapts the FileSystemWatcher to implement the <see cref="IFileSystemWatcher"/> interface
    /// </summary>
    public class FileSystemWatcherAdapter : FileSystemWatcher, IFileSystemWatcher
    {
    }
}