namespace PokerTell.LiveTracker.Tests.Overlay
{
    using System;

    using Machine.Specifications;

    using Moq;

    using PokerTell.Infrastructure.Interfaces.PokerHand;
    using PokerTell.LiveTracker.Interfaces;
    using PokerTell.LiveTracker.Overlay;

    using Tools.Interfaces;
    using Tools.WPF.Interfaces;

    using It = Machine.Specifications.It;

    // Resharper disable InconsistentNaming
    public abstract class TableOverlayManagerSpecs
    {
        protected static Mock<IPokerRoomInfoLocator> _pokerRoomInfoLocator_Stub;

        protected static Mock<IPokerRoomInfo> _pokerRoomInfo_Stub;

        protected static Mock<ITableOverlayViewModel> _tableOverlay_Mock;

        protected static Mock<IOverlayToTableAttacher> _tableAttacher_Mock;

        protected static Mock<ISeatMapper> _seatMapper_Mock;

        protected static Mock<IPokerTableStatisticsViewModel> _pokerTableStatistics_Mock;

        protected static Mock<ILayoutManager> _layoutManager_Mock;

        protected static Mock<ITableOverlaySettingsViewModel> _overlaySettings_Stub;

        protected static Mock<ITableOverlayWindowManager> _tableOverlayWindow_Mock;

        protected static Mock<IGameHistoryViewModel> _gameHistory_Mock;

        protected static Mock<IConvertedPokerHand> _newHand_Stub;

        protected static TableOverlayManagerSut _sut;

        Establish specContext = () => {
            _pokerRoomInfo_Stub = new Mock<IPokerRoomInfo>();
            _pokerRoomInfoLocator_Stub = new Mock<IPokerRoomInfoLocator>();
            _pokerRoomInfoLocator_Stub
                .Setup(l => l.GetPokerRoomInfoFor(Moq.It.IsAny<string>()))
                .Returns(_pokerRoomInfo_Stub.Object);

            _tableAttacher_Mock = new Mock<IOverlayToTableAttacher>();
            _tableAttacher_Mock
                .Setup(ta => ta.InitializeWith(
                    Moq.It.IsAny<IWindowManager>(),
                    Moq.It.IsAny<IDispatcherTimer>(),
                    Moq.It.IsAny<IDispatcherTimer>(),
                    Moq.It.IsAny<IPokerRoomInfo>(),
                    Moq.It.IsAny<string>()))
                    .Returns(_tableAttacher_Mock.Object);

            _pokerTableStatistics_Mock = new Mock<IPokerTableStatisticsViewModel>();

            _seatMapper_Mock = new Mock<ISeatMapper>();
            _gameHistory_Mock = new Mock<IGameHistoryViewModel>();
            _tableOverlay_Mock = new Mock<ITableOverlayViewModel>();

            _overlaySettings_Stub = new Mock<ITableOverlaySettingsViewModel>();
            _layoutManager_Mock = new Mock<ILayoutManager>();
            _layoutManager_Mock.Setup(lm => lm.Load(Moq.It.IsAny<string>(), Moq.It.IsAny<int>())).Returns(_overlaySettings_Stub.Object);

            _newHand_Stub = new Mock<IConvertedPokerHand>();

            _tableOverlayWindow_Mock = new Mock<ITableOverlayWindowManager>();
            _sut = new TableOverlayManagerSut(
                _pokerRoomInfoLocator_Stub.Object, 
                _layoutManager_Mock.Object, 
                _seatMapper_Mock.Object, 
                _tableAttacher_Mock.Object, 
                _tableOverlay_Mock.Object,
                _tableOverlayWindow_Mock.Object);
        };

        public abstract class Ctx_NewHand : TableOverlayManagerSpecs
        {
            protected const string heroName = "hero";

            protected const int totalSeats = 2;

            protected const int showHoleCardsDuration = 1;

            protected const string pokerSite = "PokerStars";

            protected const string tableName = "some table";

