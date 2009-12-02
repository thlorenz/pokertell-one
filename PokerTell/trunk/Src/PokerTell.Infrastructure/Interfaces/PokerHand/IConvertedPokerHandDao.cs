namespace PokerTell.Infrastructure.Interfaces.PokerHand
{
    using NHibernate;

    public interface IConvertedPokerHandDao
    {
        IConvertedPokerHandDao InitializeWith(ISession session);

        IConvertedPokerHand GetHandWith(ulong gameId, string site);

        IConvertedPokerHand Insert(IConvertedPokerHand convertedPokerHand);

        IConvertedPokerHand Get(int id);
    }
}