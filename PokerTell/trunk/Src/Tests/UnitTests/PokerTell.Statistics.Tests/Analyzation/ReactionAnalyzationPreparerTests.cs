namespace PokerTell.Statistics.Tests.Analyzation
{
    using Factories;

    using Infrastructure.Enumerations.PokerHand;
    using Infrastructure.Interfaces.PokerHand;

    using Interfaces;

    using NUnit.Framework;

    using Statistics.Analyzation;

    using UnitTests;
    using UnitTests.Tools;

    public class ReactionAnalyzationPreparerTests : TestWithLog
    {
        IReactionAnalyzationPreparer _sut;

        #region Constants and Fields

        const int HeroPosition = 2;

        #endregion

        #region Public Methods

        [SetUp]
        public void A_Init()
        {
            _sut = new ReactionAnalyzationPreparer();
        }

        [Test]
        [Sequential]
        public void PrepareAnalyzationFor_FlopHeroActsAndFirstMatchingHeroActionIsAtIndexOne_StartingActionIndexIsOne(
            [Values("[1]X,[2]X,[3]C", "[1]X,[2]B,[3]C")] string sequenceString,
            [Values(ActionSequences.HeroX, ActionSequences.HeroB)] ActionSequences actionSequence)
        {
            IConvertedPokerRound sequence = sequenceString.ToConvertedPokerRoundWithIds();

            _sut.PrepareAnalyzationFor(sequence, HeroPosition, actionSequence);
            
            _sut.StartingActionIndex.ShouldBeEqualTo(1);
        }

        [Test]
        [Sequential]
        public void PrepareAnalyzationFor_FlopHeroXOppBAndFirstMatchingHeroReactionIsAtIndexTwo_StartingActionIndexIsTwo(
            [Values("[2]X,[1]B,[2]F", "[2]X,[1]B,[2]C", "[2]X,[1]B,[2]R")] string sequenceString,
            [Values(ActionSequences.HeroXOppBHeroF, ActionSequences.HeroXOppBHeroC, ActionSequences.HeroXOppBHeroR)] ActionSequences actionSequence)
        {
            IConvertedPokerRound sequence = sequenceString.ToConvertedPokerRoundWithIds();

            _sut.PrepareAnalyzationFor(sequence, HeroPosition, actionSequence);

            _sut.StartingActionIndex.ShouldBeEqualTo(2);
        }

        [Test]
        [Sequential]
        public void PrepareAnalyzationFor_FlopOppBAndFirstMatchingHeroReactionIsAtIndexOne_StartingActionIndexIsOne(
            [Values("[1]B,[2]F,[3]C", "[1]B,[2]C,[3]C", "[1]B,[2]R,[3]C")] string sequenceString,
            [Values(ActionSequences.OppBHeroF, ActionSequences.OppBHeroC, ActionSequences.OppBHeroR)] ActionSequences
                actionSequence)
        {
            IConvertedPokerRound sequence = sequenceString.ToConvertedPokerRoundWithIds();

            _sut.PrepareAnalyzationFor(sequence, HeroPosition, actionSequence);
            
            _sut.StartingActionIndex.ShouldBeEqualTo(1);
        }

        [Test]
        [Sequential]
        public void PrepareAnalyzationFor_PreFlopRaisedPotAndFirstMatchingHeroActionIsAtIndexOne_StartingActionIndexIsOne(
            [Values("[1]C,[2]F,[3]C", "[1]C,[2]C,[3]C", "[1]C,[2]R,[3]C")] string sequenceString,
            [Values(ActionSequences.HeroF, ActionSequences.HeroC, ActionSequences.HeroR)] ActionSequences actionSequence)
        {
            IConvertedPokerRound sequence = sequenceString.ToConvertedPokerRoundWithIds();

            _sut.PrepareAnalyzationFor(sequence, HeroPosition, actionSequence);

            _sut.StartingActionIndex.ShouldBeEqualTo(1);
        }

        [Test]
        [Sequential]
        public void PrepareAnalyzationFor_PreFlopUnraisedPotAndFirstMatchingHeroActionIsAtIndexOne_StartingActionIndexIsOne(
            [Values("[1]C,[2]F,[3]C", "[1]C,[2]C,[3]C", "[1]C,[2]R,[3]C")] string sequenceString,
            [Values(ActionSequences.HeroF, ActionSequences.HeroC, ActionSequences.HeroR)] ActionSequences actionSequence)
        {
            IConvertedPokerRound sequence = sequenceString.ToConvertedPokerRoundWithIds();

            _sut.PrepareAnalyzationFor(sequence, HeroPosition, actionSequence);

            _sut.StartingActionIndex.ShouldBeEqualTo(1);
        }

        #endregion
    }
}