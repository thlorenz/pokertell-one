namespace PokerTell.Statistics.Tests.Analyzation
{
    using System;

    using Infrastructure.Enumerations.PokerHand;
    using Infrastructure.Interfaces.PokerHand;

    using Interfaces;

    using Moq;

    using NUnit.Framework;

    using PokerHand.Analyzation;

    using Statistics.Analyzation;

    public class PreflopHandStrengthStatisticTests
    {
        #region Constants and Fields

        IPreflopCallingHandStrengthStatistic _callingHandStrengthStatistic;

        Mock<IPreflopCallingAnalyzer> _mockAnalyzer;

        #endregion

        #region Public Methods

        [SetUp]
        public void A_Init()
        {
            _callingHandStrengthStatistic = new PreflopCallingHandStrengthStatistic();
            _mockAnalyzer = new Mock<IPreflopCallingAnalyzer>();
            IConvertedPokerHand hand = CreateHandHeaderWithSequences_9Max("standardHeroName");
            _mockAnalyzer.SetupGet(get => get.ConvertedHand).Returns(hand);
        }

        [Test]
        public void Add_DefinedHand_AddsItToConvertedHands()
        {
            _callingHandStrengthStatistic.Add(_mockAnalyzer.Object);

            Assert.That(_callingHandStrengthStatistic.ConvertedHands.Count, Is.EqualTo(1));
        }

        [Test]
        public void Add_KnownHeroCards_AddsThemToKnownHoleCards()
        {
            _mockAnalyzer.SetupGet(get => get.HeroHoleCards).Returns("As Kh");
            _callingHandStrengthStatistic.Add(_mockAnalyzer.Object);

            Assert.That(_callingHandStrengthStatistic.KnownCards.Count, Is.EqualTo(1));
        }

        [Test]
        public void Add_UndefinedHand_DoesNotAddIt()
        {
            var mockAnalyzer = new Mock<IPreflopCallingAnalyzer>();
            mockAnalyzer.SetupGet(get => get.ConvertedHand).Returns((IConvertedPokerHand)null);

            _callingHandStrengthStatistic.Add(mockAnalyzer.Object);

            Assert.That(_callingHandStrengthStatistic.ConvertedHands.Count, Is.EqualTo(0));
        }

        [Test]
        public void Add_UnknownHeroCards_DoesNotAddThemToKnownHoleCards()
        {
            _mockAnalyzer.SetupGet(get => get.HeroHoleCards).Returns("?? ??");
            _callingHandStrengthStatistic.Add(_mockAnalyzer.Object);

            Assert.That(_callingHandStrengthStatistic.KnownCards.Count, Is.EqualTo(0));
        }

        [Test]
        public void CalculateAverageHandStrength_WithKnownCards_CalculatesAverageHandStrengthChenValue()
        {
            const string cards1 = "As Ah";
            _mockAnalyzer.SetupGet(get => get.HeroHoleCards).Returns(cards1);
            _callingHandStrengthStatistic.Add(_mockAnalyzer.Object);
            const string cards2 = "9h Ts";
            _mockAnalyzer.SetupGet(get => get.HeroHoleCards).Returns(cards2);
            _callingHandStrengthStatistic.Add(_mockAnalyzer.Object);

            int expectedChenValue =
                new ValuedHoleCardsAverage(new[] { new ValuedHoleCards(cards1), new ValuedHoleCards(cards2) }).ChenValue;
            _callingHandStrengthStatistic.CalculateAverageHandStrength();

            Assert.That(_callingHandStrengthStatistic.AverageHandStrength.ChenValue, Is.EqualTo(expectedChenValue));
        }

        [Test]
        public void CalculateAverageHandStrength_WithKnownCards_SetsAverageHandStrengthToValid()
        {
            const string cards1 = "As Ah";
            _mockAnalyzer.SetupGet(get => get.HeroHoleCards).Returns(cards1);
            _callingHandStrengthStatistic.Add(_mockAnalyzer.Object);
            _callingHandStrengthStatistic.CalculateAverageHandStrength();

            Assert.That(_callingHandStrengthStatistic.AverageHandStrength.IsValid, Is.True);
        }

        [Test]
        public void CalculateAverageHandStrength_WithoutKnownCards_SetsAverageHandStrengthToInvalid()
        {
            _callingHandStrengthStatistic.CalculateAverageHandStrength();
            Assert.That(_callingHandStrengthStatistic.AverageHandStrength.IsValid, Is.False);
        }

        #endregion

        #region Methods

        static IConvertedPokerHand CreateHandHeaderWithSequences_9Max(string hero)
        {
            IConvertedPokerHand hand = CreateHandHeader_9Max();

            hand.AddPlayer(CreatePlayer(hero, 9));
            var round = new ConvertedPokerRound
                { new ConvertedPokerActionWithId(new ConvertedPokerAction(ActionTypes.B, 1.0), 0) };

            for (var street = (int)Streets.PreFlop; street <= (int)Streets.River; street++)
            {
                hand.Sequences[street] = round;
            }

            return hand;
        }

        static IConvertedPokerHand CreateHandHeader_9Max()
        {
            return new ConvertedPokerHand("Pokerstars", 123456, DateTime.Parse("01/01/2009 12:00:00 pm"), 20, 10, 9);
        }

        static IConvertedPokerPlayer CreatePlayer(string playerName, int totalPlayers)
        {
            return new ConvertedPokerPlayer(playerName, 10, 12, 2, totalPlayers, "As Kd");
        }

        #endregion
    }
}