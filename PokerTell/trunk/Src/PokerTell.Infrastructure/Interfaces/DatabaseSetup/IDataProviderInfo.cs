namespace PokerTell.Infrastructure.Interfaces.DatabaseSetup
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