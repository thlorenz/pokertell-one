namespace PokerTell.LiveTracker.Interfaces
{
    using System.Collections.Generic;
    using System.Windows;

    using Tools.Interfaces;

    public interface IHarringtonMViewModel : IFluentInterface
    {
        int Value { get; set; }

        double Left { get; set; }

        double Top { get; set; }

        IHarringtonMViewModel InitializeWith(IList<Point> harringtonMPositions, int seatNumber);
    }
}