namespace PokerTell.LiveTracker.Tracking
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    using Infrastructure.Interfaces.LiveTracker;

    using log4net;

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
        static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        readonly IConstructor<IGameController> _gameControllerMake;

        readonly IEventAggregator _eventAggregator;

        protected ILiveTrackerSettingsViewModel _liveTrackerSettings;

        readonly INewHandsTracker _newHandsTracker;

        public IDictionary<string, IGameController> GameControllers { get; protected set; }

        public IDictionary<string, IHandHistoryFilesWatcher> HandHistoryFilesWatchers { get; protected set; }

        // Allows to change thread option during tests since UIThread subscriptions don't work then
        public ThreadOption ThreadOption { get; set; }

        /// <summary>
        /// Creates a new instance of the <see cref="GamesTracker"/> class. 
        /// There should only be one in the entire application.
        /// </summary>
        /// <param name="eventAggregator">
        /// </param>
        /// <param name="newHandsTracker"><see cref="INewHandsTracker"/> </param>
        /// <param name="gameControllerMake"> Constructor for <see cref="IGameController"/></param>
        /// Constructor for <see cref="IHandHistoryFilesWatcher"/>
        /// </param>
        public GamesTracker(IEventAggregator eventAggregator, INewHandsTracker newHandsTracker, IConstructor<IGameController> gameControllerMake)
        {
            ThreadOption = ThreadOption.UIThread;

            _eventAggregator = eventAggregator;
            _newHandsTracker = newHandsTracker;
            _gameControllerMake = gameControllerMake;

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

            _newHandsTracker.TrackFolder(new FileInfo(fullPath).DirectoryName);
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

        void AdjustToNewLiveTrackerSettings(ILiveTrackerSettingsViewModel updatedLiveTrackerSettings)
        {
            _liveTrackerSettings = updatedLiveTrackerSettings;

            GameControllers.Values.ForEach(gc => gc.LiveTrackerSettings = _liveTrackerSettings);

            _newHandsTracker.TrackFolders(_liveTrackerSettings.HandHistoryFilesPaths);
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