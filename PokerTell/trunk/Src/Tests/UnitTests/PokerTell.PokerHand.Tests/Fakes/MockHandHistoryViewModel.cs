namespace PokerTell.PokerHand.Tests.Fakes
{
    using System;

    using Infrastructure.Interfaces.PokerHand;

    using ViewModels;

    public class MockHandHistoryViewModel : HandHistoryViewModel
    {
        public MockHandHistoryViewModel SetAdjustToConditionAction(Action<IPokerHandCondition> action)
        {
            _adjustToConditionAction = action;
            return this;
        }
    }
}