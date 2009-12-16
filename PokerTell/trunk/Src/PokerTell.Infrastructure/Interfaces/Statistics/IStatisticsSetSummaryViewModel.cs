namespace PokerTell.Infrastructure.Interfaces.Statistics
{
    using System.Collections.Generic;

    public interface IStatisticsSetSummaryViewModel
    {
        IList<IStatisticsSetSummaryRowViewModel> Rows { get; }

        IStatisticsSetSummaryViewModel UpdateWith(IActionSequenceStatisticsSet statisticsSet);
    }
}