namespace PokerTell.Statistics.Tests.Analyzation
{
    using System.Collections.Generic;
    using System.Linq;

    using Infrastructure.Services;

    using Machine.Specifications;

    using Moq;

    using PokerTell.Infrastructure.Enumerations.PokerHand;
    using PokerTell.Infrastructure.Interfaces.PokerHand;
    using PokerTell.PokerHand.Analyzation;
    using PokerTell.Statistics.Analyzation;
    using PokerTell.Statistics.Interfaces;

    using It = Machine.Specifications.It;

    // Resharper disable InconsistentNaming
    public abstract class PreFlopHandStrengthStatisticsSpecs
    {
        protected static double[] _unraisedPotCallingRatios;

        protected static double[] _raisedPotCallingRatios;

        protected static double[] _raiseSizeKeys;

        protected static IPreFlopHandStrengthStatistics _sut;

        protected static Mock<IAnalyzablePokerPlayer> _hero1;

        protected static Mock<IAnalyzablePokerPlayer> _hero2;

        protected static Mock<IAnalyzablePokerPlayer> _hero3;

        protected static IEnumerable<IAnalyzablePokerPlayer> _analyzablePokerPlayers;

        protected static ActionSequences _validActionSequence;

        protected static string _validHoleCards;

        protected static Mock<IValuedHoleCards> _valuedHoleCardsMock;

        protected static Mock<IValuedHoleCardsAverage> _valuedHoleCardsAverageMock;

        Establish specContext = () => {
            _validHoleCards = "As Ah";
            _validActionSequence = ActionSequences.HeroC;
            _unraisedPotCallingRatios = new[] { 0.1, 0.2 };
            _raisedPotCallingRatios = new[] { 0.3, 0.4 };
            _raiseSizeKeys = new[] { 1.0, 2.0, 3.0 };

            _valuedHoleCardsMock = new Mock<IValuedHoleCards>();
            _valuedHoleCardsMock.Setup(h => h.InitializeWith(Moq.It.IsAny<string>())).Returns(_valuedHoleCardsMock.Object);
            _valuedHoleCardsAverageMock = new Mock<IValuedHoleCardsAverage>();

            _hero1 = new Mock<IAnalyzablePokerPlayer>();
            _hero2 = new Mock<IAnalyzablePokerPlayer>();
            _hero3 = new Mock<IAnalyzablePokerPlayer>();
            _hero1.SetupGet(h => h.Position).Returns(1);
            _hero2.SetupGet(h => h.Position).Returns(2);
            _hero3.SetupGet(h => h.Position).Returns(3);

            _analyzablePokerPlayers = new[] { _hero1.Object, _hero2.Object, _hero3.Object };

            _sut = new PreFlopHandStrengthStatistics(new Constructor<IValuedHoleCards>(() => _valuedHoleCardsMock.Object),
                                                     _valuedHoleCardsAverageMock.Object)
                .InitializeWith(_unraisedPotCallingRatios,
                                _raisedPotCallingRatios, 
                                _raiseSizeKeys);
        };

        public abstract class Ctx_AllPlayersWith_HeroC_Ratio_is_First_Column_ValuedHoleCards_AreValid :
            PreFlopHandStrengthStatisticsSpecs
        {
            Establish context = () => {
                _hero1.SetupGet(h => h.Sequences).Returns(new[] { "[1]C0.1".ToConvertedPokerRound() });

                _hero2.SetupGet(h => h.Sequences).Returns(new[] { "[2]C0.1".ToConvertedPokerRound() });

                _hero3.SetupGet(h => h.Sequences).Returns(new[] { "[3]C0.1".ToConvertedPokerRound() });
                _valuedHoleCardsMock.SetupGet(v => v.AreValid).Returns(true);
            };
        }

        [Subject(typeof(PreFlopHandStrengthStatistics), "BuildStatisticsFor, if valued holecards are always valid")]
        public class given_hero1_has_known_cards_hero2_has_unknown_cards_and_hero3_has_empty_string :
            Ctx_AllPlayersWith_HeroC_Ratio_is_First_Column_ValuedHoleCards_AreValid
        {
            Establish context = () => {
                _hero1.SetupGet(h => h.Holecards).Returns(_validHoleCards);
                _hero2.SetupGet(h => h.Holecards).Returns("?? ??");
                _hero3.SetupGet(h => h.Holecards).Returns(string.Empty);
            };

            Because of = () => _sut.BuildStatisticsFor(_analyzablePokerPlayers, _validActionSequence);

            It AnalyzablePlayersWithKnownCards_should_contain_hero1 =
                () => _sut.AnalyzablePlayersWithKnownCards.ShouldContain(_hero1.Object);

            It AnalyzablePlayersWithKnownCards_should_not_contain_hero2 =
                () => _sut.AnalyzablePlayersWithKnownCards.ShouldNotContain(_hero2.Object);

            It AnalyzablePlayersWithKnownCards_should_not_contain_hero3 =
                () => _sut.AnalyzablePlayersWithKnownCards.ShouldNotContain(_hero3.Object);
        }

