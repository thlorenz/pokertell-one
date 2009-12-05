namespace PokerTell.PokerHand.Tests.Services
{
    using System;

    using Infrastructure.Enumerations.PokerHand;
    using Infrastructure.Interfaces.PokerHand;
    using Infrastructure.Services;

    using NUnit.Framework;

    using PokerTell.PokerHand.Analyzation;
    using PokerTell.PokerHand.Aquisition;
    using PokerTell.PokerHand.Services;

    using UnitTests;

    [TestFixture]
    public class PokerActionConverterTests : TestWithLog
    {
        #region Constants and Fields

        PokerActionConverter _converter;

        #endregion

        #region Public Methods

        [SetUp]
        public void _Init()
        {
            _converter =
                new PokerActionConverter(new Constructor<IConvertedPokerAction>(() => new ConvertedPokerAction()));
        }

        [Test]
        public void Convert_Bet_SetsToCallToRatio()
        {
            const ActionTypes actionType = ActionTypes.B;
            double toCall = 0;
            const double ratio = 1.0;
            const double expectedToCall = ratio;

            double somePot = 3.0;
            const double someTotalPot = 4.0;
            var aquiredAction = new AquiredPokerAction(actionType, ratio);

            _converter.Convert(aquiredAction, ref somePot, ref toCall, someTotalPot);

            Assert.That(toCall, Is.EqualTo(expectedToCall));
        }

        [Test]
        [Sequential]
        public void Convert_CallBet_SetsConvertedRatioToAquiredRatioDividedByPot(
            [Values(ActionTypes.C, ActionTypes.B)] ActionTypes actionType)
        {
            const double aquiredRatio = 1.0;
            const double origPot = 2.0;
            const double expectedRatio = aquiredRatio / origPot;

            double pot = origPot;
            double someToCall = 0;
            const double someTotalPot = 3.0;

            var aquiredAction = new AquiredPokerAction(actionType, aquiredRatio);

            var convertedAction = _converter.Convert(aquiredAction, ref pot, ref someToCall, someTotalPot);

            Assert.That(convertedAction.Ratio, Is.EqualTo(expectedRatio));
        }

        [Test]
        [Sequential]
        public void Convert_CallBetRaise_AddsRatioToPot(
            [Values(ActionTypes.C, ActionTypes.B, ActionTypes.R)] ActionTypes actionType)
        {
            double pot = 2.0;
            const double ratio = 1.0;
            double expectedPot = pot + ratio;

            double someToCall = 3.0;
            const double someTotalPot = 4.0;

            var aquiredAction = new AquiredPokerAction(actionType, ratio);

            _converter.Convert(aquiredAction, ref pot, ref someToCall, someTotalPot);

            Assert.That(pot, Is.EqualTo(expectedPot));
        }

        [Test]
        [Sequential]
        public void Convert_FoldCheckAllin_SetsConvertedRatioToAquiredRatio(
            [Values(ActionTypes.F, ActionTypes.X, ActionTypes.A)] ActionTypes actionType)
        {
            // For these actions the ratio is irelevant, but still should not be changed
            const double aquiredRatio = 1.0;

            double somePot = 0;
            double someToCall = 1.2;
            const double someTotalPot = 2.0;

            var aquiredAction = new AquiredPokerAction(actionType, aquiredRatio);

            var convertedAction = _converter.Convert(
                aquiredAction, ref somePot, ref someToCall, someTotalPot);

            Assert.That(convertedAction.Ratio, Is.EqualTo(aquiredRatio));
        }

        [Test]
        [Sequential]
        public void Convert_FoldCheckAllinWin_DoesntChangePot(
            [Values(ActionTypes.F, ActionTypes.X, ActionTypes.A, ActionTypes.W)] ActionTypes actionType)
        {
            const double originalValue = 3.0;
            double pot = originalValue;

            double someToCall = 1.0;
            const double someTotalPot = 4.0;
            const double someRatio = 1.5;
            var aquiredAction = new AquiredPokerAction(actionType, someRatio);

            _converter.Convert(aquiredAction, ref pot, ref someToCall, someTotalPot);

            Assert.That(pot, Is.EqualTo(originalValue));
        }

        [Test]
        [Sequential]
        public void Convert_FoldCheckCallAllinWin_DoesntChangeToCall(
            [Values(ActionTypes.F, ActionTypes.X, ActionTypes.C, ActionTypes.A, ActionTypes.W)] ActionTypes actionType)
        {
            const double originalValue = 1.5;
            double toCall = originalValue;

            double somePot = 3.0;

            const double someTotalPot = 4.0;
            const double someRatio = 1.0;
            var aquiredAction = new AquiredPokerAction(actionType, someRatio);

            _converter.Convert(aquiredAction, ref somePot, ref toCall, someTotalPot);

            Assert.That(toCall, Is.EqualTo(originalValue));
        }

        [Test]
        [Sequential]
        public void Convert_InValidActionType_ThrowsArgumentException(
            [Values(ActionTypes.E, ActionTypes.N, ActionTypes.P, ActionTypes.U)] ActionTypes actionType)
        {
            double somePot = 3.0;
            double someToCall = 1.5;
            const double someTotalPot = 4.0;
            const double someRatio = 1.0;
            var aquiredAction = new AquiredPokerAction(actionType, someRatio);

            TestDelegate invokeConvert =
                () => _converter.Convert(aquiredAction, ref somePot, ref someToCall, someTotalPot);

            Assert.Throws<ArgumentException>(invokeConvert);
        }

        [Test]
        [Sequential]
        public void Convert_Raise_SetsConvertedRatioToAquiredRatioDividedByToCall()
        {
            const ActionTypes actionType = ActionTypes.R;
            const double aquiredRatio = 2.0;
            double toCall = 1.5;
            double expectedRatio = aquiredRatio / toCall;

            double somePot = 1.0;
            
            const double someTotalPot = 2.0;

            var aquiredAction = new AquiredPokerAction(actionType, aquiredRatio);

            IConvertedPokerAction convertedAction = _converter.Convert(
                aquiredAction, ref somePot, ref toCall, someTotalPot);

            Assert.That(convertedAction.Ratio, Is.EqualTo(expectedRatio));
        }

        [Test]
        public void Convert_RaiseToCallIsGreaterThanRatio_DoesntChangeToCall()
        {
            const ActionTypes actionType = ActionTypes.R;
            const double originalValue = 2.0;
            double toCall = originalValue;
            const double ratio = 1.0;

            double somePot = 3.0;
            const double someTotalPot = 4.0;
            var aquiredAction = new AquiredPokerAction(actionType, ratio);

            _converter.Convert(aquiredAction, ref somePot, ref toCall, someTotalPot);

            Assert.That(toCall, Is.EqualTo(originalValue));
        }

        [Test]
        public void Convert_RaiseToCallIsSmallerThanRatio_SetsToCallToRatio()
        {
            const ActionTypes actionType = ActionTypes.R;
            double toCall = 0;
            const double ratio = 1.0;
            const double expectedToCall = ratio;

            double somePot = 3.0;
            const double someTotalPot = 4.0;
            var aquiredAction = new AquiredPokerAction(actionType, ratio);

            _converter.Convert(aquiredAction, ref somePot, ref toCall, someTotalPot);

            Assert.That(toCall, Is.EqualTo(expectedToCall));
        }

        [Test]
        [Sequential]
        public void Convert_ValidActionType_ReturnsConvertedActionWithSameActionType(
            [Values(ActionTypes.A, ActionTypes.B, ActionTypes.C, ActionTypes.F, 
                ActionTypes.R, ActionTypes.W, ActionTypes.X)] ActionTypes actionType)
        {
            double somePot = 3.0;
            double someToCall = 1.5;
            const double someTotalPot = 4.0;
            const double someRatio = 1.0;
            var aquiredAction = new AquiredPokerAction(actionType, someRatio);
            ActionTypes expectedActionType = actionType;

            IConvertedPokerAction convertedAction = _converter.Convert(
                aquiredAction, ref somePot, ref someToCall, someTotalPot);

            Assert.That(convertedAction.What, Is.EqualTo(expectedActionType));
        }

        [Test]
        [Sequential]
        public void Convert_Win_SetsConvertedRatioToAquiredRatioDividedByTotalPot()
        {
            const ActionTypes actionType = ActionTypes.W;
            const double aquiredRatio = 2.0;
            const double totalPot = 4.0;
            const double expectedRatio = aquiredRatio / totalPot;

            double somePot = 1.0;
            double someToCall = 1.5;

            var aquiredAction = new AquiredPokerAction(actionType, aquiredRatio);

            IConvertedPokerAction convertedAction = _converter.Convert(
                aquiredAction, ref somePot, ref someToCall, totalPot);

            Assert.That(convertedAction.Ratio, Is.EqualTo(expectedRatio));
        }

        #endregion
    }
}