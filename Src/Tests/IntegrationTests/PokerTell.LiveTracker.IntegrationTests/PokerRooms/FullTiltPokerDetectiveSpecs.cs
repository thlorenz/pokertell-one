namespace PokerTell.LiveTracker.IntegrationTests.PokerRooms
{
    using Interfaces;

    using LiveTracker.PokerRooms;

    using Machine.Specifications;

    // Resharper disable InconsistentNaming

    /// <summary>
    /// These specs assume that a FullTiltPoker client is installed and the following:
    ///     - a HandHistoryDirectory is defined in the user.ini
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

    }
}