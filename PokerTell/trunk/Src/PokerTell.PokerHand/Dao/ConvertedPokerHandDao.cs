namespace PokerTell.PokerHand.Dao
{
    using NHibernate;

    using PokerTell.Infrastructure.Interfaces.PokerHand;
    using PokerTell.PokerHand.Analyzation;

    public class ConvertedPokerHandDao : IConvertedPokerHandDao
    {
        #region Constants and Fields

        const string FindConvertedPokerHandByGameIdAndSite = "FindConvertedPokerHandByGameIdAndSite";

        readonly ISession _session;

        readonly IStatelessSession _statelessSession;

        #endregion

        #region Constructors and Destructors

        public ConvertedPokerHandDao(IStatelessSession statelessSession)
        {
            _statelessSession = statelessSession;
        }

        public ConvertedPokerHandDao(ISession session)
        {
            _session = session;
        }

        #endregion

        #region Implemented Interfaces

        #region IConvertedPokerHandDaoWithSession

        public IConvertedPokerHand Get(int id)
        {
            return _session.Get<ConvertedPokerHand>(id);
        }

        public IConvertedPokerHand GetHandWith(ulong gameId, string site)
        {
            IQuery query = _session.GetNamedQuery(FindConvertedPokerHandByGameIdAndSite);

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
            if (IsComplete(convertedPokerHand) && HandIsNotYetInDatabase(convertedPokerHand, _session))
            {
                var playerIdentityDao = new PlayerIdentityDao(_session);
                foreach (IConvertedPokerPlayer player in convertedPokerHand)
                {
                    player.PlayerIdentity = playerIdentityDao
                        .GetOrInsert(player.Name, convertedPokerHand.Site);
                }

                _session.SaveOrUpdate(convertedPokerHand);
            }

            return convertedPokerHand;
        }

        #endregion

        #region IConvertedPokerHandDaoWithStatelessSession

        public void InsertFast(IConvertedPokerHand convertedPokerHand)
        {
            if (IsComplete(convertedPokerHand) && HandIsNotYetInDatabase(convertedPokerHand, _statelessSession))
            {
                _statelessSession.Insert(convertedPokerHand);

                var playerIdentityDao = new PlayerIdentityDao(_statelessSession);

                foreach (IConvertedPokerPlayer player in convertedPokerHand)
                {
                    player.PlayerIdentity = playerIdentityDao
                        .GetOrInsertFast(player.Name, convertedPokerHand.Site);
                    _statelessSession.Insert(player);
                }
            }
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

    public class ConvertedPokerHandDaoFactory : IConvertedPokerHandDaoFactory
    {
        #region Implemented Interfaces

        #region IConvertedPokerHandDaoFactory

        public IConvertedPokerHandDaoWithSession New(ISession session)
        {
            return new ConvertedPokerHandDao(session);
        }

        public IConvertedPokerHandDaoWithStatelessSession New(IStatelessSession statelessSession)
        {
            return new ConvertedPokerHandDao(statelessSession);
        }

        #endregion

        #endregion
    }
}