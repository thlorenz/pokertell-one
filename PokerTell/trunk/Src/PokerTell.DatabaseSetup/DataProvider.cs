namespace PokerTell.DatabaseSetup
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Reflection;
    using System.Text;

    using Infrastructure;

    using log4net;

    using NHibernate;
    using NHibernate.ByteCode.Castle;
    using NHibernate.Cfg;

    using PokerTell.DatabaseSetup.Properties;
    using PokerTell.Infrastructure.Interfaces.DatabaseSetup;

    using Environment = NHibernate.Cfg.Environment;

    public class DataProvider : IDataProvider
    {
        #region Constants and Fields

        static readonly ILog Log =
            LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        IDbConnection _connection;

        DbProviderFactory _providerFactory;

        IDataProviderInfo _dataProviderInfo;

        Configuration _configuration;

        #endregion

        #region Constructors and Destructors

        public DataProvider()
        {
            DatabaseName = Resources.Status_NotConnectedToDatabase;
        }

        ~DataProvider()
        {
            if (_connection != null)
            {
                _connection.Close();
            }
        }

        #endregion

        #region Properties

        public IDbConnection Connection
        {
            get { return _connection; }
        }

        public string DatabaseName { get; set; }

        public bool IsConnectedToDatabase
        {
            get
            {
                return _connection != null && _connection.State.Equals(ConnectionState.Open) &&
                       ! string.IsNullOrEmpty(_connection.Database);
            }
        }

        public bool IsConnectedToServer
        {
            get { return _connection != null && _connection.State.Equals(ConnectionState.Open); }
        }

        public string ParameterPlaceHolder { get; set; }

        public Configuration NHibernateConfiguration
        {
            get { return _configuration; }
        }

        #endregion

        #region Implemented Interfaces

        #region IDataProvider

        public void Connect(string connString, IDataProviderInfo dataProviderInfo)
        {
            _dataProviderInfo = dataProviderInfo;
            _providerFactory = DbProviderFactories.GetFactory(dataProviderInfo.FullName);

            _connection = _providerFactory.CreateConnection();
            _connection.ConnectionString = connString;

            _connection.Open();

            DatabaseName = IsConnectedToDatabase ? Connection.Database : Resources.Status_NotConnectedToDatabase; 
        }

        public int ExecuteNonQuery(string nonQuery)
        {
            IDbCommand cmd = _connection.CreateCommand();
            cmd.CommandText = nonQuery;

            return cmd.ExecuteNonQuery();
        }

        public IDataReader ExecuteQuery(string query)
        {
            IDbCommand cmd = _connection.CreateCommand();

            cmd.CommandText = query;

            return cmd.ExecuteReader();
        }

        /// <summary>
        /// Executes an SqlQuery and adds all values in the specified column to a list
        /// </summary>
        /// <param name="query">Sql Query to be executed</param>
        /// <param name="column">Number of the column to get the results from</param>
        /// <returns>List of values found in the specified column</returns>
        public IList<T> ExecuteQueryGetColumn<T>(string query, int column)
        {
            var result = new List<T>();

            try
            {
                using (IDataReader dr = ExecuteQuery(query))
                {
                    while (dr.Read())
                    {
                        var value = (T)Convert.ChangeType(dr[column].ToString(), typeof(T));
                        result.Add(value);
                    }
                }
            }
            catch (Exception excep)
            {
                Log.Error("Unexpected", excep);
            }

            return result;
        }

        public object ExecuteScalar(string query)
        {
            IDbCommand cmd = _connection.CreateCommand();
            cmd.CommandText = query;

            return cmd.ExecuteScalar();
        }

        public IDbCommand GetCommand()
        {
            return _connection.CreateCommand();
        }

        public DataTable GetDataTableFor(string query)
        {
            var dt = new DataTable();

            using (IDataReader dr = ExecuteQuery(query))
            {
                dt.Load(dr);
            }

            return dt;
        }

        public string ListInstalledProviders()
        {
            var sb = new StringBuilder();

            DataTable dt = DbProviderFactories.GetFactoryClasses();
            foreach (DataRow row in dt.Rows)
            {
                sb.AppendLine(row[0].ToString());
            }

            return sb.ToString();
        }

        #endregion

        #region IDisposable

        public void Dispose()
        {
            Connection.Dispose();
        }

        #endregion

        /// <summary>
        /// Builds an NHibernate Session Factory using the Connection String from the current database connection and
        /// the NHibernate Dialect, ConnectionDriver specified in the DataProviderInfo.
        /// Therefore first InitializeWith(dataProviderInfo) and ConnectToDatabase() before calling this Method.
        /// </summary>
        /// <returns>Newly created NHibernate SessionFactory or null if DataProvider was not connected.</returns>
        public ISessionFactory NewSessionFactory
        {
            get { return BuildSessionFactory(); }
        }

        public ISessionFactory BuildSessionFactory()
        {
            if (IsConnectedToDatabase && _dataProviderInfo != null)
            {
                _configuration = new Configuration()
                    .SetProperty(Environment.Dialect, _dataProviderInfo.NHibernateDialect)
                    .SetProperty(Environment.ConnectionDriver, _dataProviderInfo.NHibernateConnectionDriver)
                    .SetProperty(Environment.ConnectionString, Connection.ConnectionString)
                    .AddAssembly(ApplicationProperties.MappingAssemblyName);

                return NHibernateConfiguration.BuildSessionFactory();
            }

            return null;
        }

        #endregion
    }
}