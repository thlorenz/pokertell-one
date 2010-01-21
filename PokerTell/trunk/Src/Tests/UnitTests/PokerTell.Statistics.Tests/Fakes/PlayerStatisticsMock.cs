namespace PokerTell.Statistics.Tests.Fakes
{
    using System.Collections.Generic;

    using Infrastructure.Enumerations.PokerHand;
    using Infrastructure.Interfaces.PokerHand;
    using Infrastructure.Interfaces.Repository;
    using Infrastructure.Interfaces.Statistics;

    using Interfaces;

    using Microsoft.Practices.Composite.Events;

    using Moq;

    using Statistics.Detailed;

    internal class PlayerStatisticsMock : PlayerStatistics
    {
        readonly StubBuilder _stub = new StubBuilder();

        public PlayerStatisticsMock(IEventAggregator eventAggregator, IRepository repository)
            : base(eventAggregator, repository)
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

        protected override IActionSequenceStatisticsSet NewActionSequenceSetStatistics(
            IPercentagesCalculator percentagesCalculator,
            IEnumerable<IActionSequenceStatistic> statistics,
            string playerName,
            Streets street,
            ActionSequences actionSequence,
            bool inPosition)
        {
            return _stub.Out<IActionSequenceStatisticsSet>();
        }

        protected override IActionSequenceStatisticsSet NewHeroCheckOrBetSetStatistics(
            IPercentagesCalculator percentagesCalculator,
            IEnumerable<IActionSequenceStatistic> statistics,
            string playerName,
            Streets street,
            bool inPosition)
        {
            return _stub.Out<IActionSequenceStatisticsSet>();
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