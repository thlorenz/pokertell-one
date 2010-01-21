namespace PokerTell.Infrastructure.Interfaces.Statistics
{
    using Tools.Interfaces;

    public interface IStatisticsSetSummaryRowViewModel : IFluentInterface
    {
        string ActionLetter { get; }

        IBarGraphViewModel BarGraph { get; }

        string Percentage { get; }

        IStatisticsSetSummaryRowViewModel UpdateWith(int percentage, int[] percentagesByColumn);
    }
}