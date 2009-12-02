namespace PokerTell.PokerHand.Dao
{
    using System.Linq;

    using NHibernate;
    using NHibernate.Linq;

    using PokerTell.Infrastructure.Interfaces.PokerHand;
    using PokerTell.PokerHand.Analyzation;

    public class ConvertedPokerHandDao : IConvertedPokerHandDao
    {
        #region Constants and Fields

        ISession _session;

        #endregion

        #region Implemented Interfaces

        #region IConvertedPokerHandDao

        public IConvertedPokerHand Get(int id)
        {
            return _session.Get<ConvertedPokerHand>(id);
        }

        public IConvertedPokerHand GetHandWith(ulong gameId, string site)
        {
            return (from hand in _session.Linq<ConvertedPokerHand>()
                    where hand.GameId == gameId && hand.Site == site
                    select hand)
                .SingleOrDefault();
        }

        public IConvertedPokerHandDao InitializeWith(ISession session)
        {
            _session = session;
            return this;
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

        #endregion

        #region Methods

        static bool IsComplete(IConvertedPokerHand convertedPokerHand)
        {
            return convertedPokerHand.Players != null && convertedPokerHand.Players.Count > 0;
        }

        #endregion
    }
}