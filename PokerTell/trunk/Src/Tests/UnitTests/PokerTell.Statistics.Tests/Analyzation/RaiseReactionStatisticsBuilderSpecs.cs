namespace PokerTell.Statistics.Tests.Analyzation
{
   using System;
   using System.Linq;

   using Infrastructure.Enumerations.PokerHand;
   using Infrastructure.Interfaces;
   using Infrastructure.Interfaces.PokerHand;
   using Infrastructure.Services;

   using Interfaces;

   using Machine.Specifications;

   using Moq;

   using Statistics.Analyzation;

   using It = Machine.Specifications.It;

   // ReSharper disable InconsistentNaming

   public class RaiseReactionStatisticsBuilderSpecs
   {
      /* 
        *   Specification
        *   Subject is RaiseReactionStatisticsBuilder
        *  
            Build, given 0 analyzable players
               » should throw an ArgumentException

            Build, given 2 analyzable players
               » should analyze first player passing the given street and action sequence
               » should analyze second player passing the given street and action sequence
               » should initialize statistics with the raise reactions analyzer
               » should return these statistics
        */

      protected static Mock<IAnalyzablePokerPlayer> _analyzablePokerPlayerStub_1;

      protected static Mock<IAnalyzablePokerPlayer> _analyzablePokerPlayerStub_2;

      protected static IConstructor<IRaiseReactionAnalyzer> _raiseReactionAnalyzerMake;

      protected static Mock<IRaiseReactionAnalyzer> _raiseReactionAnalyzerStub;

      protected static Mock<IRaiseReactionStatistics> _raiseReactionStatisticsMock;

      protected static Mock<IRaiseReactionsAnalyzer> _raiseReactionsAnalyzerMock;

      protected static RaiseReactionStatisticsBuilder _sut;

      Establish context = () => {
         _analyzablePokerPlayerStub_1 = new Mock<IAnalyzablePokerPlayer>();
         _analyzablePokerPlayerStub_2 = new Mock<IAnalyzablePokerPlayer>();
         _raiseReactionAnalyzerStub = new Mock<IRaiseReactionAnalyzer>();
         _raiseReactionAnalyzerMake = new Constructor<IRaiseReactionAnalyzer>(() => _raiseReactionAnalyzerStub.Object);
         _raiseReactionStatisticsMock = new Mock<IRaiseReactionStatistics>();
         _raiseReactionsAnalyzerMock = new Mock<IRaiseReactionsAnalyzer>();

         _raiseReactionStatisticsMock
            .Setup(s => s.InitializeWith(Moq.It.IsAny<IRaiseReactionsAnalyzer>()))
            .Returns(_raiseReactionStatisticsMock.Object);

         _sut = new RaiseReactionStatisticsBuilder(_raiseReactionStatisticsMock.Object,
                                                   _raiseReactionsAnalyzerMock.Object,
                                                   _raiseReactionAnalyzerMake);
      };
   }

   [Subject(typeof(RaiseReactionStatisticsBuilder), "Build")]
   public class given_0_analyzable_players
      : RaiseReactionStatisticsBuilderSpecs
   {
      static Exception exception;

      Because of = () => exception = Catch.Exception(() => _sut.Build(Enumerable.Empty<IAnalyzablePokerPlayer>(), ActionSequences.HeroB, Streets.Flop));

      It should_throw_an_ArgumentException
         = () => exception.ShouldBeOfType(typeof(ArgumentException));
   }

   [Subject(typeof(RaiseReactionStatisticsBuilder), "Build")]
   public class given_2_analyzable_players
      :RaiseReactionStatisticsBuilderSpecs
   {
      const Streets PassedStreet = Streets.Flop;

      const ActionSequences PassedActionSequence = ActionSequences.HeroB;

      static IRaiseReactionStatistics returnedStatistics;

      Because of = 
         () => returnedStatistics = _sut.Build(
            new[] { _analyzablePokerPlayerStub_1.Object, _analyzablePokerPlayerStub_2.Object },
                    PassedActionSequence,
                    PassedStreet);

      It should_analyze_first_player_passing_the_given_street_and_action_sequence
         = () => _raiseReactionsAnalyzerMock.Verify(
                    a => a.AnalyzeAndAdd(
                            _raiseReactionAnalyzerStub.Object,
                            Moq.It.Is<IAnalyzablePokerPlayer>(p => p.Equals(_analyzablePokerPlayerStub_1.Object)),
                            PassedStreet,
                            PassedActionSequence));

      It should_analyze_second_player_passing_the_given_street_and_action_sequence
         = () => _raiseReactionsAnalyzerMock.Verify(
                    a => a.AnalyzeAndAdd(
                            _raiseReactionAnalyzerStub.Object,
                            Moq.It.Is<IAnalyzablePokerPlayer>(p => p.Equals(_analyzablePokerPlayerStub_2.Object)),
                            PassedStreet,
                            PassedActionSequence));


      It should_initialize_statistics_with_the_raise_reactions_analyzer
         = () => _raiseReactionStatisticsMock.Verify(
            s => s.InitializeWith(
               Moq.It.Is<IRaiseReactionsAnalyzer>(a => a.Equals(_raiseReactionsAnalyzerMock.Object))));

      It should_return_these_statistics
         = () => returnedStatistics.ShouldEqual(_raiseReactionStatisticsMock.Object);
   }
}