        [Subject(typeof(PreFlopHandStrengthStatistics), "BuildStatisticsFor, if valued holecards are always valid")]
        public class given_hero1_and_hero2_have_known_cards_hero3_has_empty_string :
            Ctx_AllPlayersWith_HeroC_Ratio_is_First_Column_ValuedHoleCards_AreValid
        {
            Establish context = () =>
            {
                _hero1.SetupGet(h => h.Holecards).Returns(_validHoleCards);
                _hero2.SetupGet(h => h.Holecards).Returns(_validHoleCards);
                _hero3.SetupGet(h => h.Holecards).Returns(string.Empty);
            };

            Because of = () => _sut.BuildStatisticsFor(_analyzablePokerPlayers, _validActionSequence);

            It KnownCards_in_col_0_should_have_count_2 = () => _sut.KnownCards[0].Count().ShouldEqual(2);


            It KnownCards_in_col_1_should_have_count_0 = () => _sut.KnownCards[1].Count().ShouldEqual(0);
        }
        [Subject(typeof(PreFlopHandStrengthStatistics), "BuildStatisticsFor")]
        public class given_HeroC_as_ActionSequence : PreFlopHandStrengthStatisticsSpecs
        {
            Because of = () => _sut.BuildStatisticsFor(_analyzablePokerPlayers, ActionSequences.HeroC);

            It RatiosUsed_should_equal_unraised_pot_calling_ratios = () => _sut.RatiosUsed.ShouldEqual(_unraisedPotCallingRatios);
        }

        [Subject(typeof(PreFlopHandStrengthStatistics), "BuildStatisticsFor")]
        public class given_OppRHeroC_as_ActionSequence : PreFlopHandStrengthStatisticsSpecs
        {
            Because of = () => _sut.BuildStatisticsFor(_analyzablePokerPlayers, ActionSequences.OppRHeroC);

            It RatiosUsed_should_equal_raised_pot_calling_ratios = () => _sut.RatiosUsed.ShouldEqual(_raisedPotCallingRatios);
        }

        [Subject(typeof(PreFlopHandStrengthStatistics), "BuildStatisticsFor")]
        public class given_HeroR_as_ActionSequence : PreFlopHandStrengthStatisticsSpecs
        {
            Because of = () => _sut.BuildStatisticsFor(_analyzablePokerPlayers, ActionSequences.HeroR);

            It RatiosUsed_should_equal_raise_size_keys = () => _sut.RatiosUsed.ShouldEqual(_raiseSizeKeys);
        }

        public abstract class Ctx_all_have_known_cards : PreFlopHandStrengthStatisticsSpecs
        {
            Establish context = () => {
                _hero1.SetupGet(h => h.Holecards).Returns(_validHoleCards);
                _hero2.SetupGet(h => h.Holecards).Returns(_validHoleCards);
                _hero3.SetupGet(h => h.Holecards).Returns(_validHoleCards);
            };
        }

        [Subject(typeof(PreFlopHandStrengthStatistics), 
            "BuildStatisticsFor, all have known cards and ActionSequence is HeroC, given 2 ratios for unraised pot")]
        public class given_hero1_has_calling_ratio1_hero2_has_calling_ratio2_and_hero_3_has_calling_ratio_larger_than_the_last_one :
            Ctx_all_have_known_cards
        {
            Establish context = () => {
                _hero1.SetupGet(h => h.Sequences).Returns(new[] { "[1]C0.1".ToConvertedPokerRound() });
                _hero2.SetupGet(h => h.Sequences).Returns(new[] { "[2]C0.2".ToConvertedPokerRound() });

                // Hero3 has other players actions in his sequence, but it will still be found using his position as id
                _hero3.SetupGet(h => h.Sequences).Returns(new[] { "[0]F,[1]C0.1,[2]C0.2,[3]C0.9".ToConvertedPokerRound() });
            };

            Because of = () => _sut.BuildStatisticsFor(_analyzablePokerPlayers, ActionSequences.HeroC);

            It SortedAnalyzablePokerPlayers_should_have_same_length_as_there_are_items_in_unraised_pot_calling_ratios
                = () => _sut.SortedAnalyzablePokerPlayersWithKnownCards.Length.ShouldEqual(_unraisedPotCallingRatios.Length);

            It SortedAnalyzablePokerPlayers_at_col_0_should_contain_only_hero1
                = () => _sut.SortedAnalyzablePokerPlayersWithKnownCards[0].ShouldContainOnly(_hero1.Object);

            It SortedAnalyzablePokerPlayers_at_col_1_should_contain_only_hero2_and_hero3
                = () => _sut.SortedAnalyzablePokerPlayersWithKnownCards[1].ShouldContainOnly(_hero2.Object, _hero3.Object);
        }

