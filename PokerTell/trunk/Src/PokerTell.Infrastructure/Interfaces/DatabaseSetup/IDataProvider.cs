namespace PokerTell.Infrastructure.Interfaces.DatabaseSetup
{
    using System;
    using System.Data;

    public interface IDataProvider : IDisposable
    {
        #region Properties

        IDbConnection Connection { get; }

        bool IsConnectedToServer { get; }

        bool IsConnectedToDatabase { get; }

        string ParameterPlaceHolder { get; set; }

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
    }
}