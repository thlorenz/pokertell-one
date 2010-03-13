namespace PokerTell.Statistics.Tests.ViewModels
{
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
            _sut =
                new DetailedStatisticsAnalyzerViewModel(
                    new Constructor<IDetailedPreFlopStatisticsViewModel>(() => _preFlopStatisticsViewModelStub),
                    new Constructor<IDetailedPostFlopHeroActsStatisticsViewModel>(() => _postFlopActionStatisticsViewModelStub),
                    new Constructor<IDetailedPostFlopHeroReactsStatisticsViewModel>(() => _postFlopReactionStatisticsViewModelStub));
        };

        static StubBuilder _stub;

        static IDetailedPreFlopStatisticsViewModel _preFlopStatisticsViewModelStub;

        static IDetailedPostFlopHeroActsStatisticsViewModel _postFlopActionStatisticsViewModelStub;

        static IDetailedPostFlopHeroReactsStatisticsViewModel _postFlopReactionStatisticsViewModelStub;

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
    }
}