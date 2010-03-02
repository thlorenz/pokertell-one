namespace PokerTell.Statistics.Interfaces
{
   using System.Collections.Generic;

   using Infrastructure.Interfaces.PokerHand;

   using Tools.Interfaces;

   public interface IRepositoryHandBrowser : IFluentInterface
   {
      IRepositoryHandBrowser InitializeWith(IEnumerable<int> handIds);

      int PotentialHandsCount { get; }

      IConvertedPokerHand Hand(int handIndex);
   }
}