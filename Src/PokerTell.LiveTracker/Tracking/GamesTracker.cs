namespace PokerTell.LiveTracker.Tracking
{
    using System.Collections.Generic;
    using System.IO;

    using Microsoft.Practices.Composite.Events;
    using Microsoft.Practices.Composite.Presentation.Events;

    using PokerTell.Infrastructure.Events;
    using PokerTell.Infrastructure.Interfaces;
    using PokerTell.Infrastructure.Interfaces.PokerHand;
    using PokerTell.LiveTracker.Events;
    using PokerTell.LiveTracker.Interfaces;
    using PokerTell.LiveTracker.Properties;

    using Tools.FunctionalCSharp;

    public class GamesTracker : IGamesTracker
    {
        readonly IConstructor<IGameController> _gameControllerMake;

        readonly IEventAggregator _eventAggregator;

        ILiveTrackerSettings _liveTrackerSettings;

        public IDictionary<string, IGameController> GameControllers { get; protected set; }

        // Allows to change thread option during tests since UIThread subscriptions don't work then
        public ThreadOption ThreadOption { get; set; }

        public GamesTracker(IEventAggregator eventAggregator, IConstructor<IGameController> gameControllerMake)
        {
            ThreadOption = ThreadOption.UIThread;
            _eventAggregator = eventAggregator;
            _gameControllerMake = gameControllerMake;

            GameControllers = new Dictionary<string, IGameController>();
        }

        void RegisterEvents()
        {
            _eventAggregator
                .GetEvent<NewHandEvent>()
                .Subscribe(args => NewHandFound(args.FoundInFullPath, args.ConvertedPokerHand), ThreadOption);
            _eventAggregator
                .GetEvent<LiveTrackerSettingsChangedEvent>()
                .Subscribe(UpdateAllGameControllersWithNewSettings, ThreadOption);
        }

        void UpdateAllGameControllersWithNewSettings(ILiveTrackerSettings updatedLiveTrackerSettings)
        {
            GameControllers.Values.ForEach(gc => gc.LiveTrackerSettings = updatedLiveTrackerSettings);
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

        public IGamesTracker InitializeWith(ILiveTrackerSettings liveTrackerSettings)
        {
            RegisterEvents();

            // TODO: create a new HandHistoryFileWatcher for each path in settings and initialize NewHandsTracker
            _liveTrackerSettings = liveTrackerSettings;
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
    }
}