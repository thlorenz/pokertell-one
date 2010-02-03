namespace PokerTell.Infrastructure.Interfaces.PokerHand
{
    using System.Collections.Generic;

    using Statistics;

    public interface IHandBrowserViewModel : IDetailedStatisticsAnalyzerContentViewModel
    {
        IHandBrowserViewModel InitializeWith(IEnumerable<IAnalyzablePokerPlayer> analyzablePokerPlayers);
    }
}