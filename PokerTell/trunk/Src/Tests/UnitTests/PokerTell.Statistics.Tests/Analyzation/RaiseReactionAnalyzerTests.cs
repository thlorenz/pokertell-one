namespace PokerTell.Statistics.Tests.Analyzation
{
    using Factories;

    using Infrastructure;
    using Infrastructure.Enumerations.PokerHand;
    using Infrastructure.Interfaces.PokerHand;

    using Interfaces;

    using Moq;

    using NUnit.Framework;

    using Statistics.Analyzation;

    using UnitTests.Tools;

    [TestFixture]
    internal class RaiseReactionAnalyzerTests
    {
        #region Constants and Fields

        const int HeroPosition = 2;

        IReactionAnalyzationPreparer _analyzationPreparer;

        IRaiseReactionAnalyzer _sut;

        double[] _raiseSizes;

        StubBuilder _stub;

        #endregion

        #region Public Methods

        [SetUp]
        public void A_Init()
        {
            _stub = new StubBuilder();
            _analyzationPreparer = new ReactionAnalyzationPreparer();
            _sut = new RaiseReactionAnalyzer();
            _raiseSizes = ApplicationProperties.RaiseSizeKeys;
        }

        [Test]
        public void AnalyzeUsingDataFrom_HeroXOppBHeroR_OppReraisesFourTimes_OpponentRaiseSizeIsStandardizedToFive()
        {
            IConvertedPokerRound sequence = "[2]X,[1]B,[2]R3.0,[1]R4.0,[2]R3.0".ToConvertedPokerRoundWithIds();
            _analyzationPreparer.PrepareAnalyzationFor(sequence, HeroPosition, ActionSequences.HeroXOppBHeroR);

            _sut.AnalyzeUsingDataFrom(_stub.Out<IAnalyzablePokerPlayer>(), _analyzationPreparer, true, _raiseSizes);

            _sut.ConsideredRaiseSize.ShouldBeEqualTo(5);
        }

        [Test]
        [Sequential]
        public void AnalyzeUsingDataFrom_MoreThanOneOpponentRaisedBeforeHerosReaction_IsStandardSituationIsFalse(
            [Values("[2]B,[1]R3.0,[4]R2.0,[2]F", "[1]B,[2]R,[1]R3.0,[4]R2.0,[2]F", "[2]X,[1]B,[2]R,[1]R3.0,[4]R2.0,[2]F"
                )] string sequenceString,
            [Values(ActionSequences.HeroB, ActionSequences.OppBHeroR, ActionSequences.HeroXOppBHeroR)] ActionSequences
                actionSequence)
        {
            IConvertedPokerRound sequence = sequenceString.ToConvertedPokerRoundWithIds();
            _analyzationPreparer.PrepareAnalyzationFor(sequence, HeroPosition, actionSequence);

            _sut.AnalyzeUsingDataFrom(_stub.Out<IAnalyzablePokerPlayer>(), _analyzationPreparer, true, _raiseSizes);

            _sut.IsStandardSituation.ShouldBeFalse();
        }

        [Test]
        public void AnalyzeUsingDataFrom_NoProperReactionInSequence_AnalyzationIsInvalid()
        {
            IConvertedPokerRound sequence = "[2]B0.3,[3]R2.0".ToConvertedPokerRoundWithIds();
            _analyzationPreparer.PrepareAnalyzationFor(sequence, HeroPosition, ActionSequences.HeroB);

            _sut.AnalyzeUsingDataFrom(_stub.Out<IAnalyzablePokerPlayer>(), _analyzationPreparer, true, _raiseSizes);

            _sut.IsValidResult.ShouldBeFalse();
        }

        [Test]
        [Sequential]
        public void AnalyzeUsingDataFrom_OnlyOneOpponentRaisedAndMoreActionsAfterHerosReaction_IsStandardSituationIsTrue(
            [Values("[2]B,[1]R3.0,[2]F,[4]R2.0", "[1]B,[2]R,[1]R3.0,[2]F,[4]R2.0", "[2]X,[1]B,[2]R,[1]R3.0,[2]F,[4]R2.0"
                )] string sequenceString,
            [Values(ActionSequences.HeroB, ActionSequences.OppBHeroR, ActionSequences.HeroXOppBHeroR)] ActionSequences
                actionSequence)
        {
            IConvertedPokerRound sequence = sequenceString.ToConvertedPokerRoundWithIds();
            _analyzationPreparer.PrepareAnalyzationFor(sequence, HeroPosition, actionSequence);

            _sut.AnalyzeUsingDataFrom(_stub.Out<IAnalyzablePokerPlayer>(), _analyzationPreparer, true, _raiseSizes);

            _sut.IsStandardSituation.ShouldBeTrue();
        }

        [Test]
        public void AnalyzeUsingDataFrom_OnlyOneOpponentRaisedAndNoMoreActionsAfterHerosReaction_IsStandardSituationIsTrue()
        {
            IConvertedPokerRound sequence = "[2]X,[1]B,[2]R3.0,[1]R3.0,[2]R3.0".ToConvertedPokerRoundWithIds();
            _analyzationPreparer.PrepareAnalyzationFor(sequence, HeroPosition, ActionSequences.HeroXOppBHeroR);

            _sut.AnalyzeUsingDataFrom(_stub.Out<IAnalyzablePokerPlayer>(), _analyzationPreparer, true, _raiseSizes);

            _sut.IsStandardSituation.ShouldBeTrue();
        }

        [Test]
        [Sequential]
        public void AnalyzeUsingDataFrom_ReactionContainedInSequence_FindsReactionAndSetsIsValidToTrue(
            [Values("[2]B0.3,[3]R2.0,[2]F", "[2]B0.3,[3]R2.0,[2]C", "[2]B0.3,[3]R2.0,[2]R2.0")] string sequenceString,
            [Values(ActionTypes.F, ActionTypes.C, ActionTypes.R)] ActionTypes expectedReaction)
        {
            IConvertedPokerRound sequence = sequenceString.ToConvertedPokerRoundWithIds();
            _analyzationPreparer.PrepareAnalyzationFor(sequence, HeroPosition, ActionSequences.HeroB);

            _sut.AnalyzeUsingDataFrom(_stub.Out<IAnalyzablePokerPlayer>(), _analyzationPreparer, true, _raiseSizes);

            _sut.HeroReactionType.ShouldBeEqualTo(expectedReaction);
            _sut.IsValidResult.ShouldBeTrue();
        }

        #endregion
    }
}