namespace PokerTell.LiveTracker.Tracking
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Interfaces;

    using Microsoft.Practices.Composite.Events;

    using PokerTell.Infrastructure.Events;
    using PokerTell.Infrastructure.Interfaces.Repository;

    using Tools.FunctionalCSharp;
    using Tools.Interfaces;

    public class NewHandsTracker : INewHandsTracker
    {
        readonly IEventAggregator _eventAggregator;

        readonly IRepository _repository;

        public NewHandsTracker(IEventAggregator eventAggregator, IRepository repository)
        {
            _eventAggregator = eventAggregator;
            _repository = repository;
        }

        public INewHandsTracker InitializeWith(IEnumerable<IFileSystemWatcher> fileSystemWatchers)
        {
            fileSystemWatchers.ForEach(fsw => fsw .Changed += FileSystemWatcherChanged);
            return this;
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