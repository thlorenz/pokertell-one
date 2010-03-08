namespace PokerTell.Statistics.ViewModels
{
    using System;

    using PokerTell.Infrastructure.Enumerations.PokerHand;
    using PokerTell.Infrastructure.Interfaces.Statistics;

    using Tools.WPF.ViewModels;

    public class PlayerStatisticsViewModel : NotifyPropertyChanged, IPlayerStatisticsViewModel
    {
        #region Constructors and Destructors

        public PlayerStatisticsViewModel()
        {
            InitializeStatisticsSetsViewModels();

            RegisterEvents();
        }

        #endregion

        #region Events

        public event Action<IActionSequenceStatisticsSet> SelectedStatisticsSetEvent =
            delegate { };

        #endregion

        #region Properties

        public IPostFlopStatisticsSetsViewModel FlopStatisticsSets { get; protected set; }

        public string PlayerName
        {
            get { return PlayerStatistics.PlayerIdentity.Name; }
        }

        public IPreFlopStatisticsSetsViewModel PreFlopStatisticsSets { get; protected set; }

        public IPostFlopStatisticsSetsViewModel RiverStatisticsSets { get; protected set; }

        public IPostFlopStatisticsSetsViewModel TurnStatisticsSets { get; protected set; }

        public IPlayerStatistics PlayerStatistics { get; protected set; }

        public IAnalyzablePokerPlayersFilter Filter
        {
            get { return PlayerStatistics.Filter; }
            set
            {
                PlayerStatistics.Filter = value;
                UpdateStatisticsSets();
            }
        }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return PlayerName;
        }

        #endregion

        #region Implemented Interfaces

        #region IPlayerStatisticsViewModel

        /// <summary>
        /// Invoked from TableStatisticsViewModel when it is updating its Players Collection
        /// </summary>
        /// <param name="playerStatistics">Updated PlayerStatistics</param>
        /// <returns></returns>
        public IPlayerStatisticsViewModel UpdateWith(IPlayerStatistics playerStatistics)
        {
            PlayerStatistics = playerStatistics;

            UpdateStatisticsSets();

            return this;
        }

        void UpdateStatisticsSets()
        {
            PreFlopStatisticsSets.UpdateWith(PlayerStatistics);
            FlopStatisticsSets.UpdateWith(PlayerStatistics);
            TurnStatisticsSets.UpdateWith(PlayerStatistics);
            RiverStatisticsSets.UpdateWith(PlayerStatistics);
        }

        #endregion

        #endregion

        #region Methods

        protected void RegisterEvents()
        {
            PreFlopStatisticsSets.SelectedStatisticsSetEvent += 
                statisticsSet => SelectedStatisticsSetEvent(statisticsSet);

            FlopStatisticsSets.SelectedStatisticsSetEvent +=
                statisticsSet => SelectedStatisticsSetEvent(statisticsSet);

            TurnStatisticsSets.SelectedStatisticsSetEvent +=
                statisticsSet => SelectedStatisticsSetEvent(statisticsSet);

            RiverStatisticsSets.SelectedStatisticsSetEvent +=
                statisticsSet => SelectedStatisticsSetEvent(statisticsSet);
        }

        void InitializeStatisticsSetsViewModels()
        {
            PreFlopStatisticsSets = new PreFlopStatisticsSetsViewModel();
            FlopStatisticsSets = new PostFlopStatisticsSetsViewModel(Streets.Flop);
            TurnStatisticsSets = new PostFlopStatisticsSetsViewModel(Streets.Turn);
            RiverStatisticsSets = new PostFlopStatisticsSetsViewModel(Streets.River);
        }

        #endregion
    }
}