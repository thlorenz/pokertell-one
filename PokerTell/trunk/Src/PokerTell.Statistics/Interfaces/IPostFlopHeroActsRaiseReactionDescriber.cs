namespace PokerTell.Statistics.Interfaces
{
   using System.Collections.Generic;

   using Infrastructure.Enumerations.PokerHand;
   using Infrastructure.Interfaces.PokerHand;

   using Tools.Interfaces;

   public interface IPostFlopHeroActsRaiseReactionDescriber : IRaiseReactionDescriber<double>
   {
   }

   public interface IPreFlopRaiseReactionDescriber : IRaiseReactionDescriber<double>
   {
   }

   public interface IRaiseReactionDescriber<T>
   {
      string Describe(string playerName, IAnalyzablePokerPlayer analyzablePokerPlayer, Streets street, ITuple<T, T> ratioSizes);
   }
}