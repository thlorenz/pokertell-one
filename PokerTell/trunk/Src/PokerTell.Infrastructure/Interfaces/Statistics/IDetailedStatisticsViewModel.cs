namespace PokerTell.Infrastructure.Interfaces.Statistics
{
    using System;
    using System.Collections.Generic;

    using Tools.Interfaces;

    public interface IDetailedStatisticsViewModel
    {
        #region Properties

        string ColumnHeaderTitle { get; }

        IEnumerable<IDetailedStatisticsRowViewModel> Rows { get; }

        string DetailedStatisticsDescription { get; }

        IDetailedStatisticsViewModel ChildViewModel { get; }

        IList<ITuple<int, int>> SelectedCells { get;}

        event Action<IDetailedStatisticsViewModel> ChildViewModelChanged;

        #endregion

        #region Public Methods

        void AddToSelection(int row, int column);

        void ClearSelection();

        #endregion
    }
}