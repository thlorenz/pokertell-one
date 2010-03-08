namespace PokerTell.DatabaseSetup.Tests
{
    using Infrastructure.Events;
    using Infrastructure.Interfaces.DatabaseSetup;

    using Microsoft.Practices.Composite.Events;

    using Moq;

    using NUnit.Framework;

    using ViewModels;

    public class ClearDatabaseViewModelTests
    {
        IEventAggregator _eventAggregator;

        StubBuilder _stub;

        [SetUp]
        public void _Init()
        {
            _stub = new StubBuilder();
            _eventAggregator = new EventAggregator();
        }

        [Test]
        public void CommitActionComandExecute_Always_PublishesUserConfirmActionEvent()
        {
            bool confirmActionEventWasPublished = false;

            _eventAggregator
                .GetEvent<UserConfirmActionEvent>()
                .Subscribe(arg => confirmActionEventWasPublished = true);
            var sut = new ClearDatabaseViewModel(_eventAggregator, _stub.Out<IDatabaseManager>()); 

            sut.CommitActionCommand.Execute(null);

            Assert.That(confirmActionEventWasPublished, Is.True);
        }

        [Test]
        public void CommitActionComandExecute_PublishedUserConfirmActionEvent_ConfirmActionDelegatesToDatabaseClearAction()
        {
            var databaseManagerMock = new Mock<IDatabaseManager>();

            const string databasename = "databaseName";
            var sut = new ClearDatabaseViewModel(_eventAggregator, databaseManagerMock.Object)
                { SelectedItem = databasename };

            _eventAggregator
                .GetEvent<UserConfirmActionEvent>()
                .Subscribe(arg => arg.ActionToConfirm());

            sut.CommitActionCommand.Execute(null);

           databaseManagerMock.Verify(dm => dm.ClearDatabase(databasename));
        }
    }
}