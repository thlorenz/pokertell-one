namespace PokerTell.DatabaseSetup
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;

    using Infrastructure.Enumerations.DatabaseSetup;
    using Infrastructure.Interfaces.DatabaseSetup;

    using NHibernate.Tool.hbm2ddl;

    using Properties;

    public class ExternalManagedDatabase : IManagedDatabase
    {
        #region Constants and Fields

        readonly IDataProvider _dataProvider;

        readonly IDataProviderInfo _dataProviderInfo;

        #endregion

        #region Constructors and Destructors
        /// <summary>
        /// Creates a database that will be managed by the Database Manager.
        /// Make sure to set up the dependencies as explained for each parameter.
        /// </summary>
        /// <param name="dataProvider">Needs to be connected to a server 
        /// ParameterPlaceHolder needs to be set</param>
        /// <param name="dataProviderInfo">External: MySql, Postgres etc.</param>
        public ExternalManagedDatabase(IDataProvider dataProvider, IDataProviderInfo dataProviderInfo)
        {
            _dataProviderInfo = dataProviderInfo;
            _dataProvider = dataProvider;
        }

        #endregion

        #region Properties

        public string ConnectionString { get; private set; }

        public IDataProviderInfo DataProviderInfo
        {
            get { return _dataProviderInfo; }
        }

        #endregion

        #region Implemented Interfaces

        #region IManagedDatabase

        public IManagedDatabase ChooseDatabase(string databaseName)
        {
            var connectionInfo = new DatabaseConnectionInfo(_dataProvider.Connection.ConnectionString);
            ConnectionString = string.Format("{0} Database = {1};", connectionInfo.ServerConnectString, databaseName);
            
            string nonQuery = string.Format("USE {0};", databaseName);
            _dataProvider.ExecuteNonQuery(nonQuery);
         
            return this;
        }

        public IManagedDatabase CreateDatabase(string databaseName)
        {
            string nonQuery = string.Format(Resources.Sql_Queries_CreateDatabase, databaseName);
            _dataProvider.ExecuteNonQuery(nonQuery);

            nonQuery = string.Format("USE {0};", databaseName);
            _dataProvider.ExecuteNonQuery(nonQuery);

            return this;
        }

        public IManagedDatabase CreateTables()
        {
//            string nonQuery = _dataProviderInfo.CreateTablesQuery;
//            _dataProvider.ExecuteNonQuery(nonQuery);

            _dataProvider.BuildSessionFactory();

            new SchemaExport(_dataProvider.NHibernateConfiguration)
                .Execute(false, true, false, _dataProvider.Connection, null);

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

            return GetNamesOfAllDatabasesThatContainPokerTellActionTableFrom(allDatabaseNames);
        }

        IEnumerable<string> GetNamesOfAllDatabasesThatContainPokerTellActionTableFrom(IEnumerable<string> allDatabaseNames)
        {
            foreach (string databaseName in allDatabaseNames)
            {
                string query = string.Format("USE {0}; SHOW TABLES;", databaseName);

                using (IDataReader dr = _dataProvider.ExecuteQuery(query))
                {
                    while (dr.Read())
                    {
                        string tableName = dr[0].ToString();
                        if (tableName.Equals(Tables.actionhhd.ToString()))
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

        #endregion

        #endregion
    }
}