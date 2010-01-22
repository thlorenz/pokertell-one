namespace PokerTell.Statistics.Tests.ViewModels
{
    using System.Linq;

    using Infrastructure.Enumerations.PokerHand;
    using Infrastructure.Interfaces;
    using Infrastructure.Interfaces.Statistics;
    using Infrastructure.Services;

    using Moq;

    using NUnit.Framework;

    using Statistics.ViewModels;
    using Statistics.ViewModels.StatisticsSetDetails;

    using Tools.FunctionalCSharp;

    using UnitTests.Tools;

    [TestFixture]
    internal class DetailedStatisticsAnalyzerViewModelTests
    {
        #region Constants and Fields

        StubBuilder _stub;

        IDetailedStatisticsAnalyzerViewModel _sut;

        IDetailedStatisticsViewModel _preFlopStatisticsViewModelStub;

        IDetailedStatisticsViewModel _postFlopActionStatisticsViewModelStub;

        IDetailedStatisticsViewModel _postFlopReactionStatisticsViewModelStub;

        #endregion

        #region Public Methods


        [Test]
        public void AddViewModel_Always_SetsCurrentViewModelToAddedViewModel()
        {
            var addedViewModel = _stub.Out<IDetailedStatisticsViewModel>();
            _sut.AddViewModel(addedViewModel)
                .CurrentViewModel.ShouldBeEqualTo(addedViewModel);
        }

        [Test]
        public void AddViewModel_CurrentViewModelIsNotLastInHistoryList_AllViewModelsBehindItAreRemoved()
        {
            var viewModel0 = _stub.Out<IDetailedStatisticsViewModel>();
            var viewModel1 = _stub.Out<IDetailedStatisticsViewModel>();
            var addedViewModel = _stub.Out<IDetailedStatisticsViewModel>();

            _sut.AddViewModel(viewModel0)
                .AddViewModel(viewModel1)
                .CurrentViewModel = viewModel0;

            _sut.AddViewModel(addedViewModel)
                .ViewModelHistory
                .ShouldContain(viewModel0)
                .ShouldNotContain(viewModel1)
                .ShouldContain(addedViewModel);
        }

        [Test]
        public void AddViewModel_ViewModelHistoryContainsOneItem_AddsViewModelToEndOFHistoryList()
        {
            var previouslyAddedViewModel = _stub.Out<IDetailedStatisticsViewModel>();
            var addedViewModel = _stub.Out<IDetailedStatisticsViewModel>();
            _sut.AddViewModel(previouslyAddedViewModel);

            _sut.AddViewModel(addedViewModel)
                .ViewModelHistory[1].ShouldBeEqualTo(addedViewModel);
        }

        [Test]
        public void AddViewModel_ViewModelHistoryListIsNull_InitializesViewModelHistoryWithViewModel()
        {
            var addedViewModel = _stub.Out<IDetailedStatisticsViewModel>();
            _sut.AddViewModel(addedViewModel)
                .ViewModelHistory[0].ShouldBeEqualTo(addedViewModel);
        }

        [Test]
        public void NavigateBackwardCommandCanExecute_CurrentViewModelIsFirstInViewModelHistory_ReturnsFalse()
        {
            var viewModel0 = _stub.Out<IDetailedStatisticsViewModel>();
            var viewModel1 = _stub.Out<IDetailedStatisticsViewModel>();

            _sut.AddViewModel(viewModel0)
                .AddViewModel(viewModel1)
                .CurrentViewModel = viewModel0;

            _sut.NavigateBackwardCommand.CanExecute(null).ShouldBeFalse();
        }

        [Test]
        public void NavigateBackwardCommandCanExecute_CurrentViewModelIsNotFirstInViewModelHistory_ReturnsTrue()
        {
            var viewModel0 = _stub.Out<IDetailedStatisticsViewModel>();
            var viewModel1 = _stub.Out<IDetailedStatisticsViewModel>();

            _sut.AddViewModel(viewModel0)
                .AddViewModel(viewModel1)
                .NavigateBackwardCommand.CanExecute(null).ShouldBeTrue();
        }

        [Test]
        public void NavigateBackwardCommandExecute_CurrentViewModelIsSecondInViewModelHistory_CurrentViewModelIsFirst()
        {
            var viewModel0 = _stub.Out<IDetailedStatisticsViewModel>();
            var viewModel1 = _stub.Out<IDetailedStatisticsViewModel>();

            _sut.AddViewModel(viewModel0)
                .AddViewModel(viewModel1)
                .NavigateBackwardCommand.Execute(null);

            _sut.CurrentViewModel.ShouldBeEqualTo(viewModel0);
        }

        [Test]
        public void NavigateForwardCommandCanExecute_CurrentViewModelIsLastInViewModelHistory_ReturnsFalse()
        {
            var viewModel0 = _stub.Out<IDetailedStatisticsViewModel>();
            var viewModel1 = _stub.Out<IDetailedStatisticsViewModel>();

            _sut.AddViewModel(viewModel0)
                .AddViewModel(viewModel1)
                .NavigateForwardCommand.CanExecute(null).ShouldBeFalse();
        }

        [Test]
        public void NavigateForwardCommandCanExecute_CurrentViewModelIsNotLastInViewModelHistory_ReturnsTrue()
        {
            var viewModel0 = _stub.Out<IDetailedStatisticsViewModel>();
            var viewModel1 = _stub.Out<IDetailedStatisticsViewModel>();

            _sut.AddViewModel(viewModel0)
                .AddViewModel(viewModel1)
                .CurrentViewModel = viewModel0;

            _sut.NavigateForwardCommand.CanExecute(null).ShouldBeTrue();
        }

        [Test]
        public void NavigateForwardCommandExecute_CurrentViewModelIsFirstInViewModelHistory_CurrentViewModelIsSecond()
        {
            var viewModel0 = _stub.Out<IDetailedStatisticsViewModel>();
            var viewModel1 = _stub.Out<IDetailedStatisticsViewModel>();

            _sut.AddViewModel(viewModel0)
                .AddViewModel(viewModel1)
                .CurrentViewModel = viewModel0;

            _sut.NavigateForwardCommand.Execute(null);

            _sut.CurrentViewModel.ShouldBeEqualTo(viewModel1);
        }

        [Test]
        public void ViewModelRaisesChildViewModelChanged_ItWasAddedLast_CurrentViewModelIsSetToChildViewModel()
        {
            var addedViewModel = new Mock<IDetailedStatisticsViewModel>();
            var childViewModel = _stub.Out<IDetailedStatisticsViewModel>();

            _sut.AddViewModel(addedViewModel.Object);
            addedViewModel.Raise(vm => vm.ChildViewModelChanged += null, childViewModel);

            _sut.CurrentViewModel.ShouldBeEqualTo(childViewModel);
        }

        [Test]
        public void Visible_ViewModelHistoryIsNull_ReturnsFalse()
        {
            _sut.Visible.ShouldBeFalse();
        }

        [Test]
        public void Visible_ViewModelHistoryIsNotEmpty_ReturnsTrue()
        {
            _sut.AddViewModel(_stub.Out<IDetailedStatisticsViewModel>())
                .Visible.ShouldBeTrue();
        }

        [Test]
        public void InitializeWith_StreetIsPreFlop_CurrentViewModelIsDetailedPreFlopStatisticsViewModel()
        {
            var statisticsSetStub = _stub.Setup<IActionSequenceStatisticsSet>()
                 .Get(s => s.Street).Returns(Streets.PreFlop)
                 .Out;

            _sut.InitializeWith(statisticsSetStub);

            _sut.CurrentViewModel.ShouldBeEqualTo(_preFlopStatisticsViewModelStub);
        }

        [Test]
        public void InitializeWith_StreetIsFlopAndActionIs_HeroActs_CurrentViewModelIsDetailedPostFlopActionStatisticsViewModel()
        {
            var statisticsSetStub = _stub.Setup<IActionSequenceStatisticsSet>()
                .Get(s => s.Street).Returns(Streets.Flop)
                .Get(s => s.ActionSequence).Returns(ActionSequences.HeroActs)
                .Out;

            _sut.InitializeWith(statisticsSetStub);

            _sut.CurrentViewModel.ShouldBe(_postFlopActionStatisticsViewModelStub);
        }

        [Test]
        public void InitializeWith_StreetIsFlopAndActionIs_OppB_CurrentViewModelIsDetailedPostFlopActionStatisticsViewModel()
        {
            var statisticsSetStub = _stub.Setup<IActionSequenceStatisticsSet>()
                .Get(s => s.Street).Returns(Streets.Flop)
                .Get(s => s.ActionSequence).Returns(ActionSequences.OppB)
                .Out;

            _sut.InitializeWith(statisticsSetStub);

            _sut.CurrentViewModel.ShouldBe(_postFlopReactionStatisticsViewModelStub);
        }

        [Test]
        public void InitializeWith_StreetIsFlopAndActionIs_HeroXOppB_CurrentViewModelIsDetailedPostFlopActionStatisticsViewModel()
        {
            var statisticsSetStub = _stub.Setup<IActionSequenceStatisticsSet>()
                .Get(s => s.Street).Returns(Streets.Flop)
                .Get(s => s.ActionSequence).Returns(ActionSequences.HeroXOppB)
                .Out;

            _sut.InitializeWith(statisticsSetStub);

            _sut.CurrentViewModel.ShouldBe(_postFlopReactionStatisticsViewModelStub);
        }

        [Test]
        public void InitializeWith_StreetIsFlopAndActionIs_Illegal_ThrowsMatchNotFoundException()
        {
            var statisticsSetStub = _stub.Setup<IActionSequenceStatisticsSet>()
                .Get(s => s.Street).Returns(Streets.Flop)
                .Get(s => s.ActionSequence).Returns(ActionSequences.OppBHeroC)
                .Out;

            Assert.Throws<MatchNotFoundException>(() => _sut.InitializeWith(statisticsSetStub));
        }

        [Test]
        public void InitializeWith_Always_InitializesViewModelHistoryWithCurrentViewModel()
        {
            IActionSequenceStatisticsSet statisticsSetStub = _stub.Setup<IActionSequenceStatisticsSet>()
                .Get(s => s.Street).Returns(Streets.PreFlop)
                .Out;

            _sut.InitializeWith(statisticsSetStub);
            _sut.ViewModelHistory
                .ShouldHaveCount(1)
                .First().ShouldBe(_sut.CurrentViewModel);
        }

        [Test]
        public void InitializeWith_StatisticsSet_InitializesNewViewModelWithStatisticsSet()
        {
            IActionSequenceStatisticsSet statisticsSetStub = _stub.Setup<IActionSequenceStatisticsSet>()
                .Get(s => s.Street).Returns(Streets.PreFlop)
                .Out;

            var preFlopStatisticsViewModelMock = new Mock<IDetailedStatisticsViewModel>();
            _sut = new DetailedStatisticsAnalyzerViewModel(
                new Constructor<IDetailedStatisticsViewModel>(() => preFlopStatisticsViewModelMock.Object),
                _stub.Out<IConstructor<IDetailedStatisticsViewModel>>(),
                _stub.Out<IConstructor<IDetailedStatisticsViewModel>>());
            
            _sut.InitializeWith(statisticsSetStub);
            
            preFlopStatisticsViewModelMock.Verify(vm => vm.InitializeWith(statisticsSetStub));
        }

        

        [SetUp]
        public void _Init()
        {
            _stub = new StubBuilder();
            _preFlopStatisticsViewModelStub = _stub.Out<IDetailedStatisticsViewModel>();
            _postFlopActionStatisticsViewModelStub = _stub.Out<IDetailedStatisticsViewModel>();
            _postFlopReactionStatisticsViewModelStub = _stub.Out<IDetailedStatisticsViewModel>();
            _sut =
                new DetailedStatisticsAnalyzerViewModel(
                    new Constructor<IDetailedStatisticsViewModel>(() => _preFlopStatisticsViewModelStub),
                    new Constructor<IDetailedStatisticsViewModel>(() => _postFlopActionStatisticsViewModelStub),
                    new Constructor<IDetailedStatisticsViewModel>(() => _postFlopReactionStatisticsViewModelStub));
        }

        #endregion
    }
}