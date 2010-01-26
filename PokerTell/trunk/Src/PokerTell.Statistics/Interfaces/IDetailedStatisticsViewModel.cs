namespace PokerTell.Statistics.Interfaces
{
    using System.Collections.Generic;

    using Infrastructure.Interfaces.Statistics;

    using PokerTell.Infrastructure.Enumerations.PokerHand;
    using PokerTell.Infrastructure.Interfaces.PokerHand;

    using Tools.Interfaces;

    public interface IDetailedStatisticsViewModel : IDetailedStatisticsAnalyzerContentViewModel
    {
        #region Properties

        /// <summary>
        /// Indicates the meaning of the values in the the columnheaders
        /// </summary>
        string ColumnHeaderTitle { get; }

        IEnumerable<IDetailedStatisticsRowViewModel> Rows { get; }

        /// <summary>
        /// Describes the situation and player of the statistics
        /// </summary>
        string DetailedStatisticsDescription { get; }

        /// <summary>
        /// ViewModel that will present the selected details or hand histories
        /// </summary>
        IDetailedStatisticsViewModel ChildViewModel { get; }

        IList<ITuple<int, int>> SelectedCells { get;}

        /// <summary>
        /// Returns all AnalyzablePokerPlayers whose percentages were selected on the table
        /// </summary>
        IEnumerable<IAnalyzablePokerPlayer> SelectedAnalyzablePlayers { get; }

        /// <summary>
        /// Assumes that cells have been selected and that they are all in the same row.
        /// It returns the ActionSequence associated with the row of the first selected cell.
        /// </summary>
        ActionSequences SelectedActionSequence { get; }

        #endregion

        #region Public Methods

        IDetailedStatisticsViewModel AddToSelection(int row, int column);

        void ClearSelection();

        #endregion

        /// <summary>
        /// Needs to be called to fill viewmodel with data
        /// </summary>
        /// <param name="statisticsSet">Provides underlying data</param>
        /// <returns>Itself to enable fluent interface</returns>
        IDetailedStatisticsViewModel InitializeWith(IActionSequenceStatisticsSet statisticsSet);
    }
}