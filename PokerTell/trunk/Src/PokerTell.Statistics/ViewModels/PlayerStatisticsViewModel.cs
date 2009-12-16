namespace PokerTell.Statistics.ViewModels
{
    using System;

    using Infrastructure.Enumerations.PokerHand;
    using Infrastructure.Interfaces.Statistics;

    public class PlayerStatisticsViewModel : IPlayerStatisticsViewModel
    {
        public IPostFlopStatisticsSetsViewModel FlopStatisticsSets { get; protected set; }

        public IPreFlopStatisticsSetsViewModel PreFlopStatisticsSets { get; protected set; }

        public IPostFlopStatisticsSetsViewModel RiverStatisticsSets { get; protected set; }

        public IPostFlopStatisticsSetsViewModel TurnStatisticsSets { get; protected set; }

        public IPlayerStatisticsViewModel UpdateWith(IPlayerStatistics playerStatistics)
        {
            PreFlopStatisticsSets.UpdateWith(playerStatistics);
            FlopStatisticsSets.UpdateWith(playerStatistics);
            TurnStatisticsSets.UpdateWith(playerStatistics);
            RiverStatisticsSets.UpdateWith(playerStatistics);

            return this;
        }

        public PlayerStatisticsViewModel()
        {
            PreFlopStatisticsSets = new PreFlopStatisticsSetsViewModel();
            FlopStatisticsSets = new PostFlopStatisticsSetsViewModel(Streets.Flop);
            TurnStatisticsSets = new PostFlopStatisticsSetsViewModel(Streets.Turn);
            RiverStatisticsSets = new PostFlopStatisticsSetsViewModel(Streets.River);
        }
    }
}