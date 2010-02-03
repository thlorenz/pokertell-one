namespace PokerTell.Statistics.Analyzation
{
   using System;
   using System.Collections.Generic;
   using System.Linq;

   using Infrastructure.Enumerations.PokerHand;
   using Infrastructure.Interfaces;
   using Infrastructure.Interfaces.PokerHand;

   using Interfaces;

   using Tools.FunctionalCSharp;

   public class RaiseReactionStatisticsBuilder : IRaiseReactionStatisticsBuilder
   {
      readonly IConstructor<IRaiseReactionAnalyzer> _raiseReactionAnalyzerMake;

      readonly IRaiseReactionStatistics _raiseReactionStatistics;

      readonly IRaiseReactionsAnalyzer _raiseReactionsAnalyzer;

      public RaiseReactionStatisticsBuilder(
         IRaiseReactionStatistics raiseReactionStatistics,
         IRaiseReactionsAnalyzer raiseReactionsAnalyzer,
         IConstructor<IRaiseReactionAnalyzer> raiseReactionAnalyzerMake)
      {
         _raiseReactionAnalyzerMake = raiseReactionAnalyzerMake;
         _raiseReactionsAnalyzer = raiseReactionsAnalyzer;
         _raiseReactionStatistics = raiseReactionStatistics;
      }

      public IRaiseReactionStatistics Build(
         IEnumerable<IAnalyzablePokerPlayer> analyzablePokerPlayers,
         ActionSequences actionSequence,
         Streets street)
      {
         if (analyzablePokerPlayers.Count() < 1)
         {
            throw new ArgumentException("need at least one analyzable player");
         }   
         analyzablePokerPlayers.ForEach(
            analyzablePlayer => _raiseReactionsAnalyzer
                                   .AnalyzeAndAdd(_raiseReactionAnalyzerMake.New,
                                                  analyzablePlayer,
                                                  street,
                                                  actionSequence));

         return _raiseReactionStatistics.InitializeWith(_raiseReactionsAnalyzer);
      }
   }
}