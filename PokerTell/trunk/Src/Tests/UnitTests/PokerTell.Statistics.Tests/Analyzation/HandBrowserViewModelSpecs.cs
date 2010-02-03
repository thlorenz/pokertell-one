namespace PokerTell.Statistics.Tests.ViewModels.Analyzation
{
   using Infrastructure.Interfaces.PokerHand;

   using Interfaces;

   using Machine.Specifications;

   using Moq;

   using Statistics.ViewModels.Analyzation;

   using It = Machine.Specifications.It;

   // ReSharper disable InconsistentNaming
   public abstract class HandBrowserViewModelSpecs
   {
      /*
       *    Specifications
       *    Subject: HandBrowserViewModel
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

      protected static Mock<IHandBrowser> _handBrowserMock;

      protected static HandBrowserViewModel _sut;

      Establish context = () => {
         _handHistoryViewModelMock = new Mock<IHandHistoryViewModel>();
         _handBrowserMock = new Mock<IHandBrowser>();

         _sut = new HandBrowserViewModel(_handBrowserMock.Object, _handHistoryViewModelMock.Object);
      };
   }

   public abstract class Ctx_HandBrowserViewModel_Ready_to_intialize_2_analyzable_players
      : HandBrowserViewModelSpecs
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
  
 
   [Subject(typeof(HandBrowserViewModel), "Initialization")]
   public class given_2_analyzable_players
      : Ctx_HandBrowserViewModel_Ready_to_intialize_2_analyzable_players
   {
      
      Because of = () => _sut.InitializeWith(new[] { _analyzablePokerPlayerStub_1.Object, _analyzablePokerPlayerStub_2.Object });

      It should_initialize_hand_browser_with_their_hand_ids_sorted_backwards
         = () => _handBrowserMock.Verify(b => b.InitializeWith(new [] {_handId2, _handId1}));

      It should_update_hand_history_viewmodel_with_first_hand_of_hand_browser
         = () => _handHistoryViewModelMock.Verify(
                    hh => hh.UpdateWith(Moq.It.Is<IConvertedPokerHand>(
                                           h => h.Equals(_pokerHandStub_2.Object))));

      It should_set_HandCount_to_hand_browsers_potential_hands_count
         = () => _sut.HandCount.ShouldEqual(_handBrowserMock.Object.PotentialHandsCount);
      }

   [Subject(typeof(HandBrowserViewModel), "Browsing, initialized with 2 analyzable players")]
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
}