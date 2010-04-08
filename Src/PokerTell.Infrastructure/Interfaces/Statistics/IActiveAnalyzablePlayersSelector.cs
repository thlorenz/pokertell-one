namespace PokerTell.Infrastructure.Interfaces.Statistics
{
    using System.Collections.Generic;

    using PokerHand;

    public interface IActiveAnalyzablePlayersSelector : IFluentInterface
    {
        IEnumerable<IAnalyzablePokerPlayer> SelectFrom(IPlayerStatistics playerStatistics);

        IEnumerable<IAnalyzablePokerPlayer> SelectFrom(IEnumerable<IAnalyzablePokerPlayer> analyzablePokerPlayers);
    }
}