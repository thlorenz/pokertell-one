namespace PokerTell.PokerHand.Tests
{
    using Moq;

    using NUnit.Framework;

    using PokerTell.Infrastructure.Interfaces.PokerHand;
    using PokerTell.PokerHand.Analyzation;
    using PokerTell.PokerHand.Conditions;
    using PokerTell.UnitTests;

    public class ThatSawFlopCondition : TestWithLog
    {
        #region Constants and Fields

        IConvertedPokerHand _convertedHand;

        string _heroName;

        IConvertedPokerPlayer _heroPlayer;

        IPokerHandCondition _sawFlopCondition;

        StubBuilder _stub;

        #endregion

        #region Public Methods

        [SetUp]
        public void _Init()
        {
            _stub = new StubBuilder();
            _heroName = "hero";
            _convertedHand = new ConvertedPokerHand();

            _heroPlayer =
                new ConvertedPokerPlayer(
                    _heroName,
                    _stub.Some<double>(),
                    _stub.Some<double>(),
                    _stub.Valid(For.Position, 1),
                    _stub.Valid(For.TotalPlayers, 2),
                    _stub.Valid(For.HoleCards, string.Empty));

            _sawFlopCondition = new SawFlopCondition().AppliesTo(_heroName);
        }

        [Test]
        public void IsFullFilledBy_HandHasNoPlayers_ReturnsFalse()
        {
            bool result = _sawFlopCondition.IsFullFilledBy(_convertedHand);

            Assert.That(result, Is.False);
        }

        [Test]
        public void IsFullFilledBy_HandIsNull_ReturnsFalse()
        {
            bool result = _sawFlopCondition.IsFullFilledBy(null);

            Assert.That(result, Is.False);
        }

        [Test]
        public void IsFullFilledBy_HeroWithOneRound_ReturnsFalse()
        {
            _heroPlayer
                .AddRound();

            _convertedHand
                .AddPlayer(_heroPlayer);

            bool result = _sawFlopCondition.IsFullFilledBy(_convertedHand);

            Assert.That(result, Is.False);
        }

        [Test]
        public void IsFullFilledBy_HeroWithoutRound_ReturnsFalse()
        {
            _convertedHand
                .AddPlayer(_heroPlayer);

            bool result = _sawFlopCondition.IsFullFilledBy(_convertedHand);

            Assert.That(result, Is.False);
        }

        [Test]
        public void IsFullFilledBy_HeroWithoutRoundOpponentWithTwoRounds_ReturnsFalse()
        {
            IConvertedPokerPlayer opponent =
                new ConvertedPokerPlayer(
                    "other",
                    _stub.Some<double>(),
                    _stub.Some<double>(),
                    _stub.Valid(For.Position, 0),
                    _stub.Valid(For.TotalPlayers, 2),
                    _stub.Valid(For.HoleCards, string.Empty))
                    .AddRound()
                    .AddRound();

            _convertedHand
                .AddPlayer(_heroPlayer);

            bool result = _sawFlopCondition.IsFullFilledBy(_convertedHand);

            Assert.That(result, Is.False);
        }

        [Test]
        public void IsFullFilledBy_HeroWithTwoRounds_ReturnsTrue()
        {
            _heroPlayer
                .AddRound()
                .AddRound();

            _convertedHand
                .AddPlayer(_heroPlayer);

            bool result = _sawFlopCondition.IsFullFilledBy(_convertedHand);

            Assert.That(result, Is.True);
        }

        #endregion
    }
}