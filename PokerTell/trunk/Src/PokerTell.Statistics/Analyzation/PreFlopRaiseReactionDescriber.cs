namespace PokerTell.Statistics.Analyzation
{
    using Infrastructure.Enumerations.PokerHand;
    using Infrastructure.Interfaces.PokerHand;

    using Interfaces;

    using Tools.Interfaces;

    public class PreFlopRaiseReactionDescriber : IPostFlopHeroActsRaiseReactionDescriber
    {
        public string Describe(
            string playerName,
            IAnalyzablePokerPlayer analyzablePokerPlayer,
            Streets street,
            ITuple<double, double> ratioSizes)
        {
            return string.Format("{0} {1} {2} in {3} and was raised",
                                 playerName,
                                 DescribeReaction(analyzablePokerPlayer, street),
                                 street.ToString().ToLower(),
                                 DescribePot(analyzablePokerPlayer, street));
        }

        static string DescribePot(IAnalyzablePokerPlayer analyzablePokerPlayer, Streets street)
        {
            return ActionSequencesUtility.GetPreflopRaised.Contains(
                       analyzablePokerPlayer.ActionSequences[(int)street])
                       ? "a raised pot"
                       : "unraised pot";
        }

        static string DescribeReaction(IAnalyzablePokerPlayer analyzablePokerPlayer, Streets street)
        {
            return
                ActionSequencesUtility.NamePastTenseOfLastActionSequence(
                    analyzablePokerPlayer.ActionSequences[(int)street]).ToLower();
        }
    }
}