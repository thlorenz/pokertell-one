namespace PokerTell.Statistics.Tests
{
    using System.Collections.Generic;
    using System.Threading;

    using Infrastructure.Interfaces.Statistics;

    using Machine.Specifications;

    using Moq;

    using It = Machine.Specifications.It;

    // Resharper disable InconsistentNaming
    public abstract class PlayerStatisticsUpdaterSpecs
    {
        protected static Mock<IPlayerStatistics> _bobsStats_Mock;
        protected static Mock<IPlayerStatistics> _tedsStats_Mock;
        protected static IEnumerable<IPlayerStatistics> _playerStats_Stub;
        protected static IPlayerStatisticsUpdater _sut;

        Establish specContext = () => {
            _bobsStats_Mock = new Mock<IPlayerStatistics>(); 
            _tedsStats_Mock = new Mock<IPlayerStatistics>();
            _playerStats_Stub = new[] { _bobsStats_Mock.Object, _tedsStats_Mock.Object };

            _sut = new PlayerStatisticsUpdater();
        };

        [Subject(typeof(PlayerStatisticsUpdater), "Update")]
        public class when_told_to_update_bobs_and_teds_statistics : PlayerStatisticsUpdaterSpecs
        {
            static bool isFinishedWasRaised;

            static IEnumerable<IPlayerStatistics> returnedPlayerStatistics;

            Establish context = () => _sut.FinishedUpdatingMultiplePlayerStatistics += stats => {
                isFinishedWasRaised = true;
                returnedPlayerStatistics = stats;
            };

            Because of = () => {
                _sut.Update(_playerStats_Stub);

                // 50ms is plenty enough to allow Background worker to finish
                Thread.Sleep(50);
            };

            It should_update_bobs_statistics = () => _bobsStats_Mock.Verify(s => s.UpdateStatistics());

            It should_update_teds_statistics = () => _tedsStats_Mock.Verify(s => s.UpdateStatistics());

            It should_let_me_know_when_it_is_finished = () => isFinishedWasRaised.ShouldBeTrue();

            It should_include_the_updated_player_statistics_when_it_lets_me_know_that_it_is_finished = () => returnedPlayerStatistics.ShouldEqual(_playerStats_Stub);
        }


    }
}