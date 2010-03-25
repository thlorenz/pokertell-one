namespace PokerTell.LiveTracker.IntegrationTests.PokerRooms
{
    using Machine.Specifications;

    using PokerTell.LiveTracker.PokerRooms;

    // Resharper disable InconsistentNaming

    /// <summary>
    /// These specs assume that a PokerStars client is installed and the following:
    ///     - a HandHistoryDirectory is defined in the user.ini
    /// </summary>
    public abstract class PokerStarsDetectiveSpecs
    {
        protected static PokerStarsDetective _sut;

        Establish specContext = () => _sut = new PokerStarsDetective();

        [Subject(typeof(PokerStarsDetective), "Investigate HandHistoryDirectory")]
        public class when_PokerStars_saves_the_user_ini_to_the_ProgrammFiles_subdirectory_and_we_investigate : PokerStarsDetectiveSpecs
        {
            Because of = () => _sut.Investigate();

            It should_detect_that_it_is_installed = () => _sut.PokerRoomIsInstalled.ShouldBeTrue();

            It should_detect_the_hand_history_directory = () => _sut.DetectedHandHistoryDirectory.ShouldBeTrue();

            It the_HandHistory_should_contain_PokerStars_slash_HandHistory = () => _sut.HandHistoryDirectory.ShouldContain(@"PokerStars\HandHistory\");
        }

        [Subject(typeof(FullTiltPokerDetective), "DetectPreferredSeats")]
        public class given___SeatPref_eq_1_2_3_4_5_neg1_6__ : PokerStarsDetectiveSpecs
        {
            Because of = () => _sut.DetectPreferredSeats(" SeatPref=1 2 3 4 5 -1 6");

            It should_detect_the_preferred_Seats = () => _sut.DetectedPreferredSeats.ShouldBeTrue();

            It the_preferred_Seat_for_2_total_players_should_be_2 = () => _sut.PreferredSeats[2].ShouldEqual(2);

            It the_preferred_Seat_for_6_total_players_should_be_3 = () => _sut.PreferredSeats[6].ShouldEqual(3);

            It the_preferred_Seat_for_8_total_players_should_be_4 = () => _sut.PreferredSeats[8].ShouldEqual(4);

            It the_preferred_Seat_for_9_total_players_should_be_5 = () => _sut.PreferredSeats[9].ShouldEqual(5);

            It the_preferred_Seat_for_10_total_players_should_be_6 = () => _sut.PreferredSeats[10].ShouldEqual(6);

            It the_preferred_Seat_for_4_total_players_should_be_0 = () => _sut.PreferredSeats[4].ShouldEqual(0);

            It the_preferred_Seat_for_7_total_players_should_be_7 = () => _sut.PreferredSeats[7].ShouldEqual(7);
        }

        [Subject(typeof(PokerStarsDetective), "DetectPreferredSeats")]
        public class given_settings_without_preferred_seats : PokerStarsDetectiveSpecs
        {
            Because of = () => _sut.DetectPreferredSeats("settings without preferred seats");

            It should_not_have_detected_the_preferred_Seats = () => _sut.DetectedPreferredSeats.ShouldBeFalse();
        }
    }
}