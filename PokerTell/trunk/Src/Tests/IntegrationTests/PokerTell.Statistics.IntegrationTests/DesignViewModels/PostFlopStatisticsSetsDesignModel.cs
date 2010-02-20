namespace PokerTell.Statistics.IntegrationTests.DesignViewModels
{
    using Infrastructure.Enumerations.PokerHand;

    using ViewModels;

    public class PostFlopStatisticsSetsDesignModel : PostFlopStatisticsSetsViewModel
    {
        public PostFlopStatisticsSetsDesignModel(Streets street)
            : base(street)
        {
            HeroXOrHeroBOutOfPositionStatisticsSet =
                StatisticsSetSummaryDesignModel.GetHeroXOrHeroBSetSummaryDesignModel();
            OppBIntoHeroOutOfPositionStatisticsSet =
                StatisticsSetSummaryDesignModel.GetReactionStatisticsSetSummaryDesignModel();
            HeroXOutOfPositionOppBStatisticsSet =
                StatisticsSetSummaryDesignModel.GetReactionStatisticsSetSummaryDesignModel();

            HeroXOrHeroBInPositionStatisticsSet = StatisticsSetSummaryDesignModel.GetHeroXOrHeroBSetSummaryDesignModel();
            OppBIntoHeroInPositionStatisticsSet =
                StatisticsSetSummaryDesignModel.GetReactionStatisticsSetSummaryDesignModel();

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