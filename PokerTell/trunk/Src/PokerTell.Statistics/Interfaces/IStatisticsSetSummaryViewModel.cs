namespace PokerTell.Statistics.Interfaces
{
    using System.Collections.Generic;

    public interface IStatisticsSetSummaryViewModel
    {
        IList<IStatisticsSetSummaryRowViewModel> Rows { get; }

        IStatisticsSetSummaryViewModel UpdateWith(IActionSequenceStatisticsSet statisticsSet);
    }
}