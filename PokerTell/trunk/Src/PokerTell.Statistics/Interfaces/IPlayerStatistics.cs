namespace PokerTell.Statistics.Interfaces
{
    using System.Collections.Generic;

    using Infrastructure.Interfaces.PokerHand;
    using Infrastructure.Interfaces.Repository;

    public interface IPlayerStatistics
    {
        IActionSequenceStatisticsSet[] HeroXOrHeroBOutOfPosition { get; }

        IActionSequenceStatisticsSet[] HeroXOrHeroBInPosition { get; }

        IActionSequenceStatisticsSet[] OppBIntoHeroInPosition { get; }

        IActionSequenceStatisticsSet[] OppBIntoHeroOutOfPosition { get; }

        IActionSequenceStatisticsSet[] HeroXOutOfPositionOppB { get; }

        IActionSequenceStatisticsSet PreFlopUnraisedPot { get;  }

        IActionSequenceStatisticsSet PreFlopRaisedPot { get;  }

        IEnumerable<int> TotalCountsOutOfPosition { get; }

        IEnumerable<int> TotalCountsInPosition { get; }

        IPlayerIdentity PlayerIdentity { get; }

        int TotalCountsPreFlopUnraisedPot { get; }

        int TotalCountsPreFlopRaisedPot { get; }

        IPlayerStatistics SetFilter(IAnalyzablePokerPlayersFilter filter);

        IPlayerStatistics UpdateStatistics();

        /*  OverlayStatisticsTexts OverlayText { get; set; }

          StatsFilterList Filter { get; } */

        IPlayerStatistics InitializePlayer(string playerName, string pokerSite);
    }
}