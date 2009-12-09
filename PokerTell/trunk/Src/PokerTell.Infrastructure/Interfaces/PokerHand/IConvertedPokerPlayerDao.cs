namespace PokerTell.Infrastructure.Interfaces.PokerHand
{
    using System.Collections.Generic;

    public interface IConvertedPokerPlayerDao
    {
        IEnumerable<IConvertedPokerPlayer> FindByPlayerIdentity(int playerIdentity);

        IEnumerable<IAnalyzablePokerPlayer> FindAnalyzablePlayersWithLegacy(int playerIdentity, long lastQueriedId);

        IEnumerable<IAnalyzablePokerPlayer> FindAnalyzablePlayersWith(int playerIdentity, long lastQueriedId);
    }
}