namespace PokerTell.Infrastructure.Interfaces.PokerHand
{
    using NHibernate;

    public interface IConvertedPokerHandDao : IConvertedPokerHandDaoWithoutSession, IConvertedPokerHandDaoWithSession
    {
    }

     public interface IConvertedPokerHandDaoWithoutSession : IFluentInterface
     {
         IConvertedPokerHandDaoWithSession InitializeWith(ISession session);
     }

    public interface IConvertedPokerHandDaoWithSession : IFluentInterface
    {
        IConvertedPokerHand GetHandWith(ulong gameId, string site);

        IConvertedPokerHand Insert(IConvertedPokerHand convertedPokerHand);

        IConvertedPokerHand Get(int id);
    }
}