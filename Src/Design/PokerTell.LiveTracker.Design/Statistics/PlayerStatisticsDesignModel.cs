namespace PokerTell.LiveTracker.Design.Statistics
{
    using PokerTell.Statistics.ViewModels;

    public class PlayerStatisticsDesignModel : PlayerStatisticsViewModel
    {
        public PlayerStatisticsDesignModel(int seatNumber)
            : base(
                new PreFlopStatisticsSetsDesignModel(), 
                new PostFlopStatisticsSetsDesignModel(seatNumber), 
                new PostFlopStatisticsSetsDesignModel(seatNumber), 
                new PostFlopStatisticsSetsDesignModel(seatNumber))
        {
        }
    }

    public static class PlayerStatisticsDesign
    {
        public static PlayerStatisticsViewModel Model = new PlayerStatisticsDesignModel(1);
    }
}