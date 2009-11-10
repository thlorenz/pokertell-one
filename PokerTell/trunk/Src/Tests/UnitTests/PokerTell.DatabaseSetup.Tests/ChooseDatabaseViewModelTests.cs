namespace PokerTell.DatabaseSetup.Tests
{
    using System.Collections.Generic;
    using System.Linq;

    using Infrastructure.Events;
    using Infrastructure.Interfaces.DatabaseSetup;

    using Microsoft.Practices.Composite.Events;

    using Moq;

    using NUnit.Framework;

    using PokerTell.DatabaseSetup.ViewModels;

    public class ChooseDatabaseViewModelTests
    {
        #region Constants and Fields

        Mock<IDatabaseManager> _databaseManagerMock;

        StubBuilder _stub;

        IEventAggregator _eventAggregator;

        #endregion

        #region Public Methods

        [SetUp]
        public void _Init()
        {
            _stub = new StubBuilder();
            _databaseManagerMock = new Mock<IDatabaseManager>();
            _eventAggregator = new EventAggregator();
        }

        [Test]
        public void Constructor_DatabaseManagerFindsNoPokerTellDatabases_AvailableDatabasesIsEmpty()
        {
            _databaseManagerMock
                .Setup(dm => dm.GetAllPokerTellDatabases())
                .Returns(Enumerable.Empty<string>);

            var sut = new ChooseDatabaseViewModel(_eventAggregator, _databaseManagerMock.Object);

            Assert.That(sut.AvailableItems.Count, Is.EqualTo(0));
        }

        [Test]
        public void Constructor_DatabaseManagerFindsOnePokerTellDatabase_AvailableDatabasesContainsIt()
        {
            const string dataBaseName = "someDatabase";
            _databaseManagerMock
                .Setup(dm => dm.GetAllPokerTellDatabases())
                .Returns(new List<string> { dataBaseName });

            var sut = new ChooseDatabaseViewModel(_eventAggregator, _databaseManagerMock.Object);

            Assert.That(sut.AvailableItems.First(), Is.EqualTo(dataBaseName));
        }

        [Test]
        public void DetermineSelectedItem_DatabaseManagerFindsOnePokerTellDatabaseThatIsNotUsed_SelectedDatabaseIsEmpty()
        {
            const string notInUseDatabase = "someDatabase";
            _databaseManagerMock
                .Setup(dm => dm.GetAllPokerTellDatabases())
                .Returns(new List<string> { notInUseDatabase });

            var sut = new ChooseDatabaseViewModel(_eventAggregator, _databaseManagerMock.Object)
                .DetermineSelectedItem();

            Assert.That(sut.SelectedItem, Is.EqualTo(string.Empty));
        }

        [Test]
        public void DetermineSelectedItem_TwoDatabasesInSettingsOneIsCurrent_SelectedDatabaseIsSetToCurrentDatabaseName()
        {
            const string dataBaseName = "someDatabase";
            const string databaseInUse = "databaseInUse";
            
            _databaseManagerMock
                .Setup(dm => dm.GetAllPokerTellDatabases())
                .Returns(new List<string> { dataBaseName, databaseInUse });
            _databaseManagerMock
                .Setup(dm => dm.GetDatabaseInUse())
                .Returns(databaseInUse);

            var sut = new ChooseDatabaseViewModel(_eventAggregator, _databaseManagerMock.Object)
                .DetermineSelectedItem();

            Assert.That(sut.SelectedItem, Is.EqualTo(databaseInUse));
        }

        [Test]
        public void CommitActionCommandCanExecute_SelectedDatabaseIsEmpty_ReturnsFalse()
        {
            var sut = new ChooseDatabaseViewModel(_eventAggregator, _stub.Out<IDatabaseManager>()) { SelectedItem = null };
            
            Assert.That(sut.CommitActionCommand.CanExecute(null), Is.False);
        }

        [Test]
        public void CommitActionCommandCanExecute_SelectedDatabaseHasValue_ReturnsTrue()
        {
            var sut = new ChooseDatabaseViewModel(_eventAggregator, _stub.Out<IDatabaseManager>()) { SelectedItem = "someName" };

            Assert.That(sut.CommitActionCommand.CanExecute(null), Is.True);
        }

        [Test]
        public void CommitActionCommandExecute_DatabaseSelected_CallsDatabaseManagerChooseDatabaseWithSelectedItem()
        {
            const string dataBaseName = "someDatabase";

            var sut = new ChooseDatabaseViewModel(_eventAggregator, _databaseManagerMock.Object) { SelectedItem = dataBaseName };

            sut.CommitActionCommand.Execute(null);

            _databaseManagerMock.Verify(dm => dm.ChooseDatabase(dataBaseName));
        }

        [Test]
        public void CommitActionCommandExecute_DatabaseSelected_PublishesInfoMessage()
        {
            bool infoWasPublished = false;
            
            _eventAggregator.GetEvent<UserMessageEvent>().Subscribe(
                arg => infoWasPublished = arg.MessageType == UserMessageTypes.Info);
            
            var sut = new ChooseDatabaseViewModel(_eventAggregator, _databaseManagerMock.Object)
                { SelectedItem = "someDatabase" };

            sut.CommitActionCommand.Execute(null);

            Assert.That(infoWasPublished, Is.True);
        }

        #endregion
    }
}