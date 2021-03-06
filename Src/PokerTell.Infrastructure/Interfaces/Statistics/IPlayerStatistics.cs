namespace PokerTell.Infrastructure.Interfaces.Statistics
{
    using System;
    using System.Collections.Generic;

    using Enumerations.PokerHand;

    using PokerHand;

    public interface IPlayerStatistics : IFluentInterface
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

        IAnalyzablePokerPlayersFilter Filter { get; set; }

        IEnumerable<IAnalyzablePokerPlayer> FilteredAnalyzablePokerPlayers { get; }

        IPlayerStatistics UpdateStatistics();

        IPlayerStatistics InitializePlayer(string playerName, string pokerSite);

        event Action FilterChanged;
    }
}