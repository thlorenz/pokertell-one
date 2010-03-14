namespace PokerTell.LiveTracker.Tracking
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Microsoft.Practices.Composite.Events;
    using Microsoft.Practices.Composite.Presentation.Events;

    using PokerTell.Infrastructure.Events;
    using PokerTell.Infrastructure.Interfaces;
    using PokerTell.Infrastructure.Interfaces.PokerHand;
    using PokerTell.LiveTracker.Events;
    using PokerTell.LiveTracker.Interfaces;
    using PokerTell.LiveTracker.Properties;

    using Tools.FunctionalCSharp;

    /// <summary>
    /// Responsible for tracking all ongoing PokerGames, managing interaction of NewHandsTracker and FileSystemWatcher.
    /// It also takes care of creation and disposing of GameControllers.
    /// </summary>
    public class GamesTracker : IGamesTracker
    {
        readonly IConstructor<IGameController> _gameControllerMake;

        readonly IEventAggregator _eventAggregator;

        protected ILiveTrackerSettingsViewModel _liveTrackerSettings;

        readonly IConstructor<IHandHistoryFilesWatcher> _handHistoryFilesWatcherMake;

        readonly INewHandsTracker _newHandsTracker;

        readonly IWatchedDirectoriesOptimizer _watchedDirectoriesOptimizer;

        public IDictionary<string, IGameController> GameControllers { get; protected set; }

        public IDictionary<string, IHandHistoryFilesWatcher> HandHistoryFilesWatchers { get; protected set; }

        // Allows to change thread option during tests since UIThread subscriptions don't work then
        public ThreadOption ThreadOption { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GamesTracker"/> class. 
        /// Initializes the GamesTracker.
        /// There should only be one in the entire application.
        /// </summary>
        /// <param name="eventAggregator">
        /// </param>
        /// <param name="watchedDirectoriesOptimizer"><see cref="IWatchedDirectoriesOptimizer"/></param>
        /// <param name="newHandsTracker"><see cref="INewHandsTracker"/> </param>
        /// <param name="gameControllerMake"> Constructor for <see cref="IGameController"/></param>
        /// <param name="handHistoryFilesWatcherMake">
        /// Constructor for <see cref="IHandHistoryFilesWatcher"/>
        /// </param>
        public GamesTracker(
            IEventAggregator eventAggregator, 
            IWatchedDirectoriesOptimizer watchedDirectoriesOptimizer, 
            INewHandsTracker newHandsTracker, 
            IConstructor<IGameController> gameControllerMake, 
            IConstructor<IHandHistoryFilesWatcher> handHistoryFilesWatcherMake)
        {
            _watchedDirectoriesOptimizer = watchedDirectoriesOptimizer;
            ThreadOption = ThreadOption.UIThread;

            _eventAggregator = eventAggregator;
            _newHandsTracker = newHandsTracker;
            _gameControllerMake = gameControllerMake;
            _handHistoryFilesWatcherMake = handHistoryFilesWatcherMake;

            GameControllers = new Dictionary<string, IGameController>();
            HandHistoryFilesWatchers = new Dictionary<string, IHandHistoryFilesWatcher>();
        }

        public IGamesTracker InitializeWith(ILiveTrackerSettingsViewModel liveTrackerSettings)
        {
            AdjustToNewLiveTrackerSettings(liveTrackerSettings);
            RegisterEvents();
            return this;
        }

        public IGamesTracker StartTracking(string fullPath)
        {
            if (GameControllers.ContainsKey(fullPath))
            {
                PublishUserWarningMessage(fullPath);
                return this;
            }

            IGameController gameController = SetupGameController(fullPath);

            GameControllers.Add(fullPath, gameController);

            _newHandsTracker.ProcessHandHistoriesInFile(fullPath);

            return this;
        }

        IGameController SetupGameController(string fullPath)
        {
            var gameController = _gameControllerMake.New;
            gameController.LiveTrackerSettings = _liveTrackerSettings;
            gameController.ShuttingDown += () => GameControllers.Remove(fullPath);

            return gameController;
        }

        void PublishUserWarningMessage(string fullPath)
        {
            var fileName = new FileInfo(fullPath).Name;
            var msg = string.Format(Resources.Warning_HandHistoriesAreTrackedAlready, fileName);
            _eventAggregator
                .GetEvent<UserMessageEvent>()
                .Publish(new UserMessageEventArgs(UserMessageTypes.Warning, msg));
        }

        void RegisterEvents()
        {
            _eventAggregator
                .GetEvent<NewHandEvent>()
                .Subscribe(args => NewHandFound(args.FoundInFullPath, args.ConvertedPokerHand), ThreadOption);
            _eventAggregator
                .GetEvent<LiveTrackerSettingsChangedEvent>()
                .Subscribe(AdjustToNewLiveTrackerSettings, ThreadOption);
        }

        protected virtual void AdjustToNewLiveTrackerSettings(ILiveTrackerSettingsViewModel updatedLiveTrackerSettings)
        {
            _liveTrackerSettings = updatedLiveTrackerSettings;

            GameControllers.Values.ForEach(gc => gc.LiveTrackerSettings = _liveTrackerSettings);
            
            var pathsWereRemoved = RemoveFileWatchersForPathsThatShouldNotBeTrackedAnymore();
            var pathsWereAdded = AddFileWatchersToBeTrackedThatWereNotTrackedBefore();

            if (pathsWereRemoved || pathsWereAdded)
                _newHandsTracker.InitializeWith(HandHistoryFilesWatchers.Values);
        }

        bool AddFileWatchersToBeTrackedThatWereNotTrackedBefore()
        {
            bool pathAdded = false;

            var allPaths = new List<string>(HandHistoryFilesWatchers.Keys);
            allPaths.AddRange(_liveTrackerSettings.HandHistoryFilesPaths);

            var optimizedPaths = _watchedDirectoriesOptimizer.Optimize(allPaths);

            optimizedPaths.ForEach(path => {
                if (!HandHistoryFilesWatchers.Keys.Contains(path))
                {
                    HandHistoryFilesWatchers.Add(path, _handHistoryFilesWatcherMake.New.InitializeWith(path));
                    pathAdded = true;
                }
            });
            return pathAdded;
        }

        bool RemoveFileWatchersForPathsThatShouldNotBeTrackedAnymore()
        {
            var keysToBeRemoved = HandHistoryFilesWatchers.Keys
                .Filter(key => !_liveTrackerSettings.HandHistoryFilesPaths.Contains(key))
                .ToList();
            keysToBeRemoved.ForEach(key => {
                HandHistoryFilesWatchers[key].Dispose();
                HandHistoryFilesWatchers.Remove(key);
            });
            return keysToBeRemoved.Count > 0;
        }

        void NewHandFound(string fullPath, IConvertedPokerHand convertedPokerHand)
        {
            if (!GameControllers.ContainsKey(fullPath))
            {
                if (!_liveTrackerSettings.AutoTrack)
                    return;

                StartTracking(fullPath);
            }

            GameControllers[fullPath].NewHand(convertedPokerHand);
        }
    }
}