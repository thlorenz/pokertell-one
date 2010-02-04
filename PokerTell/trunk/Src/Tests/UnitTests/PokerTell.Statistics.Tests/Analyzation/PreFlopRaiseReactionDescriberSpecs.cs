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

    public class PreFlopRaiseReactionDescriberSpecs
    {
        /* 
        *  Specification
        *  Subject is PreFlopRaiseReactionDescriber
         
           Description, 
                given fred called preflop in unraised pot
                    » should contain "and was raised" 
                    » should contain "called" 
                    » should contain "fred" 
                    » should contain "preflop" 
                    » should contain "unraised pot" 

                given fred raised preflop in raised pot
                    » should contain " raised pot" 
                    » should contain "and was raised" 
                    » should contain "fred" 
                    » should contain "preflop" 
                    » should contain "raised" 

            Positions, 
                describing 2 different positions
                    » returns "in between middle position and late position" for MI and LT
                    » returns in between the small blind and early position  for SB and EA
                    » returns in between the big blind and the button  for BB and BU

                describing 2 equal positions
                    » returns "in cutoff position" for CO and CO
                    » should not contain " in between"        
         */

        protected static Mock<IAnalyzablePokerPlayer> _analyzablePokerPlayerStub;

        protected static string _playerName;

        protected static IPreFlopRaiseReactionDescriber _sut;

        protected static ITuple<StrategicPositions, StrategicPositions> _validPositions;

        protected static string description;

        Establish context = () => {
            _validPositions = Tuple.New(StrategicPositions.SB, StrategicPositions.BB);
            _playerName = "somePlayer";
            _analyzablePokerPlayerStub = new Mock<IAnalyzablePokerPlayer>();
            _sut = new PreFlopRaiseReactionDescriber();
        };
    }

    public class Ctx_PreFlopRaiseReactionDescriber_PlayerCalledInUnraisedPot
        : PreFlopRaiseReactionDescriberSpecs
    {
        Establish context = () => _analyzablePokerPlayerStub
                                      .SetupGet(p => p.ActionSequences)
                                      .Returns(new[] { ActionSequences.HeroC });

        protected static string LetValidSutDecribePositions(StrategicPositions fromPosition, StrategicPositions toPosition)
        {
            _validPositions = Tuple.New(fromPosition, toPosition);
            return _sut.Describe("fred", _analyzablePokerPlayerStub.Object, Streets.PreFlop, _validPositions);
        }
    }

    [Subject(typeof(PreFlopRaiseReactionDescriber), "Description")]
    public class given_fred_called_preflop_in_unraised_pot
        : PreFlopRaiseReactionDescriberSpecs
    {
        Establish context = () => _analyzablePokerPlayerStub
                                      .SetupGet(p => p.ActionSequences)
                                      .Returns(new[] { ActionSequences.HeroC });

        Because of =
            () =>
            description = _sut.Describe("fred", _analyzablePokerPlayerStub.Object, Streets.PreFlop, _validPositions);

        It should_contain__and_was_raised__
            = () => description.ShouldContain("and was raised");

        It should_contain__called__
            = () => description.ShouldContain("called");

        It should_contain__fred__
            = () => description.Contains("fred").ShouldBeTrue();

        It should_contain__preflop__
            = () => description.ShouldContain("preflop");

        It should_contain__unraised_pot__
            = () => description.ShouldContain("an unraised pot");
    }

    [Subject(typeof(PreFlopRaiseReactionDescriber), "Description")]
    public class given_fred_raised_preflop_in_raised_pot
        : PreFlopRaiseReactionDescriberSpecs
    {
        Establish context = () => _analyzablePokerPlayerStub
                                      .SetupGet(p => p.ActionSequences)
                                      .Returns(new[] { ActionSequences.OppRHeroR });

        Because of =
            () =>
            description = _sut.Describe("fred", _analyzablePokerPlayerStub.Object, Streets.PreFlop, _validPositions);

        It should_contain___raised_pot__
            = () => description.ShouldContain(" raised pot");

        It should_contain__and_was_raised__
            = () => description.ShouldContain("and was raised");

        It should_contain__fred__
            = () => description.Contains("fred").ShouldBeTrue();

        It should_contain__preflop__
            = () => description.ShouldContain("preflop");

        It should_contain__raised__
            = () => description.ShouldContain("raised");
    }

    [Subject(typeof(PreFlopRaiseReactionDescriber), "Positions")]
    public class describing_2_different_positions
        : Ctx_PreFlopRaiseReactionDescriber_PlayerCalledInUnraisedPot
    {
        It returns__in_between_middle_position_and_late_position__for_MI_and_LT
            = () => LetValidSutDecribePositions(StrategicPositions.MI, StrategicPositions.LT)
                        .ShouldContain("in between middle position and late position");

        It returns__in_between_the_big_blind_and_the_button__for_BB_and_BU
            = () => LetValidSutDecribePositions(StrategicPositions.BB, StrategicPositions.BU)
                        .ShouldContain("in between the big blind and the button");

        It returns_in__between_the_small_blind_and_early_position__for_SB_and_EA
            = () => LetValidSutDecribePositions(StrategicPositions.SB, StrategicPositions.EA)
                        .ShouldContain("in between the small blind and early position");
    }

    [Subject(typeof(PreFlopRaiseReactionDescriber), "Positions")]
    public class describing_2_equal_positions
        : Ctx_PreFlopRaiseReactionDescriber_PlayerCalledInUnraisedPot
    {
        It returns__in_cutoff_position__for_CO_and_CO
            = () => LetValidSutDecribePositions(StrategicPositions.CO, StrategicPositions.CO)
                        .ShouldContain("in the cut-off");

        It should_not_contain___in_between__
            = () => LetValidSutDecribePositions(StrategicPositions.CO, StrategicPositions.CO)
                        .Contains("in between").ShouldBeFalse();
    }
}