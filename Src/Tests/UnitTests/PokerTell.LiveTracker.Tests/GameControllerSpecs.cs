namespace PokerTell.LiveTracker.Tests
{
    using System.Collections.Generic;
    using System.Linq;

    using Machine.Specifications;

    using Moq;

    using PokerTell.Infrastructure.Interfaces;
    using PokerTell.Infrastructure.Interfaces.PokerHand;
    using PokerTell.Infrastructure.Interfaces.Statistics;
    using PokerTell.Infrastructure.Services;
    using PokerTell.LiveTracker.Interfaces;

    using Tools.FunctionalCSharp;

    using It = Machine.Specifications.It;

    // Resharper disable InconsistentNaming
    public abstract class GameControllerSpecs
    {
        protected static Mock<IPlayerStatistics> _playerStatistics_Mock;

        protected static Mock<IPlayerStatisticsUpdater> _playerStatisticsUpdater_Mock;

        protected static IConstructor<IPlayerStatistics> _playerStatisticsMake;

        protected static Mock<IPokerTableStatisticsViewModel> _pokerTableStatistics_Mock;

        protected static Mock<IGameHistoryViewModel> _gameHistory_Mock;

        protected static Mock<IConvertedPokerHand> _newHand_Stub;

        protected static Mock<ILiveTrackerSettingsViewModel> _liveTrackerSettings_Stub;

        protected static Mock<IPokerTableStatisticsWindowManager> _liveStatsWindow_Mock;


        protected static Mock<ITableOverlayManager> _tableOverlayManager_Mock;

        protected static GameControllerSut _sut;

        Establish specContext = () => {
            _playerStatistics_Mock = new Mock<IPlayerStatistics>();
            _playerStatisticsUpdater_Mock = new Mock<IPlayerStatisticsUpdater>();
            _playerStatisticsMake = new Constructor<IPlayerStatistics>(() => _playerStatistics_Mock.Object);
            _pokerTableStatistics_Mock = new Mock<IPokerTableStatisticsViewModel>();
            _gameHistory_Mock = new Mock<IGameHistoryViewModel>();

            _newHand_Stub = new Mock<IConvertedPokerHand>();
            _liveTrackerSettings_Stub = new Mock<ILiveTrackerSettingsViewModel>();

            _liveStatsWindow_Mock = new Mock<IPokerTableStatisticsWindowManager>();

            _tableOverlayManager_Mock = new Mock<ITableOverlayManager>();

            _sut = new GameControllerSut(
                _gameHistory_Mock.Object, 
                _pokerTableStatistics_Mock.Object, 
                _playerStatisticsMake, 
                _playerStatisticsUpdater_Mock.Object, 
                _tableOverlayManager_Mock.Object,
                _liveStatsWindow_Mock.Object) { LiveTrackerSettings = _liveTrackerSettings_Stub.Object };
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

        public abstract class Ctx_NewHandWith_Bob_Ted_and_Jim_Bob_Is_Hero_and_Sut_IsLaunched : Ctx_NewHand
        {
            protected const string bob = "bob";

            protected const string ted = "ted";

            protected const string jim = "jim";

            protected static Mock<IPlayerStatistics> bobsStats_Mock;

            protected static Mock<IPlayerStatistics> tedsStats_Mock;

            protected static Mock<IPlayerStatistics> jimsStats_Mock;

            protected static Mock<IConvertedPokerPlayer> bob_Stub;

            protected static Mock<IConvertedPokerPlayer> ted_Stub;

            protected static Mock<IConvertedPokerPlayer> jim_Stub;

            protected static int statsMade;

            Establish newHandWithBobTedAndJim_Context = () => {
                bob_Stub = new Mock<IConvertedPokerPlayer>();
                ted_Stub = new Mock<IConvertedPokerPlayer>();
                jim_Stub = new Mock<IConvertedPokerPlayer>();

                bob_Stub.SetupGet(p => p.Name).Returns(bob);
                ted_Stub.SetupGet(p => p.Name).Returns(ted);
                jim_Stub.SetupGet(p => p.Name).Returns(jim);

                bobsStats_Mock = new Mock<IPlayerStatistics>();
                tedsStats_Mock = new Mock<IPlayerStatistics>();
                jimsStats_Mock = new Mock<IPlayerStatistics>();

                var bobsPlayerIdentity_Stub = new Mock<IPlayerIdentity>();
                var tedsPlayerIdentity_Stub = new Mock<IPlayerIdentity>();
                var jimsPlayerIdentity_Stub = new Mock<IPlayerIdentity>();

                bobsStats_Mock.Setup(s => s.InitializePlayer(Moq.It.IsAny<string>(), Moq.It.IsAny<string>())).Returns(bobsStats_Mock.Object);
                tedsStats_Mock.Setup(s => s.InitializePlayer(Moq.It.IsAny<string>(), Moq.It.IsAny<string>())).Returns(tedsStats_Mock.Object);
                jimsStats_Mock.Setup(s => s.InitializePlayer(Moq.It.IsAny<string>(), Moq.It.IsAny<string>())).Returns(jimsStats_Mock.Object);

                bobsPlayerIdentity_Stub.SetupGet(pi => pi.Name).Returns(bob);
                tedsPlayerIdentity_Stub.SetupGet(pi => pi.Name).Returns(ted);
                jimsPlayerIdentity_Stub.SetupGet(pi => pi.Name).Returns(jim);
                bobsStats_Mock.SetupGet(s => s.PlayerIdentity).Returns(bobsPlayerIdentity_Stub.Object);
                tedsStats_Mock.SetupGet(s => s.PlayerIdentity).Returns(tedsPlayerIdentity_Stub.Object);
                jimsStats_Mock.SetupGet(s => s.PlayerIdentity).Returns(jimsPlayerIdentity_Stub.Object);

                statsMade = 0;
                _playerStatisticsMake = new Constructor<IPlayerStatistics>(
                    () => statsMade.Match()
                              .With(s => s == 0, 
                                    s => {
                                        statsMade++;
                                        return bobsStats_Mock.Object;
                                    })
                              .With(s => s == 1, 
                                    s => {
                                        statsMade++;
                                        return tedsStats_Mock.Object;
                                    })
                              .With(s => s == 2, 
                                    s => {
                                        statsMade++;
                                        return jimsStats_Mock.Object;
                                    })
                              .Do());

                _newHand_Stub.SetupGet(h => h.Players).Returns(new[] { bob_Stub.Object, ted_Stub.Object, jim_Stub.Object });

            _sut = new GameControllerSut(
                _gameHistory_Mock.Object, 
                _pokerTableStatistics_Mock.Object, 
                _playerStatisticsMake, 
                _playerStatisticsUpdater_Mock.Object, 
                _tableOverlayManager_Mock.Object,
                _liveStatsWindow_Mock.Object) { LiveTrackerSettings = _liveTrackerSettings_Stub.Object };
                _sut.SetIsLaunched(true);
            };
        }

        [Subject(typeof(GameController), "New Hand, first time")]
        public class when_told_for_the_first_time_that_a_new_hand_was_found_and_the_user_does_not_want_to_see_the_overlay : Ctx_NewHand
        {
            const bool showTableOverlay = false;

            Establish context = () => _liveTrackerSettings_Stub.SetupGet(lts => lts.ShowTableOverlay).Returns(showTableOverlay);

            Because of = () => _sut.NewHand(_newHand_Stub.Object);

            It should_not_initialize_the_TableOverlayManager
                = () => _tableOverlayManager_Mock.Verify(tom => tom.InitializeWith(Moq.It.IsAny<IGameHistoryViewModel>(), 
                                                                    Moq.It.IsAny<IPokerTableStatisticsViewModel>(), 
                                                                    Moq.It.IsAny<int>(), 
                                                                    Moq.It.IsAny<IConvertedPokerHand>()), 
                                                         Times.Never());

            It should_set_IsLaunched_to_true = () => _sut.IsLaunched.ShouldBeTrue();
        }

        [Subject(typeof(GameController), "New Hand, first time")]
        public class when_told_for_the_first_time_that_a_new_hand_was_found_and_user_wants_to_see_the_overlay : Ctx_NewHand
        {
            const bool showTableOverlay = true;

            Establish context = () => _liveTrackerSettings_Stub.SetupGet(lts => lts.ShowTableOverlay).Returns(showTableOverlay);

            Because of = () => _sut.NewHand(_newHand_Stub.Object);

            It should_initialize_the_TableOverlayManager
                = () => _tableOverlayManager_Mock.Verify(tom => tom.InitializeWith(_gameHistory_Mock.Object, 
                                                                    _pokerTableStatistics_Mock.Object, 
                                                                    showHoleCardsDuration, 
                                                                    _newHand_Stub.Object));

            It should_set_IsLaunched_to_true = () => _sut.IsLaunched.ShouldBeTrue();
        }

        [Subject(typeof(GameController), "New Hand, first time")]
        public class when_told_for_the_first_time_that_a_new_hand_was_found_and_user_wants_to_see_the_livestats_window_on_startup : Ctx_NewHand
        {
            const bool showLiveStatsWindowOnStartup = true;

            Establish context =
                () => _liveTrackerSettings_Stub.SetupGet(lts => lts.ShowLiveStatsWindowOnStartup).Returns(showLiveStatsWindowOnStartup);

            Because of = () => _sut.NewHand(_newHand_Stub.Object);

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
                _sut.SetIsLaunched(true);
            };

            Because of = () => _sut.NewHand(_newHand_Stub.Object);

            // TODO: Change to not initialize TableOverlayManager again
            It should_not_initialize_the_TableOverlayManager_again
                = () => _tableOverlayManager_Mock.Verify(tom => tom.InitializeWith(Moq.It.IsAny<IGameHistoryViewModel>(), 
                                                                    Moq.It.IsAny<IPokerTableStatisticsViewModel>(), 
                                                                    Moq.It.IsAny<int>(), 
                                                                    Moq.It.IsAny<IConvertedPokerHand>()), 
                                                         Times.Never());

            It should_not_set_the_DataContext_of_the_LiveStats_window_to_the_pokertable_viewmodel_again
                = () => _liveStatsWindow_Mock.VerifySet(lsw => lsw.DataContext = _pokerTableStatistics_Mock.Object, Times.Never());

            It should_not_show_the_LiveStats_window_again = () => _liveStatsWindow_Mock.Verify(lsw => lsw.Show(), Times.Never());
        }

        [Subject(typeof(GameController), "New Hand")]
        public class when_told_that_a_new_hand_was_found_and_the_user_does_not_want_to_see_the_overlay : Ctx_NewHand
        {
            const bool showTableOverlay = false;

            Establish context = () => {
                _liveTrackerSettings_Stub.SetupGet(lts => lts.ShowTableOverlay).Returns(showTableOverlay);
                _sut.SetIsLaunched(true);
            };

            Because of = () => _sut.NewHand(_newHand_Stub.Object);

            It should_add_the_new_hand_to_the_GameHistory_viewmodel = () => _gameHistory_Mock.Verify(gh => gh.AddNewHand(_newHand_Stub.Object));

            It should_not_update_the_table_overlay_manager_with_the_players_and_the_board_contained_in_the_hand
                = () => _tableOverlayManager_Mock.Verify(tom => tom.UpdateWith(_newHand_Stub.Object), Times.Never());
        }

        [Subject(typeof(GameController), "New Hand")]
        public class when_told_about_a_new_hand_and_the_user_wants_to_see_the_table_overlay : Ctx_NewHand
        {
            const string board = "As Kh Qs";

            const bool showTableOverlay = true;

            Establish context = () => {
                _newHand_Stub.SetupGet(h => h.Board).Returns(board);
                _liveTrackerSettings_Stub.SetupGet(lts => lts.ShowTableOverlay).Returns(showTableOverlay);
                _sut.SetIsLaunched(true);
            };

            Because of = () => _sut.NewHand(_newHand_Stub.Object);

            It should_add_the_new_hand_to_the_GameHistory_viewmodel = () => _gameHistory_Mock.Verify(gh => gh.AddNewHand(_newHand_Stub.Object));

            It should_update_the_table_overlay_manager_with_the_players_and_the_board_contained_in_the_hand
                = () => _tableOverlayManager_Mock.Verify(tom => tom.UpdateWith(_newHand_Stub.Object));
        }

        [Subject(typeof(GameController), "NewHand")]
        public class when_told_about_new_hand_with_bob_ted_and_jim_and_PlayerStatistics_are_empty
            : Ctx_NewHandWith_Bob_Ted_and_Jim_Bob_Is_Hero_and_Sut_IsLaunched
        {
            Because of = () => _sut.NewHand(_newHand_Stub.Object);

            It should_add_bob_to_the_PlayerStatistics = () => _sut.PlayerStatistics.Keys.ShouldContain(bob);

            It should_add_ted_to_the_PlayerStatistics = () => _sut.PlayerStatistics.Keys.ShouldContain(ted);

            It should_add_jim_to_the_PlayerStatistics = () => _sut.PlayerStatistics.Keys.ShouldContain(jim);

            It should_initialize_bobs_statistics_with_the_site_and_his_name = () => bobsStats_Mock.Verify(s => s.InitializePlayer(bob, pokerSite));

            It should_initialize_teds_statistics_with_the_site_and_his_name = () => tedsStats_Mock.Verify(s => s.InitializePlayer(ted, pokerSite));

            It should_initialize_jims_statistics_with_the_site_and_his_name = () => jimsStats_Mock.Verify(s => s.InitializePlayer(jim, pokerSite));

            It should_update_bobs_statistics
                = () => _playerStatisticsUpdater_Mock
                            .Verify(su => su.Update(Moq.It.Is<IEnumerable<IPlayerStatistics>>(ps => ps.Any(s => s.PlayerIdentity.Name == bob))));

            It should_update_teds_statistics
                = () => _playerStatisticsUpdater_Mock
                            .Verify(su => su.Update(Moq.It.Is<IEnumerable<IPlayerStatistics>>(ps => ps.Any(s => s.PlayerIdentity.Name == ted))));

            It should_update_jims_statistics
                = () => _playerStatisticsUpdater_Mock
                            .Verify(su => su.Update(Moq.It.Is<IEnumerable<IPlayerStatistics>>(ps => ps.Any(s => s.PlayerIdentity.Name == jim))));
        }

        [Subject(typeof(GameController), "NewHand")]
        public class when_told_about_new_hand_with_bob_ted_and_jim_and_PlayerStatistics_contain_bob_and_ted
            : Ctx_NewHandWith_Bob_Ted_and_Jim_Bob_Is_Hero_and_Sut_IsLaunched
        {
            Establish context = () => {
                _sut.PlayerStatistics.Add(bob, bobsStats_Mock.Object);
                _sut.PlayerStatistics.Add(ted, tedsStats_Mock.Object);
                statsMade = 2;
            };

            Because of = () => _sut.NewHand(_newHand_Stub.Object);

            It should_add_jim_to_the_PlayerStatistics = () => _sut.PlayerStatistics.Keys.ShouldContain(jim);

            It should_not_initialize_bobs_statistics_with_the_site_and_his_name_again =
                () => bobsStats_Mock.Verify(s => s.InitializePlayer(bob, pokerSite), Times.Never());

            It should_not_initialize_teds_statistics_with_the_site_and_his_name_again =
                () => tedsStats_Mock.Verify(s => s.InitializePlayer(ted, pokerSite), Times.Never());

            It should_initialize_jims_statistics_with_the_site_and_his_name = () => jimsStats_Mock.Verify(s => s.InitializePlayer(jim, pokerSite));

            It should_update_bobs_statistics
                = () => _playerStatisticsUpdater_Mock
                            .Verify(su => su.Update(Moq.It.Is<IEnumerable<IPlayerStatistics>>(ps => ps.Any(s => s.PlayerIdentity.Name == bob))));

            It should_update_teds_statistics
                = () => _playerStatisticsUpdater_Mock
                            .Verify(su => su.Update(Moq.It.Is<IEnumerable<IPlayerStatistics>>(ps => ps.Any(s => s.PlayerIdentity.Name == ted))));

            It should_update_jims_statistics
                = () => _playerStatisticsUpdater_Mock
                            .Verify(su => su.Update(Moq.It.Is<IEnumerable<IPlayerStatistics>>(ps => ps.Any(s => s.PlayerIdentity.Name == jim))));
        }

        [Subject(typeof(GameController), "NewHand")]
        public class when_told_about_new_hand_with_only_bob_and_ted_and_PlayerStatistics_contain_bob_ted_and_jim
            : Ctx_NewHandWith_Bob_Ted_and_Jim_Bob_Is_Hero_and_Sut_IsLaunched
        {
            Establish context = () => {
                _sut.PlayerStatistics.Add(bob, bobsStats_Mock.Object);
                _sut.PlayerStatistics.Add(ted, tedsStats_Mock.Object);
                _sut.PlayerStatistics.Add(jim, jimsStats_Mock.Object);

                // jim left the table
                _newHand_Stub.SetupGet(h => h.Players).Returns(new[] { bob_Stub.Object, ted_Stub.Object });
            };

            Because of = () => _sut.NewHand(_newHand_Stub.Object);

            It should_not_remove_jim_from_the_PlayerStatistics = () => _sut.PlayerStatistics.Keys.ShouldContain(jim);

            It should_update_bobs_statistics
                = () => _playerStatisticsUpdater_Mock
                            .Verify(su => su.Update(Moq.It.Is<IEnumerable<IPlayerStatistics>>(ps => ps.Any(s => s.PlayerIdentity.Name == bob))));

            It should_update_teds_statistics
                = () => _playerStatisticsUpdater_Mock
                            .Verify(su => su.Update(Moq.It.Is<IEnumerable<IPlayerStatistics>>(ps => ps.Any(s => s.PlayerIdentity.Name == ted))));

            It should_not_update_jims_statistics
                = () => _playerStatisticsUpdater_Mock
                            .Verify(su => su.Update(Moq.It.Is<IEnumerable<IPlayerStatistics>>(ps => ps.Any(s => s.PlayerIdentity.Name == jim))), 
                                    Times.Never());
        }

        [Subject(typeof(GameController), "PlayerStatisticsUpdater finished")]
        public class when_the_player_statistics_updater_says_that_he_finished_updating_the_statistics : GameControllerSpecs
        {
            static IEnumerable<IPlayerStatistics> playerStatistics_Stub;

            Establish context = () => playerStatistics_Stub = new[] { new Mock<IPlayerStatistics>().Object };

            Because of = () => _playerStatisticsUpdater_Mock.Raise(u => u.FinishedUpdatingPlayerStatistics += null, playerStatistics_Stub);

            It should_update_the_pokertable_statistics_with_the_passed_player_statistics
                = () => _pokerTableStatistics_Mock.Verify(ts => ts.UpdateWith(playerStatistics_Stub));
        }

        [Subject(typeof(GameController), "Table closed")]
        public class when_the_overlay_table_manager_says_that_the_table_is_closed : GameControllerSpecs
        {
            static bool shuttingDownRaised;

            Establish context = () => _sut.ShuttingDown += () => shuttingDownRaised = true;

            Because of = () => _tableOverlayManager_Mock.Raise(tom => tom.TableClosed += null);

            It should_dispose_the_table_overlay_manager = () => _tableOverlayManager_Mock.Verify(tom => tom.Dispose());

            It should_dispose_the_live_stats_window_manager = () => _liveStatsWindow_Mock.Verify(ls => ls.Dispose());

            It should_raise_ShuttingDown = () => shuttingDownRaised.ShouldBeTrue();
        }

        protected class GameControllerSut : GameController
        {
            public GameControllerSut(
                IGameHistoryViewModel gameHistory, 
                IPokerTableStatisticsViewModel pokerTableStatistics, 
                IConstructor<IPlayerStatistics> playerStatisticsMake, 
                IPlayerStatisticsUpdater playerStatisticsUpdater, 
                ITableOverlayManager tableOverlayManager,
                IPokerTableStatisticsWindowManager pokerTableStatisticsWindowManager)
                : base(gameHistory, pokerTableStatistics, playerStatisticsMake, playerStatisticsUpdater, tableOverlayManager, pokerTableStatisticsWindowManager)
            {
            }

            public GameControllerSut SetIsLaunched(bool isLaunched)
            {
                IsLaunched = isLaunched;
                return this;
            }
        }
    }
}