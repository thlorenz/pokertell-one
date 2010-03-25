namespace PokerTell.LiveTracker.Tests.PokerRooms
{
    using Interfaces;

    using LiveTracker.PokerRooms;

    using Machine.Specifications;

    using Moq;

    using Tools.FunctionalCSharp;
    using Tools.Interfaces;

    using It = Machine.Specifications.It;

    // Resharper disable InconsistentNaming
    public abstract class HandHistoryFolderAutoDetectorSpecs
    {

        protected static Mock<IPokerRoomInfo> _roomInfo_Stub;

        protected static Mock<IPokerRoomDetective> _detective_Mock;

        protected const string _handHistoryDirectory = "someFolder";

        protected const string _helpWithHandHistoryDirectorySetupLink = "someLink";

        protected const string _site = "someSite";

        protected static ITuple<string, string> _site_Directory_Pair;

        protected static ITuple<string, string> _site_HelpLink_Pair;

        protected static IHandHistoryFolderAutoDetector _sut;

        Establish specContext = () => { 
            _detective_Mock = new Mock<IPokerRoomDetective>();
            _detective_Mock
                .SetupGet(d => d.HandHistoryDirectory)
                .Returns(_handHistoryDirectory);
            _detective_Mock
                .Setup(d => d.Investigate())
                .Returns(_detective_Mock.Object);

            _roomInfo_Stub = new Mock<IPokerRoomInfo>();
            _roomInfo_Stub
                .SetupGet(ri => ri.Detective)
                .Returns(_detective_Mock.Object);
            _roomInfo_Stub
                .SetupGet(ri => ri.HelpWithHandHistoryDirectorySetupLink)
                .Returns(_helpWithHandHistoryDirectorySetupLink);
            _roomInfo_Stub
                .SetupGet(ri => ri.Site)
                .Returns(_site);
            _site_Directory_Pair = Tuple.New(_site, _handHistoryDirectory);
            _site_HelpLink_Pair = Tuple.New(_site, _helpWithHandHistoryDirectorySetupLink);

            _sut = new HandHistoryFolderAutoDetector().InitializeWith(new[] { _roomInfo_Stub.Object });
        };

        [Subject(typeof(HandHistoryFolderAutoDetector), "Detect")]
        public class when_poker_room_detective_did_not_find_the_room_to_be_installed : HandHistoryFolderAutoDetectorSpecs
        {
            const bool isInstalled = false;

            Establish context = () => _detective_Mock
                                          .SetupGet(d => d.PokerRoomIsInstalled)
                                          .Returns(isInstalled);

            Because of = () => _sut.Detect();

            It should_tell_the_detective_to_investigate = () => _detective_Mock.Verify(d => d.Investigate());

            It should_not_add_the_room_name_and_corresponding_handhistory_directory_to_the_PokerRoomsWithDetectedHandHistoryDirectories
                = () => _sut.PokerRoomsWithDetectedHandHistoryDirectories.ShouldNotContain(_site_Directory_Pair);

            It should_not_add_the_room_name_and_corresponding_help_link_to_the_PokerRoomsWithoutDetectedHandHistoryDirectories
                = () => _sut.PokerRoomsWithoutDetectedHandHistoryDirectories.ShouldNotContain(_site_HelpLink_Pair);
        }

        [Subject(typeof(HandHistoryFolderAutoDetector), "Detect")]
        public class when_poker_room_detective_found_the_room_to_be_installed_and_detected_the_handHistory_folder : HandHistoryFolderAutoDetectorSpecs
        {
            const bool isInstalled = true;
            const bool detectedHandHistoryDirectory = true;

            Establish context = () => {
                _detective_Mock
                    .SetupGet(d => d.PokerRoomIsInstalled)
                    .Returns(isInstalled);
                _detective_Mock
                    .SetupGet(d => d.DetectedHandHistoryDirectory)
                    .Returns(detectedHandHistoryDirectory);
            };

            Because of = () => _sut.Detect();

            It should_tell_the_detective_to_investigate = () => _detective_Mock.Verify(d => d.Investigate());

            It should_add_the_room_name_and_corresponding_handhistory_directory_to_the_PokerRoomsWithDetectedHandHistoryDirectories
                = () => _sut.PokerRoomsWithDetectedHandHistoryDirectories.ShouldContain(_site_Directory_Pair);

            It should_not_add_the_room_name_and_corresponding_help_link_to_the_PokerRoomsWithoutDetectedHandHistoryDirectories
                = () => _sut.PokerRoomsWithoutDetectedHandHistoryDirectories.ShouldNotContain(_site_HelpLink_Pair);
        }


        [Subject(typeof(HandHistoryFolderAutoDetector), "Detect")]
        public class when_poker_room_detective_found_the_room_to_be_installed_but_was_unable_to_detecte_the_handHistory_folder : HandHistoryFolderAutoDetectorSpecs
        {
            const bool isInstalled = true;
            const bool detectedHandHistoryDirectory = false;

            Establish context = () => {
                _detective_Mock
                    .SetupGet(d => d.PokerRoomIsInstalled)
                    .Returns(isInstalled);
                _detective_Mock
                    .SetupGet(d => d.DetectedHandHistoryDirectory)
                    .Returns(detectedHandHistoryDirectory);
            };

            Because of = () => _sut.Detect();

            It should_tell_the_detective_to_investigate = () => _detective_Mock.Verify(d => d.Investigate());

            It should_not_add_the_room_name_and_corresponding_handhistory_directory_to_the_PokerRoomsWithDetectedHandHistoryDirectories
                = () => _sut.PokerRoomsWithDetectedHandHistoryDirectories.ShouldNotContain(_site_Directory_Pair);

            It should_add_the_room_name_and_corresponding_help_link_to_the_PokerRoomsWithoutDetectedHandHistoryDirectories
                = () => _sut.PokerRoomsWithoutDetectedHandHistoryDirectories.ShouldContain(_site_HelpLink_Pair);
        }
    }
}