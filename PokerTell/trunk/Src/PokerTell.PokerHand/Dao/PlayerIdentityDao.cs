namespace PokerTell.PokerHand.Dao
{
    using NHibernate;

    using PokerTell.Infrastructure.Interfaces.PokerHand;
    using PokerTell.PokerHand.Analyzation;

    public class PlayerIdentityDao
    {
        const string FindPlayerIdentityByNameAndSite = "FindPlayerIdentityByNameAndSite";

        #region Constants and Fields

        readonly ISession _session;

        readonly IStatelessSession _statelessSession;

        #endregion

        #region Constructors and Destructors

        public PlayerIdentityDao(IStatelessSession statelessSession)
        {
            _statelessSession = statelessSession;
        }

        public PlayerIdentityDao(ISession session)
        {
            _session = session;
        }

        #endregion

        #region Public Methods

        public IPlayerIdentity GetOrInsert(string name, string site)
        {
            return GetOrInsert(name, site, _session);
        }

        public IPlayerIdentity GetOrInsertFast(string name, string site)
        {
            return GetOrInsert(name, site, _statelessSession);
        }

        public IPlayerIdentity GetPlayerIdentityFor(string name, string site)
        {
            return GetPlayerIdentityFor(name, site, _session);
        }

        public IPlayerIdentity Insert(IPlayerIdentity playerIdentity)
        {
            return Insert(playerIdentity, _session);
        }

        #endregion

        #region Methods

        static IPlayerIdentity FindPlayerIdentityFor(IQuery query, string name, string site)
        {
            return query
                .SetString("name", name)
                .SetString("site", site)
                .UniqueResult<IPlayerIdentity>();
        }

        static IPlayerIdentity GetOrInsert(string name, string site, IStatelessSession statelessSession)
        {
            IPlayerIdentity previouslyStoredPlayerIdentityWithSameNameAndSite =
                GetPlayerIdentityFor(name, site, statelessSession);

            if (previouslyStoredPlayerIdentityWithSameNameAndSite != null)
            {
                return previouslyStoredPlayerIdentityWithSameNameAndSite;
            }

            return Insert(new PlayerIdentity(name, site), statelessSession);
        }

        static IPlayerIdentity GetOrInsert(string name, string site, ISession session)
        {
            IPlayerIdentity previouslyStoredPlayerIdentityWithSameNameAndSite = GetPlayerIdentityFor(name, site, session);

            if (previouslyStoredPlayerIdentityWithSameNameAndSite != null)
            {
                return previouslyStoredPlayerIdentityWithSameNameAndSite;
            }

            return Insert(new PlayerIdentity(name, site), session);
        }

        static IPlayerIdentity GetPlayerIdentityFor(string name, string site, IStatelessSession statelessSession)
        {
            IQuery query = statelessSession.GetNamedQuery(FindPlayerIdentityByNameAndSite);

            return FindPlayerIdentityFor(query, name, site);
        }

        static IPlayerIdentity GetPlayerIdentityFor(string name, string site, ISession session)
        {
            IQuery query = session.GetNamedQuery(FindPlayerIdentityByNameAndSite);

            return FindPlayerIdentityFor(query, name, site);

            /* Using Linq
            return (from identity in _session.Linq<PlayerIdentity>()
                    where identity.Name == name && identity.Site == site
                    select identity)
                .SingleOrDefault();
             */
        }

        static IPlayerIdentity Insert(IPlayerIdentity playerIdentity, ISession session)
        {
            session.SaveOrUpdate(playerIdentity);
            return playerIdentity;
        }

        static IPlayerIdentity Insert(IPlayerIdentity playerIdentity, IStatelessSession statelessSession)
        {
            statelessSession.Insert(playerIdentity);
            return playerIdentity;
        }

        #endregion
    }
}