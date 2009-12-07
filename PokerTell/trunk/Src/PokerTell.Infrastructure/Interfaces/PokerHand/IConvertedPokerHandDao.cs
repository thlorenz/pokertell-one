namespace PokerTell.Infrastructure.Interfaces.PokerHand
{
    using NHibernate;

    public interface IConvertedPokerHandDao : IFluentInterface 
    {
        IConvertedPokerHandDao Insert(IConvertedPokerHand convertedPokerHand, IStatelessSession statelessSession);

        IConvertedPokerHand Get(int id);

        IConvertedPokerHand GetHandWith(ulong gameId, string site);

        IConvertedPokerHand Insert(IConvertedPokerHand convertedPokerHand);
    }
}