namespace PokerTell.LiveTracker.Tests.ViewModels
{
    using System;
    using System.Collections.Generic;

    using Infrastructure.Enumerations.PokerHand;
    using Infrastructure.Interfaces;
    using Infrastructure.Interfaces.PokerHand;
    using Infrastructure.Interfaces.Statistics;

    using LiveTracker.ViewModels;

    using Machine.Specifications;

    using Microsoft.Practices.Composite.Events;

    using Moq;

    using Utilities;

    using It = Machine.Specifications.It;

    // Resharper disable InconsistentNaming
    public abstract class PokerTableStatisticsViewModelSpecs
    {
        protected static Mock<IEventAggregator> _eventAggregator_Stub;

        protected static Mock<IConstructor<IPlayerStatisticsViewModel>> _playerStatisticsMake_Stub;

        protected static Mock<IDetailedStatisticsAnalyzerViewModel> _statisticsAnalyzer_Mock;

        protected static PokerTableStatisticsViewModelSut _sut;

        static Mock<IPlayerStatisticsViewModel> _playerStatisticsVM_Stub;

        Establish specContext = () => {
            _eventAggregator_Stub = new Mock<IEventAggregator>();

            _playerStatisticsVM_Stub = new Mock<IPlayerStatisticsViewModel>();
            _playerStatisticsVM_Stub
                .SetupGet(vm => vm.PlayerName)
                .Returns("someName");

            _playerStatisticsMake_Stub = new Mock<IConstructor<IPlayerStatisticsViewModel>>();
            _playerStatisticsMake_Stub
                .SetupGet(svm => svm.New).Returns(_playerStatisticsVM_Stub.Object);

            _statisticsAnalyzer_Mock = new Mock<IDetailedStatisticsAnalyzerViewModel>();

            _sut = new PokerTableStatisticsViewModelSut(_eventAggregator_Stub.Object, _playerStatisticsMake_Stub.Object, _statisticsAnalyzer_Mock.Object);
        };

        [Subject(typeof(PokerTableStatisticsViewModel), "GetPlayerStatisticsViewModelFor")]
        public class when_only_bob_and_ted_where_added : PokerTableStatisticsViewModelSpecs
        {
            const string bobsName = "bob";
            const string tedsName = "ted";

            static Mock<IPlayerStatisticsViewModel> bob_Stub;

            static Mock<IPlayerStatisticsViewModel> ted_Stub;

            Establish context = () => {
                bob_Stub = Utils.PlayerStatisticsVM_MockFor(bobsName);
                ted_Stub = Utils.PlayerStatisticsVM_MockFor(tedsName);
            };

            Because of = () => {
                _sut.Players.Add(bob_Stub.Object);
                _sut.Players.Add(ted_Stub.Object);
            };

            It should_return_bobs_viewmodel_when_passing_bobs_name = () => _sut.GetPlayerStatisticsViewModelFor(bobsName).ShouldBeTheSameAs(bob_Stub.Object);
            
            It should_return_teds_viewmodel_when_passing_teds_name = () => _sut.GetPlayerStatisticsViewModelFor(tedsName).ShouldBeTheSameAs(ted_Stub.Object);

            It should_return_null_when_passing_some_other_name = () => _sut.GetPlayerStatisticsViewModelFor("neverAdded").ShouldBeNull();
        }

        [Subject(typeof(PokerTableStatisticsViewModel), "UpdateWith")]
        public class when_its_statistics_were_updated : PokerTableStatisticsViewModelSpecs
        {
            static bool statisticsWereUpdatedWasRaised;

            static IEnumerable<IPlayerStatistics> playersStatistics_Stub;

            Establish context = () => {
                playersStatistics_Stub = new[] { Utils.PlayerStatisticsStubFor("p1"), Utils.PlayerStatisticsStubFor("p1") };
                _sut.PlayersStatisticsWereUpdated += () => statisticsWereUpdatedWasRaised = true;
            };

