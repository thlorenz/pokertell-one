namespace PokerTell.LiveTracker.Tracking
{
    using System.IO;
    using System.Linq;

    using Interfaces;

    using Microsoft.Practices.Composite.Events;

    using PokerTell.Infrastructure.Events;
    using PokerTell.Infrastructure.Interfaces.Repository;

    using Tools.Interfaces;

    public class NewHandsTracker : INewHandsTracker
    {
        readonly IFileSystemWatcher _fileSystemWatcher;

        readonly IEventAggregator _eventAggregator;

        readonly IRepository _repository;

        public NewHandsTracker(IEventAggregator eventAggregator, IFileSystemWatcher fileSystemWatcher, IRepository repository)
        {
            _eventAggregator = eventAggregator;
            _fileSystemWatcher = fileSystemWatcher;
            _repository = repository;

            _fileSystemWatcher.Changed += FileSystemWatcherChanged;
        }

        void FileSystemWatcherChanged(object sender, FileSystemEventArgs e)
        {
            var handsFromFile = _repository.RetrieveHandsFromFile(e.FullPath);

            if (handsFromFile.Count() > 0)
            {
                _eventAggregator
                    .GetEvent<NewHandEvent>()
                    .Publish(new NewHandEventArgs(e.FullPath, handsFromFile.Last()));

                _repository.InsertHands(handsFromFile);
            }
        }
    }
}