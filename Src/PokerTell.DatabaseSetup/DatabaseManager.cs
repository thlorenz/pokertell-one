namespace PokerTell.DatabaseSetup
{
    using System.Collections.Generic;
    using System.Data.Common;

    using Infrastructure.Interfaces.DatabaseSetup;

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

        public IDataProviderInfo DataProviderInfo
        {
            get { return _managedDatabase.DataProviderInfo; }
        }

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

        public string GetNameOfDatabaseInUse()
        {
            var databaseInUse = GetDatabaseInUse();
            if (databaseInUse != null)
            {
                return _managedDatabase.GetNameFor(databaseInUse);
            }

            return null;
        }

        public string GetDatabaseInUse()
        {
            string connectionString = _databaseSettings.GetConnectionStringFor(_managedDatabase.DataProviderInfo);

            var connectionInfo = new DatabaseConnectionInfo(connectionString);
         
            if (_managedDatabase.DataProviderInfo.IsEmbedded)
            {
                return connectionInfo.Database;
            }

            return connectionInfo.IsValidForDatabaseConnection() ? connectionInfo.Database : null;
        }

        public bool DatabaseExists(string databaseName)
        {
            return _managedDatabase.DatabaseExists(databaseName);
        }

        #endregion

        #endregion
    }
}