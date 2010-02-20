namespace PokerTell.LiveTracker.Design.Statistics
{
    using Infrastructure.Enumerations.PokerHand;

    using PokerTell.Statistics.ViewModels;

    public class PlayerStatisticsDesignModel : PlayerStatisticsViewModel
    {
        public PlayerStatisticsDesignModel()
        {
            PreFlopStatisticsSets = new PreFlopStatisticsSetsDesignModel();
            FlopStatisticsSets = new PostFlopStatisticsSetsDesignModel(Streets.Flop);
            TurnStatisticsSets = new PostFlopStatisticsSetsDesignModel(Streets.Turn);
            RiverStatisticsSets = new PostFlopStatisticsSetsDesignModel(Streets.River);
        }
    }

    public static class PlayerStatisticsDesign
    {
        public static PlayerStatisticsViewModel Model = new PlayerStatisticsDesignModel();
    }
}