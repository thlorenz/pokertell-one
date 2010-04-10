namespace PokerTell.Statistics.Design.DetailedStatistics
{
    using PokerTell.Statistics.ViewModels;

    public class PreFlopStatisticsSetsDesignModel : PreFlopStatisticsSetsViewModel
    {
        public PreFlopStatisticsSetsDesignModel()
        {
            PreFlopUnraisedPotStatisticsSet =
                StatisticsSetSummaryDesignModel.GetReactionStatisticsSetSummaryDesignModel(43, 17);

            PreFlopRaisedPotStatisticsSet =
                StatisticsSetSummaryDesignModel.GetReactionStatisticsSetSummaryDesignModel(12, 8);

            TotalCountPreFlopUnraisedPot = 22006;
            TotalCountPreFlopRaisedPot = 8654;
            Steals = "23";

            RegisterEvents();
        }
    }
}