            Establish context = () => {
                _newHand_Stub.SetupGet(h => h.HeroName).Returns(heroName);
                _newHand_Stub.SetupGet(h => h.TotalSeats).Returns(totalSeats);
                _newHand_Stub.SetupGet(h => h.Site).Returns(pokerSite);
                _newHand_Stub.SetupGet(h => h.TableName).Returns(tableName);

                var hero_Stub = new Mock<IConvertedPokerPlayer>();
                hero_Stub.SetupGet(p => p.Name).Returns(heroName);
                _newHand_Stub.SetupGet(h => h.Players).Returns(new[] { hero_Stub.Object });
            };
        }

        public abstract class Ctx_NewHand_HeroInSeat2 : Ctx_NewHand
        {
            protected const int seatOfHero = 2;

            Establish context = () => {
                var hero_Stub = new Mock<IConvertedPokerPlayer>();
                hero_Stub.SetupGet(p => p.Name).Returns(heroName);
                hero_Stub.SetupGet(p => p.SeatNumber).Returns(seatOfHero);
                _newHand_Stub.SetupGet(h => h.Players).Returns(new[] { hero_Stub.Object });
                _sut.SetHeroName(heroName);
            };
        }

        public abstract class Ctx_InitializedWithFirstHand : Ctx_NewHand
        {
            Establish context = () => {
                _newHand_Stub.SetupGet(h => h.HeroName).Returns(heroName);
                _sut.InitializeWith(_gameHistory_Mock.Object,
                                    _pokerTableStatistics_Mock.Object,
                                    showHoleCardsDuration,
                                    _newHand_Stub.Object);
            };
        }

        [Subject(typeof(TableOverlayManager), "InitializeWith")]
        public class when_initialized_because_the_first_new_hand_was_found : Ctx_NewHand_HeroInSeat2
        {
            Establish context = () => _sut.InitializeWith(_gameHistory_Mock.Object, 
                                                          _pokerTableStatistics_Mock.Object, 
                                                          showHoleCardsDuration, 
                                                          _newHand_Stub.Object);

            It should_determine_the_hero_name_from_the_hand = () => _sut.HeroName.ShouldEqual(heroName);

            It should_initialize_the_seat_mapper_with_the_total_seats = () => _seatMapper_Mock.Verify(sm => sm.InitializeWith(totalSeats));

            It should_request_the_overlay_settings_for_the_given_poker_site_and_total_seats_from_the_layout_manager
                = () => _layoutManager_Mock.Verify(lm => lm.Load(pokerSite, totalSeats));

            It should_initialize_the_table_overlay_with_the_given_settings
                = () => _tableOverlay_Mock.Verify(to => to.InitializeWith(_seatMapper_Mock.Object,
                                                                          _overlaySettings_Stub.Object,
                                                                          _gameHistory_Mock.Object,
                                                                          _pokerTableStatistics_Mock.Object,
                                                                          heroName,
                                                                          showHoleCardsDuration));

            It should_the_DataContext_of_the_Overlay_Window_to_the_overlay_viewmodel
                = () => _tableOverlayWindow_Mock.VerifySet(ow => ow.DataContext = _tableOverlay_Mock.Object);

            It should_show_the_table_overlay_window = () => _tableOverlayWindow_Mock.Verify(ow => ow.Show());

            It should_initialize_the_overlay_to_table_attacher_with_the_table_name__pokerroom_info_returned_by_the_locator_and_overlay_window
                = () => _tableAttacher_Mock.Verify(ta => ta.InitializeWith(_tableOverlayWindow_Mock.Object, 
                                                                           Moq.It.IsAny<IDispatcherTimer>(), 
                                                                           Moq.It.IsAny<IDispatcherTimer>(), 
                                                                           _pokerRoomInfo_Stub.Object, 
                                                                           tableName));

            It should_activate_the_overlay_to_table_attacher = () => _tableAttacher_Mock.Verify(ta => ta.Activate());

