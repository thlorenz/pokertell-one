namespace PokerTell.Statistics.ViewModels
{
    using System;
    using System.Linq;

    using PokerTell.Infrastructure.Enumerations.PokerHand;
    using PokerTell.Infrastructure.Interfaces.Statistics;
    using PokerTell.Statistics.ViewModels.StatisticsSetSummary;

    using Tools.WPF.ViewModels;

    public class PreFlopStatisticsSetsViewModel : NotifyPropertyChanged, IPreFlopStatisticsSetsViewModel
    {
        #region Constructors and Destructors

        public PreFlopStatisticsSetsViewModel()
        {
            InitializeStatisticsSetSummaryViewModels();

            RegisterEvents();
        }

        #endregion

        #region Events

        public event Action<IActionSequenceStatisticsSet> SelectedStatisticsSetEvent = delegate { };

        #endregion

        #region Properties

        public IStatisticsSetSummaryViewModel PreFlopRaisedPotStatisticsSet { get; protected set; }

        public IStatisticsSetSummaryViewModel PreFlopUnraisedPotStatisticsSet { get; protected set; }

        int _totalCountPreFlopRaisedPot;

        public int TotalCountPreFlopRaisedPot
        {
            get { return _totalCountPreFlopRaisedPot; }
            protected set
            {
                _totalCountPreFlopRaisedPot = value;
                RaisePropertyChanged(() => TotalCountPreFlopRaisedPot);
                RaisePropertyChanged(() => TotalCounts);
            }
        }

        int _totalCountPreFlopUnraisedPot;

        public int TotalCountPreFlopUnraisedPot
        {
            get { return _totalCountPreFlopUnraisedPot; }
            protected set
            {
                _totalCountPreFlopUnraisedPot = value;
                RaisePropertyChanged(() => TotalCountPreFlopUnraisedPot);
                RaisePropertyChanged(() => TotalCounts);
            }
        }

        public int TotalCounts
        {
            get { return TotalCountPreFlopUnraisedPot + TotalCountPreFlopRaisedPot; }
        }

        public string Steals
        {
            get { return _steals; }
            set
            {
                _steals = value;
                RaisePropertyChanged(() => Steals);
            }
        }

        string _steals;
        #endregion

        #region Implemented Interfaces

        #region IPreFlopStatisticsSetsViewModel

        public IPreFlopStatisticsSetsViewModel UpdateWith(IPlayerStatistics playerStatistics)
        {
            PreFlopUnraisedPotStatisticsSet.UpdateWith(playerStatistics.PreFlopUnraisedPot);
            PreFlopRaisedPotStatisticsSet.UpdateWith(playerStatistics.PreFlopRaisedPot);

            TotalCountPreFlopUnraisedPot = playerStatistics.TotalCountPreFlopUnraisedPot;
            TotalCountPreFlopRaisedPot = playerStatistics.TotalCountPreFlopRaisedPot;

            Steals = string.Format("{0:0#}",
                playerStatistics.PreFlopUnraisedPot.ActionSequenceStatistics.Last().Percentages[(int)StrategicPositions.BU]);

            return this;
        }

        #endregion

        #endregion

        #region Methods

        void InitializeStatisticsSetSummaryViewModels()
        {
            PreFlopUnraisedPotStatisticsSet = new StatisticsSetSummaryViewModel();
            PreFlopRaisedPotStatisticsSet = new StatisticsSetSummaryViewModel();
        }

        protected void RegisterEvents()
        {
            PreFlopUnraisedPotStatisticsSet.StatisticsSetSelectedEvent +=
                statisticsSet => SelectedStatisticsSetEvent(statisticsSet);
            PreFlopRaisedPotStatisticsSet.StatisticsSetSelectedEvent +=
                statisticsSet => SelectedStatisticsSetEvent(statisticsSet);
        }

        #endregion
    }
}