namespace PokerTell.Statistics.ViewModels.StatisticsSetDetails
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Infrastructure.Enumerations.PokerHand;
    using Infrastructure.Interfaces.PokerHand;
    using Infrastructure.Interfaces.Statistics;

    using Interfaces;

    using Tools.FunctionalCSharp;
    using Tools.Interfaces;
    using Tools.WPF.ViewModels;

    public abstract class DetailedStatisticsViewModel : NotifyPropertyChanged, IDetailedStatisticsViewModel
    {
        #region Constants and Fields

        IDetailedStatisticsViewModel _childViewModel;

        string _detailedStatisticsDescription;

        IEnumerable<IDetailedStatisticsRowViewModel> _rows;

        #endregion

        #region Constructors and Destructors

        protected DetailedStatisticsViewModel(string columnHeaderTitle)
        {
            SelectedCells = new List<ITuple<int, int>>();

            ColumnHeaderTitle = columnHeaderTitle;
        }

        #endregion

        #region Events

        public event Action<IDetailedStatisticsAnalyzerContentViewModel> ChildViewModelChanged = delegate { };

        #endregion

        #region Properties

        /// <summary>
        ///   ViewModel that will present the selected details or hand histories
        /// </summary>
        public IDetailedStatisticsViewModel ChildViewModel
        {
            get { return _childViewModel; }
            protected set
            {
                _childViewModel = value;
                ChildViewModelChanged(_childViewModel);
            }
        }

        /// <summary>
        ///   Indicates the values in the the columnheaders
        /// </summary>
        public string ColumnHeaderTitle { get; protected set; }

        /// <summary>
        ///   Describes the situation and player of the statistics
        /// </summary>
        public string DetailedStatisticsDescription
        {
            get { return _detailedStatisticsDescription; }
            protected set
            {
                _detailedStatisticsDescription = value;
                RaisePropertyChanged(() => DetailedStatisticsDescription);
            }
        }

        public IEnumerable<IDetailedStatisticsRowViewModel> Rows
        {
            get { return _rows; }
            protected set
            {
                _rows = value;
                RaisePropertyChanged(() => Rows);
            }
        }

        /// <summary>
        ///   Assumes that cells have been selected and that they are all in the same row.
        ///   It returns the ActionSequence associated with the row of the first selected cell.
        /// </summary>
        public ActionSequences SelectedActionSequence
        {
            get
            {
                int row = SelectedCells.First().First;

                return ActionSequenceStatisticsSet.ActionSequenceStatistics.ElementAt(row).ActionSequence;
            }
        }

        /// <summary>
        ///   Returns all AnalyzablePokerPlayers whose percentages were selected on the table
        /// </summary>
        public IEnumerable<IAnalyzablePokerPlayer> SelectedAnalyzablePlayers
        {
            get
            {
                return SelectedCells.SelectMany(
                    selectedCell => {
                        int row = selectedCell.First;
                        int col = selectedCell.Second;
                        return ActionSequenceStatisticsSet.ActionSequenceStatistics.ElementAt(row).MatchingPlayers[col];
                    });
            }
        }

        public IList<ITuple<int, int>> SelectedCells { get; protected set; }

        /// <summary>
        ///   Provides the data for the Viewmodel
        ///   Needs to be set via InitializeWith before ViewModel becomes useful
        /// </summary>
        protected IActionSequenceStatisticsSet ActionSequenceStatisticsSet { get; private set; }

        protected virtual string PlayerName
        {
            get { return ActionSequenceStatisticsSet.PlayerName; }
        }

        protected ITuple<int, int> SelectedColumnsSpan
        {
            get
            {
                int lowestSelectedColumnIndex = SelectedCells.Min(cell => cell.Second);
                int highestSelectedColumnIndex = SelectedCells.Max(cell => cell.Second);
                return Tuple.New(lowestSelectedColumnIndex, highestSelectedColumnIndex);
            }
        }

        /// <summary>
        ///   The street for which the given ActionSequenceStatisticsSet applies
        /// </summary>
        protected virtual Streets Street
        {
            get { return ActionSequenceStatisticsSet.Street; }
        }

        #endregion

        #region Implemented Interfaces

        #region IDetailedStatisticsViewModel

        public IDetailedStatisticsViewModel AddToSelection(int row, int column)
        {
            SelectedCells.Add(Tuple.New(row, column));
            return this;
        }

        public void ClearSelection()
        {
            SelectedCells.Clear();
        }

        /// <summary>
        ///   Needs to be called to fill viewmodel with data
        /// </summary>
        /// <param name="statisticsSet">
        ///   Provides underlying data
        /// </param>
        /// <returns>
        ///   Itself to enable fluent interface
        /// </returns>
        public IDetailedStatisticsViewModel InitializeWith(IActionSequenceStatisticsSet statisticsSet)
        {
            ActionSequenceStatisticsSet = statisticsSet;
            return CreateTableAndDescriptionFor(statisticsSet);
        }

        #endregion

        #endregion

        #region Methods

        protected abstract IDetailedStatisticsViewModel CreateTableAndDescriptionFor(IActionSequenceStatisticsSet statisticsSet);

        #endregion
    }
}