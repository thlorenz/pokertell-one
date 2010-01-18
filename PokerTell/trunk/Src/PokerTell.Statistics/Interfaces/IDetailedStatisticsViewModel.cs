namespace PokerTell.Statistics.Interfaces
{
    using System.Collections.Generic;

    using ViewModels.StatisticsSetDetails;

    public interface IDetailedStatisticsViewModel
    {
        #region Properties

        string ColumnHeaderTitle { get; }

        IEnumerable<IDetailedStatisticsRowViewModel> Rows { get; }

        IEnumerable<IDetailedStatisticsCellViewModel> SelectedCells { get; }

        #endregion

        #region Public Methods

        void AddToSelectionFrom(int row, int column);

        void ClearSelection();

        #endregion
    }
}