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

    using It = Moq.It;
    using it = Machine.Specifications.It;

    [Subject(typeof(RaiseReactionsAnalyzer), "Constructor")]
    public class given_0_raise_sizes
        : RaiseReactionsAnalyzerSpecs
    {
        static Exception exception;

        Because of = () => {
            _raiseSizeKeys = new double[] { };
            exception = Catch.Exception(() => _sut = new RaiseReactionsAnalyzer(_raiseSizeKeys, _analyzationPreparerStub.Object));
        };

        it should_fail_with_an_ArgumentException =
            () => exception.ShouldBeOfType<ArgumentException>();
    }

    [Subject(typeof(RaiseReactionsAnalyzer), "Constructor")]
    public class given_2_raise_sizes
        : RaiseReactionsAnalyzer_2_raise_sizes_Ctx
    {
        it should_create_an_empty_RaiseReactionAnlayzers_list =
            () => _sut.RaiseReactionAnalyzers.Count().ShouldEqual(0);
    }

    [Subject(typeof(RaiseReactionsAnalyzer), "Analyzation")]
    public class given_AnalyzationPreparer_was_successfull_and_results_are_valid_and_standard
        : AnalyzationPreparer_Successfull_Ctx
    {
        Because of = () => {
            _raiseReactionAnalyzerStub.SetupGet(r => r.IsValidResult).Returns(true);
            _raiseReactionAnalyzerStub.SetupGet(r => r.IsStandardSituation).Returns(true);
            _sut.AnalyzeAndAdd(
                _raiseReactionAnalyzerStub.Object, _analyzablePokerPlayerStub.Object, SomeStreet, SomeActionSequence);
        };

        it should_add_them_to_the_RaiseReactionAnalyzers_list =
            () => _sut.RaiseReactionAnalyzers.ShouldContainOnly(_raiseReactionAnalyzerStub.Object);
    }

    [Subject(typeof(RaiseReactionsAnalyzer), "Analyzation")]
    public class given_AnalyzationPreparer_was_successfull_and_results_are_valid_but_non_standard
        : AnalyzationPreparer_Successfull_Ctx
    {
        Because of = () => {
            _raiseReactionAnalyzerStub.SetupGet(r => r.IsValidResult).Returns(true);
            _raiseReactionAnalyzerStub.SetupGet(r => r.IsStandardSituation).Returns(false);
            _sut.AnalyzeAndAdd(
                _raiseReactionAnalyzerStub.Object, _analyzablePokerPlayerStub.Object, SomeStreet, SomeActionSequence);
        };

        it should_not_add_them_to_the_RaiseReactionAnalyzers_list =
            () => _sut.RaiseReactionAnalyzers.ShouldNotContain(_raiseReactionAnalyzerStub.Object);
    }

    [Subject(typeof(RaiseReactionsAnalyzer), "Analyzation")]
    public class given_AnalyzationPreparer_was_successfull_but_results_are_invalid
        : AnalyzationPreparer_Successfull_Ctx
    {
        Because of = () => {
            _raiseReactionAnalyzerStub.SetupGet(r => r.IsValidResult).Returns(false);
            _sut.AnalyzeAndAdd(
                _raiseReactionAnalyzerStub.Object, _analyzablePokerPlayerStub.Object, SomeStreet, SomeActionSequence);
        };

        it should_not_add_them_to_the_RaiseReactionAnalyzers_list =
            () => _sut.RaiseReactionAnalyzers.ShouldNotContain(_raiseReactionAnalyzerStub.Object);
    }

    [Subject(typeof(RaiseReactionsAnalyzer), "Analyzation")]
    public class given_results_would_be_valid_but_AnalyzationPreparer_was_unsuccessfull
        : RaiseReactionsAnalyzer_2_raise_sizes_Ctx
    {
        Because of = () => {
            _analyzationPreparerStub.SetupGet(p => p.WasSuccessful).Returns(false);
            _raiseReactionAnalyzerStub.SetupGet(r => r.IsValidResult).Returns(true);
            _raiseReactionAnalyzerStub.SetupGet(r => r.IsStandardSituation).Returns(true);
            _sut.AnalyzeAndAdd(
                _raiseReactionAnalyzerStub.Object, _analyzablePokerPlayerStub.Object, SomeStreet, SomeActionSequence);
        };

        it should_not_add_anything_to_the_list =
            () => _sut.RaiseReactionAnalyzers.ShouldNotContain(_raiseReactionAnalyzerStub.Object);
    }

    public abstract class RaiseReactionsAnalyzerSpecs
    {
        /*
         *  Specifications
         *  Subject: RaiseReactionsAnalyzer
         *  
         *  Constructor
         *      * given 0 raise sizes
         *          it should fail with an argument exception
         *      
         *      * given 2 raise sizes
         *          it should create an empty RaiseReactionAnalyzers list
         *          
         *  Analyzation
         *      * given AnalyzationPreparer was successfull
         *          * and results are valid and standard
         *              it should add them to the RaiseReactionAnalyzers list
         *          * but results are invalid
         *              it should not add them to the RaiseReactionAnalyzers list
         *          * and results are valid but non standard  
         *              it should not add them to the RaiseReactionAnalyzers list
         *      * given results would be valid but AnalyzationPreparer was unsuccessful
         *          it should not add anything to the list
        */

        protected static IRaiseReactionsAnalyzer _sut;

        protected static double[] _raiseSizeKeys;

        protected static Mock<IReactionAnalyzationPreparer> _analyzationPreparerStub;

        protected static Mock<IRaiseReactionAnalyzer> _raiseReactionAnalyzerStub;

        protected static Mock<IAnalyzablePokerPlayer> _analyzablePokerPlayerStub;

        protected static Streets SomeStreet;

        protected static ActionSequences SomeActionSequence;

        Establish context = () => {
            It.Is<string>(a => a == string.Empty);
            SomeStreet = Streets.Flop;
            SomeActionSequence = ActionSequences.HeroC;

            _analyzationPreparerStub = new Mock<IReactionAnalyzationPreparer>();
            _raiseReactionAnalyzerStub = new Mock<IRaiseReactionAnalyzer>();
            _analyzablePokerPlayerStub = new Mock<IAnalyzablePokerPlayer>();
            _analyzablePokerPlayerStub.SetupGet(p => p.Sequences).Returns(new IConvertedPokerRound[(int)Streets.River]);
        };
    }

    public abstract class RaiseReactionsAnalyzer_2_raise_sizes_Ctx
        : RaiseReactionsAnalyzerSpecs
    {
        Establish context = () => {
            _raiseSizeKeys = new[] { 1.0, 2.0 };
            _sut = new RaiseReactionsAnalyzer(_raiseSizeKeys, _analyzationPreparerStub.Object);
        };
    }

    public abstract class AnalyzationPreparer_Successfull_Ctx
        : RaiseReactionsAnalyzer_2_raise_sizes_Ctx
    {
        Establish context = () => _analyzationPreparerStub.SetupGet(p => p.WasSuccessful).Returns(true);
    }
}