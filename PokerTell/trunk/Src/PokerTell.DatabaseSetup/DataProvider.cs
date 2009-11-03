//Date: 5/16/2009
namespace PokerTell.DatabaseSetup
{
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Linq;
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

        public string ProviderName { get; set; }

        #endregion

        #region Public Methods

        public IEnumerable<string> GetAvailableProviders()
        {
            IList<string> availableProviders = new List<string>();
            // TODO: Reimplement
            //            var supportedProviders = new string[] { SQLiteSetup.ProviderName, MySqlSetup.ProviderName };
            //
            //            foreach (string provider in supportedProviders)
            //            {
            //                bool isEmbedded = IServerSetup.GetServerSetupFor(provider) is EmbeddedServerSetup;
            //                bool hasSavedServerConnectString =
            //                    ! string.IsNullOrEmpty(IServerSetup.GetServerSetupFor(provider).GetSavedServerConnectString());
            //
            //                if (isEmbedded || hasSavedServerConnectString)
            //                {
            //                    availableProviders.Add(provider);
            //                }
            //            }

            return availableProviders.ToArray();
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

        public bool ThisProviderIsAvailable(string theProviderName)
        {
            foreach (string fullProviderName in GetAvailableProviders())
            {
                if (fullProviderName.Equals(theProviderName))
                {
                    return true;
                }
            }
            return false;
        }

        public void Connect(string connString, string providerName)
        {

            // possibly inject DbProviderFactories for testing
            _providerFactory = DbProviderFactories.GetFactory(ProviderName);

            ProviderName = providerName;

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

        public override string ToString()
        {
            return string.Format(
                "[DataProvider ProviderName={0} Connection={1}]",
                ProviderName,
                _connection.ConnectionString);
        }

        #endregion

        #region Implemented Interfaces

        #region IDisposable

        public void Dispose()
        {
            Connection.Dispose();
        }

        #endregion

        #endregion
    }
}