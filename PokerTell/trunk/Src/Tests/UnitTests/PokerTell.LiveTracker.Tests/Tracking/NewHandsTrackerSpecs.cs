namespace PokerTell.LiveTracker.Tests.Tracking
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Infrastructure.Events;
    using Infrastructure.Interfaces.PokerHand;
    using Infrastructure.Interfaces.Repository;

    using Interfaces;

    using LiveTracker.Tracking;

    using Machine.Specifications;

    using Microsoft.Practices.Composite.Events;

    using Moq;

    using Tools.Interfaces;

    using It = Machine.Specifications.It;

    // Resharper disable InconsistentNaming
    public abstract class NewHandsTrackerSpecs
    {
        protected static IEventAggregator _eventAggregator;

        protected static Mock<IFileSystemWatcher> _fileSystemWatcher_Stub;

        protected static Mock<IRepository> _repository_Mock;

        protected static INewHandsTracker _sut;

        Establish specContext = () =>
        {
            _eventAggregator = new EventAggregator();
            _fileSystemWatcher_Stub = new Mock<IFileSystemWatcher>();
            _repository_Mock = new Mock<IRepository>();

            _sut = new NewHandsTracker(_eventAggregator, _fileSystemWatcher_Stub.Object, _repository_Mock.Object);
        };

        public abstract class Ctx_FileChanged : NewHandsTrackerSpecs
        {

            protected const string directory = @"c:\someDirectory\";

            protected const string fileName = "someFileName.txt";

            protected static bool newHandEventWasRaised;
        }

        [Subject(typeof(NewHandsTracker), "FileChanged")]
        public class when_the_filesystem_watcher_discovers_a_file_change_and_the_file_contains_hands : Ctx_FileChanged
        {
            static IEnumerable<IConvertedPokerHand> returnedPokerHands;

            static string newHandEvent_FullPath;

            static IConvertedPokerHand newHandEvent_PokerHand;

            Establish context = () =>
            {
                returnedPokerHands = new[] { new Mock<IConvertedPokerHand>().Object, new Mock<IConvertedPokerHand>().Object };

                _repository_Mock.Setup(r => r.RetrieveHandsFromFile(directory + fileName)).Returns(returnedPokerHands);

                newHandEventWasRaised = false;

                _eventAggregator
                    .GetEvent<NewHandEvent>()
                    .Subscribe(args => {
                        newHandEventWasRaised = true;
                        newHandEvent_FullPath = args.FoundInFullPath;
                        newHandEvent_PokerHand = args.ConvertedPokerHand;
                    });
            };

            Because of = () => _fileSystemWatcher_Stub.Raise(fw => fw.Changed += null, null, new FileSystemEventArgs(WatcherChangeTypes.Changed, directory, fileName));

            It should_tell_the_repository_to_retrieve_the_hands_from_the_full_path
                = () => _repository_Mock.Verify(r => r.RetrieveHandsFromFile(directory + fileName));

            It should_tell_the_repository_to_enter_the_returned_hands_into_the_database
                = () => _repository_Mock.Verify(r => r.InsertHands(returnedPokerHands));

            It should_raise_new_hand_event = () => newHandEventWasRaised.ShouldBeTrue();

            It the_new_hand_event_should_include_the_fullpath_as_argument = () => newHandEvent_FullPath.ShouldEqual(directory + fileName);

            It the_new_hand_event_should_include_the_last_hand_as_argument = () => newHandEvent_PokerHand.ShouldBeTheSameAs(returnedPokerHands.Last());
        }

        [Subject(typeof(NewHandsTracker), "FileChanged")]
        public class when_the_filesystem_watcher_discovers_a_file_change_but_the_file_contains_no_hands : Ctx_FileChanged
        {
            Establish context = () => {
                _repository_Mock.Setup(r => r.RetrieveHandsFromFile(directory + fileName)).Returns(Enumerable.Empty<IConvertedPokerHand>);

                newHandEventWasRaised = false;

                _eventAggregator
                    .GetEvent<NewHandEvent>()
                    .Subscribe(args => newHandEventWasRaised = true);
            };

            Because of = () => _fileSystemWatcher_Stub.Raise(fw => fw.Changed += null, null, new FileSystemEventArgs(WatcherChangeTypes.Changed, directory, fileName));

            It should_tell_the_repository_to_retrieve_the_hands_from_the_full_path
                = () => _repository_Mock.Verify(r => r.RetrieveHandsFromFile(directory + fileName));

            It should_not_tell_the_repository_to_enter_the_returned_hands_into_the_database
                = () => _repository_Mock.Verify(r => r.InsertHands(Moq.It.IsAny<IEnumerable<IConvertedPokerHand>>()), Times.Never());

            It should_not_raise_new_hand_event = () => newHandEventWasRaised.ShouldBeFalse();
        }
    }
}