namespace PokerTell.LiveTracker.Tests.ViewModels.Overlay
{
    using System;
    using System.Collections.Generic;
    using System.Windows;

    using Machine.Specifications;

    using Moq;

    using PokerTell.Infrastructure.Interfaces.PokerHand;
    using PokerTell.Infrastructure.Interfaces.Statistics;
    using PokerTell.LiveTracker.Interfaces;
    using PokerTell.LiveTracker.ViewModels.Overlay;

    using It = Machine.Specifications.It;

    // Resharper disable InconsistentNaming
    public abstract class PlayerOverlayViewModelSpecs
    {
        protected static Mock<ITableOverlaySettingsViewModel> _overlaySettings_Stub;

        protected static Mock<IPlayerStatusViewModel> _playerStatusVM_Mock;

        protected static Mock<IPlayerStatisticsViewModel> _playerStatisticsVM_Stub;

        protected static IPlayerOverlayViewModel _sut;

        protected const int SeatNumber = 0;

        Establish specContext = () => {
            _overlaySettings_Stub = new Mock<ITableOverlaySettingsViewModel>();
            _playerStatusVM_Mock = new Mock<IPlayerStatusViewModel>();
            _playerStatisticsVM_Stub = new Mock<IPlayerStatisticsViewModel>();

            _sut = new PlayerOverlayViewModel(_playerStatusVM_Mock.Object);
        };

        public abstract class Ctx_initialized_with_settings_and_seat_number_0 : PlayerOverlayViewModelSpecs
        {
            protected const double left = 1.0;

            protected const double top = 2.0;

            Establish context = () => {
                _overlaySettings_Stub.SetupGet(os => os.PlayerStatisticsPanelPositions).Returns(new[] { new Point(left, top) });
                _sut.InitializeWith(_overlaySettings_Stub.Object, SeatNumber);
            };
        }

        [Subject(typeof(PlayerOverlayViewModel), "InitializeWith")]
        public class when_initialized_with_settings : PlayerOverlayViewModelSpecs
        {
            static IList<Point> harringtonMPositions;

            static IList<Point> holeCardsPositions;

            const int seat = 1;

            Establish context = () => {
                harringtonMPositions = new[] { new Point(1, 1) };
                holeCardsPositions = new[] { new Point(2, 2) };
                _overlaySettings_Stub.SetupGet(os => os.HarringtonMPositions).Returns(harringtonMPositions);
                _overlaySettings_Stub.SetupGet(os => os.HoleCardsPositions).Returns(holeCardsPositions);
            };

            Because of = () => _sut.InitializeWith(_overlaySettings_Stub.Object, seat);

            It should_initialize_the_PlayerStatus_viewmodel_with_the_harringtonM_and_holecards_positions_and_the_seat_number
                = () => _playerStatusVM_Mock.Verify(ps => ps.InitializeWith(holeCardsPositions, harringtonMPositions, seat));

        }

        [Subject(typeof(PlayerOverlayViewModel), "Position")]
        public class when_initialized_with_settings_and_seat_number_0 : Ctx_initialized_with_settings_and_seat_number_0
        {
            It Left_returns_left_returned_by_settings_for_the_PlayerStatisticsPanelPositions_at_that_seat_number = () => _sut.Left.ShouldEqual(left);

            It Top_returns_top_returned_by_settings_for_the_PlayerStatisticsPanelPositions_at_that_seat_number = () => _sut.Top.ShouldEqual(top);
        }

        [Subject(typeof(PlayerOverlayViewModel), "Position")]
        public class when_initialized_with_settings_and_seat_number_0_setting_Left : Ctx_initialized_with_settings_and_seat_number_0
        {
            const double newLeft = 1.1;

            Because of = () => _sut.Left = newLeft;

            It should_set_X_on_the_settings_for_the_PlayerStatisticsPanelPositions_at_that_seat_number_to_the_new_value
                = () => _overlaySettings_Stub.Object.PlayerStatisticsPanelPositions[SeatNumber].X.ShouldEqual(newLeft);
        }

        [Subject(typeof(PlayerOverlayViewModel), "Position")]
        public class when_initialized_with_settings_and_seat_number_0_setting_Top : Ctx_initialized_with_settings_and_seat_number_0
        {
            const double newTop = 1.1;

            Because of = () => _sut.Top = newTop;

            It should_set_Y_on_the_settings_for_the_PlayerStatisticsPanelPositions_at_that_seat_number_to_the_new_value
                = () => _overlaySettings_Stub.Object.PlayerStatisticsPanelPositions[SeatNumber].Y.ShouldEqual(newTop);
        }

