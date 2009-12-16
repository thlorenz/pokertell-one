namespace PokerTell.LiveTracker.IntegrationTests.DesignViewModels
{
    using PokerTell.Infrastructure.Enumerations.PokerHand;
    using PokerTell.Statistics.ViewModels;
    using PokerTell.StatisticsIntegrationTests.DesignViewModels;

    public class PlayerStatisticsDesignModel : PlayerStatisticsViewModel
    {
        #region Constructors and Destructors

        public PlayerStatisticsDesignModel()
        {
            PreFlopStatisticsSets = new PreFlopStatisticsSetsDesignModel();

            FlopStatisticsSets = new PostFlopStatisticsSetsDesignModel(Streets.Flop)
                {
                    TotalCountOutOfPositionSet = 2001, 
                    TotalCountInPositionSet = 1001
                };

            TurnStatisticsSets = new PostFlopStatisticsSetsDesignModel(Streets.Turn)
                {
                    TotalCountOutOfPositionSet = 502, 
                    TotalCountInPositionSet = 402
                };
            RiverStatisticsSets = new PostFlopStatisticsSetsDesignModel(Streets.River)
                {
                    TotalCountOutOfPositionSet = 203, 
                    TotalCountInPositionSet = 93
                };
        }

        #endregion
    }
}