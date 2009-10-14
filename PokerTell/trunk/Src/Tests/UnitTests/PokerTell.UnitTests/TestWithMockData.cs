//Date: 4/22/2009
using PokerTell.Infrastructure;
using System;
using NUnit.Framework;

namespace PokerTell.UnitTests
{
    public abstract class TestWithMockData 
    {
        public string connString {get; protected set;}
        public string providerString {get; protected set;}
        
        public const string SQLiteNewMockDatabase = "newmock";
        public const string SQLiteMockDatabase = "mock";
        
        public const string MySqlNewMockDastabase = "newmockmysql";
        public const string MySqlMockDatabase = "mockmysql";
        
        public string mockFolder = Files.dirStartUp;
        
        
        public void PrepareNewMySql()
        {
            providerString = "MySql.Data";
            connString =  "Data Source=localhost;user id = root; Database = " + MySqlNewMockDastabase + ";";
        }
        
        public void PrepareMockMySql()
        {
            providerString = "MySql.Data";
            connString =  "data source=localhost;user id=root;database = " + MySqlMockDatabase + ";";
        }
        
        public void PrepareNewSQLite()
        {
            providerString = "System.Data.SQLite";
            string databaseDirectory = string.Format("{0}\\data\\", mockFolder);
            connString = "Data Source=" + databaseDirectory + SQLiteNewMockDatabase + ".db3";
        }
        
        public void PrepareMockSQLite()
        {
            providerString = "System.Data.SQLite";
            
            string databaseDirectory = string.Format("{0}\\data\\", mockFolder);
            connString = "Data Source=" + databaseDirectory + SQLiteMockDatabase + ".db3";
        }
    }
}
