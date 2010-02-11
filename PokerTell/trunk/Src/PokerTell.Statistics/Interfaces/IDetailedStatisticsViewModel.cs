namespace PokerTell.Statistics.Interfaces
{
    using System.Collections.Generic;
    using System.Windows.Input;

    using Infrastructure.Enumerations.PokerHand;
    using Infrastructure.Interfaces.PokerHand;
    using Infrastructure.Interfaces.Statistics;

    public interface IDetailedStatisticsViewModel : IStatisticsTableViewModel  
    {
        #region Properties

        /// <summary>
        /// ViewModel that will present the selected details or hand histories
        /// </summary>
        IDetailedStatisticsAnalyzerContentViewModel ChildViewModel { get; set; }


        /// <summary>
        /// Returns all AnalyzablePokerPlayers whose percentages were selected on the table
        /// </summary>
        IEnumerable<IAnalyzablePokerPlayer> SelectedAnalyzablePlayers { get; }

        /// <summary>
        /// Assumes that cells have been selected and that they are all in the same row.
        /// It returns the ActionSequence associated with the row of the first selected cell.
        /// </summary>
        ActionSequences SelectedActionSequence { get; }

        ICommand BrowseHandsCommand { get; }

        #endregion

        /// <summary>
        /// Needs to be called to fill viewmodel with data
        /// </summary>
        /// <param name="statisticsSet">Provides underlying data</param>
        /// <returns>Itself to enable fluent interface</returns>
        IDetailedStatisticsViewModel InitializeWith(IActionSequenceStatisticsSet statisticsSet);
    }
}