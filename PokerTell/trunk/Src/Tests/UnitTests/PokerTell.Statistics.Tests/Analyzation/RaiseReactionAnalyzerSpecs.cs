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
                    » should set ConsideredRaiseSize to 5x since it is normalized to standard raise keys
                    » should set hero reaction to raise

                hero called opp raised 6x another opp reraised and hero folded
                    » should be non standard situation
                    » should be valid
                    » should set hero reaction to fold

                opp raised hero reraised opp rereraised 2x no one else reraised and hero called
                    » should be standard situation
                    » should be valid
                    » should set ConsideredRaiseSize to 2x
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
        Establish basicContext = () =>
        {
            _stub = new StubBuilder();
            _analyzationPreparer = new ReactionAnalyzationPreparer();
            _sut = new RaiseReactionAnalyzer();
            _raiseSizes = ApplicationProperties.RaiseSizeKeys;
        };


        public class Ctx_Considering_Opponents_RaiseSize : RaiseReactionAnalyzerSpecs
        {
            protected static bool _considerOpponentsRaiseSize;

            Establish considerOppRaiseSizeContext = () => _considerOpponentsRaiseSize = true;
        }

        public class Ctx_Considering_Heros_RaiseSize : RaiseReactionAnalyzerSpecs
        {
            protected static bool _doNotConsiderOpponentsRaiseSize;

            Establish considerHeroRaiseSizeContext = () => _doNotConsiderOpponentsRaiseSize = false;
        }
        [Subject(typeof(RaiseReactionAnalyzer), "Preflop Analyzation")]
        public class hero_called_opp_raised_6x_another_opp_folded__and_hero_raised
            : Ctx_Considering_Opponents_RaiseSize
        {
            Establish context = () =>
            {
                IConvertedPokerRound sequence = "[2]C,[1]R6.0,[3]F,[2]R2.0".ToConvertedPokerRoundWithIds();
                _analyzationPreparer.PrepareAnalyzationFor(sequence, HeroPosition, ActionSequences.HeroC);
            };

            Because of = () => _sut.AnalyzeUsingDataFrom(_stub.Out<IAnalyzablePokerPlayer>(), _analyzationPreparer, true, _raiseSizes);

            It should_be_standard_situaion
                = () => _sut.IsStandardSituation.ShouldBeTrue();

            It should_be_valid
                = () => _sut.IsValidResult.ShouldBeTrue();

            It should_set_OpponentRaiseSize_to_5x_since_it_is_normalized_to_standard_raise_keys
                = () => _sut.ConsideredRaiseSize.ShouldEqual(5);

            It should_set_hero_reaction_to_raise
                = () => _sut.HeroReactionType.ShouldEqual(ActionTypes.R);
        }

        [Subject(typeof(RaiseReactionAnalyzer), "Preflop Analyzation")]
        public class hero_called_opp_raised_6x_another_opp_reraised_and_hero_folded
            : Ctx_Considering_Opponents_RaiseSize
        {
            Establish context = () =>
            {
                IConvertedPokerRound sequence = "[2]C,[1]R6.0,[3]R3.0,[2]F".ToConvertedPokerRoundWithIds();
                _analyzationPreparer.PrepareAnalyzationFor(sequence, HeroPosition, ActionSequences.HeroC);
            };

            Because of = () => _sut.AnalyzeUsingDataFrom(_stub.Out<IAnalyzablePokerPlayer>(), _analyzationPreparer, true, _raiseSizes);

            It should_be_non_standard_situation
                = () => _sut.IsStandardSituation.ShouldBeFalse();

            It should_be_valid
                = () => _sut.IsValidResult.ShouldBeTrue();

            It should_set_hero_reaction_to_fold
                = () => _sut.HeroReactionType.ShouldEqual(ActionTypes.F);
        }

        [Subject(typeof(RaiseReactionAnalyzer), "Preflop Analyzation")]
        public class opp_raised_hero_reraised_opp_rereraised_2x_no_one_else_reraised_and_hero_called
            : Ctx_Considering_Heros_RaiseSize
        {
            Establish context = () =>
            {
                IConvertedPokerRound sequence = "[1]R3.0,[2]R5.0,[1]R2.0,[2]C".ToConvertedPokerRoundWithIds();
                _analyzationPreparer.PrepareAnalyzationFor(sequence, HeroPosition, ActionSequences.OppRHeroR);
            };

            Because of = () => _sut.AnalyzeUsingDataFrom(_stub.Out<IAnalyzablePokerPlayer>(), _analyzationPreparer, true, _raiseSizes);

            It should_be_standard_situation
                = () => _sut.IsStandardSituation.ShouldBeTrue();

            It should_be_valid
                = () => _sut.IsValidResult.ShouldBeTrue();

            It should_set_OpponentRaiseSize_to_2x
                = () => _sut.ConsideredRaiseSize.ShouldEqual(2);

            It should_set_hero_reaction_to_call
                = () => _sut.HeroReactionType.ShouldEqual(ActionTypes.C);
        }

        [Subject(typeof(RaiseReactionAnalyzer), "Preflop Analyzation")]
        public class opp_raised_hero_reraised_opp_rereraised_2x_another_opp_reraised_and_hero_raised
            : Ctx_Considering_Heros_RaiseSize
        {
            Establish context = () =>
            {
                IConvertedPokerRound sequence = "[1]R3.0,[2]R5.0,[3]C,[1]R2.0,[3]R3.0,[2]R2.0".ToConvertedPokerRoundWithIds();
                _analyzationPreparer.PrepareAnalyzationFor(sequence, HeroPosition, ActionSequences.OppRHeroR);
            };

            Because of = () => _sut.AnalyzeUsingDataFrom(_stub.Out<IAnalyzablePokerPlayer>(), _analyzationPreparer, true, _raiseSizes);

            It should_be_non_standard_situation
                = () => _sut.IsStandardSituation.ShouldBeFalse();

            It should_be_valid
                = () => _sut.IsValidResult.ShouldBeTrue();

            It should_set_hero_reaction_to_raise
                = () => _sut.HeroReactionType.ShouldEqual(ActionTypes.R);
        }

        [Subject(typeof(RaiseReactionAnalyzer), "PostFlop Analyzation, HeroActs")]
        public class hero_bet_opp_raised_2x_no_one_reraised_and_hero_folded
            : Ctx_Considering_Opponents_RaiseSize
        {
            Establish context = () =>
            {
                IConvertedPokerRound sequence = "[2]B0.2,[1]R2.0,[2]F".ToConvertedPokerRoundWithIds();
                _analyzationPreparer.PrepareAnalyzationFor(sequence, HeroPosition, ActionSequences.HeroB);
            };

            Because of = () => _sut.AnalyzeUsingDataFrom(_stub.Out<IAnalyzablePokerPlayer>(), _analyzationPreparer, _considerOpponentsRaiseSize, _raiseSizes);

            It should_be_standard_situation
                = () => _sut.IsStandardSituation.ShouldBeTrue();

            It should_be_valid
                = () => _sut.IsValidResult.ShouldBeTrue();

            It should_set_hero_reaction_to_fold
                = () => _sut.HeroReactionType.ShouldEqual(ActionTypes.F);

            It should_set_considered_raise_size_to___2__ = () => _sut.ConsideredRaiseSize.ShouldEqual(2);
        }

        [Subject(typeof(RaiseReactionAnalyzer), "PostFlop Analyzation, HeroReacts")]
        public class opponent_bet_hero_raised_3x_and_was_reraised_5x_by_the_same_opponent_without_intermediate_action_and_finally_called
            : Ctx_Considering_Heros_RaiseSize
        {
            Establish context = () =>
            {
                IConvertedPokerRound sequence = "[1]B0.2,[2]R3.0,[1]R5.0,[2]C".ToConvertedPokerRoundWithIds();
                _analyzationPreparer.PrepareAnalyzationFor(sequence, HeroPosition, ActionSequences.OppBHeroR);
            };

            Because of = () => _sut.AnalyzeUsingDataFrom(_stub.Out<IAnalyzablePokerPlayer>(), _analyzationPreparer, _doNotConsiderOpponentsRaiseSize, _raiseSizes);

            It should_be_standard_situation
                = () => _sut.IsStandardSituation.ShouldBeTrue();

            It should_be_valid
                = () => _sut.IsValidResult.ShouldBeTrue();

            It should_set_hero_reaction_to_call
                = () => _sut.HeroReactionType.ShouldEqual(ActionTypes.C);

            It should_set_considered_raise_size_to___3__ = () => _sut.ConsideredRaiseSize.ShouldEqual(3);
        }

        [Subject(typeof(RaiseReactionAnalyzer), "PostFlop Analyzation, HeroReacts")]
        public class hero_checked_then_opponent_bet_and_hero_raised_it_5x_the_opponent_reraised_2x_without_intermediate_action_and_finally_the_hero_reraised
            : Ctx_Considering_Heros_RaiseSize
        {
            Establish context = () =>
            {
                IConvertedPokerRound sequence = "[2]X,[1]B0.7,[2]R5.0,[1]R2.0,[2]R3.0".ToConvertedPokerRoundWithIds();
                _analyzationPreparer.PrepareAnalyzationFor(sequence, HeroPosition, ActionSequences.HeroXOppBHeroR);
            };

            Because of = () => _sut.AnalyzeUsingDataFrom(_stub.Out<IAnalyzablePokerPlayer>(), _analyzationPreparer, _doNotConsiderOpponentsRaiseSize, _raiseSizes);

            It should_be_standard_situation
                = () => _sut.IsStandardSituation.ShouldBeTrue();

            It should_be_valid
                = () => _sut.IsValidResult.ShouldBeTrue();

            It should_set_hero_reaction_to_call
                = () => _sut.HeroReactionType.ShouldEqual(ActionTypes.R);

            It should_set_considered_raise_size_to___5__ = () => _sut.ConsideredRaiseSize.ShouldEqual(5);
        }

        [Subject(typeof(RaiseReactionAnalyzer), "PostFlop Analyzation, HeroReacts")]
        public class opponent_bet_hero_raised_3x_and_was_reraised_5x_by_the_same_opponent_and_before_he_reacted_another_opponent_reraised
            : Ctx_Considering_Heros_RaiseSize
        {
            Establish context = () =>
            {
                IConvertedPokerRound sequence = "[1]B0.2,[2]R3.0,[1]R5.0,[3]R2.0,[2]C".ToConvertedPokerRoundWithIds();
                _analyzationPreparer.PrepareAnalyzationFor(sequence, HeroPosition, ActionSequences.OppBHeroR);
            };

            Because of = () => _sut.AnalyzeUsingDataFrom(_stub.Out<IAnalyzablePokerPlayer>(), _analyzationPreparer, _doNotConsiderOpponentsRaiseSize, _raiseSizes);

            It should_not_be_standard_situation = () => _sut.IsStandardSituation.ShouldBeFalse();

            It should_be_valid = () => _sut.IsValidResult.ShouldBeTrue();
        }

        [Subject(typeof(RaiseReactionAnalyzer), "PostFlop Analyzation, HeroReacts")]
        public class hero_checked_then_opponent_bet_and_hero_raised_it_5x_the_opponent_reraised_2x_and_before_he_reacted_another_opponent_raised
            : Ctx_Considering_Heros_RaiseSize
        {
            Establish context = () =>
            {
                IConvertedPokerRound sequence = "[2]X,[1]B0.7,[2]R5.0,[1]R2.0,[3]R5.0,[2]R3.0".ToConvertedPokerRoundWithIds();
                _analyzationPreparer.PrepareAnalyzationFor(sequence, HeroPosition, ActionSequences.HeroXOppBHeroR);
            };

            Because of = () => _sut.AnalyzeUsingDataFrom(_stub.Out<IAnalyzablePokerPlayer>(), _analyzationPreparer, _doNotConsiderOpponentsRaiseSize, _raiseSizes);

            It should_not_be_standard_situation = () => _sut.IsStandardSituation.ShouldBeFalse();

            It should_be_valid = () => _sut.IsValidResult.ShouldBeTrue();
        }

        [Subject(typeof(RaiseReactionAnalyzer), "PostFlop Analyzation, HeroReacts")]
        public class ActionSequence_indicates_that_opponent_bet_hero_raised_and_was_reraised_but_no_hero_raise_action_is_contained
            : Ctx_Considering_Heros_RaiseSize
        {
            Establish context = () =>
            {
                IConvertedPokerRound sequence = "[1]B0.2,[1]R5.0,[2]C".ToConvertedPokerRoundWithIds();
                _analyzationPreparer.PrepareAnalyzationFor(sequence, HeroPosition, ActionSequences.OppBHeroR);
            };

            Because of = () => _sut.AnalyzeUsingDataFrom(_stub.Out<IAnalyzablePokerPlayer>(), _analyzationPreparer, _doNotConsiderOpponentsRaiseSize, _raiseSizes);

            It should_be_invalid = () => _sut.IsValidResult.ShouldBeFalse();
        }

        [Subject(typeof(RaiseReactionAnalyzer), "PostFlop Analyzation, HeroReacts")]
        public class ActionSequence_indicates_that_opponent_bet_hero_raised_and_was_reraised_but_no_opponent_raise_action_is_contained
            : Ctx_Considering_Heros_RaiseSize
        {
            Establish context = () =>
            {
                IConvertedPokerRound sequence = "[1]B0.2,[2]R2.0,[2]C".ToConvertedPokerRoundWithIds();
                _analyzationPreparer.PrepareAnalyzationFor(sequence, HeroPosition, ActionSequences.OppBHeroR);
            };

            Because of = () => _sut.AnalyzeUsingDataFrom(_stub.Out<IAnalyzablePokerPlayer>(), _analyzationPreparer, _doNotConsiderOpponentsRaiseSize, _raiseSizes);

            It should_be_invalid = () => _sut.IsValidResult.ShouldBeFalse();
        }
    }
}