namespace PokerTell.Statistics.ViewModels.StatisticsSetDetails
{
    using System.Collections.Generic;
    using System.Linq;

    using Interfaces;

    public abstract class DetailedStatisticsViewModel : IDetailedStatisticsViewModel
    {
        #region Constants and Fields

        readonly IList<IDetailedStatisticsCellViewModel> _selectedCells;

        #endregion

        #region Constructors and Destructors

        protected DetailedStatisticsViewModel(string columnHeaderTitle)
        {
            _selectedCells = new List<IDetailedStatisticsCellViewModel>();

            ColumnHeaderTitle = columnHeaderTitle;
        }

        #endregion

        #region Properties

        public string ColumnHeaderTitle { get; protected set; }

        public IEnumerable<IDetailedStatisticsRowViewModel> Rows { get; protected set; }

        public IEnumerable<IDetailedStatisticsCellViewModel> SelectedCells
        {
            get { return _selectedCells; }
        }

        #endregion

        #region Implemented Interfaces

        #region IDetailedStatisticsViewModel

        public void AddToSelectionFrom(int row, int column)
        {
            _selectedCells.Add(Rows.ElementAt(row).Cells.ElementAt(column));
        }

        public void ClearSelection()
        {
            _selectedCells.Clear();
        }

        #endregion

        #endregion
    }
}