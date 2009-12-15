namespace PokerTell.StatisticsIntegrationTests.DesignViewModels
{
    using Infrastructure.Enumerations.PokerHand;

    using Statistics.ViewModels;

    internal class PostFlopStatisticsSetsDesignModel : PostFlopStatisticsSetsViewModel
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
        }
    }
}