namespace PokerTell.Infrastructure.Interfaces.Repository
{
    using System.Collections.Generic;

    using DatabaseSetup;

    using PokerHand;

    public interface IRepository
    {
        IEnumerable<IConvertedPokerHand> RetrieveHandsFromFile(string fileName);

        IRepository InsertHand(IConvertedPokerHand convertedPokerHand);

        IConvertedPokerHand RetrieveConvertedHand(int handId);

        IEnumerable<IConvertedPokerHand> RetrieveConvertedHands(IEnumerable<int> handIds);

        IRepository Use(IDataProvider dataProvider);
    }
}