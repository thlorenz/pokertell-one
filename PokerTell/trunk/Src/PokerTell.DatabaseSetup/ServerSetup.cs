namespace PokerTell.DatabaseSetup
{
    using System.Collections.Generic;
    using System.Configuration;

    using Enumerations;

    using Infrastructure.Interfaces;
    using Infrastructure.Interfaces.DatabaseSetup;

    public abstract class ServerSetup
    {
        #region Constants and Fields

        protected readonly IDataProvider _dataProvider;

        protected readonly ISettings _settings;

        #endregion

        #region Constructors and Destructors

        public ServerSetup(IDataProvider dataProvider, ISettings settings, string parameterPlaceHolder)
        {
            _settings = settings;
            _dataProvider = dataProvider;
            _dataProvider.ParameterPlaceHolder = parameterPlaceHolder;
        }

        #endregion

        #region Properties

        public abstract string DatabaseName { get; }

        public string ParameterPlaceHolder
        {
            get { return _dataProvider.ParameterPlaceHolder; }
        }

        public string ProviderName
        {
            get { return _dataProvider.ProviderName; }
        }

        #endregion

        #region Public Methods

        public string GetSavedProviderName()
        {
            string providerName;
            _settings.Persist(ServerSettings.ProviderName.ToString(), out providerName);

            return providerName;
        }

        public abstract void ChooseDatabase(string databaseName);

        /// <summary>
        /// Template for database selection
        /// Only should be called after a connection has been established
        /// (e.g. if this was created as part of the Database class)
        /// </summary>
        /// <param name="databaseName">Name of database to choose</param>
        /// <exception cref="DatabaseDoesNotExistException">can't choose it</exception>
        public void ChooseDatabaseAndSaveConnectionString(string databaseName)
        {
            if (!ExistsDatabase(databaseName))
            {
                throw new DatabaseDoesNotExistException(string.Format("Databasename: '{0}'", databaseName));
            }
            ChooseDatabase(databaseName);

            //No errors so far -> save connection string
            SaveConnectionString(_dataProvider.Connection.ConnectionString);
        }

        public void ClearConnectionStrings()
        {
            _settings.ConnectionStrings.Clear();
            _settings.Save(ConfigurationSaveMode.Modified);
        }

        /// <summary>
        /// Template for clearing a given database
        /// Choose the given database and if successful recreate its tables, thereby clearing it
        /// </summary>
        /// <param name="databaseName">the database (with path and extension for SQLite)</param>
        /// <exception cref="DatabaseDoesNotExistException">can't choose or clear it</exception>
        public void ClearDatabase(string databaseName)
        {
            bool databaseExists = ExistsDatabase(databaseName);

            if (databaseExists)
            {
                ChooseDatabase(databaseName);

                CreateTables();
            }
            else
            {
                throw new DatabaseDoesNotExistException(string.Format("Databasename: '{0}'", databaseName));
            }
        }

        public abstract void ConnectToServer(string serverConnectString);

        /// <summary>
        /// Template to create a database with the given name.
        /// The tables will be created as well.
        /// </summary>
        /// <param name="databaseName">database to be created (without path and extension for SQLite)</param>
        /// <exception cref="DatabaseExistsException">don't allow overwrite of existing database</exception>
        public void CreateDatabase(string databaseName)
        {
            bool databaseDoesNotExist = !ExistsDatabase(databaseName);

            if (!databaseDoesNotExist)
            {
                throw new DatabaseExistsException(string.Format("Databasename: '{0}'", databaseName));
            }

            PerformCreateDatabase(databaseName);

            CreateTables();
        }

        public abstract void CreateTables();

        /// <summary>
        /// Template for Deletion of specified database
        /// </summary>
        /// <param name="databaseName"></param>
        /// /// <exception cref="DatabaseDoesNotExistException">can't choose or delete it</exception>
        /// <exception cref="System.IO.IOException">(Embedded only) if database is currently in use</exception>
        public void DeleteDatabase(string databaseName)
        {
            bool databaseExists = ExistsDatabase(databaseName);

            if (databaseExists)
            {
                PerformDeleteDatabase(databaseName);
            }
            else
            {
                throw new DatabaseDoesNotExistException(string.Format("Databasename: '{0}'", databaseName));
            }
        }

        public void Dispose()
        {
            if (_dataProvider != null)
            {
                _dataProvider.Dispose();
            }
        }

        public abstract bool ExistsDatabase(string dataBaseName);

        public abstract IEnumerable<string> GetAllPokerTellDatabases();

        public virtual string GetCurrentConnectionString()
        {
            return _dataProvider != null ? _dataProvider.Connection.ConnectionString : null;
        }

        public abstract string GetSavedConnectionString();

        public abstract string GetSavedServerConnectString();

        public virtual void SaveConnectionString(string connString)
        {
            if (ConnectionStringForMyProviderExists())
            {
                _settings.ConnectionStrings[ProviderName].ConnectionString = connString;
            }
            else
            {
                _settings.ConnectionStrings
                    .Add(new ConnectionStringSettings(ProviderName, connString, ProviderName));
            }
            _settings.Save(ConfigurationSaveMode.Modified);
        }

        public abstract void SaveServerConnectString(string serverConnectString);

        #endregion

        #region Methods

        protected bool ConnectionStringForMyProviderExists()
        {
            return _settings.ConnectionStrings[ProviderName] != null;
        }

        protected abstract void PerformCreateDatabase(string databaseName);

        protected abstract void PerformDeleteDatabase(string databaseName);

        #endregion
    }
}