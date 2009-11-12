namespace PokerTell.DatabaseSetup
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Reflection;
    using System.Text;

    using log4net;

    using PokerTell.Infrastructure.Interfaces.DatabaseSetup;

    using Properties;

    public class DataProvider : IDataProvider
    {
        static readonly ILog Log =
            LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        #region Constants and Fields

        IDbConnection _connection;

        DbProviderFactory _providerFactory;

        #endregion

        #region Constructors and Destructors

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

        #endregion

        #region Implemented Interfaces

        #region IDataProvider

        public string DatabaseName
        {
            get { return IsConnectedToDatabase ? Connection.Database : Resources.Status_NotConnectedToDatabase; }
        }

        public void Connect(string connString, string providerName)
        {
            _providerFactory = DbProviderFactories.GetFactory(providerName);

            _connection = _providerFactory.CreateConnection();
            _connection.ConnectionString = connString;

            _connection.Open();
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

        public DataTable GetDataTableFor(string query)
        {
            var dt = new DataTable();

            using (IDataReader dr = ExecuteQuery(query))
            {
                dt.Load(dr);
            }
            
            return dt;
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

        #endregion
    }
}