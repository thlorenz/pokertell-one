namespace PokerTell.DatabaseSetup
{
    using System;

    using Infrastructure.Interfaces.DatabaseSetup;

    using Properties;

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

        public string CreateTablesQuery
        {
            get { return Resources.SQLite_Queries_CreateTables; }
        }
    }
}