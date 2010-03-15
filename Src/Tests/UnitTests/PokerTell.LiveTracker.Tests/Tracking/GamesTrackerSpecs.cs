namespace PokerTell.LiveTracker.Tests.Tracking
{
    using System;
    using System.Collections.Generic;

    using Machine.Specifications;

    using Microsoft.Practices.Composite.Events;
    using Microsoft.Practices.Composite.Presentation.Events;

    using Moq;

    using PokerTell.Infrastructure.Events;
    using PokerTell.Infrastructure.Interfaces;
    using PokerTell.Infrastructure.Interfaces.PokerHand;
    using PokerTell.Infrastructure.Services;
    using PokerTell.LiveTracker.Events;
    using PokerTell.LiveTracker.Interfaces;
    using PokerTell.LiveTracker.Tracking;

    using It = Machine.Specifications.It;

    // Resharper disable InconsistentNaming
    public abstract class GamesTrackerSpecs
    {
        protected static IEventAggregator _eventAggregator;

        protected static Mock<IGameController> _gameController_Mock;

        protected static Mock<ILiveTrackerSettingsViewModel> _settings_Stub;

        protected static Mock<INewHandsTracker> _newHandsTracker_Mock;

        protected static GamesTrackerSut _sut;

        Establish specContext = () => {
            _eventAggregator = new EventAggregator();
            _gameController_Mock = new Mock<IGameController>();

            _newHandsTracker_Mock = new Mock<INewHandsTracker>();

            _settings_Stub = new Mock<ILiveTrackerSettingsViewModel>();
            _settings_Stub
                .SetupGet(ls => ls.HandHistoryFilesPaths).Returns(new string[] { });

            _sut = new GamesTrackerSut(_eventAggregator,
                                       _newHandsTracker_Mock.Object,
                                       new Constructor<IGameController>(() => _gameController_Mock.Object))
                { ThreadOption = ThreadOption.PublisherThread };
        };

        public abstract class Ctx_InitializedWithLiveTrackerSettings : GamesTrackerSpecs
        {
            Establish initializedContext = () => _sut.InitializeWith(_settings_Stub.Object);
        } 

        [Subject(typeof(GamesTracker), "InitializeWith")]
        public class when_initialized_with_live_tracker_settings : GamesTrackerSpecs
        {
            const string path = "firstPath";

            static string[] thePaths;

            Establish context = () => {
                thePaths = new[] { path };
                _sut.GameControllers.Add("somePath", _gameController_Mock.Object);
                _settings_Stub
                    .SetupGet(ls => ls.HandHistoryFilesPaths).Returns(thePaths);
            };

            Because of = () => _sut.InitializeWith(_settings_Stub.Object);

            It should_set_the_LiveTrackerSettings_to_the_ones_that_were_passed = () => _sut._LiveTrackerSettings.ShouldBeTheSameAs(_settings_Stub.Object);

            It should_set_the_GameController_LiveTracker_Settings_to_ones_that_were_passed
                = () => _gameController_Mock.VerifySet(gc => gc.LiveTrackerSettings = _settings_Stub.Object);

            It should_tell_the_new_hands_trakcer_to_track_the_HandHistory_Paths_returned_by_the_live_tracker_settings
                = () => _newHandsTracker_Mock.Verify(ht => ht.TrackFolders(thePaths));
        }

        [Subject(typeof(GamesTracker), "LiveTrackerSettings changed")]
        public class when_it_contains_one_game_controller_and_the_LiveTrackerSettings_changed : Ctx_InitializedWithLiveTrackerSettings
        {
            const string firstPath = "firstPath";

            const string secondPath = "secondPath";

            static string[] thePaths;

            static Mock<ILiveTrackerSettingsViewModel> newSettings_Stub;

            Establish context = () => {
                _sut.GameControllers.Add("somePath", _gameController_Mock.Object);

                thePaths = new[] { firstPath, secondPath };
                newSettings_Stub = new Mock<ILiveTrackerSettingsViewModel>();
                newSettings_Stub
                    .SetupGet(ls => ls.HandHistoryFilesPaths).Returns(thePaths);
            };

            Because of = () => _eventAggregator
                                   .GetEvent<LiveTrackerSettingsChangedEvent>()
                                   .Publish(newSettings_Stub.Object);

            It should_set_the_LiveTrackerSettings_to_the_ones_that_were_passed = () => _sut._LiveTrackerSettings.ShouldBeTheSameAs(newSettings_Stub.Object);

            It should_set_the_GameController_LiveTracker_Settings_to_the_new_ones
                = () => _gameController_Mock.VerifySet(gc => gc.LiveTrackerSettings = newSettings_Stub.Object);

            It should_tell_the_new_hands_tracker_to_track_the_HandHistory_Paths_returned_by_the_live_tracker_settings
                = () => _newHandsTracker_Mock.Verify(ht => ht.TrackFolders(thePaths));
        }

        [Subject(typeof(GamesTracker), "StartTracking")]
        public class when_told_to_start_tracking_a_file_that_is_tracked_already : Ctx_InitializedWithLiveTrackerSettings
        {
            const string directory = @"c:\someDir\";

            const string filename = "someFile.txt";

            const string fullPath = directory + filename;

            static string userMessage;

            Establish context = () => {
                _sut.GameControllers.Add(fullPath, new Mock<IGameController>().Object);

                _eventAggregator
                    .GetEvent<UserMessageEvent>()
                    .Subscribe(args => userMessage = args.UserMessage);
            };

            Because of = () => _sut.StartTracking(fullPath);

            It should_not_add_the_file_to_the_GameControllers = () => _sut.GameControllers.Count.ShouldEqual(1);

            It should_raise_a_user_message_containing_the_filename = () => userMessage.ShouldContain("\"" + filename + "\"");
        }

        [Subject(typeof(GamesTracker), "StartTracking")]
        public class when_told_to_start_tracking_a_file_that_is_not_tracked_yet : Ctx_InitializedWithLiveTrackerSettings
        {
            const string folder = @"c:\someFolder";

            const string fullPath = folder + @"\someFileName";

            Because of = () => _sut.StartTracking(fullPath);

            It should_add_a_GameController_for_the_full_path_of_the_file = () => _sut.GameControllers.Keys.ShouldContain(fullPath);

            It should_set_the_GameController_LiveTracker_Settings_to_the_ones_passed_in
                = () => _gameController_Mock.VerifySet(gc => gc.LiveTrackerSettings = _settings_Stub.Object);

            It should_tell_the_NewHandsTracker_to_process_the_file = () => _newHandsTracker_Mock.Verify(ht => ht.ProcessHandHistoriesInFile(fullPath));

            It should_tell_the_NewHandsTracker_to_start_tracking_the_folder_that_contains_the_file
                = () => _newHandsTracker_Mock.Verify(ht => ht.TrackFolder(folder));
        }

        [Subject(typeof(GamesTracker), "New Hand found")]
        public class when_told_that_a_new_hand_for_a_file_that_it_is_tracking_was_found : Ctx_InitializedWithLiveTrackerSettings
        {
            const string fullPath = "somePath";

            static Mock<IConvertedPokerHand> newHand_Stub;

            Establish context = () => {
                newHand_Stub = new Mock<IConvertedPokerHand>();
                _sut.GameControllers.Add(fullPath, _gameController_Mock.Object);
            };

            Because of = () => _eventAggregator
                                   .GetEvent<NewHandEvent>()
                                   .Publish(new NewHandEventArgs(fullPath, newHand_Stub.Object));

            It should_tell_the_GameController_about_the_new_hand = () => _gameController_Mock.Verify(gc => gc.NewHand(newHand_Stub.Object));
        }

        [Subject(typeof(GamesTracker), "New Hand found")]
        public class when_a_new_hand_was_found_and_it_is_currently_tracking_no_files_and_it_is_supposed_to_AutoTrack :
            Ctx_InitializedWithLiveTrackerSettings
        {
            const string fullPath = "somePath";

            static Mock<IConvertedPokerHand> newHand_Stub;

            Establish context = () => {
                newHand_Stub = new Mock<IConvertedPokerHand>();
                _settings_Stub.SetupGet(s => s.AutoTrack).Returns(true);
            };

            Because of = () => _eventAggregator
                                   .GetEvent<NewHandEvent>()
                                   .Publish(new NewHandEventArgs(fullPath, newHand_Stub.Object));

            It should_add_a_new_GameController_for_the_full_path = () => _sut.GameControllers.Keys.ShouldContain(fullPath);

            It should_set_the_GameController_LiveTracker_Settings_to_the_ones_passed_in
                = () => _gameController_Mock.VerifySet(gc => gc.LiveTrackerSettings = _settings_Stub.Object);

            It should_tell_the_GameController_about_the_new_hand = () => _gameController_Mock.Verify(gc => gc.NewHand(newHand_Stub.Object));
        }

        [Subject(typeof(GamesTracker), "New Hand found")]
        public class when_a_new_hand_was_found_and_it_is_currently_tracking_no_files_but_it_is_not_supposed_to_AutoTrack :
            Ctx_InitializedWithLiveTrackerSettings
        {
            const string fullPath = "somePath";

            static Mock<IConvertedPokerHand> newHand_Stub;

            Establish context = () => {
                newHand_Stub = new Mock<IConvertedPokerHand>();
                _settings_Stub.SetupGet(s => s.AutoTrack).Returns(false);
            };

            Because of = () => _eventAggregator
                                   .GetEvent<NewHandEvent>()
                                   .Publish(new NewHandEventArgs(fullPath, newHand_Stub.Object));

            It should_not_add_a_new_GameController_for_the_full_path = () => _sut.GameControllers.Keys.ShouldNotContain(fullPath);
        }

        [Subject(typeof(GamesTracker), "GameController shuts down")]
        public class when_a_GameController_signals_shut_down : Ctx_InitializedWithLiveTrackerSettings
        {
            const string fullPath = "somePath";

            Establish context = () => _sut.StartTracking(fullPath);

            Because of = () => _gameController_Mock.Raise(gc => gc.ShuttingDown += null);

            It should_remove_it_from_the_GameControllers = () => _sut.GameControllers.Values.ShouldNotContain(_gameController_Mock.Object);
        }
    }

    public class GamesTrackerSut : GamesTracker
    {
        public GamesTrackerSut(IEventAggregator eventAggregator, INewHandsTracker newHandsTracker, IConstructor<IGameController> gameControllerMake)
            : base(eventAggregator, newHandsTracker, gameControllerMake)
        {
        }

        internal ILiveTrackerSettingsViewModel _LiveTrackerSettings
        {
            get { return _liveTrackerSettings; }
        }
  }
}