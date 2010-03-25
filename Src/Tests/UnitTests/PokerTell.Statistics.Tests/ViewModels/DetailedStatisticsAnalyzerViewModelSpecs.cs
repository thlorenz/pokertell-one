namespace PokerTell.Statistics.Tests.ViewModels
{
    using System.Collections.Generic;

    using Infrastructure.Interfaces.PokerHand;
    using Infrastructure.Interfaces.Statistics;
    using Infrastructure.Services;

    using Interfaces;

    using Machine.Specifications;

    using Moq;

    using Statistics.ViewModels;

    using It = Machine.Specifications.It;

    // Resharper disable InconsistentNaming
    public class DetailedStatisticsAnalyzerViewModelSpecs
    {
        Establish specContext = () => {
            _stub = new StubBuilder();
            _preFlopStatisticsViewModelStub = _stub.Out<IDetailedPreFlopStatisticsViewModel>();
            _postFlopActionStatisticsViewModelStub = _stub.Out<IDetailedPostFlopHeroActsStatisticsViewModel>();
            _postFlopReactionStatisticsViewModelStub = _stub.Out<IDetailedPostFlopHeroReactsStatisticsViewModel>();

            _repositoryBrowserVM_Mock = new Mock<IRepositoryHandBrowserViewModel>();
            
            _sut =
                new DetailedStatisticsAnalyzerViewModel(
                    new Constructor<IDetailedPreFlopStatisticsViewModel>(() => _preFlopStatisticsViewModelStub),
                    new Constructor<IDetailedPostFlopHeroActsStatisticsViewModel>(() => _postFlopActionStatisticsViewModelStub),
                    new Constructor<IDetailedPostFlopHeroReactsStatisticsViewModel>(() => _postFlopReactionStatisticsViewModelStub),
                    _repositoryBrowserVM_Mock.Object);
        };

        protected static StubBuilder _stub;

        protected static IDetailedPreFlopStatisticsViewModel _preFlopStatisticsViewModelStub;

        protected static IDetailedPostFlopHeroActsStatisticsViewModel _postFlopActionStatisticsViewModelStub;

        protected static IDetailedPostFlopHeroReactsStatisticsViewModel _postFlopReactionStatisticsViewModelStub;

        protected static Mock<IRepositoryHandBrowserViewModel> _repositoryBrowserVM_Mock;

        protected static DetailedStatisticsAnalyzerViewModel _sut;

        [Subject(typeof(DetailedStatisticsAnalyzerViewModel), "AddViewModel")]
        public class when_ViewModel_wants_to_be_shown_as_popup : DetailedStatisticsAnalyzerViewModelSpecs
        {
            static Mock<IDetailedStatisticsAnalyzerContentViewModel> viewModelStub;
            Establish context = () => { viewModelStub = new Mock<IDetailedStatisticsAnalyzerContentViewModel>();
            viewModelStub.SetupGet(v => v.ShowAsPopup).Returns(true);
            };

            Because of = () => _sut.AddViewModel(viewModelStub.Object);

            It should_assign_it_to_Popup_ViewModel = () => _sut.PopupViewModel.ShouldBeTheSameAs(viewModelStub.Object);

            It should_set_ShowPopup_to_true = () => _sut.ShowPopup.ShouldBeTrue();
        }

        [Subject(typeof(DetailedStatisticsAnalyzerViewModel), "AddViewModel")]
        public class when_ViewModel_does_not_want_to_be_shown_as_popup : DetailedStatisticsAnalyzerViewModelSpecs
        {
            static Mock<IDetailedStatisticsAnalyzerContentViewModel> viewModelStub;
            Establish context = () => { viewModelStub = new Mock<IDetailedStatisticsAnalyzerContentViewModel>();
            viewModelStub.SetupGet(v => v.ShowAsPopup).Returns(false);
            };

            Because of = () => _sut.AddViewModel(viewModelStub.Object);

            It should_assign_it_to_CurrentViewModel = () => _sut.CurrentViewModel.ShouldBeTheSameAs(viewModelStub.Object);

            It should_set_ShowPopup_to_false = () => _sut.ShowPopup.ShouldBeFalse();
        }

        [Subject(typeof(DetailedStatisticsAnalyzerViewModel), "InitializeWith AnalyzablePlayers")]
        public class when_initialized_with_analyzable_players_and_thee_player_name : DetailedStatisticsAnalyzerViewModelSpecs
        {
            const string playerName = "someName";

            static IEnumerable<IAnalyzablePokerPlayer> analyzablePokerPlayers_Stub;

            Establish context = () => analyzablePokerPlayers_Stub = new[] { new Mock<IAnalyzablePokerPlayer>().Object };

            Because of = () => _sut.InitializeWith(analyzablePokerPlayers_Stub, playerName);

            It should_initialize_the_repository_hand_browser_viewmodel_with_the_analyzable_players_and_the_playername
                = () => _repositoryBrowserVM_Mock.Verify(rb => rb.InitializeWith(analyzablePokerPlayers_Stub, playerName));

            It should_add_the_hand_browser_viewmodel_to_the_ViewModel_History = () => _sut.ViewModelHistory.ShouldContain(_repositoryBrowserVM_Mock.Object);

            It should_set_the_current_viewmodel_to_the_hand_browser_viewmodel = () => _sut.CurrentViewModel.ShouldEqual(_repositoryBrowserVM_Mock.Object);
        }

    }
}