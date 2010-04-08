namespace PokerTell.PokerHand.Dao
{
    using System.Collections.Generic;

    using NHibernate;

    using PokerTell.Infrastructure.Interfaces.PokerHand;
    using PokerTell.Infrastructure.Interfaces.Repository;
    using PokerTell.PokerHand.Analyzation;

    public class PlayerIdentityDao : IPlayerIdentityDao
    {
        const string FindPlayerIdentityByNameAndSite = "FindPlayerIdentityByNameAndSite";

        readonly ISessionFactoryManager _sessionFactoryManager;

        public PlayerIdentityDao(ISessionFactoryManager sessionFactoryManager)
        {
            _sessionFactoryManager = sessionFactoryManager;
        }

        ISession Session
        {
            get { return _sessionFactoryManager.CurrentSession; }
        }

        public IPlayerIdentity FindOrInsert(string name, string site, IStatelessSession statelessSession)
        {
            IPlayerIdentity previouslyStoredPlayerIdentityWithSameNameAndSite =
                FindPlayerIdentityFor(name, site, statelessSession);

            if (previouslyStoredPlayerIdentityWithSameNameAndSite != null)
            {
                return previouslyStoredPlayerIdentityWithSameNameAndSite;
            }

            return Insert(new PlayerIdentity(name, site), statelessSession);
        }

        public IPlayerIdentity FindOrInsert(string name, string site)
        {
            IPlayerIdentity previouslyStoredPlayerIdentityWithSameNameAndSite =
                FindPlayerIdentityFor(name, site);

            if (previouslyStoredPlayerIdentityWithSameNameAndSite != null)
            {
                return previouslyStoredPlayerIdentityWithSameNameAndSite;
            }

            return Insert(new PlayerIdentity(name, site));
        }

        public IPlayerIdentity FindPlayerIdentityFor(string name, string site)
        {
            IQuery query = Session.GetNamedQuery(FindPlayerIdentityByNameAndSite);

            return SetQueryParametersAndReturnResultFor(query, name, site);
        }

        public IPlayerIdentity Insert(IPlayerIdentity playerIdentity)
        {
            Session.SaveOrUpdate(playerIdentity);
            return playerIdentity;
        }

        public IList<IPlayerIdentity> GetAll()
        {
            var query = string.Format("From {0}", typeof(PlayerIdentity).Name);
            return Session.CreateQuery(query).List<IPlayerIdentity>();
        }

        static IPlayerIdentity FindPlayerIdentityFor(string name, string site, IStatelessSession statelessSession)
        {
            IQuery query = statelessSession.GetNamedQuery(FindPlayerIdentityByNameAndSite);

            return SetQueryParametersAndReturnResultFor(query, name, site);
        }

        static IPlayerIdentity Insert(IPlayerIdentity playerIdentity, IStatelessSession statelessSession)
        {
            statelessSession.Insert(playerIdentity);
            return playerIdentity;
        }

        static IPlayerIdentity SetQueryParametersAndReturnResultFor(IQuery findPlayerIdentityQuery, string name, string site)
        {
            return findPlayerIdentityQuery
                .SetString("name", name)
                .SetString("site", site)
                .UniqueResult<IPlayerIdentity>();
        }
    }
}