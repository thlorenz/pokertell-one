namespace PokerTell.Statistics.Interfaces
{
    using Infrastructure.Interfaces;

    using ViewModels.StatisticsSetSummary;

    public interface IStatisticsSetSummaryRowViewModel : IFluentInterface
    {
        string ActionLetter { get; }

        IBarGraphViewModel BarGraph { get; }

        string Percentage { get; }

        StatisticsSetSummaryRowViewModel UpdateWith(int percentage, int[] percentagesByColumn);
    }
}