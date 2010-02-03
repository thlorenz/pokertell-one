namespace PokerTell.Statistics.Analyzation
{
   using System;
   using System.Collections.Generic;
   using System.Linq;

   using Infrastructure.Enumerations.PokerHand;
   using Infrastructure.Interfaces.PokerHand;

   using Interfaces;

   using Tools.Interfaces;

   public class PostFlopHeroActsRaiseReactionDescriber : IPostFlopHeroActsRaiseReactionDescriber
   {
      public string Describe(string playerName, 
         IAnalyzablePokerPlayer analyzablePokerPlayer, 
         Streets street, 
         ITuple<double, double> ratioSizes)
      {
         var actionSequence = analyzablePokerPlayer.ActionSequences[(int)street];
         return string.Format(
               "{0} {1} {2} of the pot {3} on the {4} and was raised",
               playerName,
               ActionSequencesUtility.NameLastActionInSequence(actionSequence).ToLower(),
               (ratioSizes.First == ratioSizes.Second)
                  ? ratioSizes.First.ToString()
                  : ratioSizes.First + " to " + ratioSizes.Second,
               (bool)analyzablePokerPlayer.InPosition[(int)street] ? "in position" : "out of position",
                street.ToString().ToLower()
               );
      }
   }
}