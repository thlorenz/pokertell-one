namespace PokerTell.DatabaseSetup
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;

    using Infrastructure;

    using NHibernate.Tool.hbm2ddl;

    using PokerTell.DatabaseSetup.Properties;
    using PokerTell.Infrastructure.Interfaces.DatabaseSetup;

    public class ExternalManagedDatabase : IExternalManagedDatabase
    {
        public IDataProvider DataProvider { get;  protected set; }

        IDataProviderInfo _dataProviderInfo;

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
        public IManagedDatabase InitializeWith(IDataProvider dataProvider, IDataProviderInfo dataProviderInfo)
        {
            _dataProviderInfo = dataProviderInfo;
            DataProvider = dataProvider;

            return this;
        }

        public string ConnectionString { get; private set; }

        public IDataProviderInfo DataProviderInfo
        {
            get { return _dataProviderInfo; }
        }

        public IManagedDatabase ChooseDatabase(string databaseName)
        {
            var connectionInfo = new DatabaseConnectionInfo(DataProvider.Connection.ConnectionString);
            ConnectionString = string.Format("{0} Database = {1};", connectionInfo.ServerConnectString, databaseName);

            DataProvider.Connect(ConnectionString, _dataProviderInfo);

            return this;
        }

        public IManagedDatabase CreateDatabase(string databaseName)
        {
            string nonQuery = string.Format(Resources.Sql_Queries_CreateDatabase, databaseName);
            DataProvider.ExecuteNonQuery(nonQuery);

            ChooseDatabase(databaseName);

            return this;
        }

        public IManagedDatabase CreateTables()
        {
            DataProvider.BuildSessionFactory();

            new SchemaExport(DataProvider.NHibernateConfiguration)
                .Execute(false, true, false, DataProvider.Connection, null);

            return this;
        }

        public IManagedDatabase VersionDatabase(string databaseName)
        {
            DataProvider.ExecuteNonQuery(string.Format(Resources.Sql_Queries_External_InsertVersionNumber, databaseName, ApplicationProperties.VersionNumber));
            return this;
        }

        public bool DatabaseExists(string databaseName)
        {
            string query = Resources.Sql_Queries_ShowDatabases;

            using (IDataReader dr = DataProvider.ExecuteQuery(query))
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
            DataProvider.ExecuteNonQuery(nonQuery);
            return this;
        }

        /// <summary>
        /// Examines all tables in all databases.
        /// If the convertedpokerhands table is contained, the database name is included
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetAllPokerTellDatabaseNames()
        {
            return GetNamesOfAllDatabasesWhoseFirstTableIs("convertedpokerhands");
        }

        public IEnumerable<string> GetAllPokerOfficeDatabaseNames()
        {
            return GetAllDatabaseNames()
                .Where(name => name.Equals("pokeroffice") || name.EndsWith("_podb"));
        }

        public IEnumerable<string> GetAllPokerTrackerDatabaseNames()
        {
            throw new NotImplementedException();
        }

        public string GetNameFor(string databaseInUse)
        {
            return databaseInUse;
        }

        IEnumerable<string> GetNamesOfAllDatabasesWhoseFirstTableIs(string firstTableName)
        {
            var allDatabaseNames = GetAllDatabaseNames();

            foreach (string databaseName in allDatabaseNames)
            {
                string query = string.Format("USE `{0}`; SHOW TABLES;", databaseName);

                using (IDataReader dr = DataProvider.ExecuteQuery(query))
                {
                    while (dr.Read())
                    {
                        string tableName = dr[0].ToString();
                        if (tableName.Equals(firstTableName))
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

            using (IDataReader dr = DataProvider.ExecuteQuery(query))
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