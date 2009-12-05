namespace PokerTell.Infrastructure.Interfaces.PokerHand
{
    using NHibernate;

    public interface IConvertedPokerHandDao : IConvertedPokerHandDaoWithSession, 
                                              IConvertedPokerHandDaoWithStatelessSession
    {
    }

    public interface IConvertedPokerHandDaoWithSession : IFluentInterface
    {
        #region Public Methods

        IConvertedPokerHand Get(int id);

        IConvertedPokerHand GetHandWith(ulong gameId, string site);

        IConvertedPokerHand Insert(IConvertedPokerHand convertedPokerHand);

        #endregion
    }

    public interface IConvertedPokerHandDaoWithStatelessSession : IFluentInterface
    {
        #region Public Methods

        void InsertFast(IConvertedPokerHand convertedPokerHand);

        #endregion
    }

    public interface IConvertedPokerHandDaoFactory
    {
        #region Public Methods

        IConvertedPokerHandDaoWithSession New(ISession session);

        IConvertedPokerHandDaoWithStatelessSession New(IStatelessSession statelessSession);

        #endregion
    }
}