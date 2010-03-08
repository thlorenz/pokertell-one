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
            _preFlopStatisticsViewModelStub = _stub.Out<IDetailedStatisticsViewModel>();
            _postFlopActionStatisticsViewModelStub = _stub.Out<IDetailedStatisticsViewModel>();
            _postFlopReactionStatisticsViewModelStub = _stub.Out<IDetailedStatisticsViewModel>();
            _sut =
                new DetailedStatisticsAnalyzerViewModel(
                    new Constructor<IDetailedStatisticsViewModel>(() => _preFlopStatisticsViewModelStub),
                    new Constructor<IDetailedStatisticsViewModel>(() => _postFlopActionStatisticsViewModelStub),
                    new Constructor<IDetailedStatisticsViewModel>(() => _postFlopReactionStatisticsViewModelStub));
        };

        static StubBuilder _stub;

        static IDetailedStatisticsViewModel _preFlopStatisticsViewModelStub;

        static IDetailedStatisticsViewModel _postFlopActionStatisticsViewModelStub;

        static IDetailedStatisticsViewModel _postFlopReactionStatisticsViewModelStub;

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