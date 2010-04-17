namespace PokerTell.Infrastructure.Interfaces.DatabaseSetup
{
    public interface IDataProviderInfo : IFluentInterface
    {
        string FullName { get; }

        bool IsEmbedded { get; }

        string NHibernateConnectionDriver { get; }

        string NHibernateDialect { get; }

        string NiceName { get; }

        string ParameterPlaceHolder { get; }
    }
}