            Because of = () => _sut.UpdateWith(playersStatistics_Stub);

            It should_raise_PlayersStatisticsWereUpdated_event = () => statisticsWereUpdatedWasRaised.ShouldBeTrue();
        }

        [Subject(typeof(PokerTableStatisticsViewModel), "SelectedStatisticsSet")]
        public class when_the_user_selects_a_statistics_set_of_a_player : PokerTableStatisticsViewModelSpecs
        {
            static bool selectedStatisticsWasReraised;

            static Mock<IPlayerStatisticsViewModel> playerStatisticsVM_Stub;

            static Mock<IActionSequenceStatisticsSet> statisticsSet;

            Establish context = () => {
                playerStatisticsVM_Stub = _playerStatisticsVM_Stub;
                _playerStatisticsMake_Stub.SetupGet(make => make.New).Returns(playerStatisticsVM_Stub.Object);
                
                statisticsSet = new Mock<IActionSequenceStatisticsSet>();

                _sut.AddNewPlayerToPlayersIfNotFound_Invoke(null);
                _sut.UserSelectedStatisticsSet += _ => selectedStatisticsWasReraised = true;
            };

            Because of = () => playerStatisticsVM_Stub.Raise(ps => ps.SelectedStatisticsSetEvent += null, statisticsSet.Object);

            It should_initialize_the_detailed_statistics_analyzer_with_the_statistics_Set
                = () => _statisticsAnalyzer_Mock.Verify(sa => sa.InitializeWith(statisticsSet.Object));

            It should_let_me_know = () => selectedStatisticsWasReraised.ShouldBeTrue();
        }

        [Subject(typeof(PokerTableStatisticsViewModel), "Selected Player")]
        public class when_the_a_player_with_active_analyzable_PokerPlayer_is_selected : PokerTableStatisticsViewModelSpecs
        {
            const string playerName = "someName";

            static Mock<IPlayerStatistics> playerStatistics_Stub;

            static IEnumerable<IAnalyzablePokerPlayer> analyzablePokerPlayers_Stub;

            static Mock<IPlayerStatisticsViewModel> playerStatisticsVM_Stub;

            static Mock<IAnalyzablePokerPlayer> _activeAnalyzablePokerPlayer_Stub;

            Establish context = () => {
                _activeAnalyzablePokerPlayer_Stub = new Mock<IAnalyzablePokerPlayer>();
                _activeAnalyzablePokerPlayer_Stub
                    .SetupGet(ap => ap.ActionSequences)
                    .Returns(new[] { ActionSequences.HeroC });

                analyzablePokerPlayers_Stub = new[] { _activeAnalyzablePokerPlayer_Stub.Object };

                playerStatistics_Stub = new Mock<IPlayerStatistics>();
                playerStatistics_Stub
                    .SetupGet(ps => ps.FilteredAnalyzablePokerPlayers)
                    .Returns(analyzablePokerPlayers_Stub);

                playerStatisticsVM_Stub = _playerStatisticsVM_Stub;
                playerStatisticsVM_Stub
                    .SetupGet(vm => vm.PlayerStatistics)
                    .Returns(playerStatistics_Stub.Object);
                playerStatisticsVM_Stub
                    .SetupGet(vm => vm.PlayerName)
                    .Returns(playerName);
            };

            Because of = () => _sut.SelectedPlayer = playerStatisticsVM_Stub.Object;

            It should_initialize_the_detailed_statistics_analyzer_with_the_filtered_analyzable_players_of_the_selected_player_and_the_players_name
                = () => _statisticsAnalyzer_Mock.Verify(sa => sa.InitializeWith(analyzablePokerPlayers_Stub, playerName));
        }

        [Subject(typeof(PokerTableStatisticsViewModel), "Selected Player")]
        public class when_the_a_player_without_active_analyzable_PokerPlayer_is_selected : PokerTableStatisticsViewModelSpecs
        {
            const string playerName = "someName";

