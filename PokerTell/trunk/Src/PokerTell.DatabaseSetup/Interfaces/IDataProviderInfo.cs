namespace PokerTell.DatabaseSetup.Interfaces
{
    public interface IDataProviderInfo
    {
        #region Properties

        string FullName { get; }

        bool IsEmbedded { get; }

        string NiceName { get; }

        string ParameterPlaceHolder { get; }

        string CreateTablesQuery { get; }

        #endregion
    }
}