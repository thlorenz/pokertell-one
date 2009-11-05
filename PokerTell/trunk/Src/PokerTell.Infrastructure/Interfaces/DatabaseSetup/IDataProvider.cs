using System;
using System.Data;

namespace PokerTell.Infrastructure.Interfaces.DatabaseSetup
{
    public interface IDataProvider : IDisposable
    {
        IDbConnection Connection { get; }

        string ParameterPlaceHolder { get; set; }

        string ListInstalledProviders();

        void Connect(string connString, string providerName);

        int ExecuteNonQuery(string nonQuery);

        IDataReader ExecuteQuery(string query);

        object ExecuteScalar(string query);

        IDbCommand GetCommand();

        string ToString();
    }
}