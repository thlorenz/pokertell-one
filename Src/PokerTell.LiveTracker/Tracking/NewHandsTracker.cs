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

    public class NewHandsTracker : INewHandsTracker
    {
        readonly IEventAggregator _eventAggregator;

        readonly IRepository _repository;

        IEnumerable<IHandHistoryFilesWatcher> _handHistoryFilesWatchers;

        /// <summary>
        /// Initializes a new instance of the new hands tracker.
        /// Is responsible for picking up file changes from the file system watchers it is initialized with.
        /// It then attempts to parse for hands in the file and raises a global new hand found event if it is successful.
        /// It also takes care of updating the repository with the new hand(s).
        /// There should only be one in the entire application.
        /// It is initialized from the GamesTracker.
        /// </summary>
        public NewHandsTracker(IEventAggregator eventAggregator, IRepository repository)
        {
            _eventAggregator = eventAggregator;
            _repository = repository;
        }

        public INewHandsTracker InitializeWith(IEnumerable<IHandHistoryFilesWatcher> handHistoryFilesWatchers)
        {
            _handHistoryFilesWatchers = handHistoryFilesWatchers;
            _handHistoryFilesWatchers.ForEach(fsw => fsw.Changed += FileSystemWatcherChanged);
            return this;
        }

        void FileSystemWatcherChanged(object sender, FileSystemEventArgs e)
        {
            ProcessHandHistoriesInFile(e.FullPath);
        }

        public void ProcessHandHistoriesInFile(string fullPath)
        {
            var handsFromFile = _repository.RetrieveHandsFromFile(fullPath);

            if (handsFromFile.Count() > 0)
            {
                _eventAggregator
                    .GetEvent<NewHandEvent>()
                    .Publish(new NewHandEventArgs(fullPath, handsFromFile.Last()));

                _repository.InsertHands(handsFromFile);
            }
        }
    }
}