        [Subject(typeof(PreFlopHandStrengthStatistics), "BuildStatisticsFor")]
        public class given_hero1_with_HeroC_has_cards_AsKs_ValuedHoleCards_AreValid : PreFlopHandStrengthStatisticsSpecs
        {
            const string HoleCards = "As Ks";
            const int AverageChenValue = 1;
            const int AverageSklanskyMalmuthGrouping = 11;

            Establish context = () => {
                _hero1.SetupGet(h => h.Holecards).Returns(HoleCards);
                _hero1.SetupGet(h => h.Sequences).Returns(new[] { "[1]C0.1".ToConvertedPokerRound() });
                _analyzablePokerPlayers = new[] { _hero1.Object };
                _valuedHoleCardsMock.SetupGet(h => h.AreValid).Returns(true);
                _valuedHoleCardsAverageMock.SetupGet(v => v.IsValid).Returns(true);
                _valuedHoleCardsAverageMock.SetupGet(v => v.ChenValue).Returns(AverageChenValue);
                _valuedHoleCardsAverageMock.SetupGet(v => v.SklanskyMalmuthGrouping).Returns(AverageSklanskyMalmuthGrouping);
            };

            Because of = () => _sut.BuildStatisticsFor(_analyzablePokerPlayers, ActionSequences.HeroC);

            It should_initialize_valuedHoleCards_with_his_holecards = () => _valuedHoleCardsMock.Verify(h => h.InitializeWith(HoleCards));

            It KnownCards_should_have_same_length_as_unraisedPotCallingRatios = () => _sut.KnownCards.Length.ShouldEqual(_unraisedPotCallingRatios.Length);

            It should_add_valued_hole_cards_to_col_0_of_KnownCards = () => _sut.KnownCards[0].ShouldContain(_valuedHoleCardsMock.Object);

            It should_initialize_the_average_valued_hole_cards_with_a_list_containing_the_valued_hole_cards = () => 
                _valuedHoleCardsAverageMock.Verify(a => a.InitializeWith(Moq.It.Is<IEnumerable<IValuedHoleCards>>(l => l.Contains(_valuedHoleCardsMock.Object))));

            It should_set_the_AverageChenValue_at_col_0_to_the_one_returned_by_the_valued_hole_cards_average
                = () => _sut.AverageChenValues[0].ShouldEqual(AverageChenValue.ToString());

            It should_set_the_AverageSklanskyMalmuthGrouping_at_col_0_to_the_one_returned_by_the_valued_hole_cards_average
                = () => _sut.AverageSklanskyMalmuthGroupings[0].ShouldEqual(AverageSklanskyMalmuthGrouping.ToString());
        }

        [Subject(typeof(PreFlopHandStrengthStatistics), "BuildStatisticsFor")]
        public class given_hero1_with_HeroC_has_cards_AsKs_ValuedHoleCards_AreInValid : PreFlopHandStrengthStatisticsSpecs
        {
            const string HoleCards = "As Ks";

            Establish context = () => {
                _hero1.SetupGet(h => h.Holecards).Returns(HoleCards);
                _hero1.SetupGet(h => h.Sequences).Returns(new[] { "[1]C0.1".ToConvertedPokerRound() });
                _analyzablePokerPlayers = new[] { _hero1.Object };
                _valuedHoleCardsMock.SetupGet(h => h.AreValid).Returns(false);
                _valuedHoleCardsAverageMock.SetupGet(v => v.IsValid).Returns(false);
            };

            Because of = () => _sut.BuildStatisticsFor(_analyzablePokerPlayers, ActionSequences.HeroC);

            It should_not_add_valued_hole_cards_to_col_0_of_KnownCards = () => _sut.KnownCards[0].ShouldNotContain(_valuedHoleCardsMock.Object);
                
            It should_initialize_the_average_valued_hole_cards_with_an_empty_list = () => 
                _valuedHoleCardsAverageMock.Verify(a => a.InitializeWith(Moq.It.Is<IEnumerable<IValuedHoleCards>>(l => l.Count() == 0)));

            It should_set_the_AverageChenValue_at_col_0_to_not_available
                = () => _sut.AverageChenValues[0].ShouldEqual("na");

            It should_set_the_AverageSklanskyMalmuthGrouping_at_col_0_to_not_available
                = () => _sut.AverageSklanskyMalmuthGroupings[0].ShouldEqual("na");
        }
    }

    public static class PreFlopHandStrengthStatisticsSpecsUtils
    {
        public static IConvertedPokerRound ToConvertedPokerRound(this string round)
        {
            return new PokerHandStringConverter().ConvertedRoundFrom(round);
        }
    }
}