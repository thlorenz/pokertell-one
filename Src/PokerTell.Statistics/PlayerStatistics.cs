namespace PokerTell.Statistics
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Microsoft.Practices.Composite.Events;

    using PokerTell.Infrastructure;
    using PokerTell.Infrastructure.Enumerations.PokerHand;
    using PokerTell.Infrastructure.Events;
    using PokerTell.Infrastructure.Interfaces.PokerHand;
    using PokerTell.Infrastructure.Interfaces.Repository;
    using PokerTell.Infrastructure.Interfaces.Statistics;
    using PokerTell.Statistics.Detailed;
    using PokerTell.Statistics.Filters;
    using PokerTell.Statistics.Interfaces;
    using PokerTell.Statistics.Utilities;

    public class PlayerStatistics : IPlayerStatistics, IEnumerable<IActionSequenceStatisticsSet>
    {
        protected IList<IAnalyzablePokerPlayer> _allAnalyzablePlayers;

        protected long _lastQueriedId;

        readonly IRepository _repository;

        IAnalyzablePokerPlayersFilter _filter;

        string _playerName;

        string _pokerSite;

        public PlayerStatistics(IEventAggregator eventAggregator, IRepository repository)
        {
            eventAggregator
                .GetEvent<DatabaseInUseChangedEvent>()
                .Subscribe(dp => Reinitialize());

            _repository = repository;

            _allAnalyzablePlayers = new List<IAnalyzablePokerPlayer>();

            _filter = AnalyzablePokerPlayersFilter.InactiveFilter;
        }

        public IAnalyzablePokerPlayersFilter Filter
        {
            get { return _filter; }
            set
            {
                _filter = value;
                FilterAnalyzablePlayersAndUpdateStatisticsSetsWithThem();
            }
        }

        public IActionSequenceStatisticsSet[] HeroXOrHeroBInPosition { get; protected set; }

        public IActionSequenceStatisticsSet[] HeroXOrHeroBOutOfPosition { get; protected set; }

        public IActionSequenceStatisticsSet[] HeroXOutOfPositionOppB { get; protected set; }

        public IActionSequenceStatisticsSet[] OppBIntoHeroInPosition { get; protected set; }

        public IActionSequenceStatisticsSet[] OppBIntoHeroOutOfPosition { get; protected set; }

        public IPlayerIdentity PlayerIdentity { get; protected set; }

        public IActionSequenceStatisticsSet PreFlopRaisedPot { get; protected set; }

        public IActionSequenceStatisticsSet PreFlopUnraisedPot { get; protected set; }

        public int TotalCountPreFlopRaisedPot
        {
            get { return PreFlopRaisedPot.TotalCounts.Sum(); }
        }

        public int TotalCountPreFlopUnraisedPot
        {
            get { return PreFlopUnraisedPot.TotalCounts.Sum(); }
        }

        public override string ToString()
        {
            var sb = new StringBuilder(
                string.Format(
                    "PlayerName: {0}, PokerSite: {1}, LastQueriedId: {2}\n", 
                    _playerName, 
                    _pokerSite, 
                    _lastQueriedId));
            this.ToList().ForEach(statisticsSet => sb.AppendLine(statisticsSet.ToString()));

            sb.AppendLine("Total Counts: ")
                .AppendLine("Preflop: ")
                .AppendFormat("UnraisedPot: {0}   ", TotalCountPreFlopUnraisedPot)
                .AppendFormat("RaisedPot: {0}", TotalCountPreFlopRaisedPot);

            sb.AppendLine("\nPostFlop:")
                .Append("Out of Position: ");
            for (Streets street = Streets.Flop; street <= Streets.River; street++)
            {
                sb.Append(TotalCountsOutOfPosition(street) + ", ");
            }

            sb.AppendLine()
                .Append("In Position: ");
            for (Streets street = Streets.Flop; street <= Streets.River; street++)
            {
                sb.Append(TotalCountsInPosition(street) + ", ");
            }

            return sb.ToString();
        }

        /// <summary>
        ///   Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        ///   An
        ///   <see cref="T:System.Collections.IEnumerator" />
        ///   object that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        ///   Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        ///   A
        ///   <see cref="T:System.Collections.Generic.IEnumerator`1" />
        ///   that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public IEnumerator<IActionSequenceStatisticsSet> GetEnumerator()
        {
            yield return PreFlopUnraisedPot;
            yield return PreFlopRaisedPot;

            for (Streets street = Streets.Flop; street <= Streets.River; street++)
            {
                yield return HeroXOrHeroBOutOfPosition[(int)street];
                yield return HeroXOrHeroBInPosition[(int)street];

                yield return OppBIntoHeroOutOfPosition[(int)street];
                yield return OppBIntoHeroInPosition[(int)street];

                yield return HeroXOutOfPositionOppB[(int)street];
            }
        }

        public IPlayerStatistics InitializePlayer(string playerName, string pokerSite)
        {
            _playerName = playerName;
            _pokerSite = pokerSite;

            InitializeStatistics();

            return this;
        }

        public int TotalCountsInPosition(Streets street)
        {
            return HeroXOrHeroBInPosition[(int)street].TotalCounts.Sum() +
                   OppBIntoHeroInPosition[(int)street].TotalCounts.Sum();
        }

        public int TotalCountsOutOfPosition(Streets street)
        {
            return HeroXOrHeroBOutOfPosition[(int)street].TotalCounts.Sum() +
                   OppBIntoHeroOutOfPosition[(int)street].TotalCounts.Sum();
        }

        public IPlayerStatistics UpdateStatistics()
        {
            ExtractPlayerIdentityIfItIsNullFrom(_repository);

            if (PlayerIdentity != null)
            {
                ExtractAnalyzablePlayersAndUpdateLastQueriedIdFrom(_repository);

                FilterAnalyzablePlayersAndUpdateStatisticsSetsWithThem();
            }

            return this;
        }

        protected virtual IEnumerable<IAnalyzablePokerPlayer> GetFilteredAnalyzablePlayers()
        {
            return Filter == null
                       ? _allAnalyzablePlayers
                       : Filter.Filter(_allAnalyzablePlayers);
        }

        protected virtual IActionSequenceStatisticsSet NewActionSequenceSetStatistics(
            IPercentagesCalculator percentagesCalculator, 
            IEnumerable<IActionSequenceStatistic> statistics, 
            string playerName, 
            Streets street, 
            ActionSequences actionSequence, 
            bool inPosition)
        {
            return new ActionSequenceStatisticsSet(
                percentagesCalculator, statistics, playerName, street, actionSequence, inPosition);
        }

        protected virtual IActionSequenceStatisticsSet NewHeroCheckOrBetSetStatistics(
            IPercentagesCalculator percentagesCalculator, 
            IEnumerable<IActionSequenceStatistic> statistics, 
            string playerName, 
            Streets street, 
            bool inPosition)
        {
            return new HeroCheckOrBetSetStatistics(percentagesCalculator, statistics, playerName, street, inPosition);
        }

        protected virtual IActionSequenceStatisticsSet NewPreflopActionSequenceSetStatistics(
            IPercentagesCalculator percentagesCalculator, 
            IEnumerable<IActionSequenceStatistic> actionSequenceStatistics, 
            string playerName, 
            bool raisedPot)
        {
            return new ActionSequenceStatisticsSet(
                percentagesCalculator, 
                actionSequenceStatistics, 
                playerName, 
                raisedPot ? ActionSequences.PreFlopFrontRaise : ActionSequences.PreFlopNoFrontRaise, 
                raisedPot);
        }

        protected virtual void UpdateStatisticsWith(IEnumerable<IAnalyzablePokerPlayer> filteredAnalyzablePlayers)
        {
            foreach (IActionSequenceStatisticsSet statisticsSet in this)
            {
                statisticsSet.UpdateWith(filteredAnalyzablePlayers);
            }
        }

        void CreatePostFlopStatistics()
        {
            HeroXOrHeroBInPosition = new IActionSequenceStatisticsSet[(int)(Streets.River + 1)];
            HeroXOrHeroBOutOfPosition = new IActionSequenceStatisticsSet[(int)(Streets.River + 1)];
            HeroXOutOfPositionOppB = new IActionSequenceStatisticsSet[(int)(Streets.River + 1)];
            OppBIntoHeroInPosition = new IActionSequenceStatisticsSet[(int)(Streets.River + 1)];
            OppBIntoHeroOutOfPosition = new IActionSequenceStatisticsSet[(int)(Streets.River + 1)];
        }

        void ExtractAnalyzablePlayersAndUpdateLastQueriedIdFrom(IRepository repository)
        {
            IEnumerable<IAnalyzablePokerPlayer> newAnalyzablePlayers =
                repository.FindAnalyzablePlayersWith(PlayerIdentity.Id, _lastQueriedId);

            if (newAnalyzablePlayers.Count() != 0)
            {
                _lastQueriedId = (from player in newAnalyzablePlayers select player.Id).Max();
                foreach (IAnalyzablePokerPlayer analyzablePlayer in newAnalyzablePlayers)
                {
                    _allAnalyzablePlayers.Add(analyzablePlayer);
                }
            }
        }

        void ExtractPlayerIdentityIfItIsNullFrom(IRepository repository)
        {
            if (PlayerIdentity == null)
            {
                PlayerIdentity = repository.FindPlayerIdentityFor(_playerName, _pokerSite);
            }
        }

        void FilterAnalyzablePlayersAndUpdateStatisticsSetsWithThem()
        {
            IEnumerable<IAnalyzablePokerPlayer> filteredAnalyzablePlayers = GetFilteredAnalyzablePlayers();
            UpdateStatisticsWith(filteredAnalyzablePlayers);
        }

        void InitializeHeroXorBStatistics(Streets street, int betSizeIndexCount)
        {
            var heroXOrHeroBOutOfPositionStatistics = new List<IActionSequenceStatistic>
                {
                    new PostFlopHeroXStatistic(street, false), 
                    new PostFlopActionSequenceStatistic(ActionSequences.HeroB, street, false, betSizeIndexCount)
                };

            var heroXOrHeroBInPositionStatistics = new List<IActionSequenceStatistic>
                {
                    new PostFlopHeroXStatistic(street, true), 
                    new PostFlopActionSequenceStatistic(ActionSequences.HeroB, street, true, betSizeIndexCount)
                };

            HeroXOrHeroBOutOfPosition[(int)street] = 
                NewHeroCheckOrBetSetStatistics(new SeparateRowsPercentagesCalculator(), heroXOrHeroBOutOfPositionStatistics, _playerName, street, false);

            HeroXOrHeroBInPosition[(int)street] =
                NewHeroCheckOrBetSetStatistics(new SeparateRowsPercentagesCalculator(), heroXOrHeroBInPositionStatistics, _playerName, street, true);
        }

        void InitializeHeroXOutOfPositionOppBStatistics(Streets street, int betSizeIndexCount)
        {
            var heroXOutOfPositionOppBStatistics = new List<IActionSequenceStatistic>
                {
                    new PostFlopActionSequenceStatistic(ActionSequences.HeroXOppBHeroF, street, false, betSizeIndexCount), 
                    new PostFlopActionSequenceStatistic(ActionSequences.HeroXOppBHeroC, street, false, betSizeIndexCount), 
                    new PostFlopActionSequenceStatistic(ActionSequences.HeroXOppBHeroR, street, false, betSizeIndexCount)
                };
            HeroXOutOfPositionOppB[(int)street] =
                NewActionSequenceSetStatistics(new AcrossRowsPercentagesCalculator(), heroXOutOfPositionOppBStatistics, _playerName, street, ActionSequences.HeroXOppB, false);
        }

        void InitializeOppBIntoHeroStatistics(Streets street, int betSizeIndexCount)
        {
            var oppBIntoHeroOutOfPositionStatistics = new List<IActionSequenceStatistic>
                {
                    new PostFlopActionSequenceStatistic(ActionSequences.OppBHeroF, street, false, betSizeIndexCount), 
                    new PostFlopActionSequenceStatistic(ActionSequences.OppBHeroC, street, false, betSizeIndexCount), 
                    new PostFlopActionSequenceStatistic(ActionSequences.OppBHeroR, street, false, betSizeIndexCount)
                };
            var oppBIntoHeroInPositionStatistics = new List<IActionSequenceStatistic>
                {
                    new PostFlopActionSequenceStatistic(ActionSequences.OppBHeroF, street, true, betSizeIndexCount), 
                    new PostFlopActionSequenceStatistic(ActionSequences.OppBHeroC, street, true, betSizeIndexCount), 
                    new PostFlopActionSequenceStatistic(ActionSequences.OppBHeroR, street, true, betSizeIndexCount)
                };
            OppBIntoHeroOutOfPosition[(int)street] =
                NewActionSequenceSetStatistics(new AcrossRowsPercentagesCalculator(), oppBIntoHeroOutOfPositionStatistics, _playerName, street, ActionSequences.OppB, false);

            OppBIntoHeroInPosition[(int)street] =
                NewActionSequenceSetStatistics(new AcrossRowsPercentagesCalculator(), oppBIntoHeroInPositionStatistics, _playerName, street, ActionSequences.OppB, true);
        }

        void InitializePreFlopStatistics()
        {
            var preFlopUnraisedPotStatistics = new List<IActionSequenceStatistic>
                {
                    new PreFlopActionSequenceStatistic(ActionSequences.HeroF), 
                    new PreFlopActionSequenceStatistic(ActionSequences.HeroC), 
                    new PreFlopActionSequenceStatistic(ActionSequences.HeroR)
                };

            var preFlopRaisedPotStatistics = new List<IActionSequenceStatistic>
                {
                    new PreFlopActionSequenceStatistic(ActionSequences.OppRHeroF), 
                    new PreFlopActionSequenceStatistic(ActionSequences.OppRHeroC), 
                    new PreFlopActionSequenceStatistic(ActionSequences.OppRHeroR)
                };

            PreFlopRaisedPot =
                NewPreflopActionSequenceSetStatistics(new AcrossRowsPercentagesCalculator(), preFlopRaisedPotStatistics, _playerName, true);

            PreFlopUnraisedPot =
                NewPreflopActionSequenceSetStatistics(new AcrossRowsPercentagesCalculator(), preFlopUnraisedPotStatistics, _playerName, false);
        }

        void InitializeStatistics()
        {
            int betSizeIndexCount = ApplicationProperties.BetSizeKeys.Length;

            InitializePreFlopStatistics();

            CreatePostFlopStatistics();

            for (Streets street = Streets.Flop; street <= Streets.River; street++)
            {
                InitializeHeroXorBStatistics(street, betSizeIndexCount);

                InitializeOppBIntoHeroStatistics(street, betSizeIndexCount);

                InitializeHeroXOutOfPositionOppBStatistics(street, betSizeIndexCount);
            }
        }

        void Reinitialize()
        {
            _lastQueriedId = 0;
            _allAnalyzablePlayers = new List<IAnalyzablePokerPlayer>();
            PlayerIdentity = null;
        }
    }
}