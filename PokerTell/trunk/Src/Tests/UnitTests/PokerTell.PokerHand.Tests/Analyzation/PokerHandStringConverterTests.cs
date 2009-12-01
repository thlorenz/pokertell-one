namespace PokerTell.PokerHand.Tests.Analyzation
{
    using Infrastructure.Enumerations.PokerHand;
    using Infrastructure.Interfaces.PokerHand;

    using Moq;

    using NUnit.Framework;

    using PokerTell.PokerHand.Analyzation;

    using UnitTests;

    [TestFixture]
    public class PokerHandStringConverterTests
    {
        #region Constants and Fields

        PokerHandStringConverter _converter;

        ConvertedPokerRound _round;

        StubBuilder _stub;

        #endregion

        #region Public Methods

        [SetUp]
        public void _Init()
        {
            _stub = new StubBuilder();
            _round = new ConvertedPokerRound();
            _converter = new PokerHandStringConverter();
        }

        [Test]
        public void BuildSqlStringFrom_EmptyRound_ReturnsNull()
        {
            string result = _converter.BuildSqlStringFrom(_round);

            Assert.That(result, Is.Null);
        }

        [Test]
        [Sequential]
        public void BuildSqlStringFrom_FirstActiveActionSecondInvalidAction_ReturnsStringForFirstActionAndRatio()
        {
            const ActionTypes actionType1 = ActionTypes.B;
            const double ratio1 = 0.5;
            const ActionTypes actionType2 = ActionTypes.U;
            const double ratio2 = 0.2;

            string expectedResult =
                actionType1.ToString() + ratio1;

            _round
                .Add(new ConvertedPokerAction(actionType1, ratio1))
                .Add(new ConvertedPokerAction(actionType2, ratio2));

            string result = _converter.BuildSqlStringFrom(_round);

            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        [Sequential]
        public void BuildSqlStringFrom_FirstInvalidActionSecondActiveAction_ReturnsStringForFirstActionAndRatio()
        {
            const ActionTypes actionType1 = ActionTypes.U;
            const double ratio1 = 0.5;
            const ActionTypes actionType2 = ActionTypes.C;
            const double ratio2 = 0.2;

            string expectedResult =
                actionType2.ToString() + ratio2;

            _round
                .Add(new ConvertedPokerAction(actionType1, ratio1))
                .Add(new ConvertedPokerAction(actionType2, ratio2));

            string result = _converter.BuildSqlStringFrom(_round);

            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        [Sequential]
        public void BuildSqlStringFrom_OneActiveAction_ReturnsStringForThatActionAndRatio(
            [Values(ActionTypes.B, ActionTypes.C, ActionTypes.R, ActionTypes.W)] ActionTypes actionType)
        {
            const double ratio = 0.5;

            string expectedResult = actionType.ToString() + ratio;

            _round.Add(new ConvertedPokerAction(actionType, ratio));

            string result = _converter.BuildSqlStringFrom(_round);

            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        [Sequential]
        public void BuildSqlStringFrom_OneActiveActionWithId_ReturnsStringForThatActionWithRatioAndId()
        {
            const ActionTypes actionType = ActionTypes.B;
            const double ratio = 0.5;
            const int playerId = 2;

            string expectedResult = "[" + playerId + "]" + actionType + ratio;

            _round.Add(
                new ConvertedPokerActionWithId(new ConvertedPokerAction(actionType, ratio), playerId));

            string result = _converter.BuildSqlStringFrom(_round);

            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        [Sequential]
        public void BuildSqlStringFrom_OneInActiveAction_ReturnsStringForThatActionWithoutRatio(
            [Values(ActionTypes.A, ActionTypes.F, ActionTypes.X)] ActionTypes actionType)
        {
            const double ratio = 0.5;

            string expectedResult = actionType.ToString();

            _round.Add(new ConvertedPokerAction(actionType, ratio));

            string result = _converter.BuildSqlStringFrom(_round);

            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        [Sequential]
        public void BuildSqlStringFrom_OneInvalidAction_ReturnsEmptyString(
            [Values(ActionTypes.E, ActionTypes.N, ActionTypes.P, ActionTypes.U)] ActionTypes actionType)
        {
            const double ratio = 0.5;

            string expectedResult = string.Empty;

            _round.Add(new ConvertedPokerAction(actionType, ratio));

            string result = _converter.BuildSqlStringFrom(_round);

            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        [Sequential]
        public void BuildSqlStringFrom_TwoActiveActions_ReturnsStringForBothActionAndRatioSeparatedByComma()
        {
            const ActionTypes actionType1 = ActionTypes.B;
            const double ratio1 = 0.5;
            const ActionTypes actionType2 = ActionTypes.C;
            const double ratio2 = 0.2;

            string expectedResult =
                actionType1.ToString() + ratio1 + "," +
                actionType2 + ratio2;

            _round
                .Add(new ConvertedPokerAction(actionType1, ratio1))
                .Add(new ConvertedPokerAction(actionType2, ratio2));

            string result = _converter.BuildSqlStringFrom(_round);

            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void ConvertedRoundFrom_EmptyString_ReturnsNull()
        {
            string csvString = string.Empty;

            IConvertedPokerRound round = _converter.ConvertedRoundFrom(csvString);

            Assert.That(round, Is.Null);
        }

        [Test]
        public void ConvertedRoundFrom_OneActiveAction_ReturnsRoundWithThatAction(
            [Values(ActionTypes.B, ActionTypes.C, ActionTypes.R, ActionTypes.W)] ActionTypes actionType)
        {
            const double ratio = 0.5;
            string csvString = actionType.ToString() + ratio;
            IConvertedPokerRound expectedRound = new ConvertedPokerRound()
                .Add(new ConvertedPokerAction(actionType, ratio));

            IConvertedPokerRound round = _converter.ConvertedRoundFrom(csvString);

            Assert.That(round, Is.EqualTo(expectedRound));
        }

        [Test]
        public void ConvertedRoundFrom_OneInactiveAction_ReturnsRoundWithThatAction(
            [Values(ActionTypes.A, ActionTypes.F, ActionTypes.X)] ActionTypes actionType)
        {
            _stub.Value(For.Ratio).Is(1.0);

            string csvString = actionType.ToString();
            IConvertedPokerRound expectedRound = new ConvertedPokerRound()
                .Add(new ConvertedPokerAction(actionType, _stub.Out<double>(For.Ratio)));

            IConvertedPokerRound round = _converter.ConvertedRoundFrom(csvString);

            Assert.That(round, Is.EqualTo(expectedRound));
        }

        [Test]
        public void ConvertedRoundFrom_PokerActionWithId_ReturnsRoundWithPokerActionWithGivenId()
        {
            const ActionTypes actionType1 = ActionTypes.B;
            const double ratio1 = 0.5;
            const int playerId = 2;

            var expectedAction = new ConvertedPokerActionWithId(
                new ConvertedPokerAction(actionType1, ratio1), playerId);

            string csvString =
                "[" + playerId + "]" + actionType1 + ratio1;

            IConvertedPokerRound round = _converter.ConvertedRoundFrom(csvString);
            var firstAction = round[0] as IConvertedPokerActionWithId;

            Assert.That(firstAction, Is.EqualTo(expectedAction));
        }

        [Test]
        public void ConvertedRoundFrom_PokerActionWithId_ReturnsRoundWithPokerActionWithIdType()
        {
            const ActionTypes actionType1 = ActionTypes.B;
            const double ratio1 = 0.5;

            string csvString =
                "[0]" + actionType1 + ratio1;

            IConvertedPokerRound round = _converter.ConvertedRoundFrom(csvString);

            Assert.That(round[0], Is.AssignableTo(typeof(IConvertedPokerActionWithId)));
        }

        [Test]
        public void ConvertedRoundFrom_TwoActiveActions_ReturnsRoundWithBothActions()
        {
            const ActionTypes actionType1 = ActionTypes.B;
            const double ratio1 = 0.5;
            const ActionTypes actionType2 = ActionTypes.C;
            const double ratio2 = 0.2;

            string csvString =
                actionType1.ToString() + ratio1 + "," +
                actionType2 + ratio2;

            IConvertedPokerRound expectedRound = new ConvertedPokerRound()
                .Add(new ConvertedPokerAction(actionType1, ratio1))
                .Add(new ConvertedPokerAction(actionType2, ratio2));

            IConvertedPokerRound round = _converter.ConvertedRoundFrom(csvString);

            Assert.That(round, Is.EqualTo(expectedRound));
        }

        #endregion
    }
}