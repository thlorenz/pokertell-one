namespace PokerTell.Infrastructure.Interfaces.Statistics
{
    using System.Collections.Generic;

    using Enumerations.PokerHand;

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

        int TotalCountsInPosition(Streets street);

        int TotalCountsOutOfPosition(Streets street);

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