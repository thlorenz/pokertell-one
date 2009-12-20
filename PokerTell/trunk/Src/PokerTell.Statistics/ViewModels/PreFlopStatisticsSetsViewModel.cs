namespace PokerTell.Statistics.ViewModels
{
    using System;

    using PokerTell.Infrastructure.Enumerations.PokerHand;
    using PokerTell.Infrastructure.Interfaces.Statistics;
    using PokerTell.Statistics.ViewModels.StatisticsSetSummary;

    public class PreFlopStatisticsSetsViewModel : IPreFlopStatisticsSetsViewModel
    {
        #region Constructors and Destructors

        public PreFlopStatisticsSetsViewModel()
        {
            InitializeStatisticsSetSummaryViewModels();

            RegisterEvents();
        }

        #endregion

        #region Events

        public event Action<IActionSequenceStatisticsSet, Streets> SelectedStatisticsSetEvent = delegate { };

        #endregion

        #region Properties

        public IStatisticsSetSummaryViewModel PreFlopRaisedPotStatisticsSet { get; protected set; }

        public IStatisticsSetSummaryViewModel PreFlopUnraisedPotStatisticsSet { get; protected set; }

        public int TotalCountPreFlopRaisedPot { get; protected set; }

        public int TotalCountPreFlopUnraisedPot { get; protected set; }

        #endregion

        #region Implemented Interfaces

        #region IPreFlopStatisticsSetsViewModel

        public IPreFlopStatisticsSetsViewModel UpdateWith(IPlayerStatistics playerStatistics)
        {
            PreFlopUnraisedPotStatisticsSet.UpdateWith(playerStatistics.PreFlopUnraisedPot);
            PreFlopRaisedPotStatisticsSet.UpdateWith(playerStatistics.PreFlopRaisedPot);

            TotalCountPreFlopUnraisedPot = playerStatistics.TotalCountPreFlopUnraisedPot;
            TotalCountPreFlopRaisedPot = playerStatistics.TotalCountPreFlopRaisedPot;

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
                statisticsSet => SelectedStatisticsSetEvent(statisticsSet, Streets.PreFlop);
            PreFlopRaisedPotStatisticsSet.StatisticsSetSelectedEvent +=
                statisticsSet => SelectedStatisticsSetEvent(statisticsSet, Streets.PreFlop);
        }

        #endregion
    }
}