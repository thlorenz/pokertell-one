namespace PokerTell.Statistics.Tests.Analyzation
{
   // ReSharper disable InconsistentNaming
   using System;
   using System.Linq;

   using Infrastructure.Interfaces.PokerHand;
   using Infrastructure.Interfaces.Repository;

   using Machine.Specifications;

   using Moq;

   using Statistics.Analyzation;

   using It = Machine.Specifications.It;

   public abstract class HandBrowserSpecs
   {
      /*
         *  Specifications
         *  Subject: HandBrowser
         *  
         *  Initialization
         *       
         *      given 0 hand ids
         *          It should throw an ArgumentException
         *      
         *      given 1 hand id
         *          It should not retrieve any hands yet
         *          It should have 1 potential hand
         *      
         *          when the first hand is accessed for the first time
         *              It should request it from the repository
         *              It should return the hand returned by the repository
         *          
         *          when the first hand is accessed for the second time
         *              It should not request it again from the repository
         *              It should return the hand that was previously returned by the repository
         *              
         *      given 3 hand ids
         *          It should have 3 potential hands
         *          
         *          when the 3rd hand is accessed for the first time
         *              It should request the 3rd hand from the repository
         *              It should not request the first or second hand from the repository
         *              It should return the hand returned by the repository
         *              
         *          when the 3rd hand is accessed for the second time
         *              It should not request anything from the repository again
         *              It should return the hand that was previously returned by the repository      
         */

      protected static int _handId_1;

      protected static int _handId_2;

      protected static int _handId_3;

      protected static Mock<IConvertedPokerHand> _handStub_1;

      protected static Mock<IConvertedPokerHand> _handStub_2;

      protected static Mock<IConvertedPokerHand> _handStub_3;

      protected static Mock<IRepository> _repositoryMock;

      protected static HandBrowser _sut;

      Establish context = () => {
         _handId_1 = 1;
         _handId_2 = 2;
         _handId_3 = 3;

         _handStub_1 = new Mock<IConvertedPokerHand>();
         _handStub_1.SetupGet(h => h.Id).Returns(_handId_1);
         _handStub_2 = new Mock<IConvertedPokerHand>();
         _handStub_2.SetupGet(h => h.Id).Returns(_handId_2);
         _handStub_3 = new Mock<IConvertedPokerHand>();
         _handStub_3.SetupGet(h => h.Id).Returns(_handId_3);

         _repositoryMock = new Mock<IRepository>();
         _repositoryMock.Setup(r => r.RetrieveConvertedHand(_handId_1)).Returns(_handStub_1.Object);
         _repositoryMock.Setup(r => r.RetrieveConvertedHand(_handId_2)).Returns(_handStub_2.Object);
         _repositoryMock.Setup(r => r.RetrieveConvertedHand(_handId_3)).Returns(_handStub_3.Object);
         _sut = new HandBrowser(_repositoryMock.Object);
      };
   }

   public abstract class Ctx_HandBrowser_intitialized_with_1_handId : HandBrowserSpecs
   {
      Establish context = () => _sut.InitializeWith(new[] { _handId_1 });
   }

   public abstract class Ctx_HandBrowser_intitialized_with_3_handIds : HandBrowserSpecs
   {
      Establish context = () => _sut.InitializeWith(new[] { _handId_1, _handId_2, _handId_3 });
   }

   [Subject(typeof(HandBrowser), "Initialization")]
   public class given_0_hand_Ids : HandBrowserSpecs
   {
      protected static Exception exception;

      Because of = () => exception = Catch.Exception(() => _sut.InitializeWith(Enumerable.Empty<int>()));

      It should_throw_an_ArgumentException
         = () => exception.ShouldBeOfType<ArgumentException>();
   }

   [Subject(typeof(HandBrowser), "Initialization")]
   public class given_1_hand_Id : Ctx_HandBrowser_intitialized_with_1_handId
   {
      It should_have_1_potential_hand
         = () => _sut.PotentialHandsCount.ShouldEqual(1);

      It should_not_retrieve_any_hands_yet
         = () => _repositoryMock.Verify(r => r.RetrieveConvertedHand(Moq.It.IsAny<int>()), Times.Never());
   }

   [Subject(typeof(HandBrowser), "Hand Retrieval, given 1 analyzable player")]
   public class when_the_first_hand_is_accessed_for_the_first_time : Ctx_HandBrowser_intitialized_with_1_handId
   {
      static IConvertedPokerHand returnedHand;

      Because of = () => returnedHand = _sut.Hand(0);

      It should_request_it_from_the_repository
         = () => _repositoryMock.Verify(r => r.RetrieveConvertedHand(_handId_1));

      It should_return_the_hand_returned_by_the_repository
         = () => returnedHand.Id.ShouldEqual(_handId_1);
   }

   [Subject(typeof(HandBrowser), "Hand Retrieval, given 1 analyzable player")]
   public class when_the_first_hand_is_accessed_for_the_second_time : Ctx_HandBrowser_intitialized_with_1_handId
   {
      static IConvertedPokerHand returnedHand;

      Establish context = () => _sut.Hand(0);

      Because of = () => returnedHand = _sut.Hand(0);

      It should_not_request_it_again_from_the_repository
         = () => _repositoryMock.Verify(r => r.RetrieveConvertedHand(_handId_1), Times.Exactly(1));

      It should_return_the_hand_that_was_previously_returned_by_the_repository
         = () => returnedHand.Id.ShouldEqual(_handId_1);
   }

   [Subject(typeof(HandBrowser), "Initialization")]
   public class given_3_analyzable_players : Ctx_HandBrowser_intitialized_with_3_handIds
   {
      It should_have_3_potential_hands
         = () => _sut.PotentialHandsCount.ShouldEqual(3);
   }

   [Subject(typeof(HandBrowser), "Hand Retrieval, given 3 analyzable players")]
   public class when_the_3rd_hand_is_accessed_for_the_first_time : Ctx_HandBrowser_intitialized_with_3_handIds
   {
      static IConvertedPokerHand returnedHand;

      Because of = () => returnedHand = _sut.Hand(2);

      It should_not_request_the_first_or_second_hand_from_the_repository = () => {
         _repositoryMock.Verify(r => r.RetrieveConvertedHand(_handId_1), Times.Never());
         _repositoryMock.Verify(r => r.RetrieveConvertedHand(_handId_2), Times.Never());
      };

      It should_request_the_3rd_hand_from_the_repository
         = () => _repositoryMock.Verify(r => r.RetrieveConvertedHand(_handId_3));

      It should_return_the_hand_returned_by_the_repository
         = () => returnedHand.Id.ShouldEqual(_handId_3);
   }

   [Subject(typeof(HandBrowser), "Hand Retrieval, given 3 analyzable players")]
   public class when_the_3rd_hand_is_accessed_for_the_second_time : Ctx_HandBrowser_intitialized_with_3_handIds
   {
      static IConvertedPokerHand returnedHand;

      Establish context = () => _sut.Hand(2);

      Because of = () => returnedHand = _sut.Hand(2);

      It should_not_request_anything_from_the_repository_again
         = () => _repositoryMock.Verify(r => r.RetrieveConvertedHand(Moq.It.IsAny<int>()), Times.Exactly(1));

      It should_return_the_hand_that_was_previously_returned_by_the_repository
         = () => returnedHand.Id.ShouldEqual(_handId_3);
   }
}