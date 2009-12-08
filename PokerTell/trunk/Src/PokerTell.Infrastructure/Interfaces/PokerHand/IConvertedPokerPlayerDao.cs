namespace PokerTell.PokerHand.Dao
{
    using System.Collections.Generic;

    using Infrastructure.Interfaces.PokerHand;

    public interface IConvertedPokerPlayerDao
    {
        IEnumerable<IConvertedPokerPlayer> FindByPlayerIdentity(int playerIdentity);

        IEnumerable<IAnalyzablePokerPlayer> FindAnalyzablePlayersWith(int playerIdentity, long lastQueriedId);
    }
}