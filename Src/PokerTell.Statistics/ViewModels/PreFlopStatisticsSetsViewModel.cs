namespace PokerTell.Statistics.ViewModels
{
    using System;
    using System.Linq;
    using System.Windows.Input;

    using PokerTell.Infrastructure.Enumerations.PokerHand;
    using PokerTell.Infrastructure.Interfaces.Statistics;
    using PokerTell.Statistics.ViewModels.StatisticsSetSummary;

    using Tools.WPF;
    using Tools.WPF.ViewModels;

    public class PreFlopStatisticsSetsViewModel : NotifyPropertyChanged, IPreFlopStatisticsSetsViewModel
    {
        ICommand _browseAllHandsCommand;

        string _steals;

        int _totalCountPreFlopRaisedPot;

        int _totalCountPreFlopUnraisedPot;

        public PreFlopStatisticsSetsViewModel()
        {
            InitializeStatisticsSetSummaryViewModels();

            RegisterEvents();
        }

        public event Action BrowseAllMyHandsRequested = delegate { };

        public event Action<IActionSequenceStatisticsSet> SelectedStatisticsSetEvent = delegate { };

        public ICommand BrowseAllHandsCommand
        {
            get
            {
                return _browseAllHandsCommand ?? (_browseAllHandsCommand = new SimpleCommand
                    {
                        ExecuteDelegate = arg => BrowseAllMyHandsRequested()
                    });
            }
        }

        public IStatisticsSetSummaryViewModel PreFlopRaisedPotStatisticsSet { get; protected set; }

        public IStatisticsSetSummaryViewModel PreFlopUnraisedPotStatisticsSet { get; protected set; }

        public string Steals
        {
            get { return _steals; }
            set
            {
                _steals = value;
                RaisePropertyChanged(() => Steals);
            }
        }

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

        public IPreFlopStatisticsSetsViewModel UpdateWith(IPlayerStatistics playerStatistics)
        {
            PreFlopUnraisedPotStatisticsSet.UpdateWith(playerStatistics.PreFlopUnraisedPot);
            PreFlopRaisedPotStatisticsSet.UpdateWith(playerStatistics.PreFlopRaisedPot);

            TotalCountPreFlopUnraisedPot = playerStatistics.TotalCountPreFlopUnraisedPot;
            TotalCountPreFlopRaisedPot = playerStatistics.TotalCountPreFlopRaisedPot;

            Steals = string.Format("{0:0#}", playerStatistics.PreFlopUnraisedPot.ActionSequenceStatistics.Last().Percentages[(int)StrategicPositions.BU]);

            return this;
        }

        protected void RegisterEvents()
        {
            PreFlopUnraisedPotStatisticsSet.StatisticsSetSelectedEvent += statisticsSet => SelectedStatisticsSetEvent(statisticsSet);
            PreFlopRaisedPotStatisticsSet.StatisticsSetSelectedEvent += statisticsSet => SelectedStatisticsSetEvent(statisticsSet);
        }

        void InitializeStatisticsSetSummaryViewModels()
        {
            PreFlopUnraisedPotStatisticsSet = new StatisticsSetSummaryViewModel();
            PreFlopRaisedPotStatisticsSet = new StatisticsSetSummaryViewModel();
        }
    }
}