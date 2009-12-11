namespace PokerTell.Statistics.Tests.Fakes
{
    using System.Collections.Generic;

    using Infrastructure.Interfaces.PokerHand;

    using Interfaces;

    using Microsoft.Practices.Composite.Events;

    using Moq;

    internal class PlayerStatisticsMock : PlayerStatistics
    {
        public PlayerStatisticsMock(IEventAggregator eventAggregator)
            : base(eventAggregator)
        {
        }

        public long LastQueriedId
        {
            get { return _lastQueriedId; }
            set { _lastQueriedId = value; }
        }

        public IList<IAnalyzablePokerPlayer> AnalyzablePlayers
        {
            get { return _allAnalyzablePlayers; }
            set { _allAnalyzablePlayers = value; }
        }

        public IPlayerIdentity PlayerIdentitySet
        {
            set { PlayerIdentity = value; }
        }

        protected override IActionSequenceSetStatistics NewActionSequenceSetStatistics(IEnumerable<IActionSequenceStatistic> statistics, IPercentagesCalculator percentagesCalculator)
        {
            return new StubBuilder().Out<IActionSequenceSetStatistics>();
        }

        protected override IActionSequenceSetStatistics NewHeroCheckOrBetSetStatistics(IEnumerable<IActionSequenceStatistic> statistics, IPercentagesCalculator percentagesCalculator)
        {
            return new StubBuilder().Out<IActionSequenceSetStatistics>();
        }

        protected override void UpdateStatisticsWith(IEnumerable<IAnalyzablePokerPlayer> filteredAnalyzablePlayers)
        {
            base.UpdateStatisticsWith(filteredAnalyzablePlayers);
            StatisticsWereUpdated = true;
        }

        public IEnumerable<IAnalyzablePokerPlayer> GetFilteredAnalyzablePlayersInvoke()
        {
            return base.GetFilteredAnalyzablePlayers();
        }

        protected override IEnumerable<IAnalyzablePokerPlayer> GetFilteredAnalyzablePlayers()
        {
            MatchingPlayersWereFiltered = true;
            return AnalyzablePlayers;
        }

        internal bool StatisticsWereUpdated { get; private set; }

        internal bool MatchingPlayersWereFiltered { get; private set; }
    }
}