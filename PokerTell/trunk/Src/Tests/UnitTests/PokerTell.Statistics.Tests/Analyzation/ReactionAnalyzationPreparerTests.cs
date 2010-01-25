namespace PokerTell.Statistics.Tests.Analyzation
{
    using System;

    using Infrastructure.Enumerations.PokerHand;
    using Infrastructure.Interfaces.PokerHand;

    using Moq;

    using NUnit.Framework;

    using PokerHand.Analyzation;

    using Statistics.Analyzation;

    using UnitTests.Tools;

    public class ReactionAnalyzationPreparerTests
    {
        #region Constants and Fields

        protected IConvertedPokerHand ConvertedHand;

        const string HeroName = "hero";

        const string OpponentName = "someOpponent";

        const int TotalPlayers = 9;

        StubBuilder _stub;

        #endregion

        #region Public Methods

        [SetUp]
        public void A_Init()
        {
            _stub = new StubBuilder();
            ConvertedHand = new ConvertedPokerHand(
                "someSite", 23, DateTime.MinValue, _stub.Some<double>(), _stub.Some<double>(), TotalPlayers);

            SetupHeroInSeat2AndOpponentInSeat1();
        }

        [Test]
        [Sequential]
        public void Constructor_FlopHeroActsAndFirstMatchingHeroActionIsAtIndexOne_StartingAcionIndexIsOne(
            [Values("[1]X,[2]X,[3]C", "[1]X,[2]B,[3]C")] string sequenceString,
            [Values(ActionSequences.HeroX, ActionSequences.HeroB)] ActionSequences actionSequence)
        {
            ConvertedHand.SequenceFlop = sequenceString;

            var sut = new ReactionAnalyzationPreparer(ConvertedHand, Streets.Flop, HeroName, actionSequence);

            Assert.That(sut.StartingActionIndex, Is.EqualTo(1));
        }

        [Test]
        [Sequential]
        public void Constructor_FlopOppBAndFirstMatchingHeroReactionIsAtIndexOne_StartingAcionIndexIsOne(
            [Values("[1]B,[2]F,[3]C", "[1]B,[2]C,[3]C", "[1]B,[2]R,[3]C")] string sequenceString,
            [Values(ActionSequences.OppBHeroF, ActionSequences.OppBHeroC, ActionSequences.OppBHeroR)] ActionSequences actionSequence)
        {
            ConvertedHand.SequenceFlop = sequenceString;

            var sut = new ReactionAnalyzationPreparer(ConvertedHand, Streets.Flop, HeroName, actionSequence);

            Assert.That(sut.StartingActionIndex, Is.EqualTo(1));
        }


        [Test]
        [Sequential]
        public void Constructor_FlopHeroXOppBAndFirstMatchingHeroReactionIsAtIndexTwo_StartingAcionIndexIsTwo(
            [Values("[2]X,[1]B,[2]F", "[2]X,[1]B,[2]C", "[2]X,[1]B,[2]R")] string sequenceString,
            [Values(ActionSequences.HeroXOppBHeroF, ActionSequences.HeroXOppBHeroC, ActionSequences.HeroXOppBHeroR)] ActionSequences actionSequence)
        {
            ConvertedHand.SequenceFlop = sequenceString;

            var sut = new ReactionAnalyzationPreparer(ConvertedHand, Streets.Flop, HeroName, actionSequence);

            Assert.That(sut.StartingActionIndex, Is.EqualTo(2));
        }

        [Test]
        public void Constructor_HeroIsInSeat2_HeroIndexIs2()
        {
            var sut = new ReactionAnalyzationPreparer(ConvertedHand, Streets.PreFlop, HeroName, ActionSequences.HeroC);

            sut.HeroIndex.ShouldBeEqualTo(2);
        }

        [Test]
        [Sequential]
        public void Constructor_PreFlopRaisedPotAndFirstMatchingHeroActionIsAtIndexOne_StartingAcionIndexIsOne(
            [Values("[1]C,[2]F,[3]C", "[1]C,[2]C,[3]C", "[1]C,[2]R,[3]C")] string sequenceString,
            [Values(ActionSequences.HeroF, ActionSequences.HeroC, ActionSequences.HeroR)] ActionSequences actionSequence)
        {
            ConvertedHand.SequencePreFlop = sequenceString;

            var sut = new ReactionAnalyzationPreparer(ConvertedHand, Streets.PreFlop, HeroName, actionSequence);

            Assert.That(sut.StartingActionIndex, Is.EqualTo(1));
        }

        [Test]
        [Sequential]
        public void Constructor_PreFlopUnraisedPotAndFirstMatchingHeroActionIsAtIndexOne_StartingAcionIndexIsOne(
            [Values("[1]C,[2]F,[3]C", "[1]C,[2]C,[3]C", "[1]C,[2]R,[3]C")] string sequenceString,
            [Values(ActionSequences.HeroF, ActionSequences.HeroC, ActionSequences.HeroR)] ActionSequences actionSequence)
        {
            ConvertedHand.SequencePreFlop = sequenceString;

            var sut = new ReactionAnalyzationPreparer(ConvertedHand, Streets.PreFlop, HeroName, actionSequence);

            Assert.That(sut.StartingActionIndex, Is.EqualTo(1));
        }

        [Test]
        public void Constructor_StreetIsFlop_SequenceIsFlopSequenceOfHand()
        {
            ConvertedHand.SequenceFlop = "[2]C,[3]X";

            var sut = new ReactionAnalyzationPreparer(ConvertedHand, Streets.Flop, HeroName, ActionSequences.HeroC);

            sut.Sequence.ShouldBeEqualTo(ConvertedHand.Sequences[(int)Streets.Flop]);
        }

        #endregion

        #region Methods

        IConvertedPokerPlayer CreatePlayer(string name, int position)
        {
            return new ConvertedPokerPlayer(
                name, _stub.Some<double>(), _stub.Some<double>(), position, TotalPlayers, string.Empty);
        }

        void SetupHeroInSeat2AndOpponentInSeat1()
        {
            ConvertedHand.AddPlayer(CreatePlayer(HeroName, 2));
            ConvertedHand.AddPlayer(CreatePlayer(OpponentName, 1));
        }

        #endregion
    }
}