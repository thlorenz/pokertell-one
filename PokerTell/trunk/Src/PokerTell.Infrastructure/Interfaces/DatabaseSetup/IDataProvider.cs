namespace PokerTell.Infrastructure.Interfaces.DatabaseSetup
{
    using System;
    using System.Collections.Generic;
    using System.Data;

    using NHibernate;
    using NHibernate.Cfg;

    public interface IDataProvider : IDisposable
    {
        #region Properties

        IDbConnection Connection { get; }

        bool IsConnectedToServer { get; }

        bool IsConnectedToDatabase { get; }

        string ParameterPlaceHolder { get; set; }

        string DatabaseName { get; set; }

        Configuration NHibernateConfiguration { get; }

        #endregion

        #region Public Methods

        void Connect(string connString, IDataProviderInfo dataProviderInfo);

        int ExecuteNonQuery(string nonQuery);

        IDataReader ExecuteQuery(string query);

        object ExecuteScalar(string query);

        IDbCommand GetCommand();

        string ListInstalledProviders();

        string ToString();

        #endregion

        DataTable GetDataTableFor(string query);

        /// <summary>
        /// Executes an SqlQuery and adds all values in the specified column to a list
        /// </summary>
        /// <param name="query">Sql Query to be executed</param>
        /// <param name="column">Number of the column to get the results from</param>
        /// <returns>List of values found in the specified column</returns>
        IList<T> ExecuteQueryGetColumn<T>(string query, int column);

        /// <summary>
        /// Builds an NHibernate Session Factory using the Connection String from the current database connection and
        /// the NHibernate Dialect, ConnectionDriver specified in the DataProviderInfo.
        /// Therefore first InitializeWith(dataProviderInfo) and ConnectToDatabase() before calling this Method.
        /// </summary>
        /// <returns>Newly created NHibernate SessionFactory or null if DataProvider was not connected.</returns>
        ISessionFactory BuildSessionFactory();
    }
}