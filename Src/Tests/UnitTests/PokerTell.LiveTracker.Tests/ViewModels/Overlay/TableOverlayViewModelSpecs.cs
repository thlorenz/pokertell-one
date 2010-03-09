namespace PokerTell.LiveTracker.Tests.ViewModels.Overlay
{
    using System;
    using System.Linq;

    using Machine.Specifications;

    using Moq;

    using PokerTell.Infrastructure.Interfaces.PokerHand;
    using PokerTell.Infrastructure.Interfaces.Statistics;
    using PokerTell.Infrastructure.Services;
    using PokerTell.LiveTracker.Interfaces;
    using PokerTell.LiveTracker.ViewModels.Overlay;

    using It = Machine.Specifications.It;

    // Resharper disable InconsistentNaming
    public abstract class TableOverlayViewModelSpecs
    {
        protected static Mock<IOverlaySettingsAidViewModel> _overlaySettingsAidVM_Mock;

        protected static Mock<IPokerTableStatisticsViewModel> _pokerTableStatisticsVM_Stub;

        protected static Mock<ITableOverlaySettingsViewModel> _tableOverlaySettingsVM_Stub;

        protected static Mock<IGameHistoryViewModel> _gameHistoryVM_Stub;

        protected static Mock<IPlayerOverlayViewModel> _playerOverlay_0_VM_Mock;

        protected static Mock<IPlayerOverlayViewModel> _playerOverlay_1_VM_Mock;

        protected static Mock<IOverlayBoardViewModel> _boardVM_Mock;

        protected static ITableOverlayViewModel _sut;

        protected static Mock<ISeatMapper> _seatMapper_Stub;

        protected static int ShowHoleCardsDuration;

        Establish specContext = () => {
            _boardVM_Mock = new Mock<IOverlayBoardViewModel>();
            _overlaySettingsAidVM_Mock = new Mock<IOverlaySettingsAidViewModel>();
            _gameHistoryVM_Stub = new Mock<IGameHistoryViewModel>();
            _pokerTableStatisticsVM_Stub = new Mock<IPokerTableStatisticsViewModel>();
            _tableOverlaySettingsVM_Stub = new Mock<ITableOverlaySettingsViewModel>();
            _playerOverlay_0_VM_Mock = new Mock<IPlayerOverlayViewModel>();
            _playerOverlay_1_VM_Mock = new Mock<IPlayerOverlayViewModel>();

            _seatMapper_Stub = new Mock<ISeatMapper>();
            _seatMapper_Stub.Setup(sm => sm.Map(0, 0)).Returns(0);
            _seatMapper_Stub.Setup(sm => sm.Map(1, 0)).Returns(1);
            _seatMapper_Stub.Setup(sm => sm.Map(2, 0)).Returns(2);

            ShowHoleCardsDuration = 1;

            int playerOverlaysCreated = 0;
            playerOverlaysConstructor = new Constructor<IPlayerOverlayViewModel>(() => 
            {
                if (playerOverlaysCreated == 0)
                {
                    playerOverlaysCreated++;
                    return _playerOverlay_0_VM_Mock.Object;
                }
                else
                {
                    return _playerOverlay_1_VM_Mock.Object;
                }
            });
            _sut = new TableOverlayViewModel(_boardVM_Mock.Object, _overlaySettingsAidVM_Mock.Object, playerOverlaysConstructor);
        };

        static Constructor<IPlayerOverlayViewModel> playerOverlaysConstructor;

        public abstract class Ctx_Ted_In_Seat1 : TableOverlayViewModelSpecs
        {
            protected const string tedsName = "ted";

            protected static Mock<IConvertedPokerPlayer> _ted;

            protected static IConvertedPokerPlayer[] _convertedPlayers;

            Establish context = () => {
                _ted = new Mock<IConvertedPokerPlayer>();
                _ted.SetupGet(t => t.SeatNumber).Returns(1);
                _ted.SetupGet(t => t.Name).Returns(tedsName);
                _convertedPlayers = new[] { _ted.Object };
                _tableOverlaySettingsVM_Stub.SetupGet(os => os.TotalSeats).Returns(2);
                _sut.InitializeWith(_seatMapper_Stub.Object, 
                                _tableOverlaySettingsVM_Stub.Object, 
                                _gameHistoryVM_Stub.Object, 
                                _pokerTableStatisticsVM_Stub.Object, 
                                ShowHoleCardsDuration);
            };
        }

        public abstract class Ctx_Ted_in_Seat1_and_Bob_in_Seat2 : Ctx_Ted_In_Seat1
        {
            protected const string bobsName = "bob";

            protected static Mock<IConvertedPokerPlayer> _bob;

            Establish context = () => {
                _bob = new Mock<IConvertedPokerPlayer>();
                _bob.SetupGet(b => b.SeatNumber).Returns(2);
                _bob.SetupGet(b => b.Name).Returns(bobsName);
                _convertedPlayers = new[] { _ted.Object, _bob.Object };
                _tableOverlaySettingsVM_Stub.SetupGet(os => os.TotalSeats).Returns(2);
                _sut.InitializeWith(_seatMapper_Stub.Object, 
                                _tableOverlaySettingsVM_Stub.Object, 
                                _gameHistoryVM_Stub.Object, 
                                _pokerTableStatisticsVM_Stub.Object, 
                                ShowHoleCardsDuration);
            };
        }

        [Subject(typeof(TableOverlayViewModel), "InitializedWith")]
        public class when_initialized_with_overlay_settings_for_6_total_seats : TableOverlayViewModelSpecs
        {
            Establish context = () => {
                _tableOverlaySettingsVM_Stub.SetupGet(os => os.TotalSeats).Returns(6);
                _sut.InitializeWith(_seatMapper_Stub.Object, 
                                _tableOverlaySettingsVM_Stub.Object, 
                                _gameHistoryVM_Stub.Object, 
                                _pokerTableStatisticsVM_Stub.Object, 
                                ShowHoleCardsDuration);
            };

            It should_create_6_Player_Overlays = () => _sut.PlayerOverlays.Count.ShouldEqual(6);
        }

        [Subject(typeof(TableOverlayViewModel), "InitializeWith")]
        public class when_initialized_with_2_players : Ctx_Ted_in_Seat1_and_Bob_in_Seat2
        {
            It should_initialize_player0_Overlay_with_the_overlay_settings_and_Seat_0
                = () => _playerOverlay_0_VM_Mock.Verify(p => p.InitializeWith(_tableOverlaySettingsVM_Stub.Object, 0));

            It should_initialize_player1_Overlay_with_the_overlay_settings_and_Seat_1
                = () => _playerOverlay_1_VM_Mock.Verify(p => p.InitializeWith(_tableOverlaySettingsVM_Stub.Object, 1));

            It should_initialize_the_overlay_setiings_aid_with_the_overlay_settings
                = () => _overlaySettingsAidVM_Mock.Verify(sa => sa.InitializeWith(_tableOverlaySettingsVM_Stub.Object));
        }

        [Subject(typeof(TableOverlayViewModel), "UpdateWith, no preferred Seat")]
        public class when_0_players_are_passed : TableOverlayViewModelSpecs
        {
            static Exception exception;

            Because of = () => exception = Catch.Exception(() => _sut.UpdateWith(Enumerable.Empty<IConvertedPokerPlayer>(), string.Empty));

            It should_throw_an_ArgumentException = () => exception.ShouldBeOfType(typeof(ArgumentException));
        }

        [Subject(typeof(TableOverlayViewModel), "UpdateWith, no preferred Seat")]
        public class when_ted_in_seat_1_is_passed_and_his_statistics_are_available : Ctx_Ted_In_Seat1
        {
            protected static Mock<IPlayerStatisticsViewModel> tedsStatisticsVM_Stub;

            Establish context = () => {
                tedsStatisticsVM_Stub = new Mock<IPlayerStatisticsViewModel>();
                _pokerTableStatisticsVM_Stub
                    .Setup(ts => ts.GetPlayerStatisticsViewModelFor(tedsName))
                    .Returns(tedsStatisticsVM_Stub.Object);
            };

            Because of = () => _sut.UpdateWith(_convertedPlayers, null);

            It should_update_playeroverlay_0_with_teds_converted_Player
                = () => _playerOverlay_0_VM_Mock.Verify(po => po.UpdateWith(Moq.It.IsAny<IPlayerStatisticsViewModel>(), _ted.Object));

            It should_update_playeroverlay_0_with_teds_statisticsVM_as_returned_from_the_tablestatisticsVM_for_his_Name
                = () => _playerOverlay_0_VM_Mock.Verify(po => po.UpdateWith(tedsStatisticsVM_Stub.Object, Moq.It.IsAny<IConvertedPokerPlayer>()));

            It should_update_playeroverlay_1_with_null_for_statistics_and_null_for_converted_Player
                = () => _playerOverlay_1_VM_Mock.Verify(po => po.UpdateWith(null, null));
        }

        [Subject(typeof(TableOverlayViewModel), "UpdateWith, preferred Seat")]
        public class when_ted_in_seat_1_is_passed_and_the_seatMapper_maps_his_seat_to_seat_2 : Ctx_Ted_In_Seat1
        {
            protected static Mock<IPlayerStatisticsViewModel> tedsStatisticsVM_Stub;

            Establish context = () => {
                tedsStatisticsVM_Stub = new Mock<IPlayerStatisticsViewModel>();
                _pokerTableStatisticsVM_Stub
                    .Setup(ts => ts.GetPlayerStatisticsViewModelFor(tedsName))
                    .Returns(tedsStatisticsVM_Stub.Object);
                _seatMapper_Stub.Setup(sm => sm.Map(1, Moq.It.IsAny<int>())).Returns(2);
            };

            Because of = () => _sut.UpdateWith(_convertedPlayers, null);

            It should_update_playeroverlay_1_with_teds_converted_Player
                = () => _playerOverlay_1_VM_Mock.Verify(po => po.UpdateWith(Moq.It.IsAny<IPlayerStatisticsViewModel>(), _ted.Object));
        }

        [Subject(typeof(TableOverlayViewModel), "UpdateWith, no preferred Seat")]
        public class when_ted_in_seat_1_is_passed_and_his_statistics_are_not_available : Ctx_Ted_In_Seat1
        {
            Establish context = () => _pokerTableStatisticsVM_Stub
                                          .Setup(ts => ts.GetPlayerStatisticsViewModelFor(tedsName))
                                          .Returns<IPlayerStatisticsViewModel>(null);

            Because of = () => _sut.UpdateWith(_convertedPlayers, null);

            It should_update_playeroverlay_0_with_null_for_statistics_and_null_for_converted_Player
                = () => _playerOverlay_0_VM_Mock.Verify(po => po.UpdateWith(null, null));
        }

        [Subject(typeof(TableOverlayViewModel), "UpdateWith")]
        public class when_ted_in_seat_1_showed_his_cards_and_the_board_is_non_empty : Ctx_Ted_In_Seat1
        {
            const string board = "Ad Ks Qh";

            Establish context = () => _ted.SetupGet(t => t.Holecards).Returns("AsAh");

            Because of = () => _sut.UpdateWith(_convertedPlayers, board);

            It should_tell_the_BoardViewModel_to_show_the_board_for_some_time
                = () => {
                    _boardVM_Mock.Verify(b => b.UpdateWith(board));
                    _boardVM_Mock.Verify(b => b.HideBoardAfter(ShowHoleCardsDuration));
                };

            It should_tell_teds_PlayerOverlayVM_to_show_his_holecards
                = () => _playerOverlay_0_VM_Mock.Verify(po => po.ShowHoleCardsFor(ShowHoleCardsDuration));
        }

        [Subject(typeof(TableOverlayViewModel), "UpdateWith")]
        public class when_ted_in_seat_1_showed_his_cards_but_the_board_is_empty : Ctx_Ted_In_Seat1
        {
            const string board = "";

            Establish context = () => _ted.SetupGet(t => t.Holecards).Returns("AsAh");

            Because of = () => _sut.UpdateWith(_convertedPlayers, board);

            It should_not_tell_the_BoardViewModel_to_show_the_board_for_some_time
                = () => {
                    _boardVM_Mock.Verify(b => b.UpdateWith(board), Times.Never());
                    _boardVM_Mock.Verify(b => b.HideBoardAfter(ShowHoleCardsDuration), Times.Never());
                };

            It should_not_tell_teds_PlayerOverlayVM_to_show_his_holecards
                = () => _playerOverlay_0_VM_Mock.Verify(po => po.ShowHoleCardsFor(ShowHoleCardsDuration), Times.Never());
        }

        [Subject(typeof(TableOverlayViewModel), "UpdateWith")]
        public class when_the_board_is_non_empty_but_no_player_showed_his_cards : Ctx_Ted_in_Seat1_and_Bob_in_Seat2
        {
            const string board = "As Kh Qd";

            Establish context = () => {
                _ted.SetupGet(t => t.Holecards).Returns(string.Empty);
                _bob.SetupGet(b => b.Holecards).Returns(string.Empty);
            };

            Because of = () => _sut.UpdateWith(_convertedPlayers, board);

            It should_not_tell_the_BoardViewModel_to_show_the_board_for_some_time
                = () => {
                    _boardVM_Mock.Verify(b => b.UpdateWith(board), Times.Never());
                    _boardVM_Mock.Verify(b => b.HideBoardAfter(ShowHoleCardsDuration), Times.Never());
                };

            It should_not_tell_teds_PlayerOverlayVM_to_show_his_holecards
                = () => _playerOverlay_0_VM_Mock.Verify(po => po.ShowHoleCardsFor(ShowHoleCardsDuration), Times.Never());
        }

        [Subject(typeof(TableOverlayViewModel), "PreferredSeat")]
        public class when_preferred_seat_changed_and_ted_is_in_seat_1_and_the_seatMapper_maps_his_seat_to_seat_2 : Ctx_Ted_In_Seat1
        {
            protected static Mock<IPlayerStatisticsViewModel> tedsStatisticsVM_Stub;

            Establish context = () => {
                tedsStatisticsVM_Stub = new Mock<IPlayerStatisticsViewModel>();
                _pokerTableStatisticsVM_Stub
                    .Setup(ts => ts.GetPlayerStatisticsViewModelFor(tedsName))
                    .Returns(tedsStatisticsVM_Stub.Object);
                _sut.UpdateWith(_convertedPlayers, null);
                _seatMapper_Stub.Setup(sm => sm.Map(1, Moq.It.IsAny<int>())).Returns(2);
            };

            Because of = () => _tableOverlaySettingsVM_Stub.Raise(set => set.PreferredSeatChanged += null);

            It should_update_playeroverlay_1_with_teds_converted_Player
                = () => _playerOverlay_1_VM_Mock.Verify(po => po.UpdateWith(Moq.It.IsAny<IPlayerStatisticsViewModel>(), _ted.Object));
        }
    }
}