namespace PokerTell.DatabaseSetup
{
    using System;
    using System.Collections.Generic;
    using System.Data;

    using Infrastructure;

    using NHibernate.Tool.hbm2ddl;

    using PokerTell.DatabaseSetup.Properties;
    using PokerTell.Infrastructure.Interfaces.DatabaseSetup;

    public class ExternalManagedDatabase : IManagedDatabase
    {
        readonly IDataProvider _dataProvider;

        readonly IDataProviderInfo _dataProviderInfo;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExternalManagedDatabase"/> class. 
        /// Creates a database that will be managed by the Database Manager.
        /// Make sure to set up the dependencies as explained for each parameter.
        /// </summary>
        /// <param name="dataProvider">
        /// Needs to be connected to a server 
        /// ParameterPlaceHolder needs to be set
        /// </param>
        /// <param name="dataProviderInfo">
        /// External: MySql, Postgres etc.
        /// </param>
        public ExternalManagedDatabase(IDataProvider dataProvider, IDataProviderInfo dataProviderInfo)
        {
            _dataProviderInfo = dataProviderInfo;
            _dataProvider = dataProvider;
        }

        public string ConnectionString { get; private set; }

        public IDataProviderInfo DataProviderInfo
        {
            get { return _dataProviderInfo; }
        }

        public IManagedDatabase ChooseDatabase(string databaseName)
        {
            var connectionInfo = new DatabaseConnectionInfo(_dataProvider.Connection.ConnectionString);
            ConnectionString = string.Format("{0} Database = {1};", connectionInfo.ServerConnectString, databaseName);

            _dataProvider.Connect(ConnectionString, _dataProviderInfo);

            string nonQuery = string.Format("USE `{0}`;", databaseName);
            _dataProvider.ExecuteNonQuery(nonQuery);

            _dataProvider.BuildSessionFactory();

            return this;
        }

        public IManagedDatabase CreateDatabase(string databaseName)
        {
            string nonQuery = string.Format(Resources.Sql_Queries_CreateDatabase, databaseName);
            _dataProvider.ExecuteNonQuery(nonQuery);

            ChooseDatabase(databaseName);

            return this;
        }

        public IManagedDatabase CreateTables()
        {
            new SchemaExport(_dataProvider.NHibernateConfiguration)
                .Execute(false, true, false, _dataProvider.Connection, null);

            return this;
        }

        public IManagedDatabase VersionDatabase(string databaseName)
        {
            _dataProvider.ExecuteNonQuery(string.Format(Resources.Sql_Queries_External_InsertVersionNumber, databaseName, ApplicationProperties.VersionNumber));
            return this;
        }

        public bool DatabaseExists(string databaseName)
        {
            string query = Resources.Sql_Queries_ShowDatabases;

            using (IDataReader dr = _dataProvider.ExecuteQuery(query))
            {
                while (dr.Read())
                {
                    if (dr.GetString(0).Equals(databaseName))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public IManagedDatabase DeleteDatabase(string databaseName)
        {
            string nonQuery = string.Format(Resources.Sql_Queries_DropDatabase, databaseName);
            _dataProvider.ExecuteNonQuery(nonQuery);
            return this;
        }

        /// <summary>
        /// Examines all tables in all databases.
        /// If the actionhhd table is contained, the database name is included
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetAllPokerTellDatabaseNames()
        {
            var allDatabaseNames = GetAllDatabaseNames();

            return GetNamesOfAllDatabasesThatContainConvertedPokerHandsTableFrom(allDatabaseNames);
        }

        public string GetNameFor(string databaseInUse)
        {
            return databaseInUse;
        }

        IEnumerable<string> GetNamesOfAllDatabasesThatContainConvertedPokerHandsTableFrom(IEnumerable<string> allDatabaseNames)
        {
            foreach (string databaseName in allDatabaseNames)
            {
                string query = string.Format("USE `{0}`; SHOW TABLES;", databaseName);

                using (IDataReader dr = _dataProvider.ExecuteQuery(query))
                {
                    while (dr.Read())
                    {
                        string tableName = dr[0].ToString();
                        if (tableName.Equals("convertedpokerhands"))
                        {
                            yield return databaseName;
                        }
                    }
                }
            }
        }

        IEnumerable<string> GetAllDatabaseNames()
        {
            var allDatabases = new List<string>();
            const string query = "SHOW DATABASES;";

            using (IDataReader dr = _dataProvider.ExecuteQuery(query))
            {
                while (dr.Read())
                {
                    allDatabases.Add(dr[0].ToString());
                }
            }

            return allDatabases;
        }
    }
}