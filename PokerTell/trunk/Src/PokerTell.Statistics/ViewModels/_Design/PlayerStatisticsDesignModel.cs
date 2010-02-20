namespace PokerTell.Statistics.ViewModels._Design
{
    using Infrastructure.Enumerations.PokerHand;

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