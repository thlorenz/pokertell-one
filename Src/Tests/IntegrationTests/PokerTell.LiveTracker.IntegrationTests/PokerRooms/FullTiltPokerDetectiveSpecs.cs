namespace PokerTell.LiveTracker.IntegrationTests.PokerRooms
{
    using Interfaces;

    using LiveTracker.PokerRooms;

    using Machine.Specifications;

    using Properties;

    // Resharper disable InconsistentNaming

    /// <summary>
    /// These specs assume that a FullTiltPoker client is installed and the following:
    ///     - a HandHistoryDirectory is defined in the machine.prefs
    /// </summary>
    public abstract class FullTiltPokerDetectiveSpecs
    {
        protected static IPokerRoomDetective _sut;

        Establish specContext = () => _sut = new FullTiltPokerDetective();

        [Subject(typeof(FullTiltPokerDetective), "Investigate HandHistoryDirectory")]
        public class when_FullTiltPoker_is_installed_and_has_saved_the_hand_history_folder_to_the_machine_prefs : FullTiltPokerDetectiveSpecs
        {
            Because of = () => _sut.Investigate();

            It should_detect_that_it_is_installed = () => _sut.PokerRoomIsInstalled.ShouldBeTrue();

            It should_detect_the_hand_history_directory = () => _sut.DetectedHandHistoryDirectory.ShouldBeTrue();

            It the_HandHistory_should_contain_Full_Tilt_Poker_slash_HandHistory = () => _sut.HandHistoryDirectory.ShouldContain(@"\Full Tilt Poker\HandHistory");
        }

        [Subject(typeof(FullTiltPokerDetective), "Detect PreferredSeats")]
        public class given_string_indicating_that_AutoRotate_is_true : FullTiltPokerDetectiveSpecs
        {
            Because of = () => _sut.DetectPreferredSeats(Resources.FullTiltPoker_UserPrefs_PreferredSeats_AutoRotateIsTrue);

            It should_detect_the_preferred_Seats = () => _sut.DetectedPreferredSeats.ShouldBeTrue();

            It the_preferred_Seat_for_2_total_players_should_be_1 = () => _sut.PreferredSeats[2].ShouldEqual(1);

            It the_preferred_Seat_for_6_total_players_should_be_3 = () => _sut.PreferredSeats[6].ShouldEqual(3);

            It the_preferred_Seat_for_8_total_players_should_be_4 = () => _sut.PreferredSeats[8].ShouldEqual(4);

            It the_preferred_Seat_for_9_total_players_should_be_5 = () => _sut.PreferredSeats[9].ShouldEqual(5);
        }

        [Subject(typeof(FullTiltPokerDetective), "Detect PreferredSeats")]
        public class given_string_indicating_that_AutoRotate_is_false : FullTiltPokerDetectiveSpecs
        {
            Because of = () => _sut.DetectPreferredSeats(Resources.FullTiltPoker_UserPrefs_PreferredSeats_AutoRotateIsFalse);

            It should_detect_the_preferred_Seats = () => _sut.DetectedPreferredSeats.ShouldBeTrue();

            It the_preferred_Seat_for_2_total_players_should_be_0 = () => _sut.PreferredSeats[2].ShouldEqual(0);

            It the_preferred_Seat_for_6_total_players_should_be_0 = () => _sut.PreferredSeats[6].ShouldEqual(0);

            It the_preferred_Seat_for_8_total_players_should_be_0 = () => _sut.PreferredSeats[8].ShouldEqual(0);

            It the_preferred_Seat_for_9_total_players_should_be_0 = () => _sut.PreferredSeats[9].ShouldEqual(0);
        }
    }
}