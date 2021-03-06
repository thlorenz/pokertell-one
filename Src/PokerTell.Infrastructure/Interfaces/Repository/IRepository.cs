namespace PokerTell.Infrastructure.Interfaces.Repository
{
    using System.Collections.Generic;

    using DatabaseSetup;

    using NHibernate;

    using PokerHand;

    public interface IRepository
    {
        IConvertedPokerHand RetrieveConvertedHand(int handId);

        IEnumerable<IConvertedPokerHand> RetrieveConvertedHands(IEnumerable<int> handIds);

        IEnumerable<IConvertedPokerHand> RetrieveHandsFromFile(string fileName);

        IRepository InsertHand(IConvertedPokerHand convertedPokerHand);

        IRepository InsertHands(IEnumerable<IConvertedPokerHand> handsToInsert);

        IConvertedPokerHand RetrieveConvertedHandWith(ulong gameId, string site);

        IEnumerable<IAnalyzablePokerPlayer> FindAnalyzablePlayersWith(int playerIdentity, long lastQueriedId);

        IPlayerIdentity FindPlayerIdentityFor(string name, string site);

        IEnumerable<IConvertedPokerHand> RetrieveHandsFromString(string handHistories);

        IList<IPlayerIdentity> RetrieveAllPlayerIdentities();
    }
}