namespace PokerTell.Statistics.ViewModels
{
    using System;
    using System.Linq;

    using PokerTell.Infrastructure.Enumerations.PokerHand;
    using PokerTell.Infrastructure.Interfaces.Statistics;

    public class PlayerStatisticsViewModel : IPlayerStatisticsViewModel
    {
        #region Constructors and Destructors

        public PlayerStatisticsViewModel()
        {
            InitializeStatisticsSetsViewModels();

            RegisterEvents();
        }

        #endregion

        #region Events

        public event Action<string, IActionSequenceStatisticsSet, Streets> SelectedStatisticsSetEvent =
            delegate { };

        #endregion

        #region Properties

        public IPostFlopStatisticsSetsViewModel FlopStatisticsSets { get; protected set; }

        public string PlayerName { get; protected set; }

        public IPreFlopStatisticsSetsViewModel PreFlopStatisticsSets { get; protected set; }

        public IPostFlopStatisticsSetsViewModel RiverStatisticsSets { get; protected set; }

        public IPostFlopStatisticsSetsViewModel TurnStatisticsSets { get; protected set; }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return PlayerName;
        }

        #endregion

        #region Implemented Interfaces

        #region IPlayerStatisticsViewModel

        public IPlayerStatisticsViewModel UpdateWith(IPlayerStatistics playerStatistics)
        {
            PlayerName = playerStatistics.PlayerIdentity.Name;

            PreFlopStatisticsSets.UpdateWith(playerStatistics);
            FlopStatisticsSets.UpdateWith(playerStatistics);
            TurnStatisticsSets.UpdateWith(playerStatistics);
            RiverStatisticsSets.UpdateWith(playerStatistics);

            return this;
        }

        #endregion

        #endregion

        #region Methods

        void InitializeStatisticsSetsViewModels()
        {
            PreFlopStatisticsSets = new PreFlopStatisticsSetsViewModel();
            FlopStatisticsSets = new PostFlopStatisticsSetsViewModel(Streets.Flop);
            TurnStatisticsSets = new PostFlopStatisticsSetsViewModel(Streets.Turn);
            RiverStatisticsSets = new PostFlopStatisticsSetsViewModel(Streets.River);
        }

        protected void RegisterEvents()
        {
            PreFlopStatisticsSets.SelectedStatisticsSetEvent +=
                (statisticsSet, street) =>
                SelectedStatisticsSetEvent(PlayerName, statisticsSet, street);

            FlopStatisticsSets.SelectedStatisticsSetEvent +=
                (statisticsSet, street) =>
                 SelectedStatisticsSetEvent(PlayerName, statisticsSet, street);
            
            TurnStatisticsSets.SelectedStatisticsSetEvent +=
                (statisticsSet, street) =>
                SelectedStatisticsSetEvent(PlayerName, statisticsSet, street);
            
            RiverStatisticsSets.SelectedStatisticsSetEvent +=
                (statisticsSet, street) =>
                SelectedStatisticsSetEvent(PlayerName, statisticsSet, street);
        }

        #endregion
    }
}