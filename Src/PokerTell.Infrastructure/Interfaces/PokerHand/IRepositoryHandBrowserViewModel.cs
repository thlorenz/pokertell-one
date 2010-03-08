namespace PokerTell.Infrastructure.Interfaces.PokerHand
{
    using System.Collections.Generic;

    using Statistics;

    public interface IRepositoryHandBrowserViewModel : IDetailedStatisticsAnalyzerContentViewModel
    {
        IRepositoryHandBrowserViewModel InitializeWith(IEnumerable<IAnalyzablePokerPlayer> analyzablePokerPlayers);

        IHandHistoryViewModel CurrentHandHistory { get; }

        int HandCount { get; }

        int CurrentHandIndex { get; set; }
    }
}