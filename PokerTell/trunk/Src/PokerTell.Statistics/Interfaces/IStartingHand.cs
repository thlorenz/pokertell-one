namespace PokerTell.Statistics.Interfaces
{
    public interface IStartingHand
    {
        int Left { get; }

        int Top { get; }

        int Count { get; set; }

        double FillHeight { get; set; }

        string Display { get; }
    }
}