            It should_update_the_seat_mapper_with_seat_of_the_hero = () => _seatMapper_Mock.Verify(sm => sm.UpdateWith(seatOfHero));
        }

        [Subject(typeof(GameController), "New Hand")]
        public class when_updated_with_a_new_hand_hero_name_is_non_empty_and_found_in_hand : Ctx_InitializedWithFirstHand
        {
            const string board = "As Kh Qs";

            protected const int seatOfHero = 2;

            Establish context = () => {
                var hero_Stub = new Mock<IConvertedPokerPlayer>();
                hero_Stub.SetupGet(p => p.Name).Returns(heroName);
                hero_Stub.SetupGet(p => p.SeatNumber).Returns(seatOfHero);
                _newHand_Stub.SetupGet(h => h.Players).Returns(new[] { hero_Stub.Object });
                _sut.SetHeroName(heroName);
                _newHand_Stub.SetupGet(h => h.Board).Returns(board);
            };

            Because of = () => _sut.UpdateWith(_newHand_Stub.Object);

            It should_update_the_table_overlay_view_model_with_the_players_and_the_board_contained_in_the_hand
                = () => _tableOverlay_Mock.Verify(to => to.UpdateWith(_newHand_Stub.Object.Players, board));

            It should_set_the_table_name_of_the_table_attacher_to_the_one_returned_by_the_hand
                = () => _tableAttacher_Mock.VerifySet(ta => ta.TableName = tableName);

            It should_update_the_seat_mapper_with_seat_of_Hero = () => _seatMapper_Mock.Verify(sm => sm.UpdateWith(seatOfHero));
        }

        [Subject(typeof(TableOverlayManager), "New Hand")]
        public class when_updated_with_new_hand_but_HeroName_is_empty : Ctx_InitializedWithFirstHand
        {
            Establish context = () => _sut.SetHeroName(string.Empty);

            Because of = () => _sut.UpdateWith(_newHand_Stub.Object);

            It should_update_the_seat_mapper_with_0 = () => _seatMapper_Mock.Verify(sm => sm.UpdateWith(0));
        }

        [Subject(typeof(TableOverlayManager), "New Hand")]
        public class when_updated_with_new_hand_but_HeroName_is_not_found_in_players_of_the_hand : Ctx_InitializedWithFirstHand
        {
            Establish context = () => {
                var otherPlayer_Stub = new Mock<IConvertedPokerPlayer>();
                otherPlayer_Stub.SetupGet(p => p.Name).Returns("other name");
                _newHand_Stub.SetupGet(h => h.Players).Returns(new[] { otherPlayer_Stub.Object });
                _sut.SetHeroName(heroName);
            };

            Because of = () => _sut.UpdateWith(_newHand_Stub.Object);

            It should_update_the_seat_mapper_with_0 = () => _seatMapper_Mock.Verify(sm => sm.UpdateWith(0));
        }

        [Subject(typeof(TableOverlayManager), "Table closed")]
        public class when_the_overlay_table_attacher_says_that_the_table_is_closed : Ctx_InitializedWithFirstHand
        {
            static bool tableClosedReraised;

            Establish context = () => _sut.TableClosed += () => tableClosedReraised = true;

            Because of = () => _tableAttacher_Mock.Raise(ta => ta.TableClosed += null);

            It should_reraise_TableClosed = () => tableClosedReraised.ShouldBeTrue();
        }

        [Subject(typeof(TableOverlayManager), "Dispose")]
        public class when_it_is_disposed : Ctx_InitializedWithFirstHand
        {
            Because of = () => _sut.Dispose();

            It should_dispose_the_table_overlay_window_manager = () => _tableOverlayWindow_Mock.Verify(to => to.Dispose());

            It should_dispose_the_table_attacher = () => _tableAttacher_Mock.Verify(to => to.Dispose());
        }

