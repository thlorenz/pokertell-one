//Date: 5/16/2009
namespace PokerTell.DatabaseSetup
{
    using System.Data;
    using System.Data.Common;
    using System.Text;

    using Infrastructure.Interfaces.DatabaseSetup;

    public class DataProvider : IDataProvider
    {
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

        public string ParameterPlaceHolder { get; set; }

        #endregion

        #region Implemented Interfaces

        #region IDataProvider

        public void Connect(string connString, string providerName)
        {
            // possibly inject DbProviderFactories for testing
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