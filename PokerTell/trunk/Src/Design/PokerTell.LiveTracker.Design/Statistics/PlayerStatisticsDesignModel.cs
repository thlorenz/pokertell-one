namespace PokerTell.LiveTracker.Design.Statistics
{
    using Infrastructure.Enumerations.PokerHand;

    using PokerTell.Statistics.ViewModels;

    public class PlayerStatisticsDesignModel : PlayerStatisticsViewModel
    {
        public PlayerStatisticsDesignModel(int seatNumber)
        {
            PreFlopStatisticsSets = new PreFlopStatisticsSetsDesignModel();
            FlopStatisticsSets = new PostFlopStatisticsSetsDesignModel(Streets.Flop, seatNumber);
            TurnStatisticsSets = new PostFlopStatisticsSetsDesignModel(Streets.Turn, seatNumber);
            RiverStatisticsSets = new PostFlopStatisticsSetsDesignModel(Streets.River, seatNumber);
        }
    }

    public static class PlayerStatisticsDesign
    {
        public static PlayerStatisticsViewModel Model = new PlayerStatisticsDesignModel(1);
    }
}