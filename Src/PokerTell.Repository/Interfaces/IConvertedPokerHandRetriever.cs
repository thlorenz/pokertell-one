namespace PokerTell.Repository.Interfaces
{
    using Infrastructure.Interfaces.DatabaseSetup;
    using Infrastructure.Interfaces.PokerHand;

    public interface IConvertedPokerHandRetriever
    {
        IConvertedPokerHand RetrieveHand(int handId);

        IConvertedPokerHandRetriever Use(IDataProvider dataProvider);
    }
}