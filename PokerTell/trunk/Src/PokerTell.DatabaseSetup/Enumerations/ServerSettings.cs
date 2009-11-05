namespace PokerTell.DatabaseSetup.Enumerations
{
    using Interfaces;

    internal enum ServerSettings
    {
        ServerConnect,
        ProviderName
    }

    internal static class ServerSettingsUtility
    {
        internal static string GetServerConnectKeyFor(IDataProviderInfo dataProviderInfo)
        {
            return string.Format("{0}.{1}", dataProviderInfo.FullName, ServerSettings.ServerConnect);
        }
    }
}