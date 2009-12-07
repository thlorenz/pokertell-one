namespace PokerTell.Infrastructure.Interfaces.DatabaseSetup
{
    public interface IDataProviderInfo
    {
        #region Properties

        string FullName { get; }

        bool IsEmbedded { get; }

        string NiceName { get; }

        string ParameterPlaceHolder { get; }

        string NHibernateDialect { get; }

        string NHibernateConnectionDriver { get; }

        #endregion
    }
}