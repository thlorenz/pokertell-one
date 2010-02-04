namespace PokerTell.Statistics.Tests.Analyzation
{
    using Factories;

    using Infrastructure;
    using Infrastructure.Enumerations.PokerHand;
    using Infrastructure.Interfaces.PokerHand;

    using Interfaces;

    using Machine.Specifications;

    using Moq;

    using Statistics.Analyzation;

    using It = Machine.Specifications.It;

    // ReSharper disable InconsistentNaming
    public class RaiseReactionAnalyzerSpecs
    {
        /*
         Specifications
         Subject: RaiseReactionAnalyzer
         
         * Preflop Analyzation
         *   
         *      hero called opp raised 6x another opp folded  and hero raised
                    » should be standard situaion
                    » should be valid
                    » should set OpponentRaiseSize to 5x since it is normalized to standard raise keys
                    » should set hero reaction to raise

                hero called opp raised 6x another opp reraised and hero folded
                    » should be non standard situation
                    » should be valid
                    » should set hero reaction to fold

                opp raised hero reraised opp rereraised 2x no one else reraised and hero called
                    » should be standard situation
                    » should be valid
                    » should set OpponentRaiseSize to 2x
                    » should set hero reaction to call
                opp raised hero reraised opp rereraised 2x another opp reraised and hero raised
                    » should be non standard situation
                    » should be valid
                    » should set hero reaction to raise
         */

        protected const int HeroPosition = 2;

        protected static IReactionAnalyzationPreparer _analyzationPreparer;

        protected static double[] _raiseSizes;

        protected static StubBuilder _stub;

        protected static IRaiseReactionAnalyzer _sut;

        Establish context = () => {
            _stub = new StubBuilder();
            _analyzationPreparer = new ReactionAnalyzationPreparer();
            _sut = new RaiseReactionAnalyzer();
            _raiseSizes = ApplicationProperties.RaiseSizeKeys;
        };
    }

    [Subject(typeof(RaiseReactionAnalyzer), "Preflop Analyzation")]
    public class hero_called_opp_raised_6x_another_opp_folded__and_hero_raised
        : RaiseReactionAnalyzerSpecs
    {
        Establish context = () => {
            IConvertedPokerRound sequence = "[2]C,[1]R6.0,[3]F,[2]R2.0".ToConvertedPokerRoundWithIds();
            _analyzationPreparer.PrepareAnalyzationFor(sequence, HeroPosition, ActionSequences.HeroC);
        };

        Because of = () => _sut.AnalyzeUsingDataFrom(_stub.Out<IAnalyzablePokerPlayer>(), _analyzationPreparer, _raiseSizes);

        It should_be_standard_situaion
            = () => _sut.IsStandardSituation.ShouldBeTrue();

        It should_be_valid
            = () => _sut.IsValidResult.ShouldBeTrue();

        It should_set_OpponentRaiseSize_to_5x_since_it_is_normalized_to_standard_raise_keys
            = () => _sut.OpponentRaiseSize.ShouldEqual(5);

        It should_set_hero_reaction_to_raise
            = () => _sut.HeroReactionType.ShouldEqual(ActionTypes.R);
    }

    [Subject(typeof(RaiseReactionAnalyzer), "Preflop Analyzation")]
    public class hero_called_opp_raised_6x_another_opp_reraised_and_hero_folded
        : RaiseReactionAnalyzerSpecs
    {
        Establish context = () => {
            IConvertedPokerRound sequence = "[2]C,[1]R6.0,[3]R3.0,[2]F".ToConvertedPokerRoundWithIds();
            _analyzationPreparer.PrepareAnalyzationFor(sequence, HeroPosition, ActionSequences.HeroC);
        };

        Because of = () => _sut.AnalyzeUsingDataFrom(_stub.Out<IAnalyzablePokerPlayer>(), _analyzationPreparer, _raiseSizes);

        It should_be_non_standard_situation
            = () => _sut.IsStandardSituation.ShouldBeFalse();

        It should_be_valid
            = () => _sut.IsValidResult.ShouldBeTrue();

        It should_set_hero_reaction_to_fold
            = () => _sut.HeroReactionType.ShouldEqual(ActionTypes.F);
    }

    [Subject(typeof(RaiseReactionAnalyzer), "Preflop Analyzation")]
    public class opp_raised_hero_reraised_opp_rereraised_2x_no_one_else_reraised_and_hero_called
        : RaiseReactionAnalyzerSpecs
    {
        Establish context = () => {
            IConvertedPokerRound sequence = "[1]R3.0,[2]R5.0,[1]R2.0,[2]C".ToConvertedPokerRoundWithIds();
            _analyzationPreparer.PrepareAnalyzationFor(sequence, HeroPosition, ActionSequences.OppRHeroR);
        };

        Because of = () => _sut.AnalyzeUsingDataFrom(_stub.Out<IAnalyzablePokerPlayer>(), _analyzationPreparer, _raiseSizes);

        It should_be_standard_situation
            = () => _sut.IsStandardSituation.ShouldBeTrue();

        It should_be_valid
            = () => _sut.IsValidResult.ShouldBeTrue();

        It should_set_OpponentRaiseSize_to_2x
            = () => _sut.OpponentRaiseSize.ShouldEqual(2);

        It should_set_hero_reaction_to_call
            = () => _sut.HeroReactionType.ShouldEqual(ActionTypes.C);
    }

    [Subject(typeof(RaiseReactionAnalyzer), "Preflop Analyzation")]
    public class opp_raised_hero_reraised_opp_rereraised_2x_another_opp_reraised_and_hero_raised
        : RaiseReactionAnalyzerSpecs
    {
        Establish context = () =>
        {
            IConvertedPokerRound sequence = "[1]R3.0,[2]R5.0,[3]C,[1]R2.0,[3]R3.0,[2]R2.0".ToConvertedPokerRoundWithIds();
            _analyzationPreparer.PrepareAnalyzationFor(sequence, HeroPosition, ActionSequences.OppRHeroR);
        };

        Because of = () => _sut.AnalyzeUsingDataFrom(_stub.Out<IAnalyzablePokerPlayer>(), _analyzationPreparer, _raiseSizes);

        It should_be_non_standard_situation
            = () => _sut.IsStandardSituation.ShouldBeFalse();

        It should_be_valid
            = () => _sut.IsValidResult.ShouldBeTrue();

        It should_set_hero_reaction_to_raise
            = () => _sut.HeroReactionType.ShouldEqual(ActionTypes.R);
    }
}