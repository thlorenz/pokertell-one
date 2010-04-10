namespace PokerTell.Statistics.Design.DetailedStatistics
{
    using Moq;

    using PokerTell.Statistics.ViewModels;

    using Tools.Interfaces;

    public class PlayerStatisticsDesignModel : PlayerStatisticsViewModel
    {
        public PlayerStatisticsDesignModel(int seatNumber)
            : base(new Mock<IDispatcher>().Object,
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