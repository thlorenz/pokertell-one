namespace PokerTell.Statistics.Interfaces
{
   using System.Collections.Generic;

   using PokerTell.Infrastructure.Enumerations.PokerHand;
   using PokerTell.Infrastructure.Interfaces.PokerHand;

   public interface IRaiseReactionStatisticsBuilder
   {
      IRaiseReactionStatistics Build(
         IEnumerable<IAnalyzablePokerPlayer> analyzablePokerPlayers,
         ActionSequences actionSequence,
         Streets street);
   }
}