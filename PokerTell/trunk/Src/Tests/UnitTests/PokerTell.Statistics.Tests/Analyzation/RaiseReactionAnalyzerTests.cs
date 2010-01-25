namespace PokerTell.Statistics.Tests.Analyzation
{
    using System;

    using Infrastructure.Enumerations.PokerHand;
    using Infrastructure.Interfaces;
    using Infrastructure.Interfaces.PokerHand;
    using Infrastructure.Services;

    using Moq;

    using NUnit.Framework;

    using PokerHand.Analyzation;

    using Statistics.Analyzation;

    using UnitTests.Tools;

    [TestFixture]
    internal class RaiseReactionAnalyzerTests
    {
        #region Constants and Fields

        protected IConvertedPokerHand ConvertedHand;

        const string HeroName = "hero";

        const string OpponentName = "someOpponent";

        const int TotalPlayers = 9;

        readonly IConstructor<IConvertedPokerActionWithId> _pokerActionWithIdMake =
            new Constructor<IConvertedPokerActionWithId>(() => new ConvertedPokerActionWithId());

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
        public void Construct_HeroXOppBHeroR_OppReraisesFourTimes_OpponentRaiseSizeIsStandardizedToFive()
        {
            ConvertedHand.SequenceFlop = "[2]X,[1]B,[2]R3.0,[1]R4.0,[2]R3.0";
            var anlyzationPreparer = new ReactionAnalyzationPreparer(
                ConvertedHand, Streets.Flop, HeroName, ActionSequences.HeroXOppBHeroR);

            var sut = new RaiseReactionAnalyzer(anlyzationPreparer, _pokerActionWithIdMake);

            sut.OpponentRaiseSize.ShouldBeEqualTo(5);
        }

        [Test]
        [Sequential]
        public void Construct_MoreThanOneOpponentRaisedBeforeHerosReaction_IsStandardSituationIsFalse(
            [Values("[2]B,[1]R3.0,[4]R2.0,[2]F", "[1]B,[2]R,[1]R3.0,[4]R2.0,[2]F", "[2]X,[1]B,[2]R,[1]R3.0,[4]R2.0,[2]F"
                )] string sequence,
            [Values(ActionSequences.HeroB, ActionSequences.OppBHeroR, ActionSequences.HeroXOppBHeroR)] ActionSequences
                actionSequence)
        {
            ConvertedHand.SequenceFlop = sequence;
            var anlyzationPreparer = new ReactionAnalyzationPreparer(
                ConvertedHand, Streets.Flop, HeroName, actionSequence);

            var sut = new RaiseReactionAnalyzer(anlyzationPreparer, _pokerActionWithIdMake);

            sut.IsStandardSituation.ShouldBeFalse();
        }

        [Test]
        public void Construct_NoProperReactionInSequence_AnalyzationIsInvalid_()
        {
            ConvertedHand.SequenceFlop = "[2]B0.3,[3]R2.0";
            var anlyzationPreparer = new ReactionAnalyzationPreparer(ConvertedHand, Streets.Flop, HeroName, ActionSequences.HeroB);

            var sut = new RaiseReactionAnalyzer(anlyzationPreparer, _pokerActionWithIdMake);

            sut.HeroReaction.ShouldBeEqualTo(ActionTypes.N);
            sut.IsValidResult.ShouldBeFalse();
        }

        [Test]
        [Sequential]
        public void Construct_OnlyOneOpponentRaisesAndMoreActionsAfterHerosReaction_IsStandardSituationIsTrue(
            [Values("[2]B,[1]R3.0,[2]F,[4]R2.0", "[1]B,[2]R,[1]R3.0,[2]F,[4]R2.0", "[2]X,[1]B,[2]R,[1]R3.0,[2]F,[4]R2.0"
                )] string sequence,
            [Values(ActionSequences.HeroB, ActionSequences.OppBHeroR, ActionSequences.HeroXOppBHeroR)] ActionSequences
                actionSequence)
        {
            ConvertedHand.SequenceFlop = sequence;
            var anlyzationPreparer = new ReactionAnalyzationPreparer(
                ConvertedHand, Streets.Flop, HeroName, ActionSequences.HeroXOppBHeroR);

            var sut = new RaiseReactionAnalyzer(anlyzationPreparer, _pokerActionWithIdMake);

            sut.IsStandardSituation.ShouldBeTrue();
        }

        [Test]
        public void Construct_OnlyOneOpponentRaisesAndNoMoreActionsAfterHerosReaction_IsStandardSituationIsTrue()
        {
            ConvertedHand.SequenceFlop = "[2]X,[1]B,[2]R3.0,[1]R3.0,[2]R3.0";
            var anlyzationPreparer = new ReactionAnalyzationPreparer(
                ConvertedHand, Streets.Flop, HeroName, ActionSequences.HeroXOppBHeroR);

            var sut = new RaiseReactionAnalyzer(anlyzationPreparer, _pokerActionWithIdMake);

            sut.IsStandardSituation.ShouldBeTrue();
        }

        [Test]
        [Sequential]
        public void Construct_ReactionContainedInSequence_FindsReactionAndSetsIsValidToFalse(
            [Values("[2]B0.3,[3]R2.0,[2]F", "[2]B0.3,[3]R2.0,[2]C", "[2]B0.3,[3]R2.0,[2]R2.0")] string sequence,
            [Values(ActionTypes.F, ActionTypes.C, ActionTypes.R)] ActionTypes expectedReaction)
        {
            ConvertedHand.SequenceFlop = sequence;
            var anlyzationPreparer = new ReactionAnalyzationPreparer(
                ConvertedHand, Streets.Flop, HeroName, ActionSequences.HeroB);

            var sut = new RaiseReactionAnalyzer(anlyzationPreparer, _pokerActionWithIdMake);

            sut.HeroReaction.ShouldBeEqualTo(expectedReaction);
            sut.IsValidResult.ShouldBeTrue();
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