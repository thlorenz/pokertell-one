namespace PokerTell.Statistics.ViewModels.StatisticsSetDetails
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Infrastructure.Interfaces.Statistics;

    using Tools.FunctionalCSharp;
    using Tools.Interfaces;
    using Tools.WPF.ViewModels;

    public abstract class DetailedStatisticsViewModel : NotifyPropertyChanged, IDetailedStatisticsViewModel
    {
        #region Constructors and Destructors

        protected DetailedStatisticsViewModel(string columnHeaderTitle)
        {
            SelectedCells = new List<ITuple<int, int>>();

            ColumnHeaderTitle = columnHeaderTitle;
        }

        #endregion

        public abstract IDetailedStatisticsViewModel InitializeWith(IActionSequenceStatisticsSet statisticsSet);

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

        string _detailedStatisticsDescription;

        /// <summary>
        /// Describes the situation and player of the statistics
        /// </summary>
        public string DetailedStatisticsDescription
        {
            get { return _detailedStatisticsDescription; }
            protected set { _detailedStatisticsDescription = value; RaisePropertyChanged(() => DetailedStatisticsDescription);}
        }

        IEnumerable<IDetailedStatisticsRowViewModel> _rows;

        public IEnumerable<IDetailedStatisticsRowViewModel> Rows
        {
            get { return _rows; }
            protected set { _rows = value; RaisePropertyChanged(() => Rows);}
        }

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