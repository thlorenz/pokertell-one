namespace PokerTell.Infrastructure.Interfaces.PokerHand
{
    using System.Collections.Generic;

    using NHibernate;

    public interface IPlayerIdentityDao : IFluentInterface
    {
        IPlayerIdentity FindOrInsert(string name, string site, IStatelessSession statelessSession);

        IPlayerIdentity FindOrInsert(string name, string site);

        IPlayerIdentity FindPlayerIdentityFor(string name, string site);

        IPlayerIdentity Insert(IPlayerIdentity playerIdentity);

        IList<IPlayerIdentity> GetAll();
    }
}