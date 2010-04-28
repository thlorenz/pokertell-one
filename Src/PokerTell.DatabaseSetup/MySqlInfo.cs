namespace PokerTell.DatabaseSetup
{
    using System;

    using NHibernate.Dialect;
    using NHibernate.Driver;

    using Infrastructure.Interfaces.DatabaseSetup;

    public class MySqlInfo : IDataProviderInfo
    {
        const string MySqlShowTablesQuery = "USE `{0}`; SHOW TABLES;";

        public string FullName
        {
            get { return "MySql.Data"; }
        }

        public bool IsEmbedded
        {
            get { return false; }
        }

        public string NiceName
        {
            get { return "MySql"; }
        }

        public string ParameterPlaceHolder
        {
            get { return "?"; }
        }

        public string ShowAllTablesQuery
        {
            get { return MySqlShowTablesQuery; }
        }

        public string NHibernateDialect
        {
            get { return typeof(MySQLDialect).AssemblyQualifiedName; }
        }

        public string NHibernateConnectionDriver
        {
            get { return typeof(MySqlDataDriver).AssemblyQualifiedName; }
        }
    }
}