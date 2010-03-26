namespace PokerTell.Statistics.Interfaces
{
   using System.Collections.Generic;

   using Infrastructure.Interfaces;
   using Infrastructure.Interfaces.PokerHand;

    public interface IRepositoryHandBrowser : IFluentInterface
   {
      IRepositoryHandBrowser InitializeWith(IEnumerable<int> handIds);

      int PotentialHandsCount { get; }

      IConvertedPokerHand Hand(int handIndex);
   }
}