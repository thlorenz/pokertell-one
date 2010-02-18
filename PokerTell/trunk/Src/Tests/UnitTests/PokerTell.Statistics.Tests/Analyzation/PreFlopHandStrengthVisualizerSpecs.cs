namespace PokerTell.Statistics.Tests.Analyzation
{
    using System.Collections.Generic;

    using Machine.Specifications;

    using Statistics.Analyzation;

    // Resharper disable InconsistentNaming
    public abstract class PreFlopHandStrengthVisualizerSpecs
    {
        /*
                PreFlopStartingHandsVisualizer 
                IntializeWith
                   given sideLength 1 pairMargin 0 » StartingHands should have a key for each possible starting hand 13x13
                        » StartingHands with key " AA"  should have coordinates top 0 left 0
                        » StartingHands with key " AK"  should have coordinates top 1 left 0
                        » StartingHands with key " 22"  should have coordinates top 12 left 12
                        » StartingHands with key " T7"  should have coordinates top 7 left 4
                        » StartingHands with key " AKs"  should have coordinates top 0 left 1
                        » StartingHands with key " T7s"  should have coordinates top 4 left 7
                        » StartingHands with key " AK"  should have Display " AK" 
                        » StartingHands with key " AKs"  should also have Display " AK" 

                    given sideLength 2 pairMargin 0
                        » StartingHands with key " AK"  should have coordinates top 2 left 0
                        » StartingHands with key " T7"  should have coordinates top 14 left 8
                        » StartingHands with key " AKs"  should have coordinates top 0 left 2
                        » StartingHands with key " T7s"  should have coordinates top 8 left 14

                    given sideLength 1 pairMargin 1
                        » StartingHands with key " AA"  should have coordinates top 0 left 1
                        » StartingHands with key " AK"  should have coordinates top 1 left 0
                        » StartingHands with key " T7"  should have coordinates top 7 left 4
                        » StartingHands with key " AKs"  should have coordinates top 0 left 3
                        » StartingHands with key " T7s"  should have coordinates top 4 left 9

                 Visualize
                    given sidelength 10 and 4 AA 2 T7 and 1 32s as known cards
                        » StartingHands with key " AA"  should have count 4
                        » StartingHands with key " T7"  should have count 2
                        » StartingHands with key " 32s"  should have count 1
                        » StartingHands with key " AA"  should have FillHeight 10
                        » StartingHands with key " T7"  should have FillHeight 5
                        » StartingHands with key " 32s"  should have FillHeight one fourth

                   when visualizing twice with 1 AA
                        » StartingHands with key " AA"  should have count 1 because it resets the StartingHand Counts each time
         */
        protected static PreFlopStartingHandsVisualizer _sut;

        Establish specContext = () => {
           _sut = new PreFlopStartingHandsVisualizer();  
        };

        [Subject(typeof(PreFlopStartingHandsVisualizer), "IntializeWith")]
        public class given_sideLength_1_pairMargin_0 : PreFlopHandStrengthVisualizerSpecs
        {
            Because of = () => _sut.InitializeWith(1, 0);

             It StartingHands_should_have_a_key_for_each_possible_starting_hand_13x13 = () => _sut.StartingHands.Keys.Count.ShouldEqual(169);

            It StartingHands_with_key___AA___should_have_coordinates_top_0_left_0 = () => {
                _sut.StartingHands["AA"].Top.ShouldEqual(0);
                _sut.StartingHands["AA"].Left.ShouldEqual(0);
            };

            It StartingHands_with_key___AK___should_have_coordinates_top_1_left_0 = () => {
                _sut.StartingHands["AK"].Top.ShouldEqual(1);
                _sut.StartingHands["AK"].Left.ShouldEqual(0);
            };

            It StartingHands_with_key___22___should_have_coordinates_top_12_left_12 = () => {
                _sut.StartingHands["22"].Top.ShouldEqual(12);
                _sut.StartingHands["22"].Left.ShouldEqual(12);
            };

            It StartingHands_with_key___T7___should_have_coordinates_top_7_left_4 = () => {
                _sut.StartingHands["T7"].Top.ShouldEqual(7);
                _sut.StartingHands["T7"].Left.ShouldEqual(4);
            };

            It StartingHands_with_key___AKs___should_have_coordinates_top_0_left_1 = () => {
                _sut.StartingHands["AKs"].Top.ShouldEqual(0);
                _sut.StartingHands["AKs"].Left.ShouldEqual(1);
            };

            It StartingHands_with_key___T7s___should_have_coordinates_top_4_left_7 = () => {
                _sut.StartingHands["T7s"].Top.ShouldEqual(4);
                _sut.StartingHands["T7s"].Left.ShouldEqual(7);
            };

            It StartingHands_with_key___AK___should_have_Display___AK__ = () => _sut.StartingHands["AK"].Display.ShouldEqual("AK");

            It StartingHands_with_key___AKs___should_also_have_Display___AK__ = () => _sut.StartingHands["AKs"].Display.ShouldEqual("AK");
        }

        [Subject(typeof(PreFlopStartingHandsVisualizer), "IntializeWith")]
        public class given_sideLength_2_pairMargin_0 : PreFlopHandStrengthVisualizerSpecs
        {
            Because of = () => _sut.InitializeWith(2, 0);

            It StartingHands_with_key___AK___should_have_coordinates_top_2_left_0 = () => {
                _sut.StartingHands["AK"].Top.ShouldEqual(2);
                _sut.StartingHands["AK"].Left.ShouldEqual(0);
            };

            It StartingHands_with_key___T7___should_have_coordinates_top_14_left_8 = () => {
                _sut.StartingHands["T7"].Top.ShouldEqual(14);
                _sut.StartingHands["T7"].Left.ShouldEqual(8);
            };

            It StartingHands_with_key___AKs___should_have_coordinates_top_0_left_2 = () => {
                _sut.StartingHands["AKs"].Top.ShouldEqual(0);
                _sut.StartingHands["AKs"].Left.ShouldEqual(2);
            };

            It StartingHands_with_key___T7s___should_have_coordinates_top_8_left_14 = () => {
                _sut.StartingHands["T7s"].Top.ShouldEqual(8);
                _sut.StartingHands["T7s"].Left.ShouldEqual(14);
            };
        }

        [Subject(typeof(PreFlopStartingHandsVisualizer), "IntializeWith")]
        public class given_sideLength_1_pairMargin_1 : PreFlopHandStrengthVisualizerSpecs
        {
            Because of = () => _sut.InitializeWith(1, 1);

            It StartingHands_with_key___AA___should_have_coordinates_top_0_left_1 = () => {
                _sut.StartingHands["AA"].Top.ShouldEqual(0);
                _sut.StartingHands["AA"].Left.ShouldEqual(1);
            };

            It StartingHands_with_key___AK___should_have_coordinates_top_1_left_0 = () => {
                _sut.StartingHands["AK"].Top.ShouldEqual(1);
                _sut.StartingHands["AK"].Left.ShouldEqual(0);
            };

            It StartingHands_with_key___T7___should_have_coordinates_top_7_left_4 = () => {
                _sut.StartingHands["T7"].Top.ShouldEqual(7);
                _sut.StartingHands["T7"].Left.ShouldEqual(4);
            };

            It StartingHands_with_key___AKs___should_have_coordinates_top_0_left_3 = () => {
                _sut.StartingHands["AKs"].Top.ShouldEqual(0);
                _sut.StartingHands["AKs"].Left.ShouldEqual(3);
            };

            It StartingHands_with_key___T7s___should_have_coordinates_top_4_left_9 = () => {
                _sut.StartingHands["T7s"].Top.ShouldEqual(4);
                _sut.StartingHands["T7s"].Left.ShouldEqual(9);
            };
        }

        [Subject(typeof(PreFlopStartingHandsVisualizer), "Visualize")]
        public class given_sidelength_10_and_4_AA_2_T7_and_1_32s_as_known_cards : PreFlopHandStrengthVisualizerSpecs
        {
            static string[] _valuedHoleCards;

            Establish context = () => _valuedHoleCards = new[] { "AA", "AA", "AA", "AA", "T7", "T7", "32s" };

            Because of = () => _sut.InitializeWith(10, 0).Visualize(_valuedHoleCards);

            It StartingHands_with_key___AA___should_have_count_4 = () => _sut.StartingHands["AA"].Count.ShouldEqual(4);

            It StartingHands_with_key___T7___should_have_count_2 = () => _sut.StartingHands["T7"].Count.ShouldEqual(2);

            It StartingHands_with_key___32s___should_have_count_1 = () => _sut.StartingHands["32s"].Count.ShouldEqual(1);

            It StartingHands_with_key___AA___should_have_FillHeight_10 = () => _sut.StartingHands["AA"].FillHeight.ShouldEqual(10);

            It StartingHands_with_key___T7___should_have_FillHeight_5 = () => _sut.StartingHands["T7"].FillHeight.ShouldEqual(5);

            It StartingHands_with_key___32s___should_have_FillHeight_one_fourth = () => _sut.StartingHands["32s"].FillHeight.ShouldEqual(2.5);
        }

        [Subject(typeof(PreFlopStartingHandsVisualizer), "Visualize")]
        public class when_visualizing_twice_with_1_AA : PreFlopHandStrengthVisualizerSpecs
        {
            Because of = () => _sut.InitializeWith(1, 0).Visualize(new[] { "AA" }).Visualize(new[] { "AA" });

            It StartingHands_with_key___AA___should_have_count_1_because_it_resets_the_StartingHand_Counts_each_time
                = () => _sut.StartingHands["AA"].Count.ShouldEqual(1);
        }
    }
}