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

        protected static Mock<IPlayerOverlayViewModel> _playerOverlay_Ted_VM_Mock;

        protected static Mock<IPlayerOverlayViewModel> _playerOverlay_Bob_VM_Mock;

        protected static Mock<IPlayerStatusViewModel> _playerStatus_Ted_Mock;

        protected static Mock<IPlayerStatusViewModel> _playerStatus_Bob_Mock;

        protected static Mock<IOverlayBoardViewModel> _boardVM_Mock;

        protected static ITableOverlayViewModel _sut;

        protected static Mock<ISeatMapper> _seatMapper_Stub;

        protected static int _showHoleCardsDuration;

        static Constructor<IPlayerOverlayViewModel> playerOverlaysConstructor;

        Establish specContext = () => {
            _boardVM_Mock = new Mock<IOverlayBoardViewModel>();
            _overlaySettingsAidVM_Mock = new Mock<IOverlaySettingsAidViewModel>();
            _gameHistoryVM_Stub = new Mock<IGameHistoryViewModel>();
            _pokerTableStatisticsVM_Stub = new Mock<IPokerTableStatisticsViewModel>();
            _tableOverlaySettingsVM_Stub = new Mock<ITableOverlaySettingsViewModel>();

            _playerStatus_Ted_Mock = new Mock<IPlayerStatusViewModel>();
            _playerStatus_Bob_Mock = new Mock<IPlayerStatusViewModel>();
            _playerOverlay_Ted_VM_Mock = new Mock<IPlayerOverlayViewModel>();
            _playerOverlay_Ted_VM_Mock
                .SetupGet(ted => ted.PlayerStatus)
                .Returns(_playerStatus_Ted_Mock.Object);
            _playerOverlay_Bob_VM_Mock = new Mock<IPlayerOverlayViewModel>();
            _playerOverlay_Bob_VM_Mock
                .SetupGet(bob => bob.PlayerStatus)
                .Returns(_playerStatus_Bob_Mock.Object);


            _seatMapper_Stub = new Mock<ISeatMapper>();
            _seatMapper_Stub.Setup(sm => sm.Map(0, 0)).Returns(0);
            _seatMapper_Stub.Setup(sm => sm.Map(1, 0)).Returns(1);
            _seatMapper_Stub.Setup(sm => sm.Map(2, 0)).Returns(2);

            _showHoleCardsDuration = 1;

            int playerOverlaysCreated = 0;
            playerOverlaysConstructor = new Constructor<IPlayerOverlayViewModel>(() => {
                if (playerOverlaysCreated == 0)
                {
                    playerOverlaysCreated++;
                    return _playerOverlay_Ted_VM_Mock.Object;
                }

                return _playerOverlay_Bob_VM_Mock.Object;
            });
            _sut = new TableOverlayViewModel(_boardVM_Mock.Object, _overlaySettingsAidVM_Mock.Object, playerOverlaysConstructor);
        };

        public abstract class Ctx_Initialized : TableOverlayViewModelSpecs
        {
            Establish initializedContext = () => _sut.InitializeWith(_seatMapper_Stub.Object, 
                                                                     _tableOverlaySettingsVM_Stub.Object, 
                                                                     _gameHistoryVM_Stub.Object, 
                                                                     _pokerTableStatisticsVM_Stub.Object, 
                                                                     string.Empty, 
                                                                     _showHoleCardsDuration);
        }

        public abstract class Ctx_Ted_In_Seat1 : TableOverlayViewModelSpecs
        {
            protected const string tedsName = "ted";

            protected static Mock<IConvertedPokerPlayer> _ted;

            protected static IConvertedPokerPlayer[] _convertedPlayers;

            Establish tedInSeat1Context = () => {
                _ted = new Mock<IConvertedPokerPlayer>();
                _ted.SetupGet(t => t.SeatNumber).Returns(1);
                _ted.SetupGet(t => t.Name).Returns(tedsName);

                _convertedPlayers = new[] { _ted.Object };
                _playerOverlay_Ted_VM_Mock.SetupGet(po => po.PlayerName).Returns(tedsName);
                _tableOverlaySettingsVM_Stub.SetupGet(os => os.TotalSeats).Returns(2);

                _sut.InitializeWith(_seatMapper_Stub.Object, 
                                    _tableOverlaySettingsVM_Stub.Object, 
                                    _gameHistoryVM_Stub.Object, 
                                    _pokerTableStatisticsVM_Stub.Object, 
                                    string.Empty, 
                                    _showHoleCardsDuration);
            };
        }

        public abstract class Ctx_Ted_in_Seat1_and_Bob_in_Seat2_Ted_is_Hero : Ctx_Ted_In_Seat1
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
                                    tedsName, 
                                    _showHoleCardsDuration);
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
                                    string.Empty, 
                                    _showHoleCardsDuration);
            };

            It should_create_6_Player_Overlays = () => _sut.PlayerOverlays.Count.ShouldEqual(6);
        }

        [Subject(typeof(TableOverlayViewModel), "InitializeWith")]
        public class when_initialized_with_2_players : Ctx_Ted_in_Seat1_and_Bob_in_Seat2_Ted_is_Hero
        {
            It should_initialize_player0_Overlay_with_the_overlay_settings_and_Seat_0
                = () => _playerOverlay_Ted_VM_Mock.Verify(p => p.InitializeWith(_tableOverlaySettingsVM_Stub.Object, 0));

            It should_initialize_player1_Overlay_with_the_overlay_settings_and_Seat_1
                = () => _playerOverlay_Bob_VM_Mock.Verify(p => p.InitializeWith(_tableOverlaySettingsVM_Stub.Object, 1));

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
        public class when_ted_in_seat_1_is_passed : Ctx_Ted_In_Seat1
        {
            Because of = () => _sut.UpdateWith(_convertedPlayers, null);

            It should_update_playeroverlay_0_with_teds_converted_Player
                = () => _playerOverlay_Ted_VM_Mock.Verify(po => po.UpdateStatusWith(_ted.Object));

            It should_update_playeroverlay_1_status_with_null
                = () => _playerOverlay_Bob_VM_Mock.Verify(po => po.UpdateStatusWith(null));
        }

        [Subject(typeof(TableOverlayViewModel), "UpdateWith, preferred Seat")]
        public class when_ted_in_seat_1_is_passed_and_the_seatMapper_maps_his_seat_to_seat_2 : Ctx_Ted_In_Seat1
        {
            protected static Mock<IPlayerStatisticsViewModel> tedsStatisticsVM_Stub;

            Establish context = () => _seatMapper_Stub.Setup(sm => sm.Map(1, Moq.It.IsAny<int>())).Returns(2);

            Because of = () => _sut.UpdateWith(_convertedPlayers, null);

            It should_update_playeroverlay_1_status_with_teds_converted_Player
                = () => _playerOverlay_Bob_VM_Mock.Verify(po => po.UpdateStatusWith(_ted.Object));
        }

        [Subject(typeof(TableOverlayViewModel), "PlayerStatisticsWereUpdated")]
        public class when_updated_with_ted_in_seat_1_and_the_player_statistics_were_updated_and_his_statistics_are_available : Ctx_Ted_In_Seat1
        {
            protected static Mock<IPlayerStatisticsViewModel> tedsStatisticsVM_Stub;

            Establish context = () => {
                tedsStatisticsVM_Stub = new Mock<IPlayerStatisticsViewModel>();
                _pokerTableStatisticsVM_Stub
                    .Setup(ts => ts.GetPlayerStatisticsViewModelFor(tedsName))
                    .Returns(tedsStatisticsVM_Stub.Object);
                _sut.UpdateWith(_convertedPlayers, null);
            };

            Because of = () => _pokerTableStatisticsVM_Stub.Raise(pts => pts.PlayersStatisticsWereUpdated += null);

            It should_update_playeroverlay_0_with_teds_statisticsVM_as_returned_from_the_tablestatisticsVM_for_his_Name
                = () => _playerOverlay_Ted_VM_Mock.Verify(po => po.UpdateStatisticsWith(tedsStatisticsVM_Stub.Object));

            It should_update_playeroverlay_1_statistics_with_null
                = () => _playerOverlay_Bob_VM_Mock.Verify(po => po.UpdateStatisticsWith(null));
        }

        [Subject(typeof(TableOverlayViewModel), "PlayerStatisticsWereUpdated")]
        public class when_updated_with_ted_in_seat_1_is_passed_and_his_statistics_are_not_available : Ctx_Ted_In_Seat1
        {
            Establish context = () => {
                _sut.UpdateWith(_convertedPlayers, null);
                _pokerTableStatisticsVM_Stub
                    .Setup(ts => ts.GetPlayerStatisticsViewModelFor(tedsName))
                    .Returns<IPlayerStatisticsViewModel>(null);
            };

            Because of = () => _pokerTableStatisticsVM_Stub.Raise(pts => pts.PlayersStatisticsWereUpdated += null);

            It should_update_playeroverlay_0_statistics_with_null
                = () => _playerOverlay_Ted_VM_Mock.Verify(po => po.UpdateStatisticsWith(null));
        }

        [Subject(typeof(TableOverlayViewModel), "UpdateWith")]
        public class when_ted_in_seat_1_showed_his_cards_and_the_board_is_non_empty : Ctx_Ted_In_Seat1
        {
            const string board = "Ad Ks Qh";

            Establish context = () => _ted.SetupGet(t => t.Holecards).Returns("As Ah");

            Because of = () => _sut.UpdateWith(_convertedPlayers, board);

            It should_tell_the_BoardViewModel_to_show_the_board_for_some_time
                = () => {
                    _boardVM_Mock.Verify(b => b.UpdateWith(board));
                    _boardVM_Mock.Verify(b => b.HideBoardAfter(_showHoleCardsDuration));
                };

            It should_tell_teds_PlayerOverlayVM_to_show_his_holecards
                = () => _playerOverlay_Ted_VM_Mock.Verify(po => po.ShowHoleCardsFor(_showHoleCardsDuration));
        }

        [Subject(typeof(TableOverlayViewModel), "UpdateWith")]
        public class when_ted_in_seat_1_and_bob_in_seat_2_showed_his_cards_and_the_board_is_non_empty_and_ted_is_the_hero :
            Ctx_Ted_in_Seat1_and_Bob_in_Seat2_Ted_is_Hero
        {
            const string board = "Ad Ks Qh";

            Establish context = () => {
                _ted.SetupGet(t => t.Holecards).Returns("As Ah");
                _bob.SetupGet(b => b.Holecards).Returns("Ks Kh");
            };

            Because of = () => _sut.UpdateWith(_convertedPlayers, board);

            It should_tell_the_BoardViewModel_to_show_the_board_for_some_time
                = () => {
                    _boardVM_Mock.Verify(b => b.UpdateWith(board));
                    _boardVM_Mock.Verify(b => b.HideBoardAfter(_showHoleCardsDuration));
                };

            It should_not_tell_teds_PlayerOverlayVM_to_show_his_holecards
                = () => _playerOverlay_Ted_VM_Mock.Verify(po => po.ShowHoleCardsFor(_showHoleCardsDuration), Times.Never());

            It should_tell_bobs_PlayerOverlayVM_to_show_his_holecards
                = () => _playerOverlay_Bob_VM_Mock.Verify(po => po.ShowHoleCardsFor(_showHoleCardsDuration));
        }

        [Subject(typeof(TableOverlayViewModel), "UpdateWith")]
        public class when_ted_in_seat_1_cards_and_the_board_is_non_empty_but_ted_is_the_hero : Ctx_Ted_in_Seat1_and_Bob_in_Seat2_Ted_is_Hero
        {
            const string board = "Ad Ks Qh";

            Establish context = () => {
                _ted.SetupGet(t => t.Holecards).Returns("As Ah");
                _bob.SetupGet(b => b.Holecards).Returns(string.Empty);
            };

            Because of = () => _sut.UpdateWith(_convertedPlayers, board);

            It should_not_tell_the_BoardViewModel_to_show_the_board_for_some_time
                = () => {
                    _boardVM_Mock.Verify(b => b.UpdateWith(board), Times.Never());
                    _boardVM_Mock.Verify(b => b.HideBoardAfter(_showHoleCardsDuration), Times.Never());
                };

            It should_not_tell_teds_PlayerOverlayVM_to_show_his_holecards
                = () => _playerOverlay_Ted_VM_Mock.Verify(po => po.ShowHoleCardsFor(_showHoleCardsDuration), Times.Never());

            It should_not_tell_bobs_PlayerOverlayVM_to_show_his_holecards
                = () => _playerOverlay_Bob_VM_Mock.Verify(po => po.ShowHoleCardsFor(_showHoleCardsDuration), Times.Never());
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
                    _boardVM_Mock.Verify(b => b.HideBoardAfter(_showHoleCardsDuration), Times.Never());
                };

            It should_not_tell_teds_PlayerOverlayVM_to_show_his_holecards
                = () => _playerOverlay_Ted_VM_Mock.Verify(po => po.ShowHoleCardsFor(_showHoleCardsDuration), Times.Never());
        }

        [Subject(typeof(TableOverlayViewModel), "UpdateWith")]
        public class when_the_board_is_non_empty_but_no_player_showed_his_cards : Ctx_Ted_in_Seat1_and_Bob_in_Seat2_Ted_is_Hero
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
                    _boardVM_Mock.Verify(b => b.HideBoardAfter(_showHoleCardsDuration), Times.Never());
                };

            It should_not_tell_teds_PlayerOverlayVM_to_show_his_holecards
                = () => _playerOverlay_Ted_VM_Mock.Verify(po => po.ShowHoleCardsFor(_showHoleCardsDuration), Times.Never());
        }

        [Subject(typeof(TableOverlayViewModel), "PreferredSeat")]
        public class when_preferred_seat_changed_and_ted_is_in_seat_1_and_the_seatMapper_maps_his_seat_to_seat_2 : Ctx_Ted_In_Seat1
        {
            protected static Mock<IPlayerStatisticsViewModel> tedsStatisticsVM_Stub;

            Establish context = () => {
                tedsStatisticsVM_Stub = new Mock<IPlayerStatisticsViewModel>();
                tedsStatisticsVM_Stub
                    .SetupGet(svm => svm.PlayerName).Returns(tedsName);

                _pokerTableStatisticsVM_Stub
                    .Setup(ts => ts.GetPlayerStatisticsViewModelFor(tedsName))
                    .Returns(tedsStatisticsVM_Stub.Object);
                _sut.UpdateWith(_convertedPlayers, null);
                _seatMapper_Stub.Setup(sm => sm.Map(1, Moq.It.IsAny<int>())).Returns(2);
            };

            Because of = () => _tableOverlaySettingsVM_Stub.Raise(set => set.PreferredSeatChanged += null);

            It should_update_playeroverlay_1_status_with_teds_converted_Player
                = () => _playerOverlay_Bob_VM_Mock.Verify(po => po.UpdateStatusWith(_ted.Object));

            It should_update_statistics
                = () => _playerOverlay_Bob_VM_Mock.Verify(po => po.UpdateStatisticsWith(Moq.It.IsAny<IPlayerStatisticsViewModel>()));
        }

        [Subject(typeof(TableOverlayViewModel), "ShowLiveStatsWindow Command")]
        public class when_user_executes_show_live_stats_window_command : TableOverlayViewModelSpecs
        {
            static bool showLiveStatsWindowWasRaised;

            Establish context = () => _sut.ShowLiveStatsWindowRequested += () => showLiveStatsWindowWasRaised = true;

            Because of = () => _sut.ShowLiveStatsWindowCommand.Execute(null);

            It should_let_me_know = () => showLiveStatsWindowWasRaised.ShouldBeTrue();
        }

        [Subject(typeof(TableOverlayViewModel), "ShowGameHistoryWindow Command")]
        public class when_user_executes_show_game_history_window_command : TableOverlayViewModelSpecs
        {
            static bool showGameHistoryWindowWasRaised;

            Establish context = () => _sut.ShowGameHistoryWindowRequested += () => showGameHistoryWindowWasRaised = true;

            Because of = () => _sut.ShowGameHistoryWindowCommand.Execute(null);

            It should_let_me_know = () => showGameHistoryWindowWasRaised.ShouldBeTrue();
        }

        [Subject(typeof(TableOverlayViewModel), "HideOVerlayDetailsCommand")]
        public class when_the_user_hides_the_overlay_details_while_they_are_shown : TableOverlayViewModelSpecs
        {
            Establish context = () => _sut.ShowOverlayDetails = true;

            Because of = () => _sut.HideOverlayDetailsCommand.Execute(null);

            It should_hide_the_overlay_details = () => _sut.ShowOverlayDetails.ShouldBeFalse();
        }

        [Subject(typeof(TableOverlayViewModel), "User selected statistics set")]
        public class when_the_pokertable_statistics_viewmodel_says_that_the_user_selected_a_statistics_set : Ctx_Initialized
        {
            Establish context = () => _sut.ShowOverlayDetails = false;

            Because of = () => _pokerTableStatisticsVM_Stub.Raise(pts => pts.UserSelectedStatisticsSet += null, (IActionSequenceStatisticsSet)null);

            It should_show_the_overlay_details = () => _sut.ShowOverlayDetails.ShouldBeTrue();
        }

        [Subject(typeof(TableOverlayViewModel), "User wants to browse players hands")]
        public class when_the_poker_table_statistics_viewmodel_says_that_the_user_wants_to_browse_the_players_hands : Ctx_Initialized
        {
            Establish context = () => _sut.ShowOverlayDetails = false;

            Because of = () => _pokerTableStatisticsVM_Stub.Raise(pts => pts.UserBrowsedAllHands += null, (IPlayerStatisticsViewModel)null);

            It should_show_the_overlay_details = () => _sut.ShowOverlayDetails.ShouldBeTrue();
        }

        [Subject(typeof(TableOverlayViewModel), "Hide all players")]
        public class when_told_to_hide_all_players_and_its_players_are_bob_and_ted : Ctx_Ted_in_Seat1_and_Bob_in_Seat2_Ted_is_Hero
        {
            Establish context = () => {
                _sut.PlayerOverlays.Add(_playerOverlay_Ted_VM_Mock.Object);
                _sut.PlayerOverlays.Add(_playerOverlay_Bob_VM_Mock.Object);
            };

            Because of = () => _sut.HideAllPlayers();

            It should_update_teds_status_with_null = () => _playerOverlay_Ted_VM_Mock.Verify(ted => ted.UpdateStatusWith(null));

            It should_update_bobs_status_with_null = () => _playerOverlay_Bob_VM_Mock.Verify(bob => bob.UpdateStatusWith(null));
        }


        [Subject(typeof(TableOverlayViewModel), "PlayerOverlay FilterAdjustmentRequested")]
        public class when_teds_playeroverlay_says_that_the_user_want_to_adjust_its_filter : Ctx_Ted_In_Seat1
        {
            static Mock<IPlayerStatisticsViewModel> tedsStatisticsVM_Stub;

            Establish context = () => tedsStatisticsVM_Stub = new Mock<IPlayerStatisticsViewModel>();

            Because of = () => _playerOverlay_Ted_VM_Mock.Raise(po => po.FilterAdjustmentRequested += null, tedsStatisticsVM_Stub.Object);

            It should_tell_the_PokerTableStatistics_viewmodel_to_display_the_filter_adjustment_popup_for_teds_statistics
                = () => _pokerTableStatisticsVM_Stub.Verify(pts => pts.DisplayFilterAdjustmentPopup(tedsStatisticsVM_Stub.Object));
        }
    }
}