        [Subject(typeof(TableOverlayManager), "Save Overlay Settings")]
        public class when_the_overlay_settings_request_to_be_saved : Ctx_InitializedWithFirstHand
        {
            Because of = () => _overlaySettings_Stub.Raise(os => os.SaveChanges += null);

            It should_tell_the_layout_manager_to_save_the_settings_for_the_PokerSite
                = () => _layoutManager_Mock.Verify(lm => lm.Save(_overlaySettings_Stub.Object, pokerSite));
        }

        [Subject(typeof(TableOverlayManager), "Revert Overlay Settings")]
        public class when_the_overlay_settings_request_to_undo_the_changes : Ctx_InitializedWithFirstHand
        {
            static ITableOverlaySettingsViewModel revertedToSettings;

            static Mock<ITableOverlaySettingsViewModel> loadedSettings;

            Establish context = () => {
                loadedSettings = new Mock<ITableOverlaySettingsViewModel>();
                _layoutManager_Mock.Setup(lm => lm.Load(pokerSite, totalSeats)).Returns(loadedSettings.Object);
            };

            Because of = () => _overlaySettings_Stub.Raise(os => os.UndoChanges += null, 
                                                           new Action<ITableOverlaySettingsViewModel>(settings => revertedToSettings = settings));

            It should_load_the_settings_for_the_PokerSite_and_the_total_seats_from_the_layout_manager_again
                = () => _layoutManager_Mock.Verify(lm => lm.Load(pokerSite, totalSeats), Times.Exactly(2));

            It should_call_back_with_the_settings_returned_by_the_layout_manager
                = () => revertedToSettings.ShouldEqual(loadedSettings.Object);
        }

        [Subject(typeof(TableOverlayManager), "Show LiveStats Window")]
        public class when_the_user_requests_the_livestats_window_to_be_shown : Ctx_InitializedWithFirstHand
        {
            static bool showLiveStatsWasReraised;

            Establish context = () => _sut.ShowLiveStatsWindowRequested += () => showLiveStatsWasReraised = true;

            Because of = () => _tableOverlay_Mock.Raise(to => to.ShowLiveStatsWindowRequested += null);

            It should_let_me_know = () => showLiveStatsWasReraised.ShouldBeTrue();
        }

        [Subject(typeof(TableOverlayManager), "Show GameHistory Window")]
        public class when_the_user_requests_the_gamehistory_window_to_be_shown : Ctx_InitializedWithFirstHand
        {
            static bool showGameHistoryWasReraised;

            Establish context = () => _sut.ShowGameHistoryWindowRequested += () => showGameHistoryWasReraised = true;

            Because of = () => _tableOverlay_Mock.Raise(to => to.ShowGameHistoryWindowRequested += null);

            It should_let_me_know = () => showGameHistoryWasReraised.ShouldBeTrue();
        }

        [Subject(typeof(TableOverlayManager), "Table changed")]
        public class when_the_overlay_to_table_attacher_says_that_the_table_changed : Ctx_InitializedWithFirstHand
        {
            Because of = () => _tableAttacher_Mock.Raise(ta => ta.TableChanged += null, (string) null);

            It should_tell_the_TableOverlay_to_hide_all_players = () => _tableOverlay_Mock.Verify(to => to.HideAllPlayers());
        }
    }

    public class TableOverlayManagerSut : TableOverlayManager
    {
        public TableOverlayManagerSut(
            IPokerRoomInfoLocator pokerRoomInfoLocator, 
            ILayoutManager layoutManager, 
            ISeatMapper seatMapper, 
            IOverlayToTableAttacher overlayToTableAttacher, 
            ITableOverlayViewModel tableOverlay,
            ITableOverlayWindowManager tableOverlayWindow)
            : base(pokerRoomInfoLocator, layoutManager, seatMapper, overlayToTableAttacher, tableOverlay, tableOverlayWindow)
        {
        }

        public TableOverlayManagerSut SetHeroName(string heroName)
        {
            HeroName = heroName;
            return this;
        }
    }
}