        [Subject(typeof(PlayerOverlayViewModel), "UpdateWith")]
        public class when_updating_with_null_values : PlayerOverlayViewModelSpecs
        {
            Because of = () => _sut.UpdateWith(null, null);

            It should_set_PlayerStatus_IsPresent_to_false = () => _playerStatusVM_Mock.VerifySet(ps => ps.IsPresent = false);
        }

        public abstract class Ctx_NonNull_ConvertedPlayer_Statistics_And_Setup_HarringtonM : PlayerOverlayViewModelSpecs
        {
            protected static Mock<IConvertedPokerPlayer> _convertedPlayer_Stub;

            protected static Mock<IPlayerStatisticsViewModel> _playerStatistics_Stub;

            protected static Mock<IHarringtonMViewModel> _harrintonM_VM_Stub;

            Establish context = () => {
                _convertedPlayer_Stub = new Mock<IConvertedPokerPlayer>();
                _playerStatistics_Stub = new Mock<IPlayerStatisticsViewModel>();
                _harrintonM_VM_Stub = new Mock<IHarringtonMViewModel>();
                _playerStatusVM_Mock.SetupGet(ps => ps.HarringtonM).Returns(_harrintonM_VM_Stub.Object);
                _playerStatusVM_Mock.SetupGet(ps => ps.IsPresent).Returns(true);
            };
        }

        [Subject(typeof(PlayerOverlayViewModel), "UpdateWith")]
        public class when_updating_with_non_null_statistics_and_converted_player : Ctx_NonNull_ConvertedPlayer_Statistics_And_Setup_HarringtonM
        {
            const int M = 1;

            Establish context = () => _convertedPlayer_Stub.SetupGet(cp => cp.MAfter).Returns(M);

            Because of = () => _sut.UpdateWith(_playerStatistics_Stub.Object, _convertedPlayer_Stub.Object);

            It should_set_PlayerStatus_IsPresent_to_true = () => _playerStatusVM_Mock.VerifySet(ps => ps.IsPresent = true);

            It should_set_PlayerStatus_HarringtonM_Value_to_MAfter_of_converted_player = () => _harrintonM_VM_Stub.VerifySet(hm => hm.Value = M);
        }

        [Subject(typeof(PlayerOverlayViewModel), "ShowHoleCardsFor")]
        public class when_told_to_show_his_holecards_for_2_seconds_after_updated_with_non_null_converted_player_with_unknown_cards :
            Ctx_NonNull_ConvertedPlayer_Statistics_And_Setup_HarringtonM
        {
            const int duration = 2;

            const string holecards = "";

            Establish context = () => {
                _convertedPlayer_Stub.SetupGet(cp => cp.Holecards).Returns(holecards);
                _sut.UpdateWith(_playerStatistics_Stub.Object, _convertedPlayer_Stub.Object);
            };

            Because of = () => _sut.ShowHoleCardsFor(duration);

            It should_not_tell_the_PlayerStatus_to_show_the_holecards_for_2_seconds =
                () => _playerStatusVM_Mock.Verify(ps => ps.ShowHoleCardsFor(duration, holecards), Times.Never());
        }

        [Subject(typeof(PlayerOverlayViewModel), "ShowHoleCardsFor")]
        public class when_told_to_show_his_holecards_for_2_seconds_after_updated_with_non_null_converted_player_with_known_cards :
            Ctx_NonNull_ConvertedPlayer_Statistics_And_Setup_HarringtonM
        {
            const int duration = 2;

            const string holecards = "As Ks";

            Establish context = () => {
                _convertedPlayer_Stub.SetupGet(cp => cp.Holecards).Returns(holecards);
                _sut.UpdateWith(_playerStatistics_Stub.Object, _convertedPlayer_Stub.Object);
            };

            Because of = () => _sut.ShowHoleCardsFor(duration);

            It should_tell_the_PlayerStatus_to_show_the_holecards_for_2_seconds =
                () => _playerStatusVM_Mock.Verify(ps => ps.ShowHoleCardsFor(duration, holecards));
        }

        [Subject(typeof(PlayerOverlayViewModel), "ShowHoleCardsFor")]
        public class when_told_to_show_his_holecards_for_2_seconds_after_updated_with_null_converted_player : PlayerOverlayViewModelSpecs
        {
            const int duration = 2;

            Establish context = () => _sut.UpdateWith(null, null);

            Because of = () => _sut.ShowHoleCardsFor(duration);

            It should_not_tell_the_PlayerStatus_to_show_the_holecards_for_2_seconds =
                () => _playerStatusVM_Mock.Verify(ps => ps.ShowHoleCardsFor(duration, Moq.It.IsAny<string>()), Times.Never());
        }
    }
}