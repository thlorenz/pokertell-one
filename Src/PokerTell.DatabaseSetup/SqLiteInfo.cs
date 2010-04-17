namespace PokerTell.DatabaseSetup
{
    using System;

    using Infrastructure.Interfaces.DatabaseSetup;

    using NHibernate.Dialect;
    using NHibernate.Driver;

    public class SqLiteInfo : IDataProviderInfo
    {
        public string FullName
        {
            get { return "System.Data.SQLite"; }
        }

        public bool IsEmbedded
        {
            get { return true; }
        }

        public string NiceName
        {
            get { return "Integrated (SQLite)"; }
        }

        public string ParameterPlaceHolder
        {
            get { return "@"; }
        }

        public string NHibernateDialect
        {
            get { return typeof(SQLiteDialect).AssemblyQualifiedName; }
        }

        public string NHibernateConnectionDriver
        {
            get { return typeof(SQLite20Driver).AssemblyQualifiedName; }
        }
    }
}