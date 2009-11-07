//Date: 5/26/2009
namespace PokerTell.DatabaseSetup
{
    using System;
    using System.Text.RegularExpressions;

    public class DatabaseConnectionInfo
    {
        #region Constructors and Destructors

        public DatabaseConnectionInfo(string server, string user, string password)
        {
            InitializeWith(server, user, password, null);
        }

        public DatabaseConnectionInfo(string connString)
        {
            ExtractConnectionDetailsAndInitializeFrom(connString);
        }

        void ExtractConnectionDetailsAndInitializeFrom(string connString)
        {
            const string patServer = @"Data Source\s*=\s*(?<serverName>[^;\b]+)[;\b]";
            const string patUser = @"User\sID\s*=\s*(?<userName>[^;\b]+)[;\b]";
            const string patPassword = @"(pwd|password)\s*=\s*(?<password>('(([^'])|(''))+'|[^';]+))";
            const string patDatabase = @"(Initial Catalog|Database)\s*=\s*(?<databaseName>[^;\b]+)";

            Match m = Regex.Match(connString, patServer, RegexOptions.IgnoreCase);
            string server = m.Success ? m.Groups["serverName"].Value : string.Empty;

            m = Regex.Match(connString, patUser, RegexOptions.IgnoreCase);
            string user = m.Success ? m.Groups["userName"].Value : string.Empty;

            m = Regex.Match(connString, patPassword, RegexOptions.IgnoreCase);
            string password = m.Success ? m.Groups["password"].Value : string.Empty;

            m = Regex.Match(connString, patDatabase, RegexOptions.IgnoreCase);
            string database = m.Success ? m.Groups["databaseName"].Value : string.Empty;

            InitializeWith(server, user, password, database);
        }

        #endregion

        #region Properties

        public string Database { get; protected set; }

        public string Password { get; protected set; }

        public string Server { get; protected set; }

        /// <summary>
        /// Connection string to connect to server only
        /// </summary>
        /// <exception cref="DatabaseConnectionInfoInvalidException">not at least user and server are specified</exception>
        public string ServerConnectString
        {
            get { return GetServerConnectString(); }
        }

        public string User { get; protected set; }

        #endregion

        #region Public Methods

        public bool IsValidForDatabaseConnection()
        {
            return IsValidForServerOnlyConnection() && !string.IsNullOrEmpty(Database);
        }

        public bool IsValidForServerOnlyConnection()
        {
            return !string.IsNullOrEmpty(Server) && !string.IsNullOrEmpty(User);
        }

        #endregion

        #region Methods

        /// <summary>
        /// DatabaseConnection string 
        /// </summary>
        /// <returns>ServerConnectString for current properties</returns>
        /// <exception cref="DatabaseConnectionInfoInvalidException">not at least user and server are specified</exception>
        string GetServerConnectString()
        {
            if (! IsValidForServerOnlyConnection())
            {
                throw new DatabaseConnectionInfoInvalidException("Need to at least have server and user specified");
            }
            string connString = string.Format("data source = {0}; user id = {1};", Server, User);

            if (! string.IsNullOrEmpty(Password))
            {
                connString += string.Format(" password = {0};", Password);
            }

            return connString;
        }

        void InitializeWith(string server, string user, string password, string database)
        {
            Server = server;
            User = user;
            Password = password;
            Database = database;
        }

        #endregion

       public override string ToString()
        {
            return string.Format("Database: {0}, Password: {1}, Server: {2}, User: {3}", Database, Password, Server, User);
        }
    }

    public class DatabaseConnectionInfoInvalidException : Exception
    {
        #region Constructors and Destructors

        public DatabaseConnectionInfoInvalidException(string message)
            : base(message)
        {
        }

        #endregion
    }
}