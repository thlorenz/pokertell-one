namespace PokerTell.LiveTracker.Tests.ViewModels
{
    using Infrastructure.Interfaces;
    using Infrastructure.Interfaces.Statistics;

    using Interfaces;

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

        protected static Mock<IDetailedStatisticsAnalyzerViewModel> _statisticsAnalyzer_Stub;

        protected static IPokerTableStatisticsViewModel _sut;

        Establish specContext = () => {
            _eventAggregator_Stub = new Mock<IEventAggregator>();
            _playerStatisticsMake_Stub = new Mock<IConstructor<IPlayerStatisticsViewModel>>();
            _statisticsAnalyzer_Stub = new Mock<IDetailedStatisticsAnalyzerViewModel>();

            _sut = new PokerTableStatisticsViewModel(_eventAggregator_Stub.Object, _playerStatisticsMake_Stub.Object, _statisticsAnalyzer_Stub.Object);
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
    }
}