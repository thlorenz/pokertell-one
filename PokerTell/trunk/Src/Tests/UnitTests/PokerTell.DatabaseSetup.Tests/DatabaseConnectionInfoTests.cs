namespace PokerTell.DatabaseSetup.Tests
{
    using System;

    using NUnit.Framework;

    public class DatabaseConnectionInfoTests
    {
        [Test]
        public void Construct_SqliteConnectionStringContainingFilePath_DatabaseIsFileNameWithoutExtension()
        {
            const string databaseName = "testdatabase";
            const string fileName = databaseName + ".db3";
            const string fullPath = @"C:\Documents\User\" + fileName;
            var sut = new DatabaseConnectionInfo("Data Source = " + fullPath);

            Assert.That(sut.Database, Is.EqualTo(databaseName));
        }
    }
}