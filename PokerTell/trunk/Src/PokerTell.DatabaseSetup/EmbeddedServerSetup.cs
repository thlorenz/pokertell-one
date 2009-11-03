//Date: 6/3/2009

namespace PokerTell.DatabaseSetup
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;

    using Infrastructure;
    using Infrastructure.Interfaces;
    using Infrastructure.Interfaces.DatabaseSetup;

    using log4net;

    /// <summary>
    /// Works for embedded (filebased databases
    /// Includes a "Hack function" GetFileNameOf in order to allow client to just deal with the database name
    /// From this it will automatically generate a path to the underlying file
    /// This way external and embedded servers look the same to the client
    /// </summary>
    public abstract class EmbeddedServerSetup : ServerSetup
    {
        #region Constants and Fields

        protected string DatabaseDirectory;

        static readonly ILog Log = LogManager.GetLogger(
            MethodBase.GetCurrentMethod().DeclaringType);

        #endregion

        #region Properties

        protected EmbeddedServerSetup(IDataProvider dataProvider, ISettings settings, string parameterPlaceHolder)
            : base(dataProvider, settings, parameterPlaceHolder)
        {
        }

        public override string DatabaseName
        {
            get { return GetDatabaseNameFromConnectionString(_dataProvider.Connection.ConnectionString); }
        }

        protected abstract string FileExtension { get; }

        #endregion

        #region Public Methods

        /// <summary>
        /// This needs to be called everytime before any operation requiring dp to be pointing
        /// to a database can be done and the embedded database has been "fake connected" to a server
        /// via the new Database(providerName, true) option
        /// </summary>
        /// <param name="databaseName"></param>
        public override void ChooseDatabase(string databaseName)
        {
            string connString = "data source = " + GetFullPathFor(databaseName);

            _dataProvider.Connect(connString, ProviderName);
        }

        public override void ConnectToServer(string serverConnectString)
        {
            DatabaseDirectory = string.Format("{0}{1}", Files.dirAppData, Files.dirData);
        }

        public override bool ExistsDatabase(string theDatabase)
        {
            return File.Exists(GetFullPathFor(theDatabase));
        }

        public override IEnumerable<string> GetAllPokerTellDatabases()
        {
            if (!Directory.Exists(DatabaseDirectory))
            {
                Log.Debug(DatabaseDirectory + " was deleted -> recreating");
                Directory.CreateDirectory(DatabaseDirectory);
            }

            var dirInfo = new DirectoryInfo(DatabaseDirectory);

            foreach (FileInfo fi in  dirInfo.GetFiles("*." + FileExtension))
            {
                yield return GetDatabaseNameFromPath(fi.FullName);
            }
        }

        /// <summary>
        /// Obtains connection string for current provider from the settings
        /// </summary>
        /// <returns>connection string</returns>
        public override string GetSavedConnectionString()
        {
            if (ConnectionStringForMyProviderExists())
            {
                string connString =
                    _settings.ConnectionStrings[_dataProvider.ProviderName].ConnectionString.TrimEnd(';');

                string theFileName = FindDatabaseFile(connString);

                return "Data Source = " + theFileName;
            }
            else
            {
                return string.Empty;
            }
        }

        public override string GetSavedServerConnectString()
        {
            // Not necessary for embedded DBs
            // Will cause a null reference if called
            return null;
        }

        public override void SaveConnectionString(string connString)
        {
            string tweakedConnString = RemovePathAndExtensionFromDataSourceFor(connString);
            base.SaveConnectionString(tweakedConnString);
        }

        public override void SaveServerConnectString(string serverConnectString)
        {
            //Not necessary for embedded DBs
        }

        #endregion

        #region Methods

        protected override void PerformCreateDatabase(string theDatabaseName)
        {
            //Choosing the database creates the database file if it didn't exist yet
            ChooseDatabase(theDatabaseName);
        }

        protected override void PerformDeleteDatabase(string theDatabase)
        {
            if (File.Exists(GetFullPathFor(theDatabase)))
            {
                File.Delete(GetFullPathFor(theDatabase));
            }
        }

        static string ExtractDatabaseFromConnectionString(string connString)
        {
            int indexOfEqual = connString.IndexOf("=");

            bool foundEqual = indexOfEqual > 0;
            bool connectionStringContainsDatabaseName = indexOfEqual < (connString.Length - 1);

            if (foundEqual && connectionStringContainsDatabaseName)
            {
                return connString.Substring(indexOfEqual + 1).TrimEnd(';').Trim();
            }
            else
            {
                throw new FormatException("ConnectionString: " + connString);
            }
        }

        static string GetFileNameFrom(string fullPath)
        {
            bool fileExists = File.Exists(fullPath);
            if (!fileExists)
            {
                throw new FormatException("Full Path of database file: " + fullPath);
            }

            var infFile = new FileInfo(fullPath);
            return infFile.Name;
        }

        static string RemoveFileExtensionFrom(string theFileName)
        {
            int indexOflastDot = theFileName.LastIndexOf('.');
            bool foundLastDot = indexOflastDot > 0;

            if (foundLastDot)
            {
                return theFileName.Substring(0, indexOflastDot);
            }

            throw new FormatException("FileName: " + theFileName);
        }

        string AppendFileExtensionTo(string theDatabaseName)
        {
            return theDatabaseName + "." + FileExtension;
        }

        string FindDatabaseFile(string connString)
        {
            string databaseName = ExtractDatabaseFromConnectionString(connString);
            string fileName = GetFullPathFor(databaseName);
            DirectoryInfo directoryInfo = new FileInfo(fileName).Directory;

            if (!directoryInfo.Exists)
            {
                directoryInfo.Create();
            }
            return fileName;
        }

        string GetDatabaseNameFromConnectionString(string connString)
        {
            string fullPath = ExtractDatabaseFromConnectionString(connString);
            return GetDatabaseNameFromPath(fullPath);
        }

        string GetDatabaseNameFromPath(string fullPath)
        {
            string theFileName = GetFileNameFrom(fullPath);
            return RemoveFileExtensionFrom(theFileName);
        }

        string GetFullPathFor(string theDatabaseName)
        {
            //Initialize databaseDirectory "Make sure we are connected"
            ConnectToServer(null);
            return DatabaseDirectory + AppendFileExtensionTo(theDatabaseName);
        }

        string RemovePathAndExtensionFromDataSourceFor(string connString)
        {
            return "Data Source = " + GetDatabaseNameFromConnectionString(connString);
        }

        #endregion
    }
}