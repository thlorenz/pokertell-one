namespace PokerTell.Statistics.Interfaces
{
    using System.Collections.Generic;

    using PokerTell.Infrastructure.Interfaces.Statistics;

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

        List<ITuple<int, int>> SavedSelectedCells { get; }

        /// <summary>
        /// Gives a hint on how to interpret the statistics, e.g. what the RaiseSizes indicate
        /// </summary>
        string StatisticsHint { get; }

        bool ShowStatisticsInformationPanel { get; set; }

        IStatisticsTableViewModel AddToSelection(int row, int column);

        void ClearSelection();

        /// <summary>
        /// Saves the selected cells into SavedSelectedCells e.g. when the data grid is unloading 
        /// </summary>
        void SaveSelectedCells();
    }
}