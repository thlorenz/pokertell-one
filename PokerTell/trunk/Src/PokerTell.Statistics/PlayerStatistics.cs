namespace PokerTell.Statistics
{
    using System;
    using System.Collections.Generic;

    using Microsoft.Practices.Composite.Events;

    using PokerTell.Infrastructure;
    using PokerTell.Infrastructure.Enumerations.PokerHand;
    using PokerTell.Infrastructure.Events;
    using PokerTell.Infrastructure.Interfaces.PokerHand;
    using PokerTell.Infrastructure.Interfaces.Repository;
    using PokerTell.Statistics.Detailed;
    using PokerTell.Statistics.Interfaces;

    public class PlayerStatistics : IPlayerStatistics
    {
        #region Constants and Fields

        protected IList<IAnalyzablePokerPlayer> _allAnalyzablePlayers;

        protected long _lastQueriedId;

        string _playerName;

        string _pokerSite;

        #endregion

        #region Constructors and Destructors

        public PlayerStatistics(IEventAggregator eventAggregator)
        {
            eventAggregator
                .GetEvent<DatabaseInUseChangedEvent>()
                .Subscribe(dp => Reinitialize());

            _allAnalyzablePlayers = new List<IAnalyzablePokerPlayer>();

            InitializeStatistics();
        }

        #endregion

        #region Properties

        public IActionSequenceSetStatistics[] HeroXOrHeroBInPosition { get; protected set; }

        public IActionSequenceSetStatistics[] HeroXOrHeroBOutOfPosition { get; protected set; }

        public IActionSequenceSetStatistics[] HeroXOutOfPositionOppB { get; protected set; }

        public IActionSequenceSetStatistics[] OppBIntoHeroInPosition { get; protected set; }

        public IActionSequenceSetStatistics[] OppBIntoHeroOutOfPosition { get; protected set; }

        public IPlayerIdentity PlayerIdentity { get; protected set; }

        public IActionSequenceSetStatistics PreFlopRaisedPot { get; protected set; }

        public IActionSequenceSetStatistics PreFlopUnraisedPot { get; protected set; }

        public int[] TotalCountsOutInPosition { get; set; }

        public int[] TotalCountsOutOfPosition { get; set; }

        #endregion

        #region Public Methods

        public IPlayerStatistics InitializePlayer(string playerName, string pokerSite)
        {
            _playerName = playerName;
            _pokerSite = pokerSite;

            return this;
        }

        #endregion

        #region Implemented Interfaces

        #region IPlayerStatistics

        public IPlayerStatistics SetFilter()
        {
            throw new NotImplementedException();
        }

        public IPlayerStatistics UpdateFrom(IRepository repository)
        {
            ExtractPlayerIdentityIfItIsNullFrom(repository);

            if (PlayerIdentity != null)
            {
                ExtractAnalyzablePlayersFrom(repository);
                var filteredAnalyzablePlayers = _allAnalyzablePlayers;

                UpdateStatisticsWith(filteredAnalyzablePlayers);
            }

            return this;
        }

        #endregion

        #endregion

        #region Methods

        protected virtual IActionSequenceSetStatistics NewActionSequenceSetStatistics(
            IEnumerable<IActionSequenceStatistic> statistics, IPercentagesCalculator percentagesCalculator)
        {
            return new ActionSequenceSetStatistics(statistics, percentagesCalculator);
        }

        protected virtual void UpdateStatisticsWith(IEnumerable<IAnalyzablePokerPlayer> filteredAnalyzablePlayers)
        {
            foreach (var statisticsSet in AllStatisticsSets())
            {
                statisticsSet.UpdateWith(filteredAnalyzablePlayers);
            }
        }

        IEnumerable<IActionSequenceSetStatistics> AllStatisticsSets()
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

        void CreatePostFlopStatistics()
        {
            HeroXOrHeroBInPosition = new IActionSequenceSetStatistics[(int)(Streets.River + 1)];
            HeroXOrHeroBOutOfPosition = new IActionSequenceSetStatistics[(int)(Streets.River + 1)];
            HeroXOutOfPositionOppB = new IActionSequenceSetStatistics[(int)(Streets.River + 1)];
            OppBIntoHeroInPosition = new IActionSequenceSetStatistics[(int)(Streets.River + 1)];
            OppBIntoHeroOutOfPosition = new IActionSequenceSetStatistics[(int)(Streets.River + 1)];
        }

        void ExtractAnalyzablePlayersFrom(IRepository repository)
        {
            var newAnalyzablePlayers = repository.FindAnalyzablePlayersWith(PlayerIdentity.Id, _lastQueriedId);
            foreach (var analyzablePlayer in newAnalyzablePlayers)
            {
                _allAnalyzablePlayers.Add(analyzablePlayer);
                if (analyzablePlayer.Id > _lastQueriedId)
                {
                    _lastQueriedId = analyzablePlayer.Id;
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

        void InitializeHeroXOrBStatistics(Streets street, int betSizeIndexCount)
        {
            var heroXOrHeroBOutOfPositionStatistics = new List<IActionSequenceStatistic>
                {
                    new PostFlopActionSequenceWithoutIndexesStatistic(ActionSequences.HeroX, street, false), 
                    new PostFlopActionSequenceStatistic(ActionSequences.HeroB, street, false, betSizeIndexCount)
                };
            var heroXOrHeroBInPositionStatistics = new List<IActionSequenceStatistic>
                {
                    new PostFlopActionSequenceWithoutIndexesStatistic(ActionSequences.HeroX, street, true), 
                    new PostFlopActionSequenceStatistic(ActionSequences.HeroB, street, true, betSizeIndexCount)
                };

            HeroXOrHeroBOutOfPosition[(int)street] = NewActionSequenceSetStatistics(
                heroXOrHeroBOutOfPositionStatistics, new SeparateRowsPercentagesCalculator());
            HeroXOrHeroBInPosition[(int)street] = NewActionSequenceSetStatistics(heroXOrHeroBInPositionStatistics, 
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
        }

        #endregion
    }
}