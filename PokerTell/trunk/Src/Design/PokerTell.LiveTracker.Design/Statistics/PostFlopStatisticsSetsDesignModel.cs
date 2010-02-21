namespace PokerTell.LiveTracker.Design.Statistics
{
    using PokerTell.Infrastructure.Enumerations.PokerHand;
    using PokerTell.Statistics.ViewModels;

    public class PostFlopStatisticsSetsDesignModel : PostFlopStatisticsSetsViewModel
    {
        public PostFlopStatisticsSetsDesignModel(Streets street, int seatNumber)
            : base(street)
        {
            HeroXOrHeroBOutOfPositionStatisticsSet =
                StatisticsSetSummaryDesignModel.GetHeroXOrHeroBSetSummaryDesignModel((int)street + seatNumber);
            OppBIntoHeroOutOfPositionStatisticsSet =
                StatisticsSetSummaryDesignModel.GetReactionStatisticsSetSummaryDesignModel((int)(12 + street + seatNumber), 
                                                                                           (int)(30 + street + seatNumber));
            HeroXOutOfPositionOppBStatisticsSet =
                StatisticsSetSummaryDesignModel.GetReactionStatisticsSetSummaryDesignModel(29 - (int)street + seatNumber, 
                                                                                           17 - (int)street + seatNumber);

            HeroXOrHeroBInPositionStatisticsSet = StatisticsSetSummaryDesignModel.GetHeroXOrHeroBSetSummaryDesignModel((int)(1 + street + seatNumber));
            OppBIntoHeroInPositionStatisticsSet =
                StatisticsSetSummaryDesignModel.GetReactionStatisticsSetSummaryDesignModel(02 + (3 * (int)street + seatNumber), 
                                                                                           04 + (3 * (int)street + seatNumber));

            TotalCountOutOfPosition = 2345;
            TotalCountInPosition = 1003;

            RegisterEvents();
        }

        public int TotalCountOutOfPositionSet
        {
            set { TotalCountOutOfPosition = value; }
        }

        public int TotalCountInPositionSet
        {
            set { TotalCountInPosition = value; }
        }
    }
}