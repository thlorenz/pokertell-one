namespace PokerTell.DatabaseSetup
{
    using NHibernate.Dialect;
    using NHibernate.Driver;

    using Infrastructure.Interfaces.DatabaseSetup;

    using Properties;

    public class MySqlInfo : IDataProviderInfo
    {
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