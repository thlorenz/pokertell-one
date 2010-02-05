namespace PokerTell.Statistics.Interfaces
{
   using System.Collections.Generic;

   using Infrastructure.Enumerations.PokerHand;
   using Infrastructure.Interfaces.PokerHand;

   using Tools.Interfaces;

   
   public interface IPostFlopHeroActsRaiseReactionDescriber : IRaiseReactionDescriber<double>
   {
   }

   public interface IPostFlopHeroReactsRaiseReactionDescriber : IRaiseReactionDescriber<double>
   {
   }

   /// <summary>
   /// Helps describing the situation for which preflop raise reactions apply
   /// </summary>
   public interface IPreFlopRaiseReactionDescriber : IRaiseReactionDescriber<StrategicPositions>
   {
   }

   /// <summary>
   /// Helps describing the situation in which the player first bet and then was raised
   /// </summary>
   public interface IRaiseReactionDescriber<T>
   {

       /// <summary>
       /// Describes the situation defined by the passed parameters to
       /// give the user feedback about what a statistics table depicts.
       /// </summary>
       /// <param name="playerName"></param>
       /// <param name="analyzablePokerPlayer">A sample player from the statistics set</param>
       /// <param name="street">On what street did the situation occur</param>
       /// <param name="ratioSizes">Is used to describe the range of betsizes, raise sizes 
       /// or strategic positions (depending on T of the inheritor) in which the player acted.</param>
       /// <returns>A nice description of the situation depicted by the parameters</returns>
      string Describe(string playerName, IAnalyzablePokerPlayer analyzablePokerPlayer, Streets street, ITuple<T, T> ratioSizes);
   }
}