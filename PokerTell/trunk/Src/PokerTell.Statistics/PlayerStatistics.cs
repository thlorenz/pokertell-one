namespace PokerTell.Statistics
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Infrastructure.Interfaces.Statistics;

    using Microsoft.Practices.Composite.Events;

    using PokerTell.Infrastructure;
    using PokerTell.Infrastructure.Enumerations.PokerHand;
    using PokerTell.Infrastructure.Events;
    using PokerTell.Infrastructure.Interfaces.PokerHand;
    using PokerTell.Infrastructure.Interfaces.Repository;
    using PokerTell.Statistics.Detailed;
    using PokerTell.Statistics.Interfaces;

    public class PlayerStatistics : IPlayerStatistics, IEnumerable<IActionSequenceStatisticsSet>
    {
        #region Constants and Fields

        protected IList<IAnalyzablePokerPlayer> _allAnalyzablePlayers;

        protected long _lastQueriedId;

        readonly IRepository _repository;

        IAnalyzablePokerPlayersFilter _analyzablePokerPlayerFilter;

        string _playerName;

        string _pokerSite;

        #endregion

        #region Constructors and Destructors

        public PlayerStatistics(IEventAggregator eventAggregator, IRepository repository)
        {
            eventAggregator
                .GetEvent<DatabaseInUseChangedEvent>()
                .Subscribe(dp => Reinitialize());

            _repository = repository;

            _allAnalyzablePlayers = new List<IAnalyzablePokerPlayer>();

            InitializeStatistics();
        }

        #endregion

        #region Properties

        public IActionSequenceStatisticsSet[] HeroXOrHeroBInPosition { get; protected set; }

        public IActionSequenceStatisticsSet[] HeroXOrHeroBOutOfPosition { get; protected set; }

        public IActionSequenceStatisticsSet[] HeroXOutOfPositionOppB { get; protected set; }

        public IActionSequenceStatisticsSet[] OppBIntoHeroInPosition { get; protected set; }

        public IActionSequenceStatisticsSet[] OppBIntoHeroOutOfPosition { get; protected set; }

        public IPlayerIdentity PlayerIdentity { get; protected set; }

        public IActionSequenceStatisticsSet PreFlopRaisedPot { get; protected set; }

        public IActionSequenceStatisticsSet PreFlopUnraisedPot { get; protected set; }

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

        public int TotalCountPreFlopRaisedPot
        {
            get { return PreFlopRaisedPot.TotalCounts.Sum(); }
        }

        public int TotalCountPreFlopUnraisedPot
        {
            get { return PreFlopUnraisedPot.TotalCounts.Sum(); }
        }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            var sb = new StringBuilder(string.Format("PlayerName: {0}, PokerSite: {1}, LastQueriedId: {2}\n", 
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

        #endregion

        #region Implemented Interfaces

        #region IEnumerable

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region IEnumerable<IActionSequenceStatisticsSet>

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
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

        #endregion

        #region IPlayerStatistics

        public IPlayerStatistics InitializePlayer(string playerName, string pokerSite)
        {
            _playerName = playerName;
            _pokerSite = pokerSite;

            return this;
        }

        public IPlayerStatistics SetFilter(IAnalyzablePokerPlayersFilter filter)
        {
            _analyzablePokerPlayerFilter = filter;
            FilterAnalyzablePlayersAndUpdateStatisticsSetsWithThem();
            return this;
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

        #endregion

        #endregion

        #region Methods

        protected virtual IEnumerable<IAnalyzablePokerPlayer> GetFilteredAnalyzablePlayers()
        {
            return _analyzablePokerPlayerFilter == null
                       ? _allAnalyzablePlayers
                       : _analyzablePokerPlayerFilter.Filter(_allAnalyzablePlayers);
        }

        protected virtual IActionSequenceStatisticsSet NewActionSequenceSetStatistics(
            IEnumerable<IActionSequenceStatistic> statistics, IPercentagesCalculator percentagesCalculator)
        {
            return new ActionSequenceStatisticsSet(statistics, percentagesCalculator);
        }

        protected virtual IActionSequenceStatisticsSet NewHeroCheckOrBetSetStatistics(
            IEnumerable<IActionSequenceStatistic> statistics, IPercentagesCalculator percentagesCalculator)
        {
            return new HeroCheckOrBetSetStatistics(statistics, percentagesCalculator);
        }

        protected virtual void UpdateStatisticsWith(IEnumerable<IAnalyzablePokerPlayer> filteredAnalyzablePlayers)
        {
            foreach (var statisticsSet in this)
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
            var newAnalyzablePlayers = repository.FindAnalyzablePlayersWith(PlayerIdentity.Id, _lastQueriedId);

            if (newAnalyzablePlayers.Count() != 0)
            {
                _lastQueriedId = (from player in newAnalyzablePlayers select player.Id).Max();
                foreach (var analyzablePlayer in newAnalyzablePlayers)
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
            var filteredAnalyzablePlayers = GetFilteredAnalyzablePlayers();
            UpdateStatisticsWith(filteredAnalyzablePlayers);
        }

        void InitializeHeroXOrBStatistics(Streets street, int betSizeIndexCount)
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

            HeroXOrHeroBOutOfPosition[(int)street] = NewHeroCheckOrBetSetStatistics(
                heroXOrHeroBOutOfPositionStatistics, new SeparateRowsPercentagesCalculator());
            HeroXOrHeroBInPosition[(int)street] = NewHeroCheckOrBetSetStatistics(heroXOrHeroBInPositionStatistics, 
                                                                                 new SeparateRowsPercentagesCalculator());
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
                NewActionSequenceSetStatistics(heroXOutOfPositionOppBStatistics, new AcrossRowsPercentagesCalculator());
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
                NewActionSequenceSetStatistics(oppBIntoHeroOutOfPositionStatistics, 
                                               new AcrossRowsPercentagesCalculator());
            OppBIntoHeroInPosition[(int)street] =
                NewActionSequenceSetStatistics(oppBIntoHeroInPositionStatistics, 
                                               new AcrossRowsPercentagesCalculator());
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

            PreFlopRaisedPot = NewActionSequenceSetStatistics(preFlopRaisedPotStatistics, 
                                                              new AcrossRowsPercentagesCalculator());
            PreFlopUnraisedPot = NewActionSequenceSetStatistics(preFlopUnraisedPotStatistics, 
                                                                new AcrossRowsPercentagesCalculator());
        }

        void InitializeStatistics()
        {
            var betSizeIndexCount = ApplicationProperties.BetSizeKeys.Length;

            InitializePreFlopStatistics();

            CreatePostFlopStatistics();

            for (Streets street = Streets.Flop; street <= Streets.River; street++)
            {
                InitializeHeroXOrBStatistics(street, betSizeIndexCount);

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

        #endregion
    }
}