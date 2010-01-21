namespace PokerTell.Statistics.ViewModels.StatisticsSetDetails
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Infrastructure.Interfaces.Statistics;

    using Tools.FunctionalCSharp;
    using Tools.Interfaces;

    public abstract class DetailedStatisticsViewModel : IDetailedStatisticsViewModel
    {
        #region Constructors and Destructors

        protected DetailedStatisticsViewModel(string columnHeaderTitle)
        {
            SelectedCells = new List<ITuple<int, int>>();

            ColumnHeaderTitle = columnHeaderTitle;
        }

        #endregion

        #region Events

        public event Action<IDetailedStatisticsViewModel> ChildViewModelChanged = delegate { };

        #endregion

        #region Properties
        
        /// <summary>
        /// ViewModel that will present the selected details or hand histories
        /// </summary>
        public IDetailedStatisticsViewModel ChildViewModel { get; protected set; }

        /// <summary>
        /// Indicates the values in the the columnheaders
        /// </summary>
        public string ColumnHeaderTitle { get; protected set; }

        /// <summary>
        /// Describes the situation and player of the statistics
        /// </summary>
        public string DetailedStatisticsDescription { get; protected set; }

        public IEnumerable<IDetailedStatisticsRowViewModel> Rows { get; protected set; }

        public IList<ITuple<int, int>> SelectedCells { get; protected set; }

        #endregion

        #region Implemented Interfaces

        #region IDetailedStatisticsViewModel

        public void AddToSelection(int row, int column)
        {
            SelectedCells.Add(new Tuple<int, int>(row, column));
        }

        public void ClearSelection()
        {
            SelectedCells.Clear();
        }

        #endregion

        #endregion
    }
}