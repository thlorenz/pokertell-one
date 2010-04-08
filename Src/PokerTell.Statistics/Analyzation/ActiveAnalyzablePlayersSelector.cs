namespace PokerTell.Statistics.Analyzation
{
    using System.Collections.Generic;
    using System.Linq;

    using PokerTell.Infrastructure.Enumerations.PokerHand;
    using PokerTell.Infrastructure.Interfaces.PokerHand;
    using PokerTell.Infrastructure.Interfaces.Statistics;

    public class ActiveAnalyzablePlayersSelector : IActiveAnalyzablePlayersSelector
    {
        public IEnumerable<IAnalyzablePokerPlayer> SelectFrom(IPlayerStatistics playerStatistics)
        {
            if (playerStatistics == null)
                return Enumerable.Empty<IAnalyzablePokerPlayer>();

            return SelectFrom(playerStatistics.FilteredAnalyzablePokerPlayers);
        }

        public virtual IEnumerable<IAnalyzablePokerPlayer> SelectFrom(IEnumerable<IAnalyzablePokerPlayer> analyzablePokerPlayers)
        {
            return from player in analyzablePokerPlayers
                   where
                       player.ActionSequences[(int)Streets.PreFlop] != ActionSequences.HeroF &&
                       player.ActionSequences[(int)Streets.PreFlop] != ActionSequences.OppRHeroF &&
                       player.ActionSequences[(int)Streets.PreFlop] != ActionSequences.NonStandard
                   select player;
        }
    }
}