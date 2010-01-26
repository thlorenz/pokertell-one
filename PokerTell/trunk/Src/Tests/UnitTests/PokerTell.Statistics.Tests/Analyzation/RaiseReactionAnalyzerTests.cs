namespace PokerTell.Statistics.Tests.Analyzation
{
    using Factories;

    using Infrastructure.Enumerations.PokerHand;
    using Infrastructure.Interfaces;
    using Infrastructure.Interfaces.PokerHand;
    using Infrastructure.Services;

    using NUnit.Framework;

    using PokerHand.Analyzation;

    using Statistics.Analyzation;

    using UnitTests.Tools;

    [TestFixture]
    internal class RaiseReactionAnalyzerTests
    {
        #region Constants and Fields

        const int HeroPosition = 2;

        readonly IConstructor<IConvertedPokerActionWithId> _pokerActionWithIdMake =
            new Constructor<IConvertedPokerActionWithId>(() => new ConvertedPokerActionWithId());

        #endregion

        #region Public Methods

        [Test]
        public void Construct_HeroXOppBHeroR_OppReraisesFourTimes_OpponentRaiseSizeIsStandardizedToFive()
        {
            IConvertedPokerRound sequence = "[2]X,[1]B,[2]R3.0,[1]R4.0,[2]R3.0".ToConvertedPokerRoundWithIds();
            var anlyzationPreparer = new ReactionAnalyzationPreparer(sequence, HeroPosition, ActionSequences.HeroXOppBHeroR);

            var sut = new RaiseReactionAnalyzer(anlyzationPreparer);

            sut.OpponentRaiseSize.ShouldBeEqualTo(5);
        }

        [Test]
        [Sequential]
        public void Construct_MoreThanOneOpponentRaisedBeforeHerosReaction_IsStandardSituationIsFalse(
            [Values("[2]B,[1]R3.0,[4]R2.0,[2]F", "[1]B,[2]R,[1]R3.0,[4]R2.0,[2]F", "[2]X,[1]B,[2]R,[1]R3.0,[4]R2.0,[2]F"
                )] string sequenceString,
            [Values(ActionSequences.HeroB, ActionSequences.OppBHeroR, ActionSequences.HeroXOppBHeroR)] ActionSequences
                actionSequence)
        {
            IConvertedPokerRound sequence = sequenceString.ToConvertedPokerRoundWithIds();
            var anlyzationPreparer = new ReactionAnalyzationPreparer(sequence, HeroPosition, actionSequence);

            var sut = new RaiseReactionAnalyzer(anlyzationPreparer);

            sut.IsStandardSituation.ShouldBeFalse();
        }

        [Test]
        public void Construct_NoProperReactionInSequence_AnalyzationIsInvalid()
        {
            IConvertedPokerRound sequence = "[2]B0.3,[3]R2.0".ToConvertedPokerRoundWithIds();
            var anlyzationPreparer = new ReactionAnalyzationPreparer(sequence, HeroPosition, ActionSequences.HeroB);

            var sut = new RaiseReactionAnalyzer(anlyzationPreparer);

            sut.IsValidResult.ShouldBeFalse();
        }

        [Test]
        [Sequential]
        public void Construct_OnlyOneOpponentRaisesAndMoreActionsAfterHerosReaction_IsStandardSituationIsTrue(
            [Values("[2]B,[1]R3.0,[2]F,[4]R2.0", "[1]B,[2]R,[1]R3.0,[2]F,[4]R2.0", "[2]X,[1]B,[2]R,[1]R3.0,[2]F,[4]R2.0"
                )] string sequenceString,
            [Values(ActionSequences.HeroB, ActionSequences.OppBHeroR, ActionSequences.HeroXOppBHeroR)] ActionSequences
                actionSequence)
        {
            IConvertedPokerRound sequence = sequenceString.ToConvertedPokerRoundWithIds();
            var anlyzationPreparer = new ReactionAnalyzationPreparer(sequence, HeroPosition, actionSequence);

            var sut = new RaiseReactionAnalyzer(anlyzationPreparer);

            sut.IsStandardSituation.ShouldBeTrue();
        }

        [Test]
        public void Construct_OnlyOneOpponentRaisesAndNoMoreActionsAfterHerosReaction_IsStandardSituationIsTrue()
        {
            IConvertedPokerRound sequence = "[2]X,[1]B,[2]R3.0,[1]R3.0,[2]R3.0".ToConvertedPokerRoundWithIds();
            var anlyzationPreparer = new ReactionAnalyzationPreparer(sequence, HeroPosition, ActionSequences.HeroXOppBHeroR);

            var sut = new RaiseReactionAnalyzer(anlyzationPreparer);

            sut.IsStandardSituation.ShouldBeTrue();
        }

        [Test]
        [Sequential]
        public void Construct_ReactionContainedInSequence_FindsReactionAndSetsIsValidToFalse(
            [Values("[2]B0.3,[3]R2.0,[2]F", "[2]B0.3,[3]R2.0,[2]C", "[2]B0.3,[3]R2.0,[2]R2.0")] string sequenceString,
            [Values(ActionTypes.F, ActionTypes.C, ActionTypes.R)] ActionTypes expectedReaction)
        {
            IConvertedPokerRound sequence = sequenceString.ToConvertedPokerRoundWithIds();
            var anlyzationPreparer = new ReactionAnalyzationPreparer(sequence, HeroPosition, ActionSequences.HeroB);

            var sut = new RaiseReactionAnalyzer(anlyzationPreparer);

            sut.HeroReactionType.ShouldBeEqualTo(expectedReaction);
            sut.IsValidResult.ShouldBeTrue();
        }

        #endregion
    }
}