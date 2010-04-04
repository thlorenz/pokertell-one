namespace PokerTell.Infrastructure.Interfaces.Statistics
{
    using System;
    using System.Collections.Generic;

    using PokerTell.Infrastructure.Enumerations.PokerHand;

    public interface IPostFlopStatisticsSetsViewModel : IFluentInterface, IEnumerable<IStatisticsSetSummaryViewModel>
    {
        event Action<IActionSequenceStatisticsSet> SelectedStatisticsSetEvent;

        IStatisticsSetSummaryViewModel HeroXOrHeroBInPositionStatisticsSet { get; }

        IStatisticsSetSummaryViewModel HeroXOrHeroBOutOfPositionStatisticsSet { get; }

        IStatisticsSetSummaryViewModel HeroXOutOfPositionOppBStatisticsSet { get; }

        IStatisticsSetSummaryViewModel OppBIntoHeroInPositionStatisticsSet { get; }

        IStatisticsSetSummaryViewModel OppBIntoHeroOutOfPositionStatisticsSet { get; }

        int TotalCountInPosition { get; }

        int TotalCountOutOfPosition { get; }

        IPostFlopStatisticsSetsViewModel InitializeWith(Streets street);

        IPostFlopStatisticsSetsViewModel UpdateWith(IPlayerStatistics playerStatistics);
    }
}