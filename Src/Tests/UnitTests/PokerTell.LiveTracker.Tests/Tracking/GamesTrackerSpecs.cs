namespace PokerTell.LiveTracker.Tests.Tracking
{
    using Events;

    using Machine.Specifications;

    using Microsoft.Practices.Composite.Events;
    using Microsoft.Practices.Composite.Presentation.Events;

    using Moq;

    using PokerTell.Infrastructure.Events;
    using PokerTell.Infrastructure.Interfaces.PokerHand;
    using PokerTell.Infrastructure.Services;
    using PokerTell.LiveTracker.Interfaces;
    using PokerTell.LiveTracker.Tracking;

    using It = Machine.Specifications.It;

    // Resharper disable InconsistentNaming
    public abstract class GamesTrackerSpecs
    {
        protected static IEventAggregator _eventAggregator;

        protected static Mock<IGameController> _gameController_Mock;

        protected static Mock<ILiveTrackerSettings> _settings_Stub;

        protected static IGamesTracker _sut;

        Establish specContext = () => {
            _eventAggregator = new EventAggregator();
            _gameController_Mock = new Mock<IGameController>();
            _settings_Stub = new Mock<ILiveTrackerSettings>();
            _sut = new GamesTracker(_eventAggregator, new Constructor<IGameController>(() => _gameController_Mock.Object))
                { ThreadOption = ThreadOption.PublisherThread };
            _sut.InitializeWith(_settings_Stub.Object);
        };

        [Subject(typeof(GamesTracker), "StartTracking")]
        public class when_told_to_start_tracking_a_file_that_is_tracked_already : GamesTrackerSpecs
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
        public class when_told_to_start_tracking_a_file_that_is_not_tracked_yet : GamesTrackerSpecs
        {
            const string fullPath = "somePath";

            Because of = () => _sut.StartTracking(fullPath);

            It should_add_a_GameController_for_the_full_path_of_the_file = () => _sut.GameControllers.Keys.ShouldContain(fullPath);

            It should_set_the_GameController_LiveTracker_Settings_to_the_ones_passed_in
                = () => _gameController_Mock.VerifySet(gc => gc.LiveTrackerSettings = _settings_Stub.Object);
        }

        [Subject(typeof(GamesTracker), "New Hand found")]
        public class when_told_that_a_new_hand_for_a_file_that_it_is_tracking_was_found : GamesTrackerSpecs
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
        public class when_a_new_hand_was_found_and_it_is_currently_tracking_no_files_and_it_is_supposed_to_AutoTrack : GamesTrackerSpecs
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
        public class when_a_new_hand_was_found_and_it_is_currently_tracking_no_files_but_it_is_not_supposed_to_AutoTrack : GamesTrackerSpecs
        {
            const string fullPath = "somePath";

            static Mock<IConvertedPokerHand> newHand_Stub;
            Establish context = () =>
            {
                newHand_Stub = new Mock<IConvertedPokerHand>();
                _settings_Stub.SetupGet(s => s.AutoTrack).Returns(false);
            };

            Because of = () => _eventAggregator
                                   .GetEvent<NewHandEvent>()
                                   .Publish(new NewHandEventArgs(fullPath, newHand_Stub.Object));

            It should_not_add_a_new_GameController_for_the_full_path = () => _sut.GameControllers.Keys.ShouldNotContain(fullPath);
        }

        [Subject(typeof(GamesTracker), "LiveTrackerSettings changed")]
        public class when_the_live_tracker_settings_changed_and_it_contains_one_GameController : GamesTrackerSpecs
        {
            Establish context = () => _sut.GameControllers.Add("somePath", _gameController_Mock.Object);

            Because of = () => _eventAggregator
                                    .GetEvent<LiveTrackerSettingsChangedEvent>()
                                    .Publish(_settings_Stub.Object);

            It should_set_the_GameController_LiveTracker_Settings_to_the_new_ones
                = () => _gameController_Mock.VerifySet(gc => gc.LiveTrackerSettings = _settings_Stub.Object);
        }

        [Subject(typeof(GamesTracker), "GameController shuts down")]
        public class when_a_GameController_signals_shut_down : GamesTrackerSpecs
        {
            const string fullPath = "somePath";

            Establish context = () => _sut.StartTracking(fullPath);

            Because of = () => _gameController_Mock.Raise(gc => gc.ShuttingDown += null);

            It should_remove_it_from_the_GameControllers = () => _sut.GameControllers.Values.ShouldNotContain(_gameController_Mock.Object);
        }
    }
}