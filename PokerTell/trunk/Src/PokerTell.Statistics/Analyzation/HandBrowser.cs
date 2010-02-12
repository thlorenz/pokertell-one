namespace PokerTell.Statistics.Analyzation
{
   using System;
   using System.Collections.Generic;
   using System.Linq;

   using Infrastructure.Interfaces.PokerHand;
   using Infrastructure.Interfaces.Repository;

   using Interfaces;

   public class HandBrowser : IHandBrowser
   {
      readonly IRepository _repository;

      IEnumerable<int> _handIds;

      IConvertedPokerHand[] _hands;

      public HandBrowser(IRepository repository)
      {
         _repository = repository;
      }

      public int PotentialHandsCount
      {
         get { return _hands.Count(); }
      }

      public IConvertedPokerHand Hand(int handIndex)
      {
          return _hands[handIndex] ?? (_hands[handIndex] =
             _repository.RetrieveConvertedHand(_handIds.ElementAt(handIndex)));
      }

       public IHandBrowser InitializeWith(IEnumerable<int> handIds)
      {
         if (handIds.Count() < 1)
         {
            throw new ArgumentException("need at least one hand id");
         }

          // Need to make copy here otherwise they somehow get emptied after the first retrieval
         _handIds = handIds.ToArray();
         _hands = new IConvertedPokerHand[_handIds.Count()];

         return this;
      }
   }
}