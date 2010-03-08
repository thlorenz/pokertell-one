namespace PokerTell.Statistics.Interfaces
{
    using System.Collections.Generic;

    public interface IPreFlopStartingHandsVisualizer
    {
        IPreFlopStartingHandsVisualizer InitializeWith(int sideLength, int pairMargin);

        IPreFlopStartingHandsVisualizer Visualize(IEnumerable<string> holeCards);

        IDictionary<string, IStartingHand> StartingHands { get; }
    }
}