namespace PokerTell.Statistics.Interfaces
{
    using System.Collections.Generic;

    using Infrastructure.Interfaces.Statistics;

    public interface IPreFlopStartingHandsVisualizerViewModel : IDetailedStatisticsAnalyzerContentViewModel
    {
        IPreFlopStartingHandsVisualizerViewModel Visualize(IEnumerable<string> startingHands);
    }
}