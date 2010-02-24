namespace PokerTell.LiveTracker.Tests.ViewModels.Overlay
{
    using Interfaces;

    using LiveTracker.ViewModels.Overlay;

    using Machine.Specifications;

    // Resharper disable InconsistentNaming
    public abstract class TableOverlaySettingsViewModelSpecs
    {
        protected static ITableOverlaySettingsViewModel _sut;

        Establish specContext = () => {
            _sut = new TableOverlaySettingsViewModel();
        };

        [Subject(typeof(TableOverlaySettingsViewModel), "PreferredSeatChanged")]
        public class when_Preferred_Seat_is_set : TableOverlaySettingsViewModelSpecs
        {
            static bool wasRaised = false;

            Establish context = () => _sut.PreferredSeatChanged += () => wasRaised = true;

            Because of = () => _sut.PreferredSeat = 0;

            It should_raise_preferred_seat_changed = () => wasRaised.ShouldBeTrue();
        }
    }
}