namespace PokerTell.Statistics.ViewModels._Design
{
    using Infrastructure.Enumerations.PokerHand;

    public class PostFlopStatisticsSetsDesignModel : PostFlopStatisticsSetsViewModel
    {
        public PostFlopStatisticsSetsDesignModel(Streets street)
            : base(street)
        {
            HeroXOrHeroBOutOfPositionStatisticsSet =
                StatisticsSetSummaryDesignModel.GetHeroXOrHeroBSetSummaryDesignModel((int)street);
            OppBIntoHeroOutOfPositionStatisticsSet =
                StatisticsSetSummaryDesignModel.GetReactionStatisticsSetSummaryDesignModel((int)(12 + street), (int)(30 + street));
            HeroXOutOfPositionOppBStatisticsSet =
                StatisticsSetSummaryDesignModel.GetReactionStatisticsSetSummaryDesignModel(29 - (int)street, 17 - (int)street);

            HeroXOrHeroBInPositionStatisticsSet = StatisticsSetSummaryDesignModel.GetHeroXOrHeroBSetSummaryDesignModel((int)(1 + street));
            OppBIntoHeroInPositionStatisticsSet =
                StatisticsSetSummaryDesignModel.GetReactionStatisticsSetSummaryDesignModel(02 + (3 * (int)street), 04 + (3 * (int)street));

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