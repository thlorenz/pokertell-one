namespace PokerTell.Statistics.Tests.Analyzation
{
    using Infrastructure;
    using Infrastructure.Interfaces.PokerHand;

    using PokerTell.Infrastructure.Enumerations.PokerHand;

    using Moq;

    using NUnit.Framework;

    using PokerTell.PokerHand.Analyzation;
    using PokerTell.Statistics.Interfaces;

    using Statistics.Analyzation;

    using UnitTests.Tools;

    [TestFixture]
    internal class PreflopCallingRatioAnalyzerTests
    {
        

        #region Constants and Fields

        Mock<IReactionAnalyzationPreparer> _mockReactionAnalyzationPreparer;

        StubBuilder _stub;

        #endregion

        #region Public Methods

        [SetUp]
        public void _Init()
        {
            _stub = new StubBuilder();
            
            const string someRound = "[2]C0.2";

           _mockReactionAnalyzationPreparer = new Mock<IReactionAnalyzationPreparer>();
           _mockReactionAnalyzationPreparer.SetupGet(gt => gt.Sequence).Returns(
                new PokerHandStringConverter().ConvertedRoundFrom(someRound));
        }



        [Test, Ignore("For now PreflopCallingRatioAnalyzer needs to be redone as it has a bug in my opinion")]
        public void ConstructWith_ReactionAnalyzerReturnsSequenceWhereSecondActionIsCallWithRatioOneHalfPot_HeroCallingRatioIsSetToClosestNormalizedReactionValue()
        {
            const int heroPosition = 2;
            var sequence = new ConvertedPokerRound
                {
                    new ConvertedPokerActionWithId().InitializeWith(ActionTypes.C, 0.7, 0),
                    new ConvertedPokerActionWithId().InitializeWith(ActionTypes.C, 0.5, heroPosition)
                };
           
            _mockReactionAnalyzationPreparer.SetupGet(gt => gt.Sequence).Returns(sequence);
            _mockReactionAnalyzationPreparer.SetupGet(gt => gt.HeroPosition).Returns(heroPosition);

            new PreflopCallingAnalyzer(_stub.Out<IAnalyzablePokerPlayer>(), _mockReactionAnalyzationPreparer.Object, false).Ratio
                .ShouldBeEqualTo(0.4);
        }

        [Test]
        public void ConstructWith_ReactionAnalyzerReturnsValidHolecards_SetsHerosHoleCardsToThem()
        {
            const string herosCards = "As Qh";
            var analyzablePokerPlayerStub = _stub.Setup<IAnalyzablePokerPlayer>()
                .Get(p => p.Holecards).Returns(herosCards)
                .Out;
            
            new PreflopCallingAnalyzer(analyzablePokerPlayerStub, _mockReactionAnalyzationPreparer.Object, false)
                .HeroHoleCards.ShouldBeEqualTo(herosCards);
        }

        #endregion
    }
}