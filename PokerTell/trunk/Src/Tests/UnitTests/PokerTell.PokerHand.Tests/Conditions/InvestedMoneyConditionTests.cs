namespace PokerTell.PokerHand.Tests.Conditions
{
    using Infrastructure.Enumerations.PokerHand;
    using Infrastructure.Interfaces.PokerHand;

    using Moq;

    using NUnit.Framework;

    using PokerTell.PokerHand.Analyzation;
    using PokerTell.PokerHand.Conditions;

    using UnitTests;

    [TestFixture]
    public class InvestedMoneyConditionTests : TestWithLog
    {
        #region Constants and Fields

        IConvertedPokerHand _convertedHand;

        string _heroName;

        IConvertedPokerPlayer _heroPlayer;

        StubBuilder _stub;

        IPokerHandCondition _investedMoneyCondition;

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

            _investedMoneyCondition = new InvestedMoneyCondition().AppliesTo(_heroName);
        }

        [Test]
        public void IsFullFilledBy_FoldingHero_ReturnsFalse()
        {
            _heroPlayer
                .Add(
                new ConvertedPokerRound()
                    .Add(new ConvertedPokerAction(ActionTypes.F, 1.0)));

            _convertedHand
                .AddPlayer(_heroPlayer);

            bool result = _investedMoneyCondition.IsMetBy(_convertedHand);

            Assert.That(result, Is.False);
        }

        [Test]
        public void IsFullFilledBy_HeroWithoutRound_ReturnsFalse()
        {
            _convertedHand
                .AddPlayer(_heroPlayer);

            bool result = _investedMoneyCondition.IsMetBy(_convertedHand);

            Assert.That(result, Is.False);
        }

        [Test]
        [Sequential]
        public void IsFullFilledBy_InvestingHero_ReturnsTrue(
            [Values(ActionTypes.C, ActionTypes.R, ActionTypes.X)] ActionTypes actionType)
        {
            _heroPlayer
                .Add(
                new ConvertedPokerRound()
                    .Add(new ConvertedPokerAction(actionType, 1.0)));

            _convertedHand
                .AddPlayer(_heroPlayer);

            bool result = _investedMoneyCondition.IsMetBy(_convertedHand);

            Assert.That(result, Is.True);
        }

        [Test]
        public void IsFullFilledBy_InvestingOpponent_ReturnsFalse()
        {
            IConvertedPokerPlayer opponent =
                new ConvertedPokerPlayer(
                    "other", 
                    _stub.Some<double>(), 
                    _stub.Some<double>(), 
                    _stub.Valid(For.Position, 0), 
                    _stub.Valid(For.TotalPlayers, 2), 
                    _stub.Valid(For.HoleCards, string.Empty))
                    .Add(
                    new ConvertedPokerRound()
                        .Add(new ConvertedPokerAction(ActionTypes.C, 1.0)));

            _convertedHand.AddPlayer(opponent);

            bool result = _investedMoneyCondition.IsMetBy(_convertedHand);

            Assert.That(result, Is.False);
        }

        [Test]
        [Sequential]
        public void IsFullFilledBy_NonPreflopFirstAction_ReturnsFalse(
            [Values(ActionTypes.A, ActionTypes.B, ActionTypes.E, ActionTypes.N, ActionTypes.P, ActionTypes.U, 
                ActionTypes.W)] ActionTypes actionType)
        {
            _heroPlayer
                .Add(
                new ConvertedPokerRound()
                    .Add(new ConvertedPokerAction(actionType, 1.0)));

            _convertedHand
                .AddPlayer(_heroPlayer);

            bool result = _investedMoneyCondition.IsMetBy(_convertedHand);

            Assert.That(result, Is.False);
        }

        #endregion
    }
}