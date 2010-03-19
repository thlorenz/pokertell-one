namespace PokerTell.Statistics.Tests.ViewModels.Analyzation
{
    using Machine.Specifications;

    using Moq;

    using PokerTell.Infrastructure.Interfaces.PokerHand;
    using PokerTell.Statistics.Interfaces;
    using PokerTell.Statistics.ViewModels.Analyzation;

    using Tools.Validation;

    using It = Machine.Specifications.It;

    // ReSharper disable InconsistentNaming
    public abstract class RepositoryHandBrowserViewModelSpecs
    {
        /*
       *    Specifications
       *    Subject: RepositoryHandBrowserViewModel
       *    
       *    Initialization
       *       given 2 analyzable players
       *          It should initialize hand browser with their hand ids sorted backwards
       *          It should update current hand viewmodel with first hand of hand browser
       *          It should set HandCount to hand browser potential hand count
       *          
       *    Browsing, initialized with 2 analyzable players
       *       when user browses to second hand 
       *          It should update current hand viewmodel with second hand
       */
        protected static Mock<IHandHistoryViewModel> _handHistoryViewModelMock;

        protected static Mock<IRepositoryHandBrowser> _handBrowserMock;

        protected static RepositoryHandBrowserViewModel _sut;

        Establish specContext = () => {
            _handHistoryViewModelMock = new Mock<IHandHistoryViewModel>();
            _handBrowserMock = new Mock<IRepositoryHandBrowser>();

            _sut = new RepositoryHandBrowserViewModel(_handBrowserMock.Object, _handHistoryViewModelMock.Object, new CollectionValidator());
        };

        public abstract class Ctx_HandBrowserViewModel_Ready_to_intialize_2_analyzable_players
            : RepositoryHandBrowserViewModelSpecs
        {
            protected static Mock<IAnalyzablePokerPlayer> _analyzablePokerPlayerStub_1;

            protected static Mock<IAnalyzablePokerPlayer> _analyzablePokerPlayerStub_2;

            protected static Mock<IConvertedPokerHand> _pokerHandStub_1;

            protected static Mock<IConvertedPokerHand> _pokerHandStub_2;

            protected static int _handId1;

            protected static int _handId2;

            Establish context = () => {
                _handId1 = 1;
                _handId2 = 2;

                _analyzablePokerPlayerStub_1 = new Mock<IAnalyzablePokerPlayer>();
                _analyzablePokerPlayerStub_1.SetupGet(p => p.HandId).Returns(_handId1);

                _analyzablePokerPlayerStub_2 = new Mock<IAnalyzablePokerPlayer>();
                _analyzablePokerPlayerStub_2.SetupGet(p => p.HandId).Returns(_handId2);

                _pokerHandStub_1 = new Mock<IConvertedPokerHand>();
                _pokerHandStub_2 = new Mock<IConvertedPokerHand>();
                _handBrowserMock.Setup(b => b.Hand(0)).Returns(_pokerHandStub_2.Object);
                _handBrowserMock.Setup(b => b.Hand(1)).Returns(_pokerHandStub_1.Object);
                _handBrowserMock.SetupGet(b => b.PotentialHandsCount).Returns(2);
            };
        }

        public abstract class Ctx_Initialized_with_2_AnalyzablePlayers : Ctx_HandBrowserViewModel_Ready_to_intialize_2_analyzable_players
        {
            Establish initializedContext = () => _sut.InitializeWith(new[] { _analyzablePokerPlayerStub_1.Object, _analyzablePokerPlayerStub_2.Object });
        }

        [Subject(typeof(RepositoryHandBrowserViewModel), "Initialization")]
        public class given_2_analyzable_players
            : Ctx_HandBrowserViewModel_Ready_to_intialize_2_analyzable_players
        {
            Because of = () => _sut.InitializeWith(new[] { _analyzablePokerPlayerStub_1.Object, _analyzablePokerPlayerStub_2.Object });

            It should_initialize_hand_browser_with_their_hand_ids_sorted_backwards
                = () => _handBrowserMock.Verify(b => b.InitializeWith(new[] { _handId2, _handId1 }));

            It should_update_hand_history_viewmodel_with_first_hand_of_hand_browser
                = () => _handHistoryViewModelMock.Verify(
                            hh => hh.UpdateWith(Moq.It.Is<IConvertedPokerHand>(
                                                    h => h.Equals(_pokerHandStub_2.Object))));

            It should_set_HandCount_to_hand_browsers_potential_hands_count
                = () => _sut.HandCount.ShouldEqual(_handBrowserMock.Object.PotentialHandsCount);
        }

        [Subject(typeof(RepositoryHandBrowserViewModel), "Browsing, initialized with 2 analyzable players")]
        public class when_user_browses_to_second_hand
            : Ctx_HandBrowserViewModel_Ready_to_intialize_2_analyzable_players
        {
            Establish context =
                () => _sut.InitializeWith(new[] { _analyzablePokerPlayerStub_1.Object, _analyzablePokerPlayerStub_2.Object });

            Because of = () => _sut.CurrentHandIndex = 1;

            It should_update_hand_history_viewmodel_with_second_hand_of_hand_browser
                = () => _handHistoryViewModelMock.Verify(
                            hh => hh.UpdateWith(Moq.It.Is<IConvertedPokerHand>(
                                                    h => h.Equals(_pokerHandStub_1.Object))));
        }

        [Subject(typeof(RepositoryHandBrowserViewModel), "Scroll")]
        public class when_containing_two_hands_and_current_hand_index_is_0_and_I_scroll_1_forward : Ctx_Initialized_with_2_AnalyzablePlayers
        {
            Establish context = () => _sut.CurrentHandIndex = 0;

            Because of = () => _sut.Scroll(1);

            It should_set_the_CurrentHandIndex_to_1 = () => _sut.CurrentHandIndex.ShouldEqual(1);
        }

        [Subject(typeof(RepositoryHandBrowserViewModel), "Scroll")]
        public class when_containing_two_hands_and_current_hand_index_is_1_and_I_scroll_1_backward : Ctx_Initialized_with_2_AnalyzablePlayers
        {
            Establish context = () => _sut.CurrentHandIndex = 1;

            Because of = () => _sut.Scroll(-1);

            It should_set_the_CurrentHandIndex_to_0 = () => _sut.CurrentHandIndex.ShouldEqual(0);
        }
    }
}