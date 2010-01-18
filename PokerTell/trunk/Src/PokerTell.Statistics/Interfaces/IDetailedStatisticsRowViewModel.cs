namespace PokerTell.Statistics.Interfaces
{
    using System.Collections.Generic;

    public interface IDetailedStatisticsRowViewModel
    {
        IList<IDetailedStatisticsCellViewModel> Cells { get; }

        string Title { get; }

        string Unit { get; }

        bool IsSelectable { get; }
    }
}