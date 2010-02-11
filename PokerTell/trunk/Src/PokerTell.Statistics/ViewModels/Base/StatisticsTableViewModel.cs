namespace PokerTell.Statistics.ViewModels.Base
{
    using System;
    using System.Collections.Generic;

    using Infrastructure.Interfaces.Statistics;

    using PokerTell.Statistics.Interfaces;

    using Tools.FunctionalCSharp;
    using Tools.Interfaces;
    using Tools.WPF.ViewModels;

    public abstract class StatisticsTableViewModel : NotifyPropertyChanged, IStatisticsTableViewModel
    {
        IEnumerable<IStatisticsTableRowViewModel> _rows;

        public StatisticsTableViewModel(string columnHeaderTitle)
        {
            SelectedCells = new List<ITuple<int, int>>();
            ColumnHeaderTitle = columnHeaderTitle;
        }

        public bool MayInvestigateHoleCards { get; protected set; }

        public bool MayInvestigateRaise { get; protected set; }

        public bool MayBrowseHands { get; protected set; }

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
                ChildViewModelChanged(_childViewModel);
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

        public event Action<IDetailedStatisticsAnalyzerContentViewModel> ChildViewModelChanged = delegate { };
    }

    internal class EmptyStatisticsTableViewModel : StatisticsTableViewModel
    {
        public EmptyStatisticsTableViewModel()
            : base(string.Empty)
        {
        }
    }
}