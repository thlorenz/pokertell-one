namespace PokerTell.LiveTracker.Design.Statistics
{
    using PokerTell.Statistics.ViewModels;

    public class PostFlopStatisticsSetsDesignModel : PostFlopStatisticsSetsViewModel
    {
        public PostFlopStatisticsSetsDesignModel(int seatNumber)
        {
            HeroXOrHeroBInPositionStatisticsSet = StatisticsSetSummaryDesignModel.GetHeroXOrHeroBSetSummaryDesignModel(1 + seatNumber);
            OppBIntoHeroInPositionStatisticsSet =
                StatisticsSetSummaryDesignModel.GetReactionStatisticsSetSummaryDesignModel(02 + (3 * +seatNumber), 04 + seatNumber);

            TotalCountOutOfPosition = 2345;
            TotalCountInPosition = 1003;

            RegisterEvents();
        }

        public int TotalCountInPositionSet
        {
            set { TotalCountInPosition = value; }
        }

        public int TotalCountOutOfPositionSet
        {
            set { TotalCountOutOfPosition = value; }
        }
    }
}