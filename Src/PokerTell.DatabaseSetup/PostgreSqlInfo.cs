namespace PokerTell.DatabaseSetup
{
    using NHibernate.Driver;

    using PokerTell.Infrastructure.Interfaces.DatabaseSetup;

    public class PostgreSqlInfo : IDataProviderInfo
    {
        const string PostgreSqlShowAllTablesQuery = "Select * From information_schema.tables " +
                                                    "Where table_schema='public' " +
                                                    "And table_type='BASE TABLE' " +
                                                    "Order By table_name;";

        public string FullName
        {
            get { return "Npgsql"; }
        }

        public bool IsEmbedded
        {
            get { return false; }
        }

        public string NHibernateConnectionDriver
        {
            get { return typeof(NpgsqlDriver).AssemblyQualifiedName; }
        }

        public string NHibernateDialect
        {
            get { return typeof(NpgsqlDriver).AssemblyQualifiedName; }
        }

        public string NiceName
        {
            get { return "PostgreSql"; }
        }

        public string ParameterPlaceHolder
        {
            get { return "$"; }
        }

        public string ShowAllTablesQuery
        {
            get { return PostgreSqlShowAllTablesQuery; }
        }
    }
}