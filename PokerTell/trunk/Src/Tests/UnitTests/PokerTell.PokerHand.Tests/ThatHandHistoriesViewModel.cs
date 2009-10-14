namespace PokerTell.PokerHand.Tests
{
    using System;

    using NUnit.Framework;

    using PokerTell.Infrastructure.Interfaces.PokerHand;
    using PokerTell.PokerHand.Tests.Fakes;
    using PokerTell.PokerHand.ViewModels;

    public class ThatHandHistoriesViewModel
    {
        #region Public Methods

        [Test]
        public void ExecutingApplyFilter_HasRegisteredCommands_ExecutesRegisteredCommands()
        {
            var historiesViewModel = new HandHistoriesViewModel();

            bool action1Executed = false;
            bool action2Executed = false;

            var action1 = new Action<IPokerHandCondition>(c => action1Executed = true);
            var action2 = new Action<IPokerHandCondition>(c => action2Executed = true);

             historiesViewModel.ApplyFilterCompositeAction
                .RegisterAction(action1)
                .RegisterAction(action2);

            historiesViewModel.ApplyFilterCompositeAction.Execute(null);

            bool bothCommandsExecuted = action1Executed && action2Executed;

            Assert.That(bothCommandsExecuted, Is.True);
        }

        [Test]
        public void ExecutingApplyFilter_WithPokerHandCondition_PassesConditionToChildCommands()
        {
            var historiesViewModel = new HandHistoriesViewModel();

            const string player = "player1";

            IPokerHandCondition condition = new StubCondition().AppliesTo(player);
            IPokerHandCondition passedCondition = null;

            var action1 = new Action<IPokerHandCondition>(c => passedCondition = c);

            historiesViewModel.ApplyFilterCompositeAction
                .RegisterAction(action1)
                .Execute(condition);

            Assert.That(passedCondition, Is.EqualTo(condition));
        }

        [Test]
        public void InitializeWith_EmptyHands_KeepsHandHistoriesEmpty()
        {
            
        }

        #endregion
    }
}