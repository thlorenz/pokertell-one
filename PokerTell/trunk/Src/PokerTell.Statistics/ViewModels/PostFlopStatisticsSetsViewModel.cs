namespace PokerTell.Statistics.ViewModels
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using PokerTell.Infrastructure.Enumerations.PokerHand;
    using PokerTell.Infrastructure.Interfaces.Statistics;
    using PokerTell.Statistics.ViewModels.StatisticsSetSummary;

    public class PostFlopStatisticsSetsViewModel : IPostFlopStatisticsSetsViewModel
    {
        #region Constants and Fields

        readonly Streets _street;

        #endregion

        #region Constructors and Destructors

        public PostFlopStatisticsSetsViewModel(Streets street)
        {
            _street = street;

            InitializeStatisticsSetSummaryViewModels();

            RegisterEvents();
        }

        #endregion

        #region Events

        public event Action<IActionSequenceStatisticsSet, Streets> SelectedStatisticsSetEvent = delegate { };

        #endregion

        #region Properties

        public IStatisticsSetSummaryViewModel HeroXOrHeroBInPositionStatisticsSet { get; protected set; }

        public IStatisticsSetSummaryViewModel HeroXOrHeroBOutOfPositionStatisticsSet { get; protected set; }

        public IStatisticsSetSummaryViewModel HeroXOutOfPositionOppBStatisticsSet { get; protected set; }

        public IStatisticsSetSummaryViewModel OppBIntoHeroInPositionStatisticsSet { get; protected set; }

        public IStatisticsSetSummaryViewModel OppBIntoHeroOutOfPositionStatisticsSet { get; protected set; }

        public int TotalCountInPosition { get; protected set; }

        public int TotalCountOutOfPosition { get; protected set; }

        #endregion

        #region Implemented Interfaces

        #region IEnumerable

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region IEnumerable<IStatisticsSetSummaryViewModel>

        public IEnumerator<IStatisticsSetSummaryViewModel> GetEnumerator()
        {
            return GetAllStatisticsSets().GetEnumerator();
        }

        #endregion

        #region IPostFlopStatisticsSetsViewModel

        public IPostFlopStatisticsSetsViewModel UpdateWith(IPlayerStatistics playerStatistics)
        {
            HeroXOrHeroBOutOfPositionStatisticsSet.UpdateWith(playerStatistics.HeroXOrHeroBOutOfPosition[(int)_street]);
            OppBIntoHeroOutOfPositionStatisticsSet.UpdateWith(playerStatistics.OppBIntoHeroOutOfPosition[(int)_street]);
            HeroXOutOfPositionOppBStatisticsSet.UpdateWith(playerStatistics.HeroXOutOfPositionOppB[(int)_street]);

            HeroXOrHeroBInPositionStatisticsSet.UpdateWith(playerStatistics.HeroXOrHeroBInPosition[(int)_street]);
            OppBIntoHeroInPositionStatisticsSet.UpdateWith(playerStatistics.OppBIntoHeroInPosition[(int)_street]);

            TotalCountOutOfPosition = playerStatistics.TotalCountsOutOfPosition(_street);
            TotalCountInPosition = playerStatistics.TotalCountsInPosition(_street);
            return this;
        }

        #endregion

        #endregion

        #region Methods

        IEnumerable<IStatisticsSetSummaryViewModel> GetAllStatisticsSets()
        {
            foreach (var set in GetOutOfPositionStatisticsSets())
            {
                yield return set;
            }

            foreach (var set in GetInPositionStatisticsSets())
            {
                yield return set;
            }
        }

        IEnumerable<IStatisticsSetSummaryViewModel> GetInPositionStatisticsSets()
        {
            yield return HeroXOrHeroBInPositionStatisticsSet;
            yield return OppBIntoHeroInPositionStatisticsSet;
        }

        IEnumerable<IStatisticsSetSummaryViewModel> GetOutOfPositionStatisticsSets()
        {
            yield return HeroXOrHeroBOutOfPositionStatisticsSet;
            yield return OppBIntoHeroOutOfPositionStatisticsSet;
            yield return HeroXOutOfPositionOppBStatisticsSet;
        }

        void InitializeStatisticsSetSummaryViewModels()
        {
            HeroXOrHeroBOutOfPositionStatisticsSet = new StatisticsSetSummaryViewModel();
            OppBIntoHeroOutOfPositionStatisticsSet = new StatisticsSetSummaryViewModel();
            HeroXOutOfPositionOppBStatisticsSet = new StatisticsSetSummaryViewModel();

            HeroXOrHeroBInPositionStatisticsSet = new StatisticsSetSummaryViewModel();
            OppBIntoHeroInPositionStatisticsSet = new StatisticsSetSummaryViewModel();
        }

        protected void RegisterEvents()
        {
            foreach (var statisticsSetsSummary in this)
            {
                statisticsSetsSummary.StatisticsSetSelectedEvent +=
                    statisticsSet => SelectedStatisticsSetEvent(statisticsSet, _street);
            }
        }

        #endregion
    }
}