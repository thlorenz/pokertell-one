namespace PokerTell.DatabaseSetup.Tests
{
    using Infrastructure.Events;
    using Infrastructure.Interfaces.DatabaseSetup;

    using Microsoft.Practices.Composite.Events;

    using Moq;

    using NUnit.Framework;

    using ViewModels;

    public class DeleteDatabaseViewModelTests
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
        public void CommitActionComandExecute_PublishedUserConfirmActionEvent_ConfirmActionDelegatesToDatabaseDeleteAction()
        {
            var databaseManagerMock = new Mock<IDatabaseManager>();

            const string databasename = "databaseName";
            var sut = new DeleteDatabaseViewModel(_eventAggregator, databaseManagerMock.Object)
                { SelectedItem = databasename };

            _eventAggregator
                .GetEvent<UserConfirmActionEvent>()
                .Subscribe(arg => arg.ActionToConfirm());

            sut.CommitActionCommand.Execute(null);

            databaseManagerMock.Verify(dm => dm.DeleteDatabase(databasename));
        }

        [Test]
        public void CommitActionComandExecute_ActionGetsConfirmed_RemovesDeletedDatabaseFromAvailableItems()
        {
            const string otherDatabase = "databaseName";
            const string deletedDatabase = "deletedDatabase";
            var sut = new DeleteDatabaseViewModel(_eventAggregator, _stub.Out<IDatabaseManager>()) { SelectedItem = deletedDatabase };
            sut.AvailableItems.Add(otherDatabase);
            sut.AvailableItems.Add(deletedDatabase);

            _eventAggregator
                .GetEvent<UserConfirmActionEvent>()
                .Subscribe(arg => arg.ActionToConfirm());

            sut.CommitActionCommand.Execute(null);

            Assert.That(sut.AvailableItems.Contains(deletedDatabase), Is.False);
        }

        [Test]
        public void CommitActionComandExecute_ActionGetsConfirmed_DoesNotRemoveOtherDatabaseFromAvailableItems()
        {
            const string otherDatabase = "databaseName";
            const string deletedDatabase = "deletedDatabase";
            var sut = new DeleteDatabaseViewModel(_eventAggregator, _stub.Out<IDatabaseManager>()) { SelectedItem = deletedDatabase };
            sut.AvailableItems.Add(otherDatabase);
            sut.AvailableItems.Add(deletedDatabase);

            _eventAggregator
                .GetEvent<UserConfirmActionEvent>()
                .Subscribe(arg => arg.ActionToConfirm());

            sut.CommitActionCommand.Execute(null);

            Assert.That(sut.AvailableItems.Contains(otherDatabase), Is.True);
        }

        [Test]
        public void RemoveDatabaseInUseFromAvailableItems_OneOfTheAvailableDatabasesIsInUse_RemovesThatDatabaseFromAvailableItems()
        {
            const string otherDatabase = "databaseName";
            const string databaseInUse = "databaseInUse";
            
            var databaseManagerMock = new Mock<IDatabaseManager>();
            databaseManagerMock
                .Setup(dm => dm.GetDatabaseInUse())
                .Returns(databaseInUse);

            var sut = new DeleteDatabaseViewModel(_eventAggregator, databaseManagerMock.Object);
            sut.AvailableItems.Add(otherDatabase);
            sut.AvailableItems.Add(databaseInUse);

            sut.RemoveDatabaseInUseFromAvailableItems();

            Assert.That(sut.AvailableItems.Contains(databaseInUse), Is.False);
        }
    }
}