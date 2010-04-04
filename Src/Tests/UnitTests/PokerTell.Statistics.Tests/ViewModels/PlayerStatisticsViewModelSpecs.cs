namespace PokerTell.Statistics.Tests.ViewModels
{
    using System.Windows;

    using Infrastructure.Enumerations.PokerHand;

    using Machine.Specifications;

    using Moq;

    using NUnit.Framework;

    using PokerTell.Infrastructure.Interfaces.Statistics;
    using PokerTell.Statistics.ViewModels;

    using It=Machine.Specifications.It;

    // Resharper disable InconsistentNaming
    public class PlayerStatisticsViewModelSpecs
    {
        protected static Mock<IPreFlopStatisticsSetsViewModel> _preFlopStatisticsSetsVM_Mock;

        protected static Mock<IPostFlopStatisticsSetsViewModel> _flopStatisticsSetsVM_Mock;

        protected static Mock<IPostFlopStatisticsSetsViewModel> _turnStatisticsSetsVM_Mock;

        protected static Mock<IPostFlopStatisticsSetsViewModel> _riverStatisticsSetsVM_Mock;
            
        protected static Mock<IPlayerStatistics> _playerStatistics_Stub;

        protected static PlayerStatisticsViewModel _sut;

        Establish specContext = () => {
            _preFlopStatisticsSetsVM_Mock = new Mock<IPreFlopStatisticsSetsViewModel>();

            _flopStatisticsSetsVM_Mock = new Mock<IPostFlopStatisticsSetsViewModel>();
            _turnStatisticsSetsVM_Mock = new Mock<IPostFlopStatisticsSetsViewModel>();
            _riverStatisticsSetsVM_Mock = new Mock<IPostFlopStatisticsSetsViewModel>();

            _playerStatistics_Stub = new Mock<IPlayerStatistics>();
        };

        public class Ctx_InstantiatedSut : PlayerStatisticsViewModelSpecs
        {
            Establish instantiatedContext = () => _sut = new PlayerStatisticsViewModel(
                                                 _preFlopStatisticsSetsVM_Mock.Object,
                                                 _flopStatisticsSetsVM_Mock.Object,
                                                 _turnStatisticsSetsVM_Mock.Object,
                                                 _riverStatisticsSetsVM_Mock.Object);
        }

        public class Ctx_UpdatedWithStatistics : Ctx_InstantiatedSut
        {
            Establish updatedContext = () => _sut.UpdateWith(_playerStatistics_Stub.Object);
        }

        [Subject(typeof(PlayerStatisticsViewModel), "Instantiation")]
        public class when_it_is_instantiated : PlayerStatisticsViewModelSpecs
        {
            Because of = () => _sut = new PlayerStatisticsViewModel(
                                          _preFlopStatisticsSetsVM_Mock.Object,
                                          _flopStatisticsSetsVM_Mock.Object,
                                          _turnStatisticsSetsVM_Mock.Object,
                                          _riverStatisticsSetsVM_Mock.Object);

           It should_initialize_the_flopStatistics_viewmodel_with_the_Flop =
                () => _flopStatisticsSetsVM_Mock.Verify(vm => vm.InitializeWith(Streets.Flop));

            It should_initialize_the_turnStatistics_viewmodel_with_the_Turn =
                () => _turnStatisticsSetsVM_Mock.Verify(vm => vm.InitializeWith(Streets.Turn));

            It should_initialize_the_riverStatistics_viewmodel_with_the_River 
                = () => _riverStatisticsSetsVM_Mock.Verify(vm => vm.InitializeWith(Streets.River));

            It should_assign_its_preflop_statistics_viewmodel_to_the_one_passed_in 
                = () => _sut.PreFlopStatisticsSets.ShouldBeTheSameAs(_preFlopStatisticsSetsVM_Mock.Object);

            It should_assign_its_flop_statistics_viewmodel_to_the_one_passed_in 
                = () => _sut.FlopStatisticsSets.ShouldBeTheSameAs(_flopStatisticsSetsVM_Mock.Object);

            It should_assign_its_turn_statistics_viewmodel_to_the_one_passed_in 
                = () => _sut.TurnStatisticsSets.ShouldBeTheSameAs(_turnStatisticsSetsVM_Mock.Object);

            It should_assign_its_river_statistics_viewmodel_to_the_one_passed_in
                = () => _sut.RiverStatisticsSets.ShouldBeTheSameAs(_riverStatisticsSetsVM_Mock.Object);
        }

        [Subject(typeof(PlayerStatisticsViewModel), "UpdateWith")]
        public class when_it_is_updated_with_new_statistics : Ctx_InstantiatedSut
        {
            Because of = () => _sut.UpdateWith(_playerStatistics_Stub.Object);

            It should_update_the_preflop_statistics_viewmodel_with_it
                = () => _preFlopStatisticsSetsVM_Mock.Verify(vm => vm.UpdateWith(_playerStatistics_Stub.Object));

            It should_update_the_flop_statistics_viewmodel_with_it
                = () => _flopStatisticsSetsVM_Mock.Verify(vm => vm.UpdateWith(_playerStatistics_Stub.Object));

            It should_update_the_turn_statistics_viewmodel_with_it
                = () => _turnStatisticsSetsVM_Mock.Verify(vm => vm.UpdateWith(_playerStatistics_Stub.Object));

            It should_update_the_river_statistics_viewmodel_with_it
                = () => _riverStatisticsSetsVM_Mock.Verify(vm => vm.UpdateWith(_playerStatistics_Stub.Object));
        }

        [RequiresSTA]
        [Subject(typeof(PlayerStatisticsViewModel), "PlayerStatisticsWereUpdated")]
        public class when_the_playerstatistics_say_that_they_were_updated : Ctx_UpdatedWithStatistics
        {
            Because of = () => _playerStatistics_Stub.Raise(ps => ps.StatisticsWereUpdated += null);                

            It should_update_the_preflop_statistics_viewmodel_with_it_again
                = () => _preFlopStatisticsSetsVM_Mock.Verify(vm => vm.UpdateWith(_playerStatistics_Stub.Object), Times.Exactly(2));

            It should_update_the_flop_statistics_viewmodel_with_it_again
                = () => _flopStatisticsSetsVM_Mock.Verify(vm => vm.UpdateWith(_playerStatistics_Stub.Object), Times.Exactly(2));

            It should_update_the_turn_statistics_viewmodel_with_it_again
                = () => _turnStatisticsSetsVM_Mock.Verify(vm => vm.UpdateWith(_playerStatistics_Stub.Object), Times.Exactly(2));

            It should_update_the_river_statistics_viewmodel_with_it_again
                = () => _riverStatisticsSetsVM_Mock.Verify(vm => vm.UpdateWith(_playerStatistics_Stub.Object), Times.Exactly(2));
        }
    }
}