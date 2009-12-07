namespace PokerTell.Infrastructure.Interfaces.PokerHand
{
    using NHibernate;

    public interface IPlayerIdentityDao
    {
        IPlayerIdentity FindOrInsert(string name, string site, IStatelessSession statelessSession);

        IPlayerIdentity FindOrInsert(string name, string site);

        IPlayerIdentity FindPlayerIdentityFor(string name, string site);

        IPlayerIdentity Insert(IPlayerIdentity playerIdentity);
    }
}