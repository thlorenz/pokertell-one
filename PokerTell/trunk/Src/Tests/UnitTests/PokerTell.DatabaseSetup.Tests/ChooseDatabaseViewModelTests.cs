namespace PokerTell.DatabaseSetup.Tests
{
    using System.Collections.Generic;
    using System.Linq;

    using Infrastructure.Interfaces.DatabaseSetup;

    using Moq;

    using NUnit.Framework;

    using PokerTell.DatabaseSetup.ViewModels;

    public class ChooseDatabaseViewModelTests
    {
        #region Constants and Fields

        Mock<IDatabaseManager> _databaseManagerMock;

        StubBuilder _stub;

        #endregion

        #region Public Methods

        [SetUp]
        public void _Init()
        {
            _stub = new StubBuilder();
            _databaseManagerMock = new Mock<IDatabaseManager>();
        }

        [Test]
        public void Constructor_DatabaseManagerFindsNoPokerTellDatabases_AvailableDatabasesIsEmpty()
        {
            _databaseManagerMock
                .Setup(dm => dm.GetAllPokerTellDatabases())
                .Returns(Enumerable.Empty<string>);
          
            var sut = new ChooseDatabaseViewModel(_databaseManagerMock.Object);

            Assert.That(sut.AvailableItems.Count, Is.EqualTo(0));
        }

        [Test]
        public void Constructor_DatabaseManagerFindsOnePokerTellDatabase_AvailableDatabasesContainsIt()
        {
            const string dataBaseName = "someDatabase";
            _databaseManagerMock
                .Setup(dm => dm.GetAllPokerTellDatabases())
                .Returns(new List<string> { dataBaseName });

            var sut = new ChooseDatabaseViewModel(_databaseManagerMock.Object);

            Assert.That(sut.AvailableItems.First(), Is.EqualTo(dataBaseName));
        }

        [Test]
        public void Constructor_DatabaseManagerFindsOnePokerTellDatabase_SelectedDatabaseIsSetToFirstDatabaseInList()
        {
            const string dataBaseName = "someDatabase";
            _databaseManagerMock
                .Setup(dm => dm.GetAllPokerTellDatabases())
                .Returns(new List<string> { dataBaseName });

            var sut = new ChooseDatabaseViewModel(_databaseManagerMock.Object);

            Assert.That(sut.SelectedItem, Is.EqualTo(dataBaseName));
        }

        [Test]
        public void SaveSettingsCommandCanExecute_SelectedDatabaseIsEmpty_ReturnsFalse()
        {
            var sut = new ChooseDatabaseViewModel(_stub.Out<IDatabaseManager>()) { SelectedItem = null };
            
            Assert.That(sut.SaveSettingsCommand.CanExecute(null), Is.False);
        }

        [Test]
        public void SaveSettingsCommandCanExecute_SelectedDatabaseHasValue_ReturnsTrue()
        {
            var sut = new ChooseDatabaseViewModel(_stub.Out<IDatabaseManager>()) { SelectedItem = "someName" };

            Assert.That(sut.SaveSettingsCommand.CanExecute(null), Is.True);
        }

        [Test]
        public void SaveSettingsCommandExecute_DatabaseSelected_CallsChooseDatabaseOnDatabaseManager()
        {
            const string dataBaseName = "someDatabase";
            _databaseManagerMock
                .Setup(dm => dm.GetAllPokerTellDatabases())
                .Returns(new List<string> { dataBaseName });

            var sut = new ChooseDatabaseViewModel(_databaseManagerMock.Object);

            sut.SaveSettingsCommand.Execute(null);

            _databaseManagerMock.Verify(dm => dm.ChooseDatabase(dataBaseName));
        }

        #endregion
    }
}