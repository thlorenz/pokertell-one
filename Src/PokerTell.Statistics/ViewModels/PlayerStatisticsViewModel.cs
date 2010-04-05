namespace PokerTell.Statistics.ViewModels
{
    using System;
    using System.Windows.Threading;

    using PokerTell.Infrastructure.Enumerations.PokerHand;
    using PokerTell.Infrastructure.Interfaces.Statistics;

    using Tools.Interfaces;
    using Tools.WPF.ViewModels;

    public class PlayerStatisticsViewModel : NotifyPropertyChanged, IPlayerStatisticsViewModel
    {
        readonly IDispatcher _dispatcher;

        public PlayerStatisticsViewModel(
            IDispatcher dispatcher, 
            IPreFlopStatisticsSetsViewModel preFlopStatisticsSetsViewModel, 
            IPostFlopStatisticsSetsViewModel flopStatisticsSetsViewModel, 
            IPostFlopStatisticsSetsViewModel turnStatisticsSetsViewModel, 
            IPostFlopStatisticsSetsViewModel riverStatisticsSetsViewModel)
        {
            _dispatcher = dispatcher;
            PreFlopStatisticsSets = preFlopStatisticsSetsViewModel;

            FlopStatisticsSets = flopStatisticsSetsViewModel;
            TurnStatisticsSets = turnStatisticsSetsViewModel;
            RiverStatisticsSets = riverStatisticsSetsViewModel;

            InitializeStatisticsSets();

            RegisterEvents();
        }

        public event Action<IPlayerStatisticsViewModel> BrowseAllMyHandsRequested = delegate { };

        public event Action<IActionSequenceStatisticsSet> SelectedStatisticsSetEvent = delegate { };

        public IAnalyzablePokerPlayersFilter Filter
        {
            get { return PlayerStatistics.Filter; }
            set { PlayerStatistics.Filter = value; }
        }

        public IPostFlopStatisticsSetsViewModel FlopStatisticsSets { get; protected set; }

        public string PlayerName
        {
            get { return PlayerStatistics.PlayerIdentity.Name; }
        }

        public IPlayerStatistics PlayerStatistics { get; protected set; }

        public IPreFlopStatisticsSetsViewModel PreFlopStatisticsSets { get; protected set; }

        public IPostFlopStatisticsSetsViewModel RiverStatisticsSets { get; protected set; }

        public IPostFlopStatisticsSetsViewModel TurnStatisticsSets { get; protected set; }

        public override string ToString()
        {
            return PlayerName;
        }

        /// <summary>
        /// Invoked from TableStatisticsViewModel when it is updating its Players Collection
        /// </summary>
        /// <param name="playerStatistics">Updated PlayerStatistics</param>
        /// <returns></returns>
        public IPlayerStatisticsViewModel UpdateWith(IPlayerStatistics playerStatistics)
        {
            PlayerStatistics = playerStatistics;

            // Need to dispatch here since the filter change could also be done on a background thread and we will
            // affect the gui when we update the underlying viewmodels
            PlayerStatistics.FilterChanged += () => _dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.DataBind, new Action(UpdateStatisticsSets));

            UpdateStatisticsSets();

            return this;
        }

        protected void RegisterEvents()
        {
            PreFlopStatisticsSets.SelectedStatisticsSetEvent +=
                statisticsSet => SelectedStatisticsSetEvent(statisticsSet);

            PreFlopStatisticsSets.BrowseAllMyHandsRequested +=
                () => BrowseAllMyHandsRequested(this);

            FlopStatisticsSets.SelectedStatisticsSetEvent +=
                statisticsSet => SelectedStatisticsSetEvent(statisticsSet);

            TurnStatisticsSets.SelectedStatisticsSetEvent +=
                statisticsSet => SelectedStatisticsSetEvent(statisticsSet);

            RiverStatisticsSets.SelectedStatisticsSetEvent +=
                statisticsSet => SelectedStatisticsSetEvent(statisticsSet);
        }

        void InitializeStatisticsSets()
        {
            FlopStatisticsSets.InitializeWith(Streets.Flop);
            TurnStatisticsSets.InitializeWith(Streets.Turn);
            RiverStatisticsSets.InitializeWith(Streets.River);
        }

        void UpdateStatisticsSets()
        {
            PreFlopStatisticsSets.UpdateWith(PlayerStatistics);
            FlopStatisticsSets.UpdateWith(PlayerStatistics);
            TurnStatisticsSets.UpdateWith(PlayerStatistics);
            RiverStatisticsSets.UpdateWith(PlayerStatistics);
        }
    }
}