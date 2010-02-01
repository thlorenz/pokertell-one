namespace PokerTell.Statistics.Interfaces
{
    using System.Collections.Generic;

    using Infrastructure.Interfaces.Statistics;

    using Tools.Interfaces;

    public interface IStatisticsTableViewModel : IDetailedStatisticsAnalyzerContentViewModel
    {
        /// <summary>
        ///   Indicates the values in the the columnheaders
        /// </summary>
        string ColumnHeaderTitle { get; }

        IEnumerable<IStatisticsTableRowViewModel> Rows { get; }

        IList<ITuple<int, int>> SelectedCells { get; }

        /// <summary>
        /// Describes the situation and player of the statistics
        /// </summary>
        string StatisticsDescription { get; }

        IStatisticsTableViewModel AddToSelection(int row, int column);

        void ClearSelection();
    }
}