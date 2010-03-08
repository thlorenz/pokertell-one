namespace PokerTell.Statistics.Tests.Analyzation
{
    using Infrastructure.Enumerations.PokerHand;
    using Infrastructure.Interfaces.PokerHand;

    using Interfaces;

    using Machine.Specifications;

    using Moq;

    using Statistics.Analyzation;

    using Tools.FunctionalCSharp;
    using Tools.Interfaces;

    using It = Machine.Specifications.It;

    // ReSharper disable InconsistentNaming

    public abstract class PostFlopHeroReactsRaiseReactionDescriberSpecs
    {
        /* 
           *  Specification
           *  Subject is PostFlopHeroActsRaiseReactionDescriber
           *  
           *  Description 
           *  
                when fred raised a bet 0.2 of the pot, when in position on the flop, and was reraised
                    » should contain " flop"  
                    » should contain " fred"   
                    » should contain " in position"  
                    » should contain " raised a bet"  
                    » should contain " was reraised"  
                when fred check raised a bet between 0.2 and 0.5 of the pot, when out of position on the turn, and was reraised
                    » should contain " and was raised"  
                    » should contain " check-raised a bet"  
                    » should contain " fred"   
                    » should contain " out of position"   
                    » should contain " turn"   
                when freds raise sizes are 0.2 and 0.5
                    » should contain " between 0.2 and 0.5" 
                when betsizes are 3x and 3x
                    » should contain " 3x"   
                    » should not contain " between 3 x and 3x"    
           */

        protected static Mock<IAnalyzablePokerPlayer> _analyzablePokerPlayerStub;

        protected static IPostFlopHeroReactsRaiseReactionDescriber _sut;

        protected static ITuple<double, double> _validBetSizes;

        Establish context = () => {
            _validBetSizes = Tuple.New(0.5, 0.7);
            _analyzablePokerPlayerStub = new Mock<IAnalyzablePokerPlayer>();
            _sut = new PostFlopHeroReactsRaiseReactionDescriber();
        };
    }

    public abstract class Ctx_PostFlopHeroReactsRaiseReactionDescriber_HeroRaisedABetOnFlop
        : PostFlopHeroReactsRaiseReactionDescriberSpecs
    {
        Establish context = () => {
            _analyzablePokerPlayerStub
                .SetupGet(p => p.ActionSequences)
                .Returns(new[] { ActionSequences.NonStandard, ActionSequences.OppBHeroR });
            _analyzablePokerPlayerStub
                .SetupGet(p => p.InPosition)
                .Returns(new bool?[] { null, true });
        };
    }

    [Subject(typeof(PostFlopHeroReactsRaiseReactionDescriber), "Description")]
    public class when_fred_raised_a_bet_02_when_in_position_on_the_flop_and_was_reraised
        : PostFlopHeroReactsRaiseReactionDescriberSpecs
    {
        static string description;

        Establish context = () => {
            _analyzablePokerPlayerStub
                .SetupGet(p => p.ActionSequences)
                .Returns(new[] { ActionSequences.NonStandard, ActionSequences.OppBHeroR });
            _analyzablePokerPlayerStub
                .SetupGet(p => p.InPosition)
                .Returns(new bool?[] { null, true });
        };

        Because of = () => description = _sut.Describe("fred", _analyzablePokerPlayerStub.Object, Streets.Flop, _validBetSizes);

        It should_contain__flop__
            = () => description.ShouldContain("flop");

        It should_contain__fred___
            = () => description.ShouldContain("fred");

        It should_contain__in_position__
            = () => description.ShouldContain("in position");

        It should_contain__raised_a_bet__
            = () => description.ShouldContain("raised a bet");

        It should_contain__was_reraised__
            = () => description.ShouldContain("was reraised");
    }

    [Subject(typeof(PostFlopHeroReactsRaiseReactionDescriber), "Description")]
    public class when_fred_check_raised_a_bet_between_0_2_and_0_5_when_out_of_position_on_the_turn_and_was_reraised
        : PostFlopHeroReactsRaiseReactionDescriberSpecs
    {
        static string description;

        Establish context = () => {
            _analyzablePokerPlayerStub
                .SetupGet(p => p.ActionSequences)
                .Returns(new[] { ActionSequences.NonStandard, ActionSequences.NonStandard, ActionSequences.HeroXOppBHeroR });
            _analyzablePokerPlayerStub
                .SetupGet(p => p.InPosition)
                .Returns(new bool?[] { null, null, false });
        };

        Because of = () => description = _sut.Describe("fred", _analyzablePokerPlayerStub.Object, Streets.Turn, _validBetSizes);

        It should_contain__and_was_raised__
            = () => description.ShouldContain("was reraised");

        It should_contain__check_raised_a_bet__
            = () => description.ShouldContain("check-raised a bet");

        It should_contain__fred__
            = () => description.ShouldContain("fred");

        It should_contain__out_of_position__
            = () => description.ShouldContain("out of position");

        It should_contain__turn__
            = () => description.ShouldContain("turn");
    }

    [Subject(typeof(PostFlopHeroReactsRaiseReactionDescriber), "Description")]
    public class when_freds_raise_sizes_are_0_2_and_0_5
        : Ctx_PostFlopHeroReactsRaiseReactionDescriber_HeroRaisedABetOnFlop
    {
        static string description;

        Establish context = () => { _validBetSizes = Tuple.New(0.2, 0.5); };

        Because of = () => description = _sut.Describe("fred", _analyzablePokerPlayerStub.Object, Streets.Flop, _validBetSizes);

        It should_contain__between_0_2_and_0_5__
            = () => description.ShouldContain("(0.2 to 0.5 of the pot)");
    }

    [Subject(typeof(PostFlopHeroReactsRaiseReactionDescriber), "Description")]
    public class when_betsizes_are_0_3_and_0_3_
        : Ctx_PostFlopHeroReactsRaiseReactionDescriber_HeroRaisedABetOnFlop
    {
        static string description;

        Establish context = () => { _validBetSizes = Tuple.New(0.3, 0.3); };

        Because of = () => description = _sut.Describe("fred", _analyzablePokerPlayerStub.Object, Streets.Flop, _validBetSizes);

        It should_contain_0_3___
            = () => description.ShouldContain("0.3");

        It should_not_contain__between_0_3_and_0_3___
            = () => description.Contains("(0.3 to 0.3)").ShouldBeFalse();
    }
}