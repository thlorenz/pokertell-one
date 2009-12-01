namespace PokerTell.PokerHand.Dao
{
    using System.Linq;
    using System.Reflection;

    using Analyzation;

    using Infrastructure.Interfaces.PokerHand;

    using log4net;

    using NHibernate;
    using NHibernate.Linq;

    public class ConvertedPokerHandDao
    {
        #region Constants and Fields

        static readonly ILog Log =
            LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        readonly ISession _session;

        #endregion

        #region Constructors and Destructors

        public ConvertedPokerHandDao(ISession session)
        {
            _session = session;
        }

        #endregion

        #region Public Methods

        public IConvertedPokerHand GetHandWith(ulong gameId, string site)
        {
            return (from hand in _session.Linq<ConvertedPokerHand>()
                    where hand.GameId == gameId && hand.Site == site
                    select hand)
                .SingleOrDefault();
        }

        public IConvertedPokerHand Insert(IConvertedPokerHand convertedPokerHand)
        {
            if (IsComplete(convertedPokerHand))
            {
                var playerIdentityDao = new PlayerIdentityDao(_session);
                convertedPokerHand.Players.ToList().ForEach(
                    player =>
                    player.PlayerIdentity =
                    playerIdentityDao.GetOrInsert(player.Name, convertedPokerHand.Site));

                _session.SaveOrUpdate(convertedPokerHand);
            }

            return convertedPokerHand;
        }

        #endregion

        #region Methods

        static bool IsComplete(IConvertedPokerHand convertedPokerHand)
        {
            return convertedPokerHand.Players != null && convertedPokerHand.Players.Count > 0;
        }

        #endregion

        public IConvertedPokerHand Get(int id)
        {
            return _session.Get<ConvertedPokerHand>(id);
        }
    }
}