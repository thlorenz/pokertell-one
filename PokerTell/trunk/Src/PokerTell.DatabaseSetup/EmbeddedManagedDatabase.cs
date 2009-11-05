namespace PokerTell.DatabaseSetup
{
    using System;
    using System.Collections.Generic;

    using Interfaces;

    public class EmbeddedManagedDatabase : IManagedDatabase
    {
        public string ConnectionString
        {
            get { throw new NotImplementedException(); }
        }

        public IDataProviderInfo DataProviderInfo
        {
            get { throw new NotImplementedException(); }
        }

        public IManagedDatabase ChooseDatabase(string databaseName)
        {
            throw new NotImplementedException();
        }

        public IManagedDatabase CreateDatabase(string databaseName)
        {
            throw new NotImplementedException();
        }

        public IManagedDatabase CreateTables()
        {
            throw new NotImplementedException();
        }

        public bool DatabaseExists(string databaseName)
        {
            throw new NotImplementedException();
        }

        public IManagedDatabase DeleteDatabase(string databaseName)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> GetAllPokerTellDatabases()
        {
            throw new NotImplementedException();
        }
    }
}