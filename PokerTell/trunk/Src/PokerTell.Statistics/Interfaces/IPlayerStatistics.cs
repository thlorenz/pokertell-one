namespace PokerTell.Statistics.Interfaces
{
    using Infrastructure.Interfaces.PokerHand;
    using Infrastructure.Interfaces.Repository;

    public interface IPlayerStatistics
    {
        IActionSequenceSetStatistics[] HeroXOrHeroBOutOfPosition { get; }

        IActionSequenceSetStatistics[] HeroXOrHeroBInPosition { get; }

        IActionSequenceSetStatistics[] OppBIntoHeroInPosition { get; }

        IActionSequenceSetStatistics[] OppBIntoHeroOutOfPosition { get; }

        IActionSequenceSetStatistics[] HeroXOutOfPositionOppB { get; }

        IActionSequenceSetStatistics PreFlopUnraisedPot { get;  }

        IActionSequenceSetStatistics PreFlopRaisedPot { get;  }

        int[] TotalCountsOutOfPosition { get; }

        int[] TotalCountsOutInPosition { get; }

        IPlayerIdentity PlayerIdentity { get; }

        IPlayerStatistics SetFilter();

        IPlayerStatistics UpdateFrom(IRepository repository);

        /*  OverlayStatisticsTexts OverlayText { get; set; }

          StatsFilterList Filter { get; } */
    }
}