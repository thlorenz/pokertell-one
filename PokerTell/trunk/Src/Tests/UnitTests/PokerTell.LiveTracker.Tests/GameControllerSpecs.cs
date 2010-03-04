namespace PokerTell.LiveTracker.Tests
{
    using Machine.Specifications;

    using Moq;

    using PokerTell.Infrastructure.Interfaces;
    using PokerTell.Infrastructure.Interfaces.PokerHand;
    using PokerTell.Infrastructure.Interfaces.Statistics;
    using PokerTell.Infrastructure.Services;
    using PokerTell.LiveTracker.Interfaces;

    using Tools.WPF.Interfaces;

    using It = Machine.Specifications.It;

    // Resharper disable InconsistentNaming
    public abstract class GameControllerSpecs
    {
        protected static Mock<ITableOverlayViewModel> _tableOverlay_Mock;

        protected static Mock<IOverlayToTableAttacher> _tableAttacher_Mock;

        protected static Mock<IPlayerStatistics> _playerStatistics_Mock;

        protected static IConstructor<IPlayerStatistics> _playerStatisticsMake;

        protected static Mock<IPokerTableStatisticsViewModel> _pokerTableStatistics_Mock;

        protected static Mock<ISeatMapper> _seatMapper_Mock;

        protected static Mock<IGameHistoryViewModel> _gameHistory_Mock;

        protected static Mock<ILayoutManager> _layoutManager_Mock;

        protected static Mock<IConvertedPokerHand> _newHand_Stub;

        protected static Mock<ITableOverlaySettingsViewModel> _overlaySettings_Stub;

        protected static Mock<ILiveTrackerSettings> _liveTrackerSettings_Stub;

        protected static Mock<IWindowManager> _liveStatsWindow_Mock;

        protected static Mock<IWindowManager> _tableOverlayWindow_Mock;

        protected static IGameController _sut;

        Establish specContext = () => {
            _tableOverlay_Mock = new Mock<ITableOverlayViewModel>();
            _tableAttacher_Mock = new Mock<IOverlayToTableAttacher>();
            _playerStatistics_Mock = new Mock<IPlayerStatistics>();
            _playerStatisticsMake = new Constructor<IPlayerStatistics>(() => _playerStatistics_Mock.Object);
            _pokerTableStatistics_Mock = new Mock<IPokerTableStatisticsViewModel>();
            _seatMapper_Mock = new Mock<ISeatMapper>();
            _gameHistory_Mock = new Mock<IGameHistoryViewModel>();
            _tableOverlay_Mock = new Mock<ITableOverlayViewModel>();

            _overlaySettings_Stub = new Mock<ITableOverlaySettingsViewModel>();
            _layoutManager_Mock = new Mock<ILayoutManager>();
            _layoutManager_Mock.Setup(lm => lm.Load(Moq.It.IsAny<string>(), Moq.It.IsAny<int>())).Returns(_overlaySettings_Stub.Object);

            _newHand_Stub = new Mock<IConvertedPokerHand>();
            _liveTrackerSettings_Stub = new Mock<ILiveTrackerSettings>();

            _liveStatsWindow_Mock = new Mock<IWindowManager>();
            _liveStatsWindow_Mock.Setup(lsw => lsw.CreateWindow()).Returns(_liveStatsWindow_Mock.Object);

            _tableOverlayWindow_Mock = new Mock<IWindowManager>();
            _tableOverlayWindow_Mock.Setup(ow => ow.CreateWindow()).Returns(_tableOverlayWindow_Mock.Object);

            _sut = new GameController(
                _layoutManager_Mock.Object, 
                _gameHistory_Mock.Object, 
                _pokerTableStatistics_Mock.Object, 
                _playerStatisticsMake, 
                _seatMapper_Mock.Object, 
                _tableAttacher_Mock.Object, 
                _tableOverlay_Mock.Object)
                .InitializeWith(_liveStatsWindow_Mock.Object, _tableOverlayWindow_Mock.Object);

            _sut.LiveTrackerSettings = _liveTrackerSettings_Stub.Object;
        };

        public abstract class Ctx_NewHand : GameControllerSpecs
        {
            protected const string heroName = "hero";

            protected const int totalSeats = 2;

            protected const string pokerSite = "PokerStars";

            protected const int showHoleCardsDuration = 1;

            protected const string tableName = "some table";

            Establish context = () => {
                _newHand_Stub.SetupGet(h => h.HeroName).Returns(heroName);
                _newHand_Stub.SetupGet(h => h.TotalSeats).Returns(totalSeats);
                _newHand_Stub.SetupGet(h => h.Site).Returns(pokerSite);
                _newHand_Stub.SetupGet(h => h.TableName).Returns(tableName);

                var hero_Stub = new Mock<IConvertedPokerPlayer>();
                hero_Stub.SetupGet(p => p.Name).Returns(heroName);
                _newHand_Stub.SetupGet(h => h.Players).Returns(new[] { hero_Stub.Object });

                _liveTrackerSettings_Stub.SetupGet(lts => lts.ShowHoleCardsDuration).Returns(showHoleCardsDuration);
            };
        }

        [Subject(typeof(GameController), "New Hand, first time")]
        public class when_told_for_the_first_time_that_a_new_hand_was_found_and_the_user_does_not_want_to_see_the_overlay : Ctx_NewHand
        {
            const bool showTableOverlay = false;

            Establish context = () => _liveTrackerSettings_Stub.SetupGet(lts => lts.ShowTableOverlay).Returns(showTableOverlay);

            Because of = () => _sut.NewHand(_newHand_Stub.Object);

            It should_determine_the_hero_name_from_the_hand = () => _sut.HeroName.ShouldEqual(heroName);

            It should_not_initialize_the_seat_mapper_with_the_total_seats =
                () => _seatMapper_Mock.Verify(sm => sm.InitializeWith(totalSeats), Times.Never());

            It should_not_request_the_overlay_settings_for_the_given_poker_site_and_total_seats_from_the_layout_manager
                = () => _layoutManager_Mock.Verify(lm => lm.Load(pokerSite, totalSeats), Times.Never());

            It should_not_initialize_the_table_overlay_with_the_given_settings
                = () => _tableOverlay_Mock.Verify(to => to.InitializeWith(_seatMapper_Mock.Object, 
                                                                          _overlaySettings_Stub.Object, 
                                                                          _gameHistory_Mock.Object, 
                                                                          _pokerTableStatistics_Mock.Object, 
                                                                          showHoleCardsDuration), 
                                                  Times.Never());

            It should_not_initialize_the_overlay_to_table_attacher_with_the_table_name__poker_site_and_overlay_window
                = () => _tableAttacher_Mock.Verify(ta => ta.InitializeWith(_tableOverlayWindow_Mock.Object, pokerSite, tableName), Times.Never());

            It should_not_show_the_overlay_window = () => _tableOverlayWindow_Mock.Verify(ow => ow.Show(), Times.Never());

            It should_set_IsLaunched_to_true = () => _sut.IsLaunched.ShouldBeTrue();
        }

        [Subject(typeof(GameController), "New Hand, first time")]
        public class when_told_for_the_first_time_that_a_new_hand_was_found_and_user_wants_to_see_the_overlay : Ctx_NewHand
        {
            const bool showTableOverlay = true;

            Establish context = () => _liveTrackerSettings_Stub.SetupGet(lts => lts.ShowTableOverlay).Returns(showTableOverlay);

            Because of = () => _sut.NewHand(_newHand_Stub.Object);

            It should_determine_the_hero_name_from_the_hand = () => _sut.HeroName.ShouldEqual(heroName);

            It should_initialize_the_seat_mapper_with_the_total_seats = () => _seatMapper_Mock.Verify(sm => sm.InitializeWith(totalSeats));

            It should_request_the_overlay_settings_for_the_given_poker_site_and_total_seats_from_the_layout_manager
                = () => _layoutManager_Mock.Verify(lm => lm.Load(pokerSite, totalSeats));

            It should_initialize_the_table_overlay_with_the_given_settings
                = () => _tableOverlay_Mock.Verify(to => to.InitializeWith(_seatMapper_Mock.Object, 
                                                                          _overlaySettings_Stub.Object, 
                                                                          _gameHistory_Mock.Object, 
                                                                          _pokerTableStatistics_Mock.Object, 
                                                                          showHoleCardsDuration));

            It should_initialize_the_overlay_to_table_attacher_with_the_table_name__poker_site_and_overlay_window
                = () => _tableAttacher_Mock.Verify(ta => ta.InitializeWith(_tableOverlayWindow_Mock.Object, pokerSite, tableName));

            It should_create_the_overlay_window = () => _tableOverlayWindow_Mock.Verify(ow => ow.CreateWindow());

            It should_set_the_DataContext_of_the_overlay_window_to_the_overlay_viewmodel
                = () => _tableOverlayWindow_Mock.VerifySet(ow => ow.DataContext = _tableOverlay_Mock.Object);

            It should_show_the_overlay_window = () => _tableOverlayWindow_Mock.Verify(ow => ow.Show());

            It should_set_IsLaunched_to_true = () => _sut.IsLaunched.ShouldBeTrue();
        }

        [Subject(typeof(GameController), "New Hand, first time")]
        public class when_told_for_the_first_time_that_a_new_hand_was_found_and_user_wants_to_see_the_livestats_window_on_startup : Ctx_NewHand
        {
            const bool showLiveStatsWindowOnStartup = true;

            Establish context =
                () => _liveTrackerSettings_Stub.SetupGet(lts => lts.ShowLiveStatsWindowOnStartup).Returns(showLiveStatsWindowOnStartup);

            Because of = () => _sut.NewHand(_newHand_Stub.Object);

            It should_create_the_LiveStats_window = () => _liveStatsWindow_Mock.Verify(lsw => lsw.CreateWindow());

            It should_set_the_DataContext_of_the_LiveStats_window_to_the_pokertable_viewmodel
                = () => _liveStatsWindow_Mock.VerifySet(lsw => lsw.DataContext = _pokerTableStatistics_Mock.Object);

            It should_show_the_LiveStats_window = () => _liveStatsWindow_Mock.Verify(lsw => lsw.Show());
        }

        [Subject(typeof(GameController), "New Hand, first time")]
        public class when_told_for_the_first_time_that_a_new_hand_was_found_and_user_does_not_want_to_see_the_livestats_window_on_startup :
            Ctx_NewHand
        {
            const bool showLiveStatsWindowOnStartup = false;

            Establish context =
                () => _liveTrackerSettings_Stub.SetupGet(lts => lts.ShowLiveStatsWindowOnStartup).Returns(showLiveStatsWindowOnStartup);

            Because of = () => _sut.NewHand(_newHand_Stub.Object);

            It should_not_create_the_LiveStats_window = () => _liveStatsWindow_Mock.Verify(lsw => lsw.CreateWindow(), Times.Never());

            It should_not_set_the_DataContext_of_the_LiveStats_window_to_the_pokertable_viewmodel
                = () => _liveStatsWindow_Mock.VerifySet(lsw => lsw.DataContext = _pokerTableStatistics_Mock.Object, Times.Never());

            It should_not_show_the_LiveStats_window = () => _liveStatsWindow_Mock.Verify(lsw => lsw.Show(), Times.Never());
        }

        [Subject(typeof(GameController), "New Hand, not first time")]
        public class when_told_that_a_new_hand_was_found_but_not_the_first_time_and_the_user_wants_to_see_LiveStats_and_Overlay_windows : Ctx_NewHand
        {
            const bool showLiveStatsWindowOnStartup = true;

            const bool showTableOverlay = true;

            Establish context = () => {
                _liveTrackerSettings_Stub.SetupGet(lts => lts.ShowTableOverlay).Returns(showTableOverlay);
                _liveTrackerSettings_Stub.SetupGet(lts => lts.ShowLiveStatsWindowOnStartup).Returns(showLiveStatsWindowOnStartup);
                _sut.HeroName = heroName;
                _sut.IsLaunched = true;
            };

            Because of = () => _sut.NewHand(_newHand_Stub.Object);

            It should_not_initialize_the_table_overlay_again
                = () => _tableOverlay_Mock.Verify(to => to.InitializeWith(Moq.It.IsAny<ISeatMapper>(), 
                                                                          Moq.It.IsAny<ITableOverlaySettingsViewModel>(), 
                                                                          Moq.It.IsAny<IGameHistoryViewModel>(), 
                                                                          Moq.It.IsAny<IPokerTableStatisticsViewModel>(), 
                                                                          Moq.It.IsAny<int>()), 
                                                  Times.Never());

            It should_not_initialize_the_overlay_to_table_attacher_with_the_table_name__poker_site_and_overlay_window_again
                = () => _tableAttacher_Mock.Verify(ta => ta.InitializeWith(_tableOverlayWindow_Mock.Object, Moq.It.IsAny<string>(), Moq.It.IsAny<string>()), Times.Never());

            It should_not_show_the_overlay_window_again = () => _tableOverlayWindow_Mock.Verify(ow => ow.Show(), Times.Never());

            It should_not_create_the_LiveStats_window_again = () => _liveStatsWindow_Mock.Verify(lsw => lsw.CreateWindow(), Times.Never());

            It should_not_set_the_DataContext_of_the_LiveStats_window_to_the_pokertable_viewmodel_again
                = () => _liveStatsWindow_Mock.VerifySet(lsw => lsw.DataContext = _pokerTableStatistics_Mock.Object, Times.Never());

            It should_not_show_the_LiveStats_window_again = () => _liveStatsWindow_Mock.Verify(lsw => lsw.Show(), Times.Never());
        }

        [Subject(typeof(GameController), "New Hand")]
        public class when_told_about_a_new_hand
        {
            const string board = "As Kh Qs";

            Establish context = () => _newHand_Stub.SetupGet(h => h.Board).Returns(board);

            Because of = () => _sut.NewHand(_newHand_Stub.Object);

            It should_add_the_new_hand_to_the_GameHistory_viewmodel = () => _gameHistory_Mock.Verify(gh => gh.AddNewHand(_newHand_Stub.Object));

            It should_update_the_table_overlay_view_model_with_the_players_and_the_board_contained_in_the_hand
                = () => _tableOverlay_Mock.Verify(to => to.UpdateWith(_newHand_Stub.Object.Players, board));
        }

        [Subject(typeof(GameController), "NewHand")]
        public class when_told_about_new_hand_with_hero_in_seat_2 : Ctx_NewHand
        {
            const int seat = 2;

            Establish context = () => {
                var hero_Stub = new Mock<IConvertedPokerPlayer>();
                hero_Stub.SetupGet(p => p.Name).Returns(heroName);
                hero_Stub.SetupGet(p => p.SeatNumber).Returns(seat);
                _newHand_Stub.SetupGet(h => h.Players).Returns(new[] { new Mock<IConvertedPokerPlayer>().Object, hero_Stub.Object });
                _sut.HeroName = heroName;
            };

            Because of = () => _sut.NewHand(_newHand_Stub.Object);

            It should_update_the_seat_mapper_with_seat_2 = () => _seatMapper_Mock.Verify(sm => sm.UpdateWith(seat));
        }

        public abstract class Ctx_NewHandWith_Bob_Ted_and_Jim_Bob_Is_Hero_and_IsLaunched : Ctx_NewHand
        {
            protected const string bob = "bob";
            protected const string ted = "ted";
            protected const string jim = "jim";

            Establish context = () => {
                var bob_Stub = new Mock<IConvertedPokerPlayer>();
                var ted_Stub = new Mock<IConvertedPokerPlayer>();
                var jim_Stub = new Mock<IConvertedPokerPlayer>();
                bob_Stub.SetupGet(p => p.Name).Returns(bob);
                ted_Stub.SetupGet(p => p.Name).Returns(ted);
                jim_Stub.SetupGet(p => p.Name).Returns(jim);

                _newHand_Stub.SetupGet(h => h.Players).Returns(new[] { bob_Stub.Object, ted_Stub.Object, jim_Stub.Object });
                _sut.HeroName = bob;
                _sut.IsLaunched = true;
            };

            [Subject(typeof(GameController), "NewHand")]
            public class when_told_about_new_hand_with_bob_ted_and_jim_and_PlayerStatistics_are_empty 
                : Ctx_NewHandWith_Bob_Ted_and_Jim_Bob_Is_Hero_and_IsLaunched
            {
                Because of = () => _sut.NewHand(_newHand_Stub.Object);

                It should_add_bob_to_the_PlayerStatistics = () => _sut.PlayerStatistics.Keys.ShouldContain(bob);
                It should_add_ted_to_the_PlayerStatistics = () => _sut.PlayerStatistics.Keys.ShouldContain(ted);
                It should_add_jim_to_the_PlayerStatistics = () => _sut.PlayerStatistics.Keys.ShouldContain(jim);
            }
        }


    }
}