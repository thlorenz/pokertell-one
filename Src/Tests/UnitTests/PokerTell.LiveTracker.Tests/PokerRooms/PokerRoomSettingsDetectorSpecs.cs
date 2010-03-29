namespace PokerTell.LiveTracker.Tests.PokerRooms
{
    using System.Collections.Generic;

    using Interfaces;

    using LiveTracker.PokerRooms;

    using Machine.Specifications;

    using Moq;

    using Tools.FunctionalCSharp;
    using Tools.Interfaces;

    using It = Machine.Specifications.It;

    // Resharper disable InconsistentNaming
    public abstract class PokerRoomSettingsDetectorSpecs
    {

        protected static Mock<IPokerRoomInfo> _roomInfo_Stub;

        protected static Mock<IPokerRoomDetective> _detective_Mock;

        protected const string _handHistoryDirectory = "someFolder";

        protected const string _helpWithHandHistoryDirectorySetupLink = "someLink";

        protected const string _site = "someSite";

        protected static ITuple<string, string> _site_Directory_Pair;

        protected static ITuple<string, string> _site_HelpLink_Pair;

        protected static IPokerRoomSettingsDetector _sut;

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

            _sut = new PokerRoomSettingsDetector().InitializeWith(new[] { _roomInfo_Stub.Object });
        };

        [Subject(typeof(PokerRoomSettingsDetector), "Detect HandHistoryFolders")]
        public class when_detecting_HandHistoryFolders_and_poker_room_detective_did_not_find_the_room_to_be_installed : PokerRoomSettingsDetectorSpecs
        {
            const bool isInstalled = false;

            Establish context = () => _detective_Mock
                                          .SetupGet(d => d.PokerRoomIsInstalled)
                                          .Returns(isInstalled);

            Because of = () => _sut.DetectHandHistoryFolders();

            It should_tell_the_detective_to_investigate = () => _detective_Mock.Verify(d => d.Investigate());

            It should_not_add_the_room_name_and_corresponding_handhistory_directory_to_the_PokerRoomsWithDetectedHandHistoryDirectories
                = () => _sut.PokerRoomsWithDetectedHandHistoryDirectories.ShouldNotContain(_site_Directory_Pair);

            It should_not_add_the_room_name_and_corresponding_help_link_to_the_PokerRoomsWithoutDetectedHandHistoryDirectories
                = () => _sut.PokerRoomsWithoutDetectedHandHistoryDirectories.ShouldNotContain(_site_HelpLink_Pair);
        }

        [Subject(typeof(PokerRoomSettingsDetector), "Detect HandHistoryFolders")]
        public class when_detecting_HandHistoryFolders_and_poker_room_detective_found_the_room_to_be_installed_and_detected_the_handHistory_folder : PokerRoomSettingsDetectorSpecs
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

            Because of = () => _sut.DetectHandHistoryFolders();

            It should_tell_the_detective_to_investigate = () => _detective_Mock.Verify(d => d.Investigate());

            It should_add_the_room_name_and_corresponding_handhistory_directory_to_the_PokerRoomsWithDetectedHandHistoryDirectories
                = () => _sut.PokerRoomsWithDetectedHandHistoryDirectories.ShouldContain(_site_Directory_Pair);

            It should_not_add_the_room_name_and_corresponding_help_link_to_the_PokerRoomsWithoutDetectedHandHistoryDirectories
                = () => _sut.PokerRoomsWithoutDetectedHandHistoryDirectories.ShouldNotContain(_site_HelpLink_Pair);
        }

        [Subject(typeof(PokerRoomSettingsDetector), "Detect HandHistoryFolders")]
        public class when_detecting_HandHistoryFolders_and_poker_room_detective_found_the_room_to_be_installed_but_was_unable_to_detecte_the_handHistory_folder : PokerRoomSettingsDetectorSpecs
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

            Because of = () => _sut.DetectHandHistoryFolders();

            It should_tell_the_detective_to_investigate = () => _detective_Mock.Verify(d => d.Investigate());

            It should_not_add_the_room_name_and_corresponding_handhistory_directory_to_the_PokerRoomsWithDetectedHandHistoryDirectories
                = () => _sut.PokerRoomsWithDetectedHandHistoryDirectories.ShouldNotContain(_site_Directory_Pair);

            It should_add_the_room_name_and_corresponding_help_link_to_the_PokerRoomsWithoutDetectedHandHistoryDirectories
                = () => _sut.PokerRoomsWithoutDetectedHandHistoryDirectories.ShouldContain(_site_HelpLink_Pair);
        }

        [Subject(typeof(PokerRoomSettingsDetector), "Detect PreferredSeats")]
        public class when_detecting_preferredSeats_and_poker_room_detective_says_the_PokerRoom_does_not_save_PreferredSeats : PokerRoomSettingsDetectorSpecs
        {
            const bool savesPreferredSeats = false;

            Establish context = () => _detective_Mock
                                          .SetupGet(d => d.PokerRoomSavesPreferredSeats)
                                          .Returns(savesPreferredSeats);

            Because of = () => _sut.DetectPreferredSeats();

            It should_not_tell_the_detective_to_investigate = () => _detective_Mock.Verify(d => d.Investigate(), Times.Never());
        }

        [Subject(typeof(PokerRoomSettingsDetector), "Detect PreferredSeats")]
        public class when_detecting_preferredSeats_and_poker_room_detective_did_not_find_the_room_to_be_installed : PokerRoomSettingsDetectorSpecs
        {
            const bool savesPreferredSeats = true;
            const bool isInstalled = false;

            Establish context = () => {
                _detective_Mock
                    .SetupGet(d => d.PokerRoomSavesPreferredSeats)
                    .Returns(savesPreferredSeats);
                _detective_Mock
                    .SetupGet(d => d.PokerRoomIsInstalled)
                    .Returns(isInstalled);
            };

            Because of = () => _sut.DetectPreferredSeats();

            It should_tell_the_detective_to_investigate = () => _detective_Mock.Verify(d => d.Investigate());

            It should_not_add_the_room_name_and_corresponding_preferredSeats_the_PokerRoomsWithDetectedPreferredSeats
                = () => _sut.PokerRoomsWithDetectedPreferredSeats.ShouldBeEmpty();
        }

        [Subject(typeof(PokerRoomSettingsDetector), "Detect PreferredSeats")]
        public class when_detecting_preferredSeats_and_poker_room_detective_did_find_the_room_to_be_installed_but_did_not_detect_the_preferredSeats : PokerRoomSettingsDetectorSpecs
        {
            const bool savesPreferredSeats = true;
            const bool isInstalled = true;
            const bool detectedPreferredSeats = false;

            Establish context = () => {
                _detective_Mock
                    .SetupGet(d => d.PokerRoomSavesPreferredSeats)
                    .Returns(savesPreferredSeats);
                _detective_Mock
                    .SetupGet(d => d.PokerRoomIsInstalled)
                    .Returns(isInstalled);
                _detective_Mock
                    .SetupGet(d => d.DetectedPreferredSeats)
                    .Returns(detectedPreferredSeats);
            };

            Because of = () => _sut.DetectPreferredSeats();

            It should_tell_the_detective_to_investigate = () => _detective_Mock.Verify(d => d.Investigate());

            It should_not_add_the_room_name_and_corresponding_preferredSeats_the_PokerRoomsWithDetectedPreferredSeats
                = () => _sut.PokerRoomsWithDetectedPreferredSeats.ShouldBeEmpty();
        }

        [Subject(typeof(PokerRoomSettingsDetector), "Detect PreferredSeats")]
        public class when_detecting_preferredSeats_and_poker_room_detective_found_the_room_to_be_installed_and_detected_the_preferredSeats : PokerRoomSettingsDetectorSpecs
        {
            const bool savesPreferredSeats = true;
            const bool isInstalled = true;
            const bool detectedPreferredSeats = true;

            static IDictionary<int, int> preferredSeats_Stub;

            static ITuple<string, IDictionary<int, int>> expectedPreferredSeatsForSite;

            Establish context = () => {
                preferredSeats_Stub = new Dictionary<int, int> { { 2, 1 } };
                expectedPreferredSeatsForSite = Tuple.New(_site, preferredSeats_Stub);
                
                _detective_Mock
                    .SetupGet(d => d.PokerRoomSavesPreferredSeats)
                    .Returns(savesPreferredSeats);
                _detective_Mock
                    .SetupGet(d => d.PokerRoomIsInstalled)
                    .Returns(isInstalled);
                _detective_Mock
                    .SetupGet(d => d.DetectedPreferredSeats)
                    .Returns(detectedPreferredSeats);
                _detective_Mock
                    .SetupGet(d => d.PreferredSeats)
                    .Returns(preferredSeats_Stub);
            };

            Because of = () => _sut.DetectPreferredSeats();

            It should_tell_the_detective_to_investigate = () => _detective_Mock.Verify(d => d.Investigate());

            It should_add_the_room_name_and_corresponding_preferredSeats_to_the_PokerRoomsWithDetectedPreferredSeats
                = () => _sut.PokerRoomsWithDetectedPreferredSeats.ShouldContain(expectedPreferredSeatsForSite);
        }
    }
}