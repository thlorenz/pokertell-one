namespace PokerTell.DatabaseSetup
{
    using System;

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

        public string CreateTablesQuery
        {
            get { return Resources.MySql_Queries_CreateTables; }
        }
    }
}