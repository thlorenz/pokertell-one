namespace PokerTell.Statistics.Tests.Detailed
{
    using Machine.Specifications;

    using PokerTell.Infrastructure.Enumerations.PokerHand;
    using PokerTell.Statistics.Detailed;

    // Resharper DisableInconsistentNaming
    public abstract class DetailedPostFlopHeroReactsStatisticsDescriberSpecs
    {
        protected static DetailedPostFlopHeroReactsStatisticsDescriber _sut;

        Establish basicContext = () => _sut = new DetailedPostFlopHeroReactsStatisticsDescriber();

        protected static string description;

        protected static string hint;

        [Subject(typeof(DetailedPostFlopHeroActsStatisticsDescriber), "Describe")]
        public class given_fred__reacted_to_a_bet_on_the_flop_in_position : DetailedPostFlopHeroReactsStatisticsDescriberSpecs
        {
            Because of = () => description = _sut.Describe("fred", ActionSequences.OppB, Streets.Flop, true);

            It should_return__fred_reacted_to_a_bet_on_the_flop_when_in_position__
                = () => description.ShouldContain("fred reacted to a bet on the flop when in position.");
        }

        [Subject(typeof(DetailedPostFlopHeroActsStatisticsDescriber), "Describe")]
        public class given_fred_checked_and_then_reacted_to_a_bet_on_the_river_out_of_position :
            DetailedPostFlopHeroReactsStatisticsDescriberSpecs
        {
            Because of = () => description = _sut.Describe("fred", ActionSequences.HeroXOppB, Streets.River, false);

            It should_return__fred_checked_first_and_then_reacted_to_a_bet_on_the_river_when_out_of_position__
                =
                () => description.ShouldContain("fred checked first and then reacted to a bet on the river when out of position.");
        }

        [Subject(typeof(DetailedPostFlopHeroActsStatisticsDescriber), "Hint")]
        public class when_obtaining_hint_for_player_fred : DetailedPostFlopHeroReactsStatisticsDescriberSpecs
        {
            Because of = () => hint = _sut.Hint("fred", ActionSequences.NonStandard, false);

            It returns___The_table_indicates_how_often_fred_bets_a_certain_fraction_of_the_pot___
                = () => hint.ShouldContain("The table shows how fred reacted depending on the bet size of the opponent");
        }
    }
}