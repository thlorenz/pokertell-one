namespace PokerTell.LiveTracker.Tests.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;

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

        protected static Mock<IDetailedStatisticsAnalyzerViewModel> _statisticsAnalyzer_Mock;

        protected static PokerTableStatisticsViewModelSut _sut;

        Establish specContext = () => {
            _eventAggregator_Stub = new Mock<IEventAggregator>();
            _playerStatisticsMake_Stub = new Mock<IConstructor<IPlayerStatisticsViewModel>>();
            _playerStatisticsMake_Stub
                .SetupGet(svm => svm.New).Returns(new Mock<IPlayerStatisticsViewModel>().Object);

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
                playersStatistics_Stub = new[] { Utils.PlayerStatisticsStubFor("p1"), Utils.PlayerStatisticsStubFor("p2") };
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
                playerStatisticsVM_Stub = new Mock<IPlayerStatisticsViewModel>();
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


    }

    public class PokerTableStatisticsViewModelSut : PokerTableStatisticsViewModel
    {
        public PokerTableStatisticsViewModelSut(IEventAggregator eventAggregator, IConstructor<IPlayerStatisticsViewModel> playerStatisticsViewModelMake, IDetailedStatisticsAnalyzerViewModel detailedStatisticsAnalyzerViewModel)
            : base(eventAggregator, playerStatisticsViewModelMake, detailedStatisticsAnalyzerViewModel)
        {
        }

        public void AddNewPlayerToPlayersIfNotFound_Invoke(IPlayerStatisticsViewModel matchingPlayer)
        {
            AddNewPlayerToPlayersIfNotFound(matchingPlayer);
        }
    }
}