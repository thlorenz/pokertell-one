namespace PokerTell.PokerHand.Dao
{
    using Infrastructure.Interfaces.Repository;

    using NHibernate;

    using PokerTell.Infrastructure.Interfaces.PokerHand;
    using PokerTell.PokerHand.Analyzation;

    public class ConvertedPokerHandDao : IConvertedPokerHandDao
    {
        #region Constants and Fields

        const string FindConvertedPokerHandByGameIdAndSite = "FindConvertedPokerHandByGameIdAndSite";

        ISession _session;

        readonly ISessionFactoryManager _sessionFactoryManager;

        readonly IPlayerIdentityDao _playerIdentityDao;

        ISession Session
        {
            get { return _session ?? (_session = _sessionFactoryManager.CurrentSession); }
        }

        #endregion

        #region Constructors and Destructors

        public ConvertedPokerHandDao(ISessionFactoryManager sessionFactoryManager, IPlayerIdentityDao playerIdentityDao)
        {
            _playerIdentityDao = playerIdentityDao;
            _sessionFactoryManager = sessionFactoryManager;
        }

        #endregion

        #region Implemented Interfaces

        #region IConvertedPokerHandDaoWithSession

        public IConvertedPokerHand Get(int id)
        {
            return Session.Get<ConvertedPokerHand>(id);
        }

        public IConvertedPokerHand GetHandWith(ulong gameId, string site)
        {
            IQuery query = Session.GetNamedQuery(FindConvertedPokerHandByGameIdAndSite);

            return SetParametersForFindConvertedPokerHand(query, gameId, site)
                .UniqueResult<IConvertedPokerHand>();

            /* Using Linq
            return (from hand in _session.Linq<ConvertedPokerHand>()
                    where hand.GameId == gameId && hand.Site == site
                    select hand)
                .SingleOrDefault();
             */
        }

        public IConvertedPokerHand Insert(IConvertedPokerHand convertedPokerHand)
        {
            if (IsComplete(convertedPokerHand) && HandIsNotYetInDatabase(convertedPokerHand, Session))
            {
                foreach (IConvertedPokerPlayer player in convertedPokerHand)
                {
                    player.PlayerIdentity = _playerIdentityDao.FindOrInsert(player.Name, convertedPokerHand.Site);
                }

                Session.SaveOrUpdate(convertedPokerHand);
            }

            return convertedPokerHand;
        }

        #endregion

        #region IConvertedPokerHandDaoWithStatelessSession

        public IConvertedPokerHandDao Insert(IConvertedPokerHand convertedPokerHand, IStatelessSession statelessSession)
        {
            if (IsComplete(convertedPokerHand) && HandIsNotYetInDatabase(convertedPokerHand, statelessSession))
            {
                statelessSession.Insert(convertedPokerHand);

                foreach (IConvertedPokerPlayer player in convertedPokerHand)
                {
                    player.PlayerIdentity = _playerIdentityDao.FindOrInsert(player.Name, convertedPokerHand.Site, statelessSession);
                    statelessSession.Insert(player);
                }
            }

            return this;
        }

        #endregion

        #endregion

        #region Methods

        static bool HandIsNotYetInDatabase(IPokerHand convertedHand, IStatelessSession statelessSession)
        {
            IQuery query = statelessSession.GetNamedQuery(FindConvertedPokerHandByGameIdAndSite);

            return SetParametersForFindConvertedPokerHand(query, convertedHand.GameId, convertedHand.Site)
                       .UniqueResult<IConvertedPokerHand>() == null;
        }

        static bool HandIsNotYetInDatabase(IPokerHand convertedHand, ISession session)
        {
            IQuery query = session.GetNamedQuery(FindConvertedPokerHandByGameIdAndSite);
            return SetParametersForFindConvertedPokerHand(query, convertedHand.GameId, convertedHand.Site)
                       .UniqueResult<IConvertedPokerHand>() == null;
        }

        static bool IsComplete(IConvertedPokerHand convertedPokerHand)
        {
            return convertedPokerHand.Players != null && convertedPokerHand.Players.Count > 0;
        }

        static IQuery SetParametersForFindConvertedPokerHand(IQuery query, ulong gameId, string site)
        {
            return query
                .SetParameter("gameId", gameId)
                .SetString("site", site);
        }

        #endregion
    }
    
}