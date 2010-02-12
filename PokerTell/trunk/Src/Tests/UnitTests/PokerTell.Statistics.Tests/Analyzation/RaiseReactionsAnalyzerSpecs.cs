namespace PokerTell.Statistics.Tests.Analyzation
{
    // ReSharper disable InconsistentNaming
    using System;
    using System.Linq;

    using Infrastructure.Enumerations.PokerHand;
    using Infrastructure.Interfaces.PokerHand;

    using Interfaces;

    using Machine.Specifications;

    using Moq;

    using Statistics.Analyzation;

    using It = Machine.Specifications.It;

    public abstract class RaiseReactionsAnalyzerSpecs
    {
        /*
         *  Specifications
         *  Subject: RaiseReactionsAnalyzer
         *  
         *  Initialization
         *      given 0 raise sizes
         *        It should fail with an argument exception
         *      
         *      given 2 raise sizes
         *        It should create an empty RaiseReactionAnalyzers list
         *          
         *  Analyzation
         *      given AnalyzationPreparer was successfull
         *          and results are valid and standard
         *              It should add them to the RaiseReactionAnalyzers list
         *          but results are invalid
         *              It should not add them to the RaiseReactionAnalyzers list
         *          and results are valid but non standard  
         *              It should not add them to the RaiseReactionAnalyzers list
         *      given results would be valid but AnalyzationPreparer was unsuccessful
         *          It should not add anything to the list
        */

        protected static ActionSequences SomeActionSequence;

        protected static Streets SomeStreet;

        protected static Mock<IAnalyzablePokerPlayer> _analyzablePokerPlayerStub;

        protected static Mock<IReactionAnalyzationPreparer> _analyzationPreparerStub;

        protected static Mock<IRaiseReactionAnalyzer> _raiseReactionAnalyzerStub;

        protected static double[] _raiseSizeKeys;

        protected static IRaiseReactionsAnalyzer _sut;

        protected static bool _considerOpponentsRaiseSize;

        Establish context = () => {
            Moq.It.Is<string>(a => a == string.Empty);
            SomeStreet = Streets.Flop;
            SomeActionSequence = ActionSequences.HeroC;
            _considerOpponentsRaiseSize = true; 
            _analyzationPreparerStub = new Mock<IReactionAnalyzationPreparer>();
            _raiseReactionAnalyzerStub = new Mock<IRaiseReactionAnalyzer>();
            _analyzablePokerPlayerStub = new Mock<IAnalyzablePokerPlayer>();
            _analyzablePokerPlayerStub.SetupGet(p => p.Sequences).Returns(new IConvertedPokerRound[(int)Streets.River]);
        };
    }

    public abstract class Ctx_RaiseReactionsAnalyzer_2_raise_sizes
        : RaiseReactionsAnalyzerSpecs
    {
        Establish context = () => {
            _raiseSizeKeys = new[] { 1.0, 2.0 };
            _sut = new RaiseReactionsAnalyzer(_analyzationPreparerStub.Object).InitializeWith(_raiseSizeKeys);
        };
    }

    public abstract class Ctx_AnalyzationPreparer_Successfull
        : Ctx_RaiseReactionsAnalyzer_2_raise_sizes
    {
        Establish context = () => _analyzationPreparerStub.SetupGet(p => p.WasSuccessful).Returns(true);
    }

    [Subject(typeof(RaiseReactionsAnalyzer), "Initialization")]
    public class given_0_raise_sizes
        : RaiseReactionsAnalyzerSpecs
    {
        static Exception exception;

        Because of = () => {
            _raiseSizeKeys = new double[] { };
            exception = Catch.Exception(() => _sut = new RaiseReactionsAnalyzer(_analyzationPreparerStub.Object).InitializeWith(_raiseSizeKeys));
        };

        It should_fail_with_an_ArgumentException =
            () => exception.ShouldBeOfType<ArgumentException>();
    }

    [Subject(typeof(RaiseReactionsAnalyzer), "Initialization")]
    public class given_2_raise_sizes
        : Ctx_RaiseReactionsAnalyzer_2_raise_sizes
    {
        It should_create_an_empty_RaiseReactionAnlayzers_list =
            () => _sut.RaiseReactionAnalyzers.Count().ShouldEqual(0);
    }

    [Subject(typeof(RaiseReactionsAnalyzer), "Analyzation")]
    public class given_AnalyzationPreparer_was_successfull_and_results_are_valid_and_standard
        : Ctx_AnalyzationPreparer_Successfull
    {
        Because of = () => {
            _raiseReactionAnalyzerStub.SetupGet(r => r.IsValidResult).Returns(true);
            _raiseReactionAnalyzerStub.SetupGet(r => r.IsStandardSituation).Returns(true);
            _sut.AnalyzeAndAdd(
                _raiseReactionAnalyzerStub.Object, _analyzablePokerPlayerStub.Object, SomeStreet, SomeActionSequence, _considerOpponentsRaiseSize);
        };

        It should_add_them_to_the_RaiseReactionAnalyzers_list =
            () => _sut.RaiseReactionAnalyzers.ShouldContainOnly(_raiseReactionAnalyzerStub.Object);
    }

    [Subject(typeof(RaiseReactionsAnalyzer), "Analyzation")]
    public class given_AnalyzationPreparer_was_successfull_and_results_are_valid_but_non_standard
        : Ctx_AnalyzationPreparer_Successfull
    {
        Because of = () => {
            _raiseReactionAnalyzerStub.SetupGet(r => r.IsValidResult).Returns(true);
            _raiseReactionAnalyzerStub.SetupGet(r => r.IsStandardSituation).Returns(false);
            _sut.AnalyzeAndAdd(
                _raiseReactionAnalyzerStub.Object, _analyzablePokerPlayerStub.Object, SomeStreet, SomeActionSequence, _considerOpponentsRaiseSize);
        };

        It should_not_add_them_to_the_RaiseReactionAnalyzers_list =
            () => _sut.RaiseReactionAnalyzers.ShouldNotContain(_raiseReactionAnalyzerStub.Object);
    }

    [Subject(typeof(RaiseReactionsAnalyzer), "Analyzation")]
    public class given_AnalyzationPreparer_was_successfull_but_results_are_invalid
        : Ctx_AnalyzationPreparer_Successfull
    {
        Because of = () => {
            _raiseReactionAnalyzerStub.SetupGet(r => r.IsValidResult).Returns(false);
            _sut.AnalyzeAndAdd(
                _raiseReactionAnalyzerStub.Object, _analyzablePokerPlayerStub.Object, SomeStreet, SomeActionSequence, _considerOpponentsRaiseSize);
        };

        It should_not_add_them_to_the_RaiseReactionAnalyzers_list =
            () => _sut.RaiseReactionAnalyzers.ShouldNotContain(_raiseReactionAnalyzerStub.Object);
    }

    [Subject(typeof(RaiseReactionsAnalyzer), "Analyzation")]
    public class given_results_would_be_valid_but_AnalyzationPreparer_was_unsuccessfull
        : Ctx_RaiseReactionsAnalyzer_2_raise_sizes
    {
        Because of = () => {
            _analyzationPreparerStub.SetupGet(p => p.WasSuccessful).Returns(false);
            _raiseReactionAnalyzerStub.SetupGet(r => r.IsValidResult).Returns(true);
            _raiseReactionAnalyzerStub.SetupGet(r => r.IsStandardSituation).Returns(true);
            _sut.AnalyzeAndAdd(
                _raiseReactionAnalyzerStub.Object, _analyzablePokerPlayerStub.Object, SomeStreet, SomeActionSequence, _considerOpponentsRaiseSize);
        };

        It should_not_add_anything_to_the_list =
            () => _sut.RaiseReactionAnalyzers.ShouldNotContain(_raiseReactionAnalyzerStub.Object);
    }
}