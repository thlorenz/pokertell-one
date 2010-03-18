namespace PokerTell.Statistics.ViewModels.Base
{
    using System;
    using System.Collections.Generic;

    using Infrastructure.Interfaces.Statistics;

    using PokerTell.Statistics.Interfaces;

    using Tools.FunctionalCSharp;
    using Tools.Interfaces;
    using Tools.WPF.ViewModels;

    public abstract class StatisticsTableViewModel : DetailedStatisticsAnalyzerContentViewModel, IStatisticsTableViewModel
    {
        IEnumerable<IStatisticsTableRowViewModel> _rows;

        public StatisticsTableViewModel(string columnHeaderTitle)
        {
            SelectedCells = new List<ITuple<int, int>>();
            SavedSelectedCells = new List<ITuple<int, int>>();
            ColumnHeaderTitle = columnHeaderTitle;
        }

        /// <summary>
        ///   Indicates the values in the the columnheaders
        /// </summary>
        public string ColumnHeaderTitle { get; protected set; }

        public IEnumerable<IStatisticsTableRowViewModel> Rows
        {
            get { return _rows; }
            protected set
            {
                _rows = value;
                RaisePropertyChanged(() => Rows);
            }
        }

        public IList<ITuple<int, int>> SelectedCells { get; protected set; }

        string _statisticsDescription;

        /// <summary>
        ///   Describes the situation and player of the statistics
        /// </summary>
        public string StatisticsDescription
        {
            get { return _statisticsDescription; }
            protected set
            {
                _statisticsDescription = value;
                RaisePropertyChanged(() => StatisticsDescription);
            }
        }

        public bool ShowStatisticsInformationPanel { get; set; } 

        string _statisticsHint;

        /// <summary>
        /// Gives a hint on how to interpret the statistics, e.g. what the RaiseSizes indicate
        /// </summary>
        public string StatisticsHint
        {
            get { return _statisticsHint; }
            protected set
            {
                _statisticsHint = value;
                RaisePropertyChanged(() => StatisticsHint);
            }
        }

        IDetailedStatisticsAnalyzerContentViewModel _childViewModel;

        /// <summary>
        ///   ViewModel that will present the selected details or hand histories
        /// </summary>
        public IDetailedStatisticsAnalyzerContentViewModel ChildViewModel
        {
            get { return _childViewModel; }
            set
            {
                _childViewModel = value;
                RaiseChildViewModelChanged(_childViewModel);
            }
        }

        public static IStatisticsTableViewModel Emty
        {
            get { return new EmptyStatisticsTableViewModel(); }
        }

        public IStatisticsTableViewModel AddToSelection(int row, int column)
        {
            SelectedCells.Add(Tuple.New(row, column));
            return this;
        }

        public void ClearSelection()
        {
            SelectedCells.Clear();
        }

        /// <summary>
        /// Saves the selected cells into SavedSelectedCells e.g. when the data grid is unloading 
        /// </summary>
        public void SaveSelectedCells()
        {
            SavedSelectedCells = new List<ITuple<int, int>>(SelectedCells);
        }

        public List<ITuple<int, int>> SavedSelectedCells { get; protected set; }
    }

    internal class EmptyStatisticsTableViewModel : StatisticsTableViewModel
    {
        public EmptyStatisticsTableViewModel()
            : base(string.Empty)
        {
        }
    }
}