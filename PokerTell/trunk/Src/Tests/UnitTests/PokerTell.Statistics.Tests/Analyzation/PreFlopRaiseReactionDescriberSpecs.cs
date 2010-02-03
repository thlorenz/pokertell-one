namespace PokerTell.Statistics.Tests.Analyzation
{
    using Infrastructure.Enumerations.PokerHand;

    using PokerTell.Infrastructure.Interfaces.PokerHand;
    using PokerTell.Statistics.Interfaces;

    using Machine.Specifications;

    using Moq;

    using PokerTell.Statistics.Analyzation;

    using Tools.FunctionalCSharp;
    using Tools.Interfaces;

    using It = Machine.Specifications.It;

    // ReSharper disable InconsistentNaming

    public class PreFlopRaiseReactionDescriberSpecs
    {
        protected static Mock<IAnalyzablePokerPlayer> _analyzablePokerPlayerStub;

        protected static string _playerName;

        protected static IPostFlopHeroActsRaiseReactionDescriber _sut;

        protected static ITuple<double, double> _validBetSizes;

        Establish context = () =>
        {
            _validBetSizes = Tuple.New(0.5, 0.7);
            _playerName = "somePlayer";
            _analyzablePokerPlayerStub = new Mock<IAnalyzablePokerPlayer>();
            _sut = new PreFlopRaiseReactionDescriber();
        };
    }

    public class when_fred_called_preflop_in_unraised_pot
      : PreFlopRaiseReactionDescriberSpecs
    {
        static string description;

        Establish context = () => _analyzablePokerPlayerStub
                                      .SetupGet(p => p.ActionSequences)
                                      .Returns(new[] { ActionSequences.HeroC });

        Because of =
           () => description = _sut.Describe("fred", _analyzablePokerPlayerStub.Object, Streets.PreFlop, _validBetSizes);

        It should_contain__called__in_description
           = () => description.ShouldContain("called");

        It should_contain__preflop__in_description
           = () => description.ShouldContain("preflop");

        It should_contain__fred___in_description
           = () => description.Contains("fred").ShouldBeTrue();

        It should_contain__unraised_pot__in_description
           = () => description.ShouldContain("unraised pot");

        It should_contain_and_was_raised_in_description
           = () => description.ShouldContain("and was raised");
    }

    public class when_fred_raised_preflop_in_raised_pot
  : PreFlopRaiseReactionDescriberSpecs
    {
        static string description;

        Establish context = () => _analyzablePokerPlayerStub
                                      .SetupGet(p => p.ActionSequences)
                                      .Returns(new[] { ActionSequences.OppRHeroR });

        Because of =
           () => description = _sut.Describe("fred", _analyzablePokerPlayerStub.Object, Streets.PreFlop, _validBetSizes);

        It should_contain__raised__in_description
           = () => description.ShouldContain("raised");

        It should_contain__preflop__in_description
           = () => description.ShouldContain("preflop");

        It should_contain__fred___in_description
           = () => description.Contains("fred").ShouldBeTrue();

        It should_contain__unraised_pot__in_description
           = () => description.ShouldContain(" raised pot");

        It should_contain_and_was_raised_in_description
           = () => description.ShouldContain("and was raised");
    }

}