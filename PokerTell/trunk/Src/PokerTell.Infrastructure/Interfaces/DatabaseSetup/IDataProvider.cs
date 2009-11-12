namespace PokerTell.Infrastructure.Interfaces.DatabaseSetup
{
    using System;
    using System.Collections.Generic;
    using System.Data;

    public interface IDataProvider : IDisposable
    {
        #region Properties

        IDbConnection Connection { get; }

        bool IsConnectedToServer { get; }

        bool IsConnectedToDatabase { get; }

        string ParameterPlaceHolder { get; set; }

        string DatabaseName { get; }

        #endregion

        #region Public Methods

        void Connect(string connString, string providerName);

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
    }
}