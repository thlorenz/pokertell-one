namespace PokerTell.DatabaseSetup
{
    using System.Collections.Generic;

    using Interfaces;

    public class DatabaseManager : IDatabaseManager
    {
        #region Constants and Fields

        readonly IDatabaseSettings _databaseSettings;

        readonly IManagedDatabase _managedDatabase;

        #endregion

        #region Constructors and Destructors

        public DatabaseManager(IManagedDatabase managedDatabase, IDatabaseSettings databaseSettings)
        {
            _databaseSettings = databaseSettings;
            _managedDatabase = managedDatabase;
        }

        #endregion

        #region Implemented Interfaces

        #region IDatabaseManager

        public IDatabaseManager ChooseDatabase(string databaseName)
        {
            if (!_managedDatabase.DatabaseExists(databaseName))
            {
                throw new DatabaseDoesNotExistException(string.Format("Databasename: '{0}'", databaseName));
            }

            _managedDatabase.ChooseDatabase(databaseName);
            _databaseSettings.SetConnectionStringFor(
                _managedDatabase.DataProviderInfo, _managedDatabase.ConnectionString);
            return this;
        }

        public IDatabaseManager ClearDatabase(string databaseName)
        {
            if (!_managedDatabase.DatabaseExists(databaseName))
            {
                throw new DatabaseDoesNotExistException(string.Format("Databasename: '{0}'", databaseName));
            }

            _managedDatabase.ChooseDatabase(databaseName);
            _managedDatabase.CreateTables();
            return this;
        }

        public IDatabaseManager CreateDatabase(string databaseName)
        {
            if (_managedDatabase.DatabaseExists(databaseName))
            {
                throw new DatabaseExistsException(string.Format("Databasename: '{0}'", databaseName));
            }

            _managedDatabase.CreateDatabase(databaseName);
            _managedDatabase.CreateTables();
            return this;
        }

        public IDatabaseManager DeleteDatabase(string databaseName)
        {
            if (!_managedDatabase.DatabaseExists(databaseName))
            {
                throw new DatabaseDoesNotExistException(string.Format("Databasename: '{0}'", databaseName));
            }

            _managedDatabase.DeleteDatabase(databaseName);
            return this;
        }

        public IEnumerable<string> GetAllPokerTellDatabases()
        {
            return _managedDatabase.GetAllPokerTellDatabaseNames();
        }

        #endregion

        #endregion
    }
}