namespace PokerTell.DatabaseSetup
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    using Infrastructure;
    using Infrastructure.Interfaces.DatabaseSetup;

    using log4net;

    public class EmbeddedManagedDatabase : IManagedDatabase
    {
        #region Constants and Fields

        const string FileExtension = "db3";

        static readonly ILog Log =
            LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        readonly string _databaseDirectory;

        readonly IDataProvider _dataProvider;

        readonly IDataProviderInfo _dataProviderInfo;

        #endregion

        #region Constructors and Destructors

        public EmbeddedManagedDatabase(IDataProvider dataProvider, IDataProviderInfo dataProviderInfo)
        {
            _dataProviderInfo = dataProviderInfo;
            _dataProvider = dataProvider;

            _databaseDirectory = string.Format("{0}{1}", Files.dirAppData, Files.dirData);

            ValidateDatabaseDirectoryAndRecreateIfNeccessary();
        }

        #endregion

        #region Properties

        public string ConnectionString { get; private set; }
        
        public IDataProviderInfo DataProviderInfo
        {
            get { return _dataProviderInfo; }
        }

        #endregion

        #region Implemented Interfaces

        #region IManagedDatabase

        public IManagedDatabase ChooseDatabase(string databaseName)
        {
            ConnectionString = "data source = " + FullPathFor(databaseName);
            
            _dataProvider.Connect(ConnectionString, _dataProviderInfo.FullName);

            return this;
        }

        public IManagedDatabase CreateDatabase(string databaseName)
        {
            return ChooseDatabase(databaseName);
        }

        public IManagedDatabase CreateTables()
        {
            string nonQuery = _dataProviderInfo.CreateTablesQuery;
            _dataProvider.ExecuteNonQuery(nonQuery);

            return this;
        }

        public bool DatabaseExists(string databaseName)
        {
            return File.Exists(FullPathFor(databaseName));
        }

        public IManagedDatabase DeleteDatabase(string databaseName)
        {
            File.Delete(FullPathFor(databaseName));

            return this;
        }

        public IEnumerable<string> GetAllPokerTellDatabaseNames()
        {
            var dirInfo = new DirectoryInfo(_databaseDirectory);

            return dirInfo.GetFiles("*." + FileExtension).Select(fi => GetDatabaseNameFromPath(fi.FullName));
        }

        #endregion

        #endregion

        #region Methods

        static string DatabaseNameWithAddedFileExtension(string databaseName)
        {
            return databaseName + "." + FileExtension;
        }

        static string GetDatabaseNameFromPath(string fullPath)
        {
            string fileName = GetFileNameFrom(fullPath);
            return RemoveFileExtensionFrom(fileName);
        }

        static string GetFileNameFrom(string fullPath)
        {
            bool fileExists = File.Exists(fullPath);

            if (fileExists)
            {
                var fileInfo = new FileInfo(fullPath);
                return fileInfo.Name;
            }

            throw new FormatException("Full Path of database file: " + fullPath);
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

        string FullPathFor(string databaseName)
        {
            return _databaseDirectory + DatabaseNameWithAddedFileExtension(databaseName);
        }

        void ValidateDatabaseDirectoryAndRecreateIfNeccessary()
        {
            if (!Directory.Exists(_databaseDirectory))
            {
                Log.Debug(_databaseDirectory + " was deleted and is being recreated.");
                Directory.CreateDirectory(_databaseDirectory);
            }
        }

        #endregion
    }
}