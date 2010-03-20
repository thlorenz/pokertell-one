namespace PokerTell.Statistics.Tests.Detailed
{
    using Infrastructure.Enumerations.PokerHand;

    using Machine.Specifications;

    using Statistics.Detailed;

    // Resharper DisableInconsistentNaming
    public abstract class DetailedPostFlopHeroActsStatisticsDescriberSpecs
    {
        protected static DetailedPostFlopHeroActsStatisticsDescriber _sut;

        Establish basicContext = () => _sut = new DetailedPostFlopHeroActsStatisticsDescriber();

        protected static string description;

        protected static string hint;

        [Subject(typeof(DetailedPostFlopHeroActsStatisticsDescriber), "Describe")]
        public class given_fred_bet_on_the_flop_in_position : DetailedPostFlopHeroActsStatisticsDescriberSpecs
        {
            Because of = () => description = _sut.Describe("fred", ActionSequences.HeroActs, Streets.Flop, true);
           
            It should_return___fred_bet_on_the_flop_when_in_position__ = () => description.ShouldContain("fred bet on the flop when in position.");
        }

        [Subject(typeof(DetailedPostFlopHeroActsStatisticsDescriber), "Describe")]
        public class given_fred_bet_on_the_river_out_of_position : DetailedPostFlopHeroActsStatisticsDescriberSpecs
        {
            Because of = () => description = _sut.Describe("fred", ActionSequences.HeroActs, Streets.River, false);
           
            It should_return___fred_bet_on_the_river_when_out_of_position__ = () => description.ShouldContain("fred bet on the river when out of position.");
        }

        [Subject(typeof(DetailedPostFlopHeroActsStatisticsDescriber), "Hint")]
        public class when_obtaining_hint_for_player_fred : DetailedPostFlopHeroActsStatisticsDescriberSpecs
        {
            Because of = () => hint = _sut.Hint("fred", ActionSequences.NonStandard, false);
            It returns___The_table_indicates_how_often_fred_bets_a_certain_fraction_of_the_pot___ 
                = () => hint.ShouldContain("The table indicates how often fred bet a certain fraction of the pot.");
        }
    }
}