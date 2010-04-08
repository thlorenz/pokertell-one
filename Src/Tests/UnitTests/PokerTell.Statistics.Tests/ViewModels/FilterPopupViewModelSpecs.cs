namespace PokerTell.Statistics.Tests.ViewModels
{
    using System;

    using Infrastructure.Interfaces.Statistics;

    using Machine.Specifications;

    using Moq;

    using Statistics.ViewModels;

    using It = Machine.Specifications.It;

    // Resharper disable InconsistentNaming
    public abstract class FilterPopupViewModelSpecs
    {
        protected const string PlayerName = "some Player";

        protected static Mock<IAnalyzablePokerPlayersFilter> _filter_Stub;

        protected static Mock<IAnalyzablePokerPlayersFilterAdjustmentViewModel> _filterAdjustmentVM_Mock;

        protected static Action<string, IAnalyzablePokerPlayersFilter> _applyTo_Stub;

        protected static Action<IAnalyzablePokerPlayersFilter> _applyToAll_Stub; 

        protected static IFilterPopupViewModel _sut;

        Establish context = () => {
            _filter_Stub = new Mock<IAnalyzablePokerPlayersFilter>();
            _filterAdjustmentVM_Mock = new Mock<IAnalyzablePokerPlayersFilterAdjustmentViewModel>();
            _applyTo_Stub = new Action<string, IAnalyzablePokerPlayersFilter>((n, f) => { });
            _applyToAll_Stub = new Action<IAnalyzablePokerPlayersFilter>(n => { });

            _sut = new FilterPopupViewModel(_filterAdjustmentVM_Mock.Object);
        };

        [Subject(typeof(FilterPopupViewModel), "ShowFilter")]
        public class when_told_to_show_the_filter_for_a_player_passing_filter_and_ApplyTo_and_ApplyToAll_actions : FilterPopupViewModelSpecs
        {
            Because of = () => _sut.ShowFilter(PlayerName, _filter_Stub.Object, _applyTo_Stub, _applyToAll_Stub);

            It should_set_Show_to_true = () => _sut.Show.ShouldBeTrue();

            It intitialize_the_filter_adjustment_viewmodel_with_the_PlayerName_the_filter_and_the_applyTo_and_applyToAll_actions
                = () => _filterAdjustmentVM_Mock.Verify(fa => fa.InitializeWith(PlayerName, _filter_Stub.Object, _applyTo_Stub, _applyToAll_Stub));
        }

        [Subject(typeof(FilterPopupViewModel), "ShowFilter")]
        public class when_told_to_show_the_filter_for_a_player_passing_filter_and_ApplyTo_action : FilterPopupViewModelSpecs
        {
            Because of = () => _sut.ShowFilter(PlayerName, _filter_Stub.Object, _applyTo_Stub);

            It should_set_Show_to_true = () => _sut.Show.ShouldBeTrue();

            It intitialize_the_filter_adjustment_viewmodel_with_the_PlayerName_the_filter_and_the_applyTo_and_null_for_applyToAll_actions
                = () => _filterAdjustmentVM_Mock.Verify(fa => fa.InitializeWith(PlayerName, _filter_Stub.Object, _applyTo_Stub, null));
        }
    }
}