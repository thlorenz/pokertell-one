//Date: 4/22/2009
namespace PokerTell.UnitTests
{
    using PokerTell.Infrastructure;

    public abstract class TestWithMockData
    {
        #region Constants and Fields

        public const string MySqlMockDatabase = "mockmysql";

        public const string MySqlNewMockDastabase = "newmockmysql";

        public const string SQLiteMockDatabase = "mock";

        public const string SQLiteNewMockDatabase = "newmock";

        public string MockFolder = Files.StartupFolder;

        #endregion

        #region Properties

        public string ConnString { get; protected set; }

        public string ProviderString { get; protected set; }

        #endregion

        #region Public Methods

        public void PrepareMockMySql()
        {
            ProviderString = "MySql.Data";
            ConnString = "data source=localhost;user id=root;database = " + MySqlMockDatabase + ";";
        }

        public void PrepareMockSQLite()
        {
            ProviderString = "System.Data.SQLite";

            string databaseDirectory = string.Format("{0}\\data\\", MockFolder);
            ConnString = "Data Source=" + databaseDirectory + SQLiteMockDatabase + ".db3";
        }

        public void PrepareNewMySql()
        {
            ProviderString = "MySql.Data";
            ConnString = "Data Source=localhost;user id = root; Database = " + MySqlNewMockDastabase + ";";
        }

        public void PrepareNewSQLite()
        {
            ProviderString = "System.Data.SQLite";
            string databaseDirectory = string.Format("{0}\\data\\", MockFolder);
            ConnString = "Data Source=" + databaseDirectory + SQLiteNewMockDatabase + ".db3";
        }

        #endregion
    }
}