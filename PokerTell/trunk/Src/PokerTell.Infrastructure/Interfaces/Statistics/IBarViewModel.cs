namespace PokerTell.Infrastructure.Interfaces.Statistics
{
    using System.Windows.Media;

    public interface IBarViewModel
    {
        int Percentage { get; }

        SolidColorBrush Stroke { get; }
    }
}