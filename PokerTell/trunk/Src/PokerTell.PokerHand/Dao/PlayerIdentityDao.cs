namespace PokerTell.PokerHand.Dao
{
    using System.Linq;

    using Analyzation;

    using Infrastructure.Interfaces.PokerHand;

    using NHibernate;
    using NHibernate.Linq;

    public class PlayerIdentityDao
    {
        readonly ISession _session;

        public PlayerIdentityDao(ISession session)
        {
            _session = session;
        }

        public IPlayerIdentity GetPlayerIdentityFor(string name, string site)
        {
            return (from identity in _session.Linq<PlayerIdentity>()
                    where identity.Name == name && identity.Site == site
                    select identity)
                .SingleOrDefault();
        }

        public IPlayerIdentity Insert(IPlayerIdentity playerIdentity)
        {
            _session.SaveOrUpdate(playerIdentity);
            return playerIdentity;
        }

        public IPlayerIdentity GetOrInsert(string name, string site)
        {
            var previouslyStoredPlayerIdentityWithSameNameAndSite = GetPlayerIdentityFor(name, site);

            if (previouslyStoredPlayerIdentityWithSameNameAndSite != null)
            {
                return previouslyStoredPlayerIdentityWithSameNameAndSite;
            }

            return Insert(new PlayerIdentity(name, site));
        }
    }
}