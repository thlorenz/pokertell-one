namespace PokerTell.LiveTracker.Tests.Overlay
{
    using System.Collections.Generic;

    using Interfaces;

    using LiveTracker.Overlay;

    using Machine.Specifications;

    using Moq;

    using It=Machine.Specifications.It;

    // Resharper disable InconsistentNaming
    public abstract class LayoutAutoConfiguratorSpecs
    {
        protected static Mock<ILayoutManager> _layoutManager_Mock;


        protected const string PokerSite = "SomeSite";

        protected static ILayoutAutoConfigurator _sut;

        Establish specContext = () => {
            _layoutManager_Mock = new Mock<ILayoutManager>();

            _sut = new LayoutAutoConfigurator(_layoutManager_Mock.Object);
        };

        [Subject(typeof(LayoutAutoConfigurator), "Configure preferred seats")]
        public class when_told_to_configure_the_preferred_seats_for_the_given_site_for_total_seats_6_it_is_seat_3_and_totalseats_9_it_is_5
            : LayoutAutoConfiguratorSpecs
        {
        static Mock<ITableOverlaySettingsViewModel> totalSeats9_OverlaySettingsVM_Mock;
        static Mock<ITableOverlaySettingsViewModel> totalSeats6_OverlaySettingsVM_Mock;
            const int totalSeats6 = 6;
            const int preferredSeatFor6 = 3;

            const int totalSeats9 = 9;
            const int preferredSeatFor9 = 5;

            Establish context = () =>
            {
                totalSeats6_OverlaySettingsVM_Mock = new Mock<ITableOverlaySettingsViewModel>();
                totalSeats9_OverlaySettingsVM_Mock = new Mock<ITableOverlaySettingsViewModel>();
                _layoutManager_Mock
                    .Setup(lm => lm.Load(PokerSite, totalSeats6))
                    .Returns(totalSeats6_OverlaySettingsVM_Mock.Object);
                _layoutManager_Mock
                    .Setup(lm => lm.Load(PokerSite, totalSeats9))
                    .Returns(totalSeats9_OverlaySettingsVM_Mock.Object);
            };

            Because of = () => 
                _sut.ConfigurePreferredSeats(PokerSite, new Dictionary<int, int> { { totalSeats6, preferredSeatFor6 }, { totalSeats9, preferredSeatFor9 } });

            It should_load_the_overlay_settings_for_the_given_site_and_6_total_seats_from_the_layout_manager 
                = () => _layoutManager_Mock.Verify(lm => lm.Load(PokerSite, totalSeats6));

            It should_set_the_preferred_seat_of_the_settings_for_6_total_seats_to_3
                = () => totalSeats6_OverlaySettingsVM_Mock.VerifySet(os => os.PreferredSeat = preferredSeatFor6);

            It should_tell_the_layout_manager_to_save_the_layout_for_the_given_site_and_6_total_seats
                = () => _layoutManager_Mock.Verify(lm => lm.Save(totalSeats6_OverlaySettingsVM_Mock.Object, PokerSite));

            It should_load_the_overlay_settings_for_the_given_site_and_9_total_seats_from_the_layout_manager
                = () => _layoutManager_Mock.Verify(lm => lm.Load(PokerSite, totalSeats9));

            It should_set_the_preferred_seat_of_the_settings_for_9_total_seats_to_5
                = () => totalSeats9_OverlaySettingsVM_Mock.VerifySet(os => os.PreferredSeat = preferredSeatFor9);

            It should_tell_the_layout_manager_to_save_the_layout_for_the_given_site_and_9_total_seats
                = () => _layoutManager_Mock.Verify(lm => lm.Save(totalSeats9_OverlaySettingsVM_Mock.Object, PokerSite));
        }
    }
}