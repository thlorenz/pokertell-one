namespace PokerTell.Statistics.Tests.Analyzation
{
    using Infrastructure;

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
        const string HerosCards = "As Qh";

        #region Constants and Fields

        Mock<IReactionAnalyzationPreparer> _mockReactionAnalyzationPreparer;

        #endregion

        #region Public Methods

        [SetUp]
        public void _Init()
        {
            const string someRound = "[2]C0.2";
            const string heroName = "Hero";

            var hand = new ConvertedPokerHand();
            hand.AddPlayer(new ConvertedPokerPlayer(heroName, 10, 9, 1, 6, HerosCards));
           
            _mockReactionAnalyzationPreparer = new Mock<IReactionAnalyzationPreparer>();
            _mockReactionAnalyzationPreparer.SetupGet(gt => gt.ConvertedHand).Returns(hand);
            _mockReactionAnalyzationPreparer.SetupGet(gt => gt.HeroName).Returns(heroName);
            
            _mockReactionAnalyzationPreparer.SetupGet(gt => gt.Sequence).Returns(
                new PokerHandStringConverter().ConvertedRoundFrom(someRound));
        }

        [Test]
        public void ConstructWith_ReactionAnalyzerReturnsSequenceWhereSecondActionIsCallWithRatioOneHalfPot_HeroCallingRatioIsSetToClosestNormalizedReactionValue()
        {
            const int heroId = 2;
            const int heroIndex = 1;
            var round = new ConvertedPokerRound
                {
                    new ConvertedPokerActionWithId().InitializeWith(ActionTypes.C, 0.7, 1),
                    new ConvertedPokerActionWithId().InitializeWith(ActionTypes.C, 0.5, heroId)
                };

            _mockReactionAnalyzationPreparer.SetupGet(gt => gt.Sequence).Returns(round);
            _mockReactionAnalyzationPreparer.SetupGet(gt => gt.HeroIndex).Returns(heroIndex);

            new PreflopCallingAnalyzer(_mockReactionAnalyzationPreparer.Object, false).Ratio
                .ShouldBeEqualTo(0.4);
        }

        [Test]
        public void ConstructWith_ReactionAnalyzerReturnsValidHolecards_SetsHerosHoleCardsToThem()
        {
            new PreflopCallingAnalyzer(_mockReactionAnalyzationPreparer.Object, false)
                .HeroHoleCards.ShouldBeEqualTo(HerosCards);
        }

        #endregion
    }
}