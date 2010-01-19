namespace PokerTell.StatisticsIntegrationTests.DesignViewModels
{
    using Statistics.ViewModels;

    public class PreFlopStatisticsSetsDesignModel : PreFlopStatisticsSetsViewModel
    {
        public PreFlopStatisticsSetsDesignModel()
        {
            PreFlopUnraisedPotStatisticsSet =
                StatisticsSetSummaryDesignModel.GetReactionStatisticsSetSummaryDesignModel();

            PreFlopRaisedPotStatisticsSet =
                StatisticsSetSummaryDesignModel.GetReactionStatisticsSetSummaryDesignModel();

            TotalCountPreFlopUnraisedPot = 22006;
            TotalCountPreFlopRaisedPot = 8654;

            RegisterEvents();
        }
    }
}