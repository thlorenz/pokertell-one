namespace PokerTell.LiveTracker.Tracking
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    using log4net;

    using Microsoft.Practices.Composite.Events;

    using PokerTell.Infrastructure.Events;
    using PokerTell.Infrastructure.Interfaces;
    using PokerTell.Infrastructure.Interfaces.Repository;
    using PokerTell.LiveTracker.Interfaces;

    using Tools.FunctionalCSharp;

    public class NewHandsTracker : INewHandsTracker
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NewHandsTracker"/> class. 
        /// Initializes a new instance of the new hands tracker.
        /// Is responsible for picking up file changes from the file system watchers it is initialized with.
        /// It then attempts to parse for hands in the file and raises a global new hand found event if it is successful.
        /// It also takes care of updating the repository with the new hand(s).
        /// When told to track folders it uses the watcher optimizer to remove redundant folders (e.g. sub directories of folders already tracked)
        /// There should only be one in the entire application.
        /// It is initialized from the GamesTracker.
        /// </summary>
        public NewHandsTracker(
            IEventAggregator eventAggregator, 
            IRepository repository, 
            IWatchedDirectoriesOptimizer watchedDirectoriesOptimizer, 
            IConstructor<IHandHistoryFilesWatcher> handHistoryFilesWatcherMake)
        {
            _eventAggregator = eventAggregator;
            _repository = repository;
            _watchedDirectoriesOptimizer = watchedDirectoriesOptimizer;
            _handHistoryFilesWatcherMake = handHistoryFilesWatcherMake;

            HandHistoryFilesWatchers = new Dictionary<string, IHandHistoryFilesWatcher>();
        }

        static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        readonly IEventAggregator _eventAggregator;

        readonly IRepository _repository;

        readonly IWatchedDirectoriesOptimizer _watchedDirectoriesOptimizer;

        public IDictionary<string, IHandHistoryFilesWatcher> HandHistoryFilesWatchers { get; protected set; }

        readonly IConstructor<IHandHistoryFilesWatcher> _handHistoryFilesWatcherMake;

        public void TrackFolder(string fullPath)
        {
            if (! HandHistoryFilesWatchers.Keys.Contains(fullPath))
            {
                var allPaths = new List<string>(HandHistoryFilesWatchers.Keys) { fullPath };
                var optimizedPaths = _watchedDirectoriesOptimizer.Optimize(allPaths);

                if (optimizedPaths.Contains(fullPath))
                {
                    AddNewWatcherFor(fullPath);

                    RemoveWatchersNotContainedIn(optimizedPaths);
                }
            }
        }

        public void TrackFolders(IEnumerable<string> fullPaths)
        {
            fullPaths.ForEach(TrackFolder);
        }

        void RemoveWatchersNotContainedIn(IEnumerable<string> optimizedPaths)
        {
            var redundantPaths =
                HandHistoryFilesWatchers.Keys.Where(key => ! optimizedPaths.Contains(key))
                    .ToList();
            redundantPaths
                .ForEach(path => {
                    HandHistoryFilesWatchers[path].Dispose();
                    HandHistoryFilesWatchers.Remove(path);
                });
        }

        void AddNewWatcherFor(string fullPath)
        {
            var watcher = _handHistoryFilesWatcherMake.New.InitializeWith(fullPath);
            watcher.Changed += FileSystemWatcherChanged;
            HandHistoryFilesWatchers.Add(fullPath, watcher);
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
                Log.Debug("About to publish new hand");
                _eventAggregator
                    .GetEvent<NewHandEvent>()
                    .Publish(new NewHandEventArgs(fullPath, handsFromFile.Last()));

                _repository.InsertHands(handsFromFile);
            }
        }
    }
}