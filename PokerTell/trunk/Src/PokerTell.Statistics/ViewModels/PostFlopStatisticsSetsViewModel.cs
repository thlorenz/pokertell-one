namespace PokerTell.Statistics.ViewModels
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    using Infrastructure.Enumerations.PokerHand;

    using PokerTell.Statistics.Interfaces;

    using System.Linq;

    public class PostFlopStatisticsSetsViewModel : IPostFlopStatisticsSetsViewModel
    {
        readonly Streets _street;

        public IStatisticsSetSummaryViewModel HeroXOrHeroBOutOfPositionStatisticsSet { get; protected set; }

        public IStatisticsSetSummaryViewModel HeroXOrHeroBInPositionStatisticsSet { get; protected set; }

        public IStatisticsSetSummaryViewModel OppBIntoHeroOutOfPositionStatisticsSet { get; protected set; }

        public IStatisticsSetSummaryViewModel OppBIntoHeroInPositionStatisticsSet { get; protected set; }

        public IStatisticsSetSummaryViewModel HeroXOutOfPositionOppBStatisticsSet { get; protected set; }

        public int TotalCountOutOfPosition { get; protected set; }

        public int TotalCountInPosition { get; protected set; }

        public PostFlopStatisticsSetsViewModel(Streets street)
        {
            _street = street;
        }

        public IPostFlopStatisticsSetsViewModel UpdateWith(IPlayerStatistics playerStatistics)
        {
            HeroXOrHeroBOutOfPositionStatisticsSet.UpdateWith(playerStatistics.HeroXOrHeroBOutOfPosition[(int)_street]);
            OppBIntoHeroOutOfPositionStatisticsSet.UpdateWith(playerStatistics.OppBIntoHeroOutOfPosition[(int)_street]);
            HeroXOrHeroBOutOfPositionStatisticsSet.UpdateWith(playerStatistics.HeroXOrHeroBOutOfPosition[(int)_street]);

            HeroXOrHeroBInPositionStatisticsSet.UpdateWith(playerStatistics.HeroXOrHeroBInPosition[(int)_street]);
            OppBIntoHeroInPositionStatisticsSet.UpdateWith(playerStatistics.OppBIntoHeroInPosition[(int)_street]);

            return this;
        }

        public IEnumerator<IStatisticsSetSummaryViewModel> GetEnumerator()
        {
            return GetAllStatisticsSets().GetEnumerator();
        }

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

        IEnumerable<IStatisticsSetSummaryViewModel> GetOutOfPositionStatisticsSets()
        {
            yield return HeroXOrHeroBOutOfPositionStatisticsSet;
            yield return OppBIntoHeroOutOfPositionStatisticsSet;
            yield return HeroXOutOfPositionOppBStatisticsSet;
        }

        IEnumerable<IStatisticsSetSummaryViewModel> GetInPositionStatisticsSets()
        {
            yield return HeroXOrHeroBInPositionStatisticsSet;
            yield return OppBIntoHeroInPositionStatisticsSet;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}