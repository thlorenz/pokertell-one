namespace PokerTell.LiveTracker.Tests.Tracking
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Infrastructure.Events;
    using Infrastructure.Interfaces;
    using Infrastructure.Interfaces.PokerHand;
    using Infrastructure.Interfaces.Repository;

    using Interfaces;

    using LiveTracker.Tracking;

    using Machine.Specifications;

    using Microsoft.Practices.Composite.Events;

    using Moq;

    using It = Machine.Specifications.It;

    // Resharper disable InconsistentNaming
    public abstract class NewHandsTrackerSpecs
    {
        protected static IEventAggregator _eventAggregator;

        protected static Mock<IHandHistoryFilesWatcher> _fileSystemWatcher_Stub;

        protected static Mock<IConstructor<IHandHistoryFilesWatcher>> _filesWatcherMake_Mock;

        protected static Mock<IWatchedDirectoriesOptimizer> _watchedDirectoriesOptimizer_Mock;

        protected static Mock<IRepository> _repository_Mock;

        protected static INewHandsTracker _sut;

        Establish specContext = () => {
            _eventAggregator = new EventAggregator();
            _repository_Mock = new Mock<IRepository>();
            _fileSystemWatcher_Stub = new Mock<IHandHistoryFilesWatcher>();
            _fileSystemWatcher_Stub
                .Setup(fw => fw.InitializeWith(Moq.It.IsAny<string>())).Returns(_fileSystemWatcher_Stub.Object);

            _filesWatcherMake_Mock = new Mock<IConstructor<IHandHistoryFilesWatcher>>();
            _watchedDirectoriesOptimizer_Mock = new Mock<IWatchedDirectoriesOptimizer>();

            _sut = new NewHandsTracker(_eventAggregator, 
                                       _repository_Mock.Object, 
                                       _watchedDirectoriesOptimizer_Mock.Object, 
                                       _filesWatcherMake_Mock.Object);
        };

        public abstract class Ctx_AddedTracker : NewHandsTrackerSpecs
        {
            protected const string trackedDirectory = @"c:\someDirectory\";

            protected const string fileName = "someFileName.txt";

            protected static bool newHandEventWasRaised;

            Establish context = () => {
                _filesWatcherMake_Mock
                    .SetupGet(fwm => fwm.New).Returns(_fileSystemWatcher_Stub.Object);
                _watchedDirectoriesOptimizer_Mock
                    .Setup(wo => wo.Optimize(Moq.It.IsAny<IList<string>>()))
                    .Returns(new[] { trackedDirectory });
                _sut.TrackFolder(trackedDirectory);
            };
        }

        [Subject(typeof(NewHandsTracker), "TrackFolder")]
        public class when_no_folders_are_tracked_so_far_and_a_new_folder_is_tracked : NewHandsTrackerSpecs
        {
            const string trackedFolder = "tracked Folder";

            Establish context = () => {
                _filesWatcherMake_Mock
                    .SetupGet(fwm => fwm.New).Returns(_fileSystemWatcher_Stub.Object);

                _watchedDirectoriesOptimizer_Mock
                    .Setup(wo => wo.Optimize(Moq.It.IsAny<IList<string>>()))
                    .Returns(new[] { trackedFolder });
            };

            Because of = () => _sut.TrackFolder(trackedFolder);

            It should_ask_the_optimizer_to_optimize
                = () => _watchedDirectoriesOptimizer_Mock.Verify(wo => wo.Optimize(Moq.It.Is<IList<string>>(list => list.Contains(trackedFolder) && list.Count == 1)));

            It should_add_a_filewatcher_for_that_folder = () => _sut.HandHistoryFilesWatchers.Keys.ShouldContain(trackedFolder);

            It should_initialize_the_filewatcher_with_the_tracked_folder =
                () => _fileSystemWatcher_Stub.Verify(fw => fw.InitializeWith(trackedFolder));
        }

        [Subject(typeof(NewHandsTracker), "TrackFolder")]
        public class when_a_folder_is_tracked_and_the_same_folder_should_be_tracked_again : Ctx_AddedTracker
        {
            const string toBeTracked = trackedDirectory;

            Because of = () => _sut.TrackFolder(toBeTracked);

            It should_not_ask_the_optimizer_to_optimize_again
                = () => _watchedDirectoriesOptimizer_Mock.Verify(wo => wo.Optimize(Moq.It.IsAny<IList<string>>()), Times.Exactly(1));

            It should_not_initialize_the_filewatcher_with_the_tracked_folder_again =
                () => _fileSystemWatcher_Stub.Verify(fw => fw.InitializeWith(toBeTracked), Times.Exactly(1));
        }

        [Subject(typeof(NewHandsTracker), "TrackFolder")]
        public class when_a_folder_should_be_tracked_but_the_optimizer_removes_it_during_optimization : NewHandsTrackerSpecs
        {
            const string toBeTracked = "sub dir of a dir already tracked";

            Establish context = () => {
                _filesWatcherMake_Mock.SetupGet(fwm => fwm.New).Returns(_fileSystemWatcher_Stub.Object);
                _watchedDirectoriesOptimizer_Mock
                    .Setup(wo => wo.Optimize(Moq.It.IsAny<IList<string>>())).Returns(new List<string>());
            };

            Because of = () => _sut.TrackFolder(toBeTracked);

            It should_ask_the_optimizer_to_optimize
                = () => _watchedDirectoriesOptimizer_Mock.Verify(wo => wo.Optimize(Moq.It.Is<IList<string>>(list => list.Contains(toBeTracked) && list.Count == 1)));

            It should_not_add_a_new_watcher_for_the_path = () => _filesWatcherMake_Mock.VerifyGet(fwm => fwm.New, Times.Never());
        }

        [Subject(typeof(NewHandsTracker), "TrackFolder")]
        public class when_tracking_two_folders_and_told_to_track_a_parent_folder_of_both_so_the_optimizer_removes_them : NewHandsTrackerSpecs
        {
            const string firstPrevTracked = "subfolder1";
            
            const string secondPrevTracked = "subfolder2";

            const string toBeTracked = "parentfolder";

            static Mock<IHandHistoryFilesWatcher> firstWatcher;
            static Mock<IHandHistoryFilesWatcher> secondWatcher;

            Establish context = () => {
                _filesWatcherMake_Mock
                    .SetupGet(fwm => fwm.New).Returns(_fileSystemWatcher_Stub.Object);

                firstWatcher = new Mock<IHandHistoryFilesWatcher>();
                secondWatcher = new Mock<IHandHistoryFilesWatcher>();
                _sut.HandHistoryFilesWatchers.Add(firstPrevTracked, firstWatcher.Object);
                _sut.HandHistoryFilesWatchers.Add(secondPrevTracked, secondWatcher.Object);
                _watchedDirectoriesOptimizer_Mock
                    .Setup(wo => wo.Optimize(Moq.It.IsAny<IList<string>>())).Returns(new[] { toBeTracked });
            };

            Because of = () => _sut.TrackFolder(toBeTracked);

            It should_ask_the_optimizer_to_optimize_all_three_folders
                = () => _watchedDirectoriesOptimizer_Mock.Verify(
                            wo => wo.Optimize(Moq.It.Is<IList<string>>(
                                                  list => list.Contains(firstPrevTracked) && list.Contains(secondPrevTracked) && list.Contains(toBeTracked) && list.Count == 3)));

            It should_add_a_filewatcher_for_that_folder = () => _sut.HandHistoryFilesWatchers.Keys.ShouldContain(toBeTracked);

            It should_initialize_the_filewatcher_with_the_tracked_folder =
                () => _fileSystemWatcher_Stub.Verify(fw => fw.InitializeWith(toBeTracked));

            It should_dispose_the_first_watcher = () => firstWatcher.Verify(fw => fw.Dispose());

            It should_dispose_the_second_watcher = () => secondWatcher.Verify(fw => fw.Dispose());

            It should_remove_both_watchers_for_the_previously_tracked_folders = () => {
                _sut.HandHistoryFilesWatchers.Keys.ShouldNotContain(firstPrevTracked);
                _sut.HandHistoryFilesWatchers.Keys.ShouldNotContain(secondPrevTracked);
            };
        }

        [Subject(typeof(NewHandsTracker), "FileChanged")]
        public class when_a_filesystem_watcher_discovers_a_file_change_and_the_file_contains_hands : Ctx_AddedTracker
        {
            static IEnumerable<IConvertedPokerHand> returnedPokerHands;

            static string newHandEvent_FullPath;

            static IConvertedPokerHand newHandEvent_PokerHand;

            Establish context = () => {
                returnedPokerHands = new[] { new Mock<IConvertedPokerHand>().Object, new Mock<IConvertedPokerHand>().Object };

                _repository_Mock.Setup(r => r.RetrieveHandsFromFile(trackedDirectory + fileName)).Returns(returnedPokerHands);

                newHandEventWasRaised = false;

                _eventAggregator
                    .GetEvent<NewHandEvent>()
                    .Subscribe(args => {
                        newHandEventWasRaised = true;
                        newHandEvent_FullPath = args.FoundInFullPath;
                        newHandEvent_PokerHand = args.ConvertedPokerHand;
                    });
            };

            Because of =
                () =>
                _fileSystemWatcher_Stub.Raise(fw => fw.Changed += null, null, new FileSystemEventArgs(WatcherChangeTypes.Changed, trackedDirectory, fileName));

            It should_tell_the_repository_to_retrieve_the_hands_from_the_full_path
                = () => _repository_Mock.Verify(r => r.RetrieveHandsFromFile(trackedDirectory + fileName));

            It should_tell_the_repository_to_enter_the_returned_hands_into_the_database
                = () => _repository_Mock.Verify(r => r.InsertHands(returnedPokerHands));

            It should_raise_new_hand_event = () => newHandEventWasRaised.ShouldBeTrue();

            It the_new_hand_event_should_include_the_fullpath_as_argument = () => newHandEvent_FullPath.ShouldEqual(trackedDirectory + fileName);

            It the_new_hand_event_should_include_the_last_hand_as_argument = () => newHandEvent_PokerHand.ShouldBeTheSameAs(returnedPokerHands.Last());
        }

        [Subject(typeof(NewHandsTracker), "FileChanged")]
        public class when_a_filesystem_watcher_discovers_a_file_change_but_the_file_contains_no_hands : Ctx_AddedTracker
        {
            Establish context = () => {
                _repository_Mock.Setup(r => r.RetrieveHandsFromFile(trackedDirectory + fileName)).Returns(Enumerable.Empty<IConvertedPokerHand>);

                newHandEventWasRaised = false;

                _eventAggregator
                    .GetEvent<NewHandEvent>()
                    .Subscribe(args => newHandEventWasRaised = true);
            };

            Because of =
                () =>
                _fileSystemWatcher_Stub.Raise(fw => fw.Changed += null, null, new FileSystemEventArgs(WatcherChangeTypes.Changed, trackedDirectory, fileName));

            It should_tell_the_repository_to_retrieve_the_hands_from_the_full_path
                = () => _repository_Mock.Verify(r => r.RetrieveHandsFromFile(trackedDirectory + fileName));

            It should_not_tell_the_repository_to_enter_the_returned_hands_into_the_database
                = () => _repository_Mock.Verify(r => r.InsertHands(Moq.It.IsAny<IEnumerable<IConvertedPokerHand>>()), Times.Never());

            It should_not_raise_new_hand_event = () => newHandEventWasRaised.ShouldBeFalse();
        }
    }
}