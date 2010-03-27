namespace PokerTell.DatabaseSetup.Tests
{
    using Infrastructure.Events;
    using Infrastructure.Interfaces.DatabaseSetup;
    using Infrastructure.Interfaces.Repository;

    using Microsoft.Practices.Composite.Events;

    using Moq;

    using NUnit.Framework;

    using ViewModels;

    public class CreateDatabaseViewModelTests
    {
        IEventAggregator _eventAggregator;

        [SetUp]
        public void _Init()
        {
            _eventAggregator = new EventAggregator();
        }

        [Test]
        public void CommitActionCommandExecute_DatabaseToCreateExists_DoesNotCallDatabaseManagerCreateDatabase()
        {
            const string databaseToCreate = "databaseToCreate";
            
            var databaseManagerMock = new Mock<IDatabaseManager>();
            databaseManagerMock
                .Setup(dm => dm.DatabaseExists(databaseToCreate))
                .Returns(true);

            var sut = new CreateDatabaseViewModel(_eventAggregator, databaseManagerMock.Object)
                { SelectedItem = databaseToCreate };

            sut.CommitActionCommand.Execute(null);

            databaseManagerMock.Verify(dm => dm.CreateDatabase(It.IsAny<string>()), Times.Never());
        }

        [Test]
        public void CommitActionCommandExecute_DatabaseToCreateExists_PublishesWarning()
        {
            const string databaseToCreate = "databaseToCreate";

            var databaseManagerMock = new Mock<IDatabaseManager>();
            databaseManagerMock
                .Setup(dm => dm.DatabaseExists(databaseToCreate))
                .Returns(true);

            bool warningWasPublished = false;
            _eventAggregator
                .GetEvent<UserMessageEvent>()
                .Subscribe(arg => warningWasPublished = arg.MessageType == UserMessageTypes.Warning);

            var sut = new CreateDatabaseViewModel(_eventAggregator, databaseManagerMock.Object) { SelectedItem = databaseToCreate };

            sut.CommitActionCommand.Execute(null);

            Assert.That(warningWasPublished, Is.True);
        }


        [Test]
        public void CommitActionCommandExecute_DatabaseToCreateDoesNotExist_CallsDatabaseManagerCreateDatabaseWithDatabaseToCreate()
        {
            const string databaseToCreate = "databaseToCreate";

            var databaseManagerMock = new Mock<IDatabaseManager>();
            databaseManagerMock
                .Setup(dm => dm.DatabaseExists(databaseToCreate))
                .Returns(false);

            var sut = new CreateDatabaseViewModel(_eventAggregator, databaseManagerMock.Object) { SelectedItem = databaseToCreate };

            sut.CommitActionCommand.Execute(null);

            databaseManagerMock.Verify(dm => dm.CreateDatabase(databaseToCreate));
        }

        [Test]
        public void CommitActionCommandExecute_DatabaseToCreateDoesNotExist_PublishesSuccesInfo()
        {
            const string databaseToCreate = "databaseToCreate";

            var databaseManagerMock = new Mock<IDatabaseManager>();
            databaseManagerMock
                .Setup(dm => dm.DatabaseExists(databaseToCreate))
                .Returns(false);

            bool infoWasPublished = false;
            _eventAggregator
                .GetEvent<UserMessageEvent>()
                .Subscribe(arg => infoWasPublished = arg.MessageType == UserMessageTypes.Info);

            var sut = new CreateDatabaseViewModel(_eventAggregator, databaseManagerMock.Object) { SelectedItem = databaseToCreate };

            sut.CommitActionCommand.Execute(null);

           Assert.That(infoWasPublished, Is.True);
        }
    }
}