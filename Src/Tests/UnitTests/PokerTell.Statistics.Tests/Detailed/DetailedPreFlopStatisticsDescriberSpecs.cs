namespace PokerTell.Statistics.Tests.Detailed
{
    using Infrastructure.Enumerations.PokerHand;

    using Machine.Specifications;

    using Statistics.Detailed;

    // Resharper DisableInconsistentNaming
    public abstract class DetailedPreFlopStatisticsDescriberSpecs
    {
        protected static string description;

        protected static string hint;

        Establish basicContext = () => _sut = new DetailedPreFlopStatisticsDescriber();

        protected static DetailedPreFlopStatisticsDescriber _sut;

        [Subject(typeof(DetailedPreFlopStatisticsDescriber), "Describe")]
        public class given_fred_acted_in_an_unraised_pot : DetailedPreFlopStatisticsDescriberSpecs
        {
            Because of = () => description = _sut.Describe("fred", ActionSequences.PreFlopNoFrontRaise);

            It returns___fred_acted_preflop_in_an_unraised_pot__ = () => description.ShouldContain("fred acted preflop in an unraised pot.");
        } 
        [Subject(typeof(DetailedPreFlopStatisticsDescriber), "Describe")]
        public class given_fred_acted_in_a_raised_pot : DetailedPreFlopStatisticsDescriberSpecs
        {
            Because of = () => description = _sut.Describe("fred", ActionSequences.PreFlopFrontRaise);

            It returns___fred_acted_preflop_in_a_raised_pot__ = () => description.ShouldContain("fred acted preflop in a raised pot.");
        }

        [Subject(typeof(DetailedPreFlopStatisticsDescriber), "Hint")]
        public class when_obtaining_hint_for_player_fred
        {
            Because of = () => hint = _sut.Hint("fred");

            It returns_The_table_shows_how_fred_acted_when_sitting_in_a_certain_position_at_the_table 
                = () => hint.ShouldContain("The table shows how fred acted when sitting in a certain position at the table");
        }
    }
}