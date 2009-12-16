namespace PokerTell.Infrastructure.Interfaces.Statistics
{
    using System.Collections.Generic;

    using PokerHand;

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

        int TotalCountPreFlopUnraisedPot { get; }

        int TotalCountPreFlopRaisedPot { get; }

        IPlayerStatistics SetFilter(IAnalyzablePokerPlayersFilter filter);

        IPlayerStatistics UpdateStatistics();

        /*  OverlayStatisticsTexts OverlayText { get; set; }

          */

        IPlayerStatistics InitializePlayer(string playerName, string pokerSite);
    }
}