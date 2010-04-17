namespace PokerTell.Infrastructure.Interfaces.DatabaseSetup
{
    using System;
    using System.Collections.Generic;
    using System.Data;

    using NHibernate;
    using NHibernate.Cfg;

    public interface IDataProvider : IDisposable
    {
        IDbConnection Connection { get; }

        string DatabaseName { get; set; }

        bool IsConnectedToDatabase { get; }

        bool IsConnectedToServer { get; }

        /// <summary>
        /// Builds an NHibernate Session Factory using the Connection String from the current database connection and
        /// the NHibernate Dialect, ConnectionDriver specified in the DataProviderInfo.
        /// Therefore first InitializeWith(dataProviderInfo) and ConnectToDatabase() before calling this Method.
        /// </summary>
        /// <returns>Newly created NHibernate SessionFactory or null if DataProvider was not connected.</returns>
        ISessionFactory NewSessionFactory { get; }

        Configuration NHibernateConfiguration { get; }

        string ParameterPlaceHolder { get; set; }

        ISessionFactory BuildSessionFactory();

        void Connect(string connString, IDataProviderInfo dataProviderInfo);

        int ExecuteNonQuery(string nonQuery);

        IDataReader ExecuteQuery(string query);

        /// <summary>
        /// Executes an SqlQuery and adds all values in the specified column to a list
        /// </summary>
        /// <param name="query">Sql Query to be executed</param>
        /// <param name="column">Number of the column to get the results from</param>
        /// <returns>List of values found in the specified column</returns>
        IList<T> ExecuteQueryGetColumn<T>(string query, int column);

        object ExecuteScalar(string query);

        IDbCommand GetCommand();

        DataTable GetDataTableFor(string query);

        string ListInstalledProviders();

        string ToString();
    }
}