namespace PokerTell.Infrastructure.Interfaces.Repository
{
    using DatabaseSetup;

    using NHibernate;

    public interface ISessionFactoryManager 
    {
        ISessionFactoryManager Use(IDataProvider dataProvider);

        ISession CurrentSession { get; }

        ISessionFactory SessionFactory { get; }

        ISession OpenSession();

        IStatelessSession OpenStatelessSession();
    }
}