            static Mock<IPlayerStatistics> playerStatistics_Stub;

            static IEnumerable<IAnalyzablePokerPlayer> analyzablePokerPlayers_Stub;

            static Mock<IPlayerStatisticsViewModel> playerStatisticsVM_Stub;
            
            static Mock<IAnalyzablePokerPlayer> _activeAnalyzablePokerPlayer_Stub;

            Establish context = () => {
                _activeAnalyzablePokerPlayer_Stub = new Mock<IAnalyzablePokerPlayer>();
                _activeAnalyzablePokerPlayer_Stub
                    .SetupGet(ap => ap.ActionSequences)
                    .Returns(new[] { ActionSequences.HeroF });

                analyzablePokerPlayers_Stub = new[] { _activeAnalyzablePokerPlayer_Stub.Object };

                playerStatistics_Stub = new Mock<IPlayerStatistics>();
                playerStatistics_Stub
                    .SetupGet(ps => ps.FilteredAnalyzablePokerPlayers)
                    .Returns(analyzablePokerPlayers_Stub);

                playerStatisticsVM_Stub = _playerStatisticsVM_Stub;
                playerStatisticsVM_Stub
                    .SetupGet(vm => vm.PlayerStatistics)
                    .Returns(playerStatistics_Stub.Object);
                playerStatisticsVM_Stub
                    .SetupGet(vm => vm.PlayerName)
                    .Returns(playerName);
            };

            Because of = () => _sut.SelectedPlayer = playerStatisticsVM_Stub.Object;

            It should_not_initialize_the_detailed_statistics_analyzer_with_the_filtered_analyzable_players_of_the_selected_player_and_the_players_name
                = () => _statisticsAnalyzer_Mock.Verify(sa => sa.InitializeWith(analyzablePokerPlayers_Stub, playerName), Times.Never());
        }

        [Subject(typeof(PokerTableStatisticsViewModel), "BrowseAllHands")]
        public class when_a_PlayerStatistics_viewmodel_says_that_the_user_wants_to_browse_all_hands_of_the_player : PokerTableStatisticsViewModelSpecs
        {
            static bool browseHandsWasReraised;

            Establish context = () => {
                _sut.AddNewPlayerToPlayersIfNotFound_Invoke(null);
                _sut.UserBrowsedAllHands += _ => browseHandsWasReraised = true;
            };

            Because of = () => _playerStatisticsVM_Stub.Raise(psvm => psvm.BrowseAllMyHandsRequested += null, _playerStatisticsVM_Stub.Object);

            It should_let_me_know = () => browseHandsWasReraised.ShouldBeTrue();

            It should_browse_the_hands_of_the_player = () => _sut.BrowsedAllHandsOfPlayer.ShouldBeTrue();
        }
    }

    public class PokerTableStatisticsViewModelSut : PokerTableStatisticsViewModel
    {
        public PokerTableStatisticsViewModelSut(IEventAggregator eventAggregator, IConstructor<IPlayerStatisticsViewModel> playerStatisticsViewModelMake, IDetailedStatisticsAnalyzerViewModel detailedStatisticsAnalyzerViewModel)
            : base(eventAggregator, playerStatisticsViewModelMake, detailedStatisticsAnalyzerViewModel)
        {
        }

        // Passing null will cause the sut to make a new player statistics viewmodel
        public void AddNewPlayerToPlayersIfNotFound_Invoke(IPlayerStatisticsViewModel matchingPlayer)
        {
            AddNewPlayerToPlayersIfNotFound(matchingPlayer);
        }

        protected override void BrowseAllHandsOf(IPlayerStatisticsViewModel selectedPlayer)
        {
            base.BrowseAllHandsOf(selectedPlayer);
            BrowsedAllHandsOfPlayer = true;
        }

        public bool BrowsedAllHandsOfPlayer { get; private set; }
    }
}