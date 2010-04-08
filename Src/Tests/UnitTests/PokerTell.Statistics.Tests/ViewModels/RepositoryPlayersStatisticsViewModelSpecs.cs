namespace PokerTell.Statistics.Tests.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Infrastructure.Interfaces;
    using Infrastructure.Interfaces.PokerHand;
    using Infrastructure.Interfaces.Repository;
    using Infrastructure.Interfaces.Statistics;

    using Interfaces;

    using Machine.Specifications;

    using Moq;

    using Statistics.ViewModels;

    using Tools;

    using It = Machine.Specifications.It;

    // Resharper disable InconsistentNaming
    public abstract class RepositoryPlayersStatisticsViewModelSpecs
    {
        const string PlayerName = "somePlayer";

        const string FirstPlayerName = "firstName";

        const int FirstPlayerId = 0;

        const string FirstPlayerSite = "firstSite";

        const string SecondPlayerName = "secondName";

        const int SecondPlayerId = 1;

        const string SecondPlayerSite = "secondSite";

        static Mock<IPlayerStatisticsUpdater> _playerStatisticsUpdater_Mock;

        static Mock<IPlayerIdentity> _firstPlayerIdentity_Stub;

        static Mock<IPlayerIdentity> _secondPlayerIdentity_Stub;

        static Mock<IRepository> _repository_Mock;

        static Mock<IFilterPopupViewModel> _filterPopupVM_Mock;

        static Mock<IPlayerStatistics> _playerStatistics_Mock;

        static Mock<IConstructor<IPlayerStatistics>> _playerStatisticsMake_Mock;

        static Mock<IDetailedStatisticsAnalyzerViewModel> _statisticsAnalyzer_Mock;

        static Mock<IActiveAnalyzablePlayersSelector> _activePlayersSelector_Mock;

        static Mock<IPlayerStatisticsViewModel> _playerStatisticsVM_Mock;

        static RepositoryPlayersStatisticsViewModelSut _sut;

        Establish specContext = () => {
            _repository_Mock = new Mock<IRepository>();
            _repository_Mock
                .Setup(r => r.RetrieveAllPlayerIdentities())
                .Returns(new List<IPlayerIdentity>());

            _playerStatisticsUpdater_Mock = new Mock<IPlayerStatisticsUpdater>();

            _filterPopupVM_Mock = new Mock<IFilterPopupViewModel>();

            _playerStatistics_Mock = new Mock<IPlayerStatistics>();
            _playerStatistics_Mock
                .Setup(ps => ps.InitializePlayer(Moq.It.IsAny<string>(), Moq.It.IsAny<string>()))
                .Returns(_playerStatistics_Mock.Object);

            _playerStatisticsMake_Mock = new Mock<IConstructor<IPlayerStatistics>>();
            _playerStatisticsMake_Mock
                .SetupGet(psm => psm.New)
                .Returns(_playerStatistics_Mock.Object);

            _playerStatisticsVM_Mock = new Mock<IPlayerStatisticsViewModel>();
            _playerStatisticsVM_Mock
                .Setup(psvm => psvm.UpdateWith(Moq.It.IsAny<IPlayerStatistics>()))
                .Returns(_playerStatisticsVM_Mock.Object);
            _playerStatisticsVM_Mock
                .SetupGet(psvm => psvm.PlayerName)
                .Returns(PlayerName);

            _statisticsAnalyzer_Mock = new Mock<IDetailedStatisticsAnalyzerViewModel>();

            _activePlayersSelector_Mock = new Mock<IActiveAnalyzablePlayersSelector>();

            _firstPlayerIdentity_Stub = new Mock<IPlayerIdentity>();
            _firstPlayerIdentity_Stub
                .SetupGet(pi => pi.Name)
                .Returns(FirstPlayerName);
            _firstPlayerIdentity_Stub
                .SetupGet(pi => pi.Id)
                .Returns(FirstPlayerId);
            _firstPlayerIdentity_Stub
                .SetupGet(pi => pi.Site)
                .Returns(FirstPlayerSite);

            _secondPlayerIdentity_Stub = new Mock<IPlayerIdentity>();
            _secondPlayerIdentity_Stub
                .SetupGet(pi => pi.Name)
                .Returns(SecondPlayerName);
            _secondPlayerIdentity_Stub
                .SetupGet(pi => pi.Id)
                .Returns(SecondPlayerId);
            _secondPlayerIdentity_Stub
                .SetupGet(pi => pi.Site)
                .Returns(SecondPlayerSite);

        };


        public class Ctx_InstantiatedSut : RepositoryPlayersStatisticsViewModelSpecs
        {
            Establish instantiatedContext = () => _sut = new RepositoryPlayersStatisticsViewModelSut(_repository_Mock.Object, 
                                                                                                  _playerStatisticsMake_Mock.Object, 
                                                                                                  _playerStatisticsUpdater_Mock.Object,
                                                                                                  _playerStatisticsVM_Mock.Object,
                                                                                                  _statisticsAnalyzer_Mock.Object,
                                                                                                  _activePlayersSelector_Mock.Object,
                                                                                                  _filterPopupVM_Mock.Object);
        }

        [Subject(typeof(RepositoryPlayersStatisticsViewModel), "Instantiation")]
        public class when_instantiated_and_the_repository_contains_two_playeridentities : RepositoryPlayersStatisticsViewModelSpecs
        {
            Establish context = () => {
                var playerIdentitiesContainedInRepository = new[] { _firstPlayerIdentity_Stub.Object, _secondPlayerIdentity_Stub.Object };

                _repository_Mock
                    .Setup(r => r.RetrieveAllPlayerIdentities())
                    .Returns(playerIdentitiesContainedInRepository);
            };

            Because of = () => _sut = new RepositoryPlayersStatisticsViewModelSut(_repository_Mock.Object,
                                                                               _playerStatisticsMake_Mock.Object,
                                                                               _playerStatisticsUpdater_Mock.Object,
                                                                               _playerStatisticsVM_Mock.Object,
                                                                               _statisticsAnalyzer_Mock.Object,
                                                                               _activePlayersSelector_Mock.Object,
                                                                               _filterPopupVM_Mock.Object);

            It should_retrieve_all_playeridentities_from_the_repository = () => _repository_Mock.Verify(r => r.RetrieveAllPlayerIdentities());

            It should_add_the_first_returned_player_identity_to_its_PlayerIdentities = () => _sut.PlayerIdentities.ShouldContain(_firstPlayerIdentity_Stub.Object);
            
            It should_add_the_second_returned_player_identity_to_its_PlayerIdentities = () => _sut.PlayerIdentities.ShouldContain(_secondPlayerIdentity_Stub.Object);
        }

        [Subject(typeof(RepositoryPlayersStatisticsViewModel), "Select PlayerIdentity")]
        public class when_the_user_selects_a_player_identity : Ctx_InstantiatedSut
        {
            static IEnumerable<IAnalyzablePokerPlayer> analyzablePlayersReturnedByRepository;

            Establish context = () => {
                analyzablePlayersReturnedByRepository = new[] { new Mock<IAnalyzablePokerPlayer>().Object };
                _repository_Mock
                    .Setup(r => r.FindAnalyzablePlayersWith(FirstPlayerId, 0))
                    .Returns(analyzablePlayersReturnedByRepository);
            };

            Because of = () => _sut.SelectedPlayerIdentity = _firstPlayerIdentity_Stub.Object;

            It should_create_new_PlayerStatistics = () => _playerStatisticsMake_Mock.VerifyGet(psm => psm.New);

            It should_initialize_the_PlayerStatistics_with_the_playername_and_pokersite_of_the_selected_player
                = () => _playerStatistics_Mock.Verify(ps => ps.InitializePlayer(FirstPlayerName, FirstPlayerSite));

            It should_tell_the_playerstatistics_updater_to_update_the_PlayerStatistics
                = () => _playerStatisticsUpdater_Mock.Verify(psu => psu.Update(_playerStatistics_Mock.Object));
        }

        [Subject(typeof(RepositoryPlayersStatisticsViewModel), "StatisticsUpdater is Finished")]
        public class when_the_statistics_updater_says_that_it_finished_updating_the_statistics_of_a_player : Ctx_InstantiatedSut
        {
            static IEnumerable<IAnalyzablePokerPlayer> returnedActivePlayers;

            Establish context = () => {
                returnedActivePlayers = new[] { new Mock<IAnalyzablePokerPlayer>().Object };
                _activePlayersSelector_Mock
                    .Setup(ap => ap.SelectFrom(_playerStatistics_Mock.Object))
                    .Returns(returnedActivePlayers);
            };

            Because of = () => _playerStatisticsUpdater_Mock.Raise(psu => psu.FinishedUpdatingPlayerStatistics += null, _playerStatistics_Mock.Object);

            It should_update_the_PlayerStatisticsViewModel_with_the_updated_statistics
                = () => _playerStatisticsVM_Mock.Verify(psvm => psvm.UpdateWith(_playerStatistics_Mock.Object));

            It should_assign_the_updated_PlayerStatisticsViewModel_to_its_SelectedPlayer
                = () => _sut.SelectedPlayer.ShouldEqual(_playerStatisticsVM_Mock.Object);

            It should_tell_the_active_player_selector_to_select_the_active_players_from_the_playerstatistics
                = () => _activePlayersSelector_Mock.Verify(aps => aps.SelectFrom(_playerStatistics_Mock.Object));

            It should_initialize_the_DetailedStatisticsAnalyzer_with_the_returned_active_analyzable_players_and_the_name_of_the_player
                = () => _statisticsAnalyzer_Mock.Verify(dsa => dsa.InitializeWith(returnedActivePlayers, PlayerName));
        }

        [Subject(typeof(RepositoryPlayersStatisticsViewModel), "FilterAdjustmentRequestedCommand")]
        public class when_the_user_clicks_the_Filter_Button_in_the_PokerTableStatisticsWindow_and_a_player_was_selected : Ctx_InstantiatedSut
        {
            static Mock<IAnalyzablePokerPlayersFilter> playerFilter_Stub;

            Establish context = () => {
                playerFilter_Stub = new Mock<IAnalyzablePokerPlayersFilter>();
                _playerStatisticsVM_Mock
                    .SetupGet(p => p.Filter)
                    .Returns(playerFilter_Stub.Object);

                _sut.Set_SelectedPlayer(_playerStatisticsVM_Mock.Object);
            };

            Because of = () => _sut.FilterAdjustmentRequestedCommand.Execute(null);

            It should_tell_the_filter_popup_viewmodel_to_show_the_filter_for_the_selected_player
                = () => _filterPopupVM_Mock.Verify(
                            fp => fp.ShowFilter(PlayerName,
                                                playerFilter_Stub.Object,
                                                Moq.It.IsAny<Action<string, IAnalyzablePokerPlayersFilter>>()));
        }
    }

    public class RepositoryPlayersStatisticsViewModelSut : RepositoryPlayersStatisticsViewModel
    {
        public RepositoryPlayersStatisticsViewModelSut(IRepository repository, IConstructor<IPlayerStatistics> playerStatisticsMake, IPlayerStatisticsUpdater playerStatisticsUpdater, IPlayerStatisticsViewModel playerStatisticsViewModel, IDetailedStatisticsAnalyzerViewModel detailedStatisticsAnalyzerViewModel, IActiveAnalyzablePlayersSelector activePlayersSelector, IFilterPopupViewModel filterPopupViewModel)
            : base(repository, playerStatisticsMake, playerStatisticsUpdater, playerStatisticsViewModel, detailedStatisticsAnalyzerViewModel, activePlayersSelector, filterPopupViewModel)
        {
        }

        public void Set_SelectedPlayer(IPlayerStatisticsViewModel playerStatisticsViewModel)
        {
            SelectedPlayer = playerStatisticsViewModel;
        }
    }
}