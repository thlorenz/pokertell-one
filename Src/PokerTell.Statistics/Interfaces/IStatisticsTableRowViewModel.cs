namespace PokerTell.Statistics.Interfaces
{
    using System.Collections.Generic;

    using Infrastructure.Interfaces.Statistics;

    public interface IStatisticsTableRowViewModel
    {
        IList<IStatisticsTableCellViewModel> Cells { get; }

        string Title { get; }

        string Unit { get; }

        bool IsSelectable { get; }
    }
}