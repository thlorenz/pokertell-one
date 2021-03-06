namespace PokerTell.PokerHand.Tests.Aquisition
{
    using Infrastructure.Enumerations.PokerHand;
    using Infrastructure.Interfaces.PokerHand;

    using NUnit.Framework;

    using PokerTell.PokerHand.Aquisition;

    using UnitTests;

    [TestFixture]
    public class AquiredPokerActionTests : TestWithLog
    {
        #region Constants and Fields

        const double BetSize = 2.0;

        const double CallSize = 1.5;

        const double NeutralSize = 1.0;

        const double PostSize = 0.2;

        const double RaiseSize = 3.0;

        const double WinSize = 5.0;

        IAquiredPokerAction _action;

        #endregion

        #region Public Methods

        [SetUp]
        public void _Init()
        {
            _action = new AquiredPokerAction();
        }

        [Test]
        [Sequential]
        public void ChipsGain_InitializedWithActiveActionAndRatio_ReturnsCorrectChipsGain(
            [Values(ActionTypes.B, ActionTypes.C, ActionTypes.P, ActionTypes.R, ActionTypes.W)] ActionTypes actionType, 
            [Values(BetSize, CallSize, PostSize, RaiseSize, WinSize)] double ratio, 
            [Values(-BetSize, -CallSize, -PostSize, -RaiseSize, WinSize)] double expectedGain)
        {
            _action.InitializeWith(actionType, ratio);
            Assert.That(_action.ChipsGained, Is.EqualTo(expectedGain));
        }

        [Test]
        [Sequential]
        public void ChipsGain_InitializedWithPassiveActionAndSomeRatio_ReturnsZero(
            [Values(ActionTypes.A, ActionTypes.F, ActionTypes.X)] ActionTypes actionType,
            [Values(NeutralSize, NeutralSize, NeutralSize)] double ratio,
            [Values(0, 0, 0)] double expectedGain)
        {
            _action.InitializeWith(actionType, ratio);
            Assert.That(_action.ChipsGained, Is.EqualTo(expectedGain));
        }

        [Test]
        [Sequential]
        public void ChipsGain_InitializedWithIllegalActionAndSomeRatio_ReturnsZero(
            [Values(ActionTypes.E, ActionTypes.N)] ActionTypes actionType,
            [Values(NeutralSize, NeutralSize)] double ratio,
            [Values(0, 0)] double expectedGain)
        {
            _action.InitializeWith(actionType, ratio);
            Assert.That(_action.ChipsGained, Is.EqualTo(expectedGain));
        }

        #endregion
    }
}