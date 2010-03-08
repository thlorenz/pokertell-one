namespace PokerTell.Infrastructure.Interfaces.Statistics
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Input;

    public interface IStatisticsSetSummaryViewModel
    {
        IList<IStatisticsSetSummaryRowViewModel> Rows { get; }

        ICommand SelectStatisticsSetCommand { get; }

        IStatisticsSetSummaryViewModel UpdateWith(IActionSequenceStatisticsSet statisticsSet);

        event Action<IActionSequenceStatisticsSet> StatisticsSetSelectedEvent;
    }
}