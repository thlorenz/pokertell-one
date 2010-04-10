namespace PokerTell.Statistics.Tests
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Threading;

    using Infrastructure.Interfaces;
    using Infrastructure.Interfaces.PokerHand;
    using Infrastructure.Interfaces.Statistics;

    using Machine.Specifications;

    using Moq;

    using Tools.Interfaces;

    using UnitTests.Fakes;

    using It = Machine.Specifications.It;

    // Resharper disable InconsistentNaming
    public abstract class PlayerStatisticsUpdaterSpecs
    {
        protected static Mock<IPlayerStatistics> _bobsStats_Mock;
        protected static Mock<IPlayerStatistics> _tedsStats_Mock;
        protected static IEnumerable<IPlayerStatistics> _playerStats_Stub;

        protected static Mock<IPlayerIdentity> _bobsIdentity_Stub;
        protected static Mock<IPlayerIdentity> _tedsIdentity_Stub;

        protected static BackgroundWorkerMock _workerForCollectionUpdate_Mock;
        protected static BackgroundWorkerMock _workerForSingleStatisticsUpdate_Mock;
        protected static Mock<IConstructor<IBackgroundWorker>> _backgroundWorkerMake_Stub;

        protected static IPlayerStatisticsUpdater _sut;

        Establish specContext = () => {
            _bobsIdentity_Stub = new Mock<IPlayerIdentity>();
            _tedsIdentity_Stub = new Mock<IPlayerIdentity>();

            _bobsStats_Mock = new Mock<IPlayerStatistics>(); 
            _bobsStats_Mock
                .SetupGet(s => s.PlayerIdentity)
                .Returns(_bobsIdentity_Stub.Object);

            _tedsStats_Mock = new Mock<IPlayerStatistics>();
            _tedsStats_Mock
                .SetupGet(s => s.PlayerIdentity)
                .Returns(_tedsIdentity_Stub.Object);

            _playerStats_Stub = new[] { _bobsStats_Mock.Object, _tedsStats_Mock.Object };

            // Return collection update worker during instantiation
            _workerForCollectionUpdate_Mock = new BackgroundWorkerMock();
            _backgroundWorkerMake_Stub = new Mock<IConstructor<IBackgroundWorker>>();
            _backgroundWorkerMake_Stub
                .SetupGet(bwm => bwm.New)
                .Returns(_workerForCollectionUpdate_Mock);
 
            _sut = new PlayerStatisticsUpdater(_backgroundWorkerMake_Stub.Object);

            // Only worker created after instantiation will be for single statistic update
            _workerForSingleStatisticsUpdate_Mock = new BackgroundWorkerMock();
            _backgroundWorkerMake_Stub
                .SetupGet(bwm => bwm.New)
                .Returns(_workerForSingleStatisticsUpdate_Mock);
        };

        [Subject(typeof(PlayerStatisticsUpdater), "Update PlayerStatistics Collection")]
        public class when_told_to_update_bobs_and_teds_statistics_and_the_background_worker_is_not_busy : PlayerStatisticsUpdaterSpecs
        {
            Because of = () => _sut.Update(_playerStats_Stub);

            It should_update_bobs_statistics = () => _bobsStats_Mock.Verify(s => s.UpdateStatistics());

            It should_update_teds_statistics = () => _tedsStats_Mock.Verify(s => s.UpdateStatistics());
        }

        [Subject(typeof(PlayerStatisticsUpdater), "Update PlayerStatistics Collection")]
        public class when_told_to_update_bobs_and_teds_statistics_and_the_background_worker_is_busy : PlayerStatisticsUpdaterSpecs
        {
            Establish context = () => _workerForCollectionUpdate_Mock.IsBusy = true;

            Because of = () => _sut.Update(_playerStats_Stub);

            It should_not_update_bobs_statistics = () => _bobsStats_Mock.Verify(s => s.UpdateStatistics(), Times.Never());

            It should_not_update_teds_statistics = () => _tedsStats_Mock.Verify(s => s.UpdateStatistics(), Times.Never());
        }

        [Subject(typeof(PlayerStatisticsUpdater), "Update PlayerStatistics Collection")]
        public class when_the_background_worker_says_the_he_is_finished_with_the_update : PlayerStatisticsUpdaterSpecs
        {
            static bool isFinishedWasRaised;

            static IEnumerable<IPlayerStatistics> returnedPlayerStatistics;

            Establish context = () => _sut.FinishedUpdatingMultiplePlayerStatistics += stats => {
                isFinishedWasRaised = true;
                returnedPlayerStatistics = stats;
            };

            Because of = () => _workerForCollectionUpdate_Mock.RunWorkerCompletedInvoke(new RunWorkerCompletedEventArgs(_playerStats_Stub, null, false));

            It should_let_me_know_when_it_is_finished = () => isFinishedWasRaised.ShouldBeTrue();

            It should_include_the_updated_player_statistics_when_it_lets_me_know_that_it_is_finished = () => returnedPlayerStatistics.ShouldEqual(_playerStats_Stub);
        }

        [Subject(typeof(PlayerStatisticsUpdater), "Update PlayerStatistics")]
        public class when_told_to_update_bobs_statistics : PlayerStatisticsUpdaterSpecs
        {
            Because of = () => _sut.Update(_bobsStats_Mock.Object);

            It should_create_a_new_background_worker = () => _backgroundWorkerMake_Stub.VerifyGet(bwm => bwm.New);

            It should_update_bobs_statistics = () => _bobsStats_Mock.Verify(s => s.UpdateStatistics());

            It should_set_PlayerThatIsCurrentlyUpdated_to_bobs_identity = () => _sut.PlayerThatIsCurrentlyUpdated.ShouldEqual(_bobsIdentity_Stub.Object);
        }

        [Subject(typeof(PlayerStatisticsUpdater), "Update PlayerStatistics")]
        public class when_told_to_update_bobs_statistics_and_the_background_worker_is_busy_updating_teds_statistics : PlayerStatisticsUpdaterSpecs
        {
            Establish context = () => _sut.Update(_tedsStats_Mock.Object);

            Because of = () => _sut.Update(_bobsStats_Mock.Object);

            It should_create_a_new_background_worker = () => _backgroundWorkerMake_Stub.VerifyGet(bwm => bwm.New);

            It should_update_bobs_statistics = () => _bobsStats_Mock.Verify(s => s.UpdateStatistics());

            It should_set_PlayerThatIsCurrentlyUpdated_to_bobs_identity = () => _sut.PlayerThatIsCurrentlyUpdated.ShouldEqual(_bobsIdentity_Stub.Object);
        }

        [Subject(typeof(PlayerStatisticsUpdater), "Update PlayerStatistics")]
        public class when_the_background_worker_says_he_finished_updating_bobs_statistics_and_they_are_the_last_ones_currently_updated : PlayerStatisticsUpdaterSpecs
        {
                static bool isFinishedWasRaised;
            static IPlayerStatistics returnedPlayerStatistics;

            Establish context = () => {
                _sut.Update(_bobsStats_Mock.Object);
                _sut.FinishedUpdatingPlayerStatistics += stats => {
                    isFinishedWasRaised = true;
                    returnedPlayerStatistics = stats;
                };
            };

            Because of = () => _workerForSingleStatisticsUpdate_Mock.RunWorkerCompletedInvoke(new RunWorkerCompletedEventArgs(_bobsStats_Mock.Object, null, false));

            It should_let_me_know_when_it_is_finished = () => isFinishedWasRaised.ShouldBeTrue();

            It should_include_the_updated_bobs_statistics_when_it_lets_me_know_that_it_is_finished = () => returnedPlayerStatistics.ShouldEqual(_bobsStats_Mock.Object);
        }

        [Subject(typeof(PlayerStatisticsUpdater), "Update PlayerStatistics")]
        public class when_the_background_worker_says_he_finished_updating_bobs_statistics_but_it_was_currently_updating_teds_statistics_because_bobs_took_too_long
            : PlayerStatisticsUpdaterSpecs
        {
            static bool isFinishedWasRaised;

            Establish context = () => {
                _sut.Update(_bobsStats_Mock.Object);
                _sut.Update(_tedsStats_Mock.Object);
                _sut.FinishedUpdatingPlayerStatistics += stats => isFinishedWasRaised = true;
            };

            Because of = () => _workerForSingleStatisticsUpdate_Mock.RunWorkerCompletedInvoke(new RunWorkerCompletedEventArgs(_bobsStats_Mock.Object, null, false));

            It should_not_let_me_know_when_it_is_finished = () => isFinishedWasRaised.ShouldBeFalse();
        }
    }
}