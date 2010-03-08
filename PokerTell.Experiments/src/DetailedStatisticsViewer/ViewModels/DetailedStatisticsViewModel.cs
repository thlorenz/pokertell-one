namespace DetailedStatisticsViewer.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows.Input;

    using Tools.WPF;

    public interface IDetailedStatisticsViewModel
    {
        #region Properties

        string ColumnHeaderTitle { get; }

        IEnumerable<IRowViewModel> Rows { get; }

        IEnumerable<ICellViewModel> SelectedCells { get; }

        #endregion

        #region Public Methods

        void AddToSelectionFrom(int row, int column);

        void ClearSelection();

        #endregion
    }

    public abstract class DetailedStatisticsViewModel : IDetailedStatisticsViewModel
    {
        #region Constants and Fields

        readonly IList<ICellViewModel> _selectedCells;

        #endregion

        #region Constructors and Destructors

        public DetailedStatisticsViewModel(string columnHeaderTitle)
        {
            _selectedCells = new List<ICellViewModel>();

            ColumnHeaderTitle = columnHeaderTitle;
        }

        #endregion

        #region Properties

        public string ColumnHeaderTitle { get; protected set; }

        
        public IEnumerable<IRowViewModel> Rows { get; protected set; }

        public IEnumerable<ICellViewModel> SelectedCells
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