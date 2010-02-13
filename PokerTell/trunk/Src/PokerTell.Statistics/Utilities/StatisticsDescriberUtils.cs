namespace PokerTell.Statistics.Utilities
{
    using System;

    using PokerTell.Infrastructure.Enumerations.PokerHand;
    using PokerTell.Infrastructure.Interfaces.PokerHand;

    internal class StatisticsDescriberUtils
    {
        internal static string DescribePosition(IAnalyzablePokerPlayer analyzablePokerPlayer, Streets street)
        {
            return DescribePosition((bool)analyzablePokerPlayer.InPosition[(int)street]);
        }

        internal static string DescribePosition(bool inPosition)
        {
            return inPosition ? "in position" : "out of position";
        }

        internal static string DescribePot(IAnalyzablePokerPlayer analyzablePokerPlayer, Streets street)
        {
            return DescribePot(analyzablePokerPlayer.ActionSequences[(int)street]);
        }

        public static string DescribePot(ActionSequences actionSequence)
        {
            return ActionSequencesUtility.GetPreflopRaised.Contains(actionSequence) || actionSequence == ActionSequences.PreFlopFrontRaise
                       ? "a raised pot"
                       : "an unraised pot";
        }
    }
}