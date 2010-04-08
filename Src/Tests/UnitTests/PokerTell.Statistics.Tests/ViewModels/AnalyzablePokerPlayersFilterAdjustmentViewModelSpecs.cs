namespace PokerTell.Statistics.Tests.ViewModels
{
    using System;

    using Infrastructure.Interfaces;
    using Infrastructure.Interfaces.Statistics;

    using Machine.Specifications;

    using Moq;

    using Statistics.ViewModels;

    using It = Machine.Specifications.It;

    // Resharper disable InconsistentNaming
    public abstract class AnalyzablePokerPlayersFilterAdjustmentViewModelSpecs
    {
        protected const string PlayerName = "some Player";

        protected static Mock<IAnalyzablePokerPlayersFilter> _filter_Stub;

        protected static Mock<IAnalyzablePokerPlayersFilterViewModel> _filterVM_Mock;

        static Mock<IConstructor<IAnalyzablePokerPlayersFilterViewModel>> filterVMConstructor_Stub;

        protected static Action<string, IAnalyzablePokerPlayersFilter> _applyTo_Stub;

        protected static Action<IAnalyzablePokerPlayersFilter> _applyToAll_Stub; 

        protected static IAnalyzablePokerPlayersFilterAdjustmentViewModel _sut;

        Establish specContext = () => {
            _filter_Stub = new Mock<IAnalyzablePokerPlayersFilter>();

            _filterVM_Mock = new Mock<IAnalyzablePokerPlayersFilterViewModel>();
            _filterVM_Mock
                .Setup(f => f.InitializeWith(Moq.It.IsAny<IAnalyzablePokerPlayersFilter>()))
                .Returns(_filterVM_Mock.Object);

            filterVMConstructor_Stub = new Mock<IConstructor<IAnalyzablePokerPlayersFilterViewModel>>();
            filterVMConstructor_Stub
                .SetupGet(ct => ct.New)
                .Returns(_filterVM_Mock.Object);

            _applyTo_Stub = new Action<string, IAnalyzablePokerPlayersFilter>((n, f) => { });
            _applyToAll_Stub = new Action<IAnalyzablePokerPlayersFilter>(n => { });

            _sut = new AnalyzablePokerPlayersFilterAdjustmentViewModel(filterVMConstructor_Stub.Object);
        };

        public class Ctx_InitializedWithPlayerNameFilterAndActions : AnalyzablePokerPlayersFilterAdjustmentViewModelSpecs
        {
            Establish initializedContext = () => _sut.InitializeWith(PlayerName, _filter_Stub.Object, _applyTo_Stub, _applyToAll_Stub);
        }

        [Subject(typeof(AnalyzablePokerPlayersFilterAdjustmentViewModel), "InitializeWith")]
        public class when_initialized_with_player_name_his_filter_and_applyTo_and_applyToAll_actions : AnalyzablePokerPlayersFilterAdjustmentViewModelSpecs
        {
            Because of = () => _sut.InitializeWith(PlayerName, _filter_Stub.Object, _applyTo_Stub, _applyToAll_Stub);

            It should_assign_its_PlayerName_to_the_passed_player_name = () => _sut.PlayerName.ShouldEqual(PlayerName);

            It should_initialize_its_Filter_viewmodel_with_the_passed_filter = () => _filterVM_Mock.Verify(f => f.InitializeWith(_filter_Stub.Object));

            It should_show_the_ApplyToAll_Command = () => _sut.ShowApplyToAllCommand.ShouldBeTrue();
        }

        [Subject(typeof(AnalyzablePokerPlayersFilterAdjustmentViewModel), "InitializeWith")]
        public class when_initialized_with_player_name_his_filter_and_applyTo_and_null_for_applyToAll_action : AnalyzablePokerPlayersFilterAdjustmentViewModelSpecs
        {
            Because of = () => _sut.InitializeWith(PlayerName, _filter_Stub.Object, _applyTo_Stub, null);

            It should_assign_its_PlayerName_to_the_passed_player_name = () => _sut.PlayerName.ShouldEqual(PlayerName);

            It should_initialize_its_Filter_viewmodel_with_the_passed_filter = () => _filterVM_Mock.Verify(f => f.InitializeWith(_filter_Stub.Object));

            It should_not_show_the_ApplyToAll_Command = () => _sut.ShowApplyToAllCommand.ShouldBeFalse();
        }

        [Subject(typeof(AnalyzablePokerPlayersFilterAdjustmentViewModel), "ApplyFilterTo Command")]
        public class when_the_user_wants_to_apply_the_filter_to_a_player : AnalyzablePokerPlayersFilterAdjustmentViewModelSpecs
        {
            static bool wasInvokedWithCorrectName;

            static bool wasInvokedWithCorrectFilter;

            Establish context = () => {
                var currentFilter = new Mock<IAnalyzablePokerPlayersFilter>();
                _filterVM_Mock
                    .SetupGet(fvm => fvm.CurrentFilter)
                    .Returns(currentFilter.Object);

                Action<string, IAnalyzablePokerPlayersFilter> applyTo =
                    (name, filter) => {
                        wasInvokedWithCorrectName = name == PlayerName;
                        wasInvokedWithCorrectFilter = filter.Equals(currentFilter.Object);
                    };
                _sut.InitializeWith(PlayerName, _filter_Stub.Object, applyTo, _applyToAll_Stub);
            };

            Because of = () => _sut.ApplyFilterToPlayerCommand.Execute(null);

            It should_apply_the_filter_with_the_given_player_name = () => wasInvokedWithCorrectName.ShouldBeTrue();

            It should_apply_the_filter_with_the_currently_defined_filter = () => wasInvokedWithCorrectFilter.ShouldBeTrue();
        }

        [Subject(typeof(AnalyzablePokerPlayersFilterAdjustmentViewModel), "ApplyFilterTo Command")]
        public class when_the_user_wants_to_apply_the_filter_to_all_players : AnalyzablePokerPlayersFilterAdjustmentViewModelSpecs
        {
            static bool wasInvokedWithCorrectFilter;

            Establish context = () => {
                var currentFilter = new Mock<IAnalyzablePokerPlayersFilter>();
                _filterVM_Mock
                    .SetupGet(fvm => fvm.CurrentFilter)
                    .Returns(currentFilter.Object);

                Action<IAnalyzablePokerPlayersFilter> applyToAll = filter => wasInvokedWithCorrectFilter = filter.Equals(currentFilter.Object);
                    
                _sut.InitializeWith(PlayerName, _filter_Stub.Object, _applyTo_Stub, applyToAll);
            };

            Because of = () => _sut.ApplyFilterToAllCommand.Execute(null);

            It should_apply_the_filter_with_the_currently_defined_filter = () => wasInvokedWithCorrectFilter.ShouldBeTrue();
        }
    }
}