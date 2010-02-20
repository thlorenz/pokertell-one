namespace PokerTell.StatisticsIntegrationTests.DesignViewModels
{
    using Statistics.IntegrationTests.DesignViewModels;
    using Statistics.ViewModels;

    using StatisticsSetSummaryDesignModel=PokerTell.Statistics.ViewModels._Design.StatisticsSetSummaryDesignModel;

    public class PreFlopStatisticsSetsDesignModel : PreFlopStatisticsSetsViewModel
    {
        public PreFlopStatisticsSetsDesignModel()
        {
            PreFlopUnraisedPotStatisticsSet =
                StatisticsSetSummaryDesignModel.GetReactionStatisticsSetSummaryDesignModel(29, 17);

            PreFlopRaisedPotStatisticsSet =
                StatisticsSetSummaryDesignModel.GetReactionStatisticsSetSummaryDesignModel(29, 17);

            TotalCountPreFlopUnraisedPot = 22006;
            TotalCountPreFlopRaisedPot = 8654;

            RegisterEvents();
        }
    }
}