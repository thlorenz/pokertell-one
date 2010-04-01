namespace PokerTell.User.Tests.ViewModels
{
    using System;

    using NUnit.Framework;

    using User.ViewModels;

    public class ConfirmActionViewModelTests
    {
        [Test]
        public void ConfirmActionCommandExecute_ActionWasPassedInTheConstructor_ExecutesThatAction()
        {
            bool actionExecuted = false;
            Action actionToExecute = () => actionExecuted = true;
            var sut = new ConfirmActionViewModel(actionToExecute, "some Warning Message");

            sut.ConfirmActionCommand.Execute(null);

            Assert.That(actionExecuted, Is.True);
        }
    }
}