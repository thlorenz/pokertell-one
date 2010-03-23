namespace PokerTell.LiveTracker.IntegrationTests.PokerRooms
{
    using Interfaces;

    using LiveTracker.PokerRooms;

    using Machine.Specifications;

    // Resharper disable InconsistentNaming

    /// <summary>
    /// These specs assume that a PokerStars client is installed and the following:
    ///     - a HandHistoryDirectory is defined in the user.ini
    /// </summary>
    public abstract class PokerStarsDetectiveSpecs
    {
        protected static IPokerRoomDetective _sut;

        Establish specContext = () => _sut = new PokerStarsDetective();

        [Subject(typeof(PokerStarsDetective), "Investigate HandHistoryDirectory")]
        public class when_PokerStars_saves_the_user_ini_to_the_ProgrammFiles_subdirectory_and_we_investigate : PokerStarsDetectiveSpecs
        {
            Because of = () => _sut.Investigate();

            It should_detect_that_it_is_installed = () => _sut.IsInstalled.ShouldBeTrue();

            It should_detect_the_hand_history_directory = () => _sut.DetectedHandHistoryDirectory.ShouldBeTrue();

            It the_HandHistory_should_contain_PokerStars_slash_HandHistory = () => _sut.HandHistoryDirectory.ShouldContain(@"PokerStars\HandHistory\");
        }
    }
}