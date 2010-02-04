namespace PokerTell.Statistics.Analyzation
{
    using Infrastructure.Enumerations.PokerHand;
    using Infrastructure.Interfaces.PokerHand;

    using Interfaces;

    using Tools.Interfaces;

    /// <summary>
    /// Helps describing the situation for which preflop raise reaction apply
    /// </summary>
    public class PreFlopRaiseReactionDescriber : IPreFlopRaiseReactionDescriber
    {
        
        /// <summary>
        /// Describes the situation defined by the passed parameters to
        /// give the user feedback about what a statistics table depicts.
        /// </summary>
        /// <param name="playerName"></param>
        /// <param name="analyzablePokerPlayer">A sample player from the statistics set</param>
        /// <param name="street">Should always be preflop</param>
        /// <param name="ratioSizes">In this case is used to describe the range of strategic positions in which the player acted</param>
        /// <returns>A nice description of the situation depicted by the parameters</returns>
        public string Describe(
            string playerName,
            IAnalyzablePokerPlayer analyzablePokerPlayer,
            Streets street,
            ITuple<StrategicPositions, StrategicPositions> ratioSizes)
        {
            return string.Format("{0} was sitting {1}, {2} {3} in {4} and was raised.",
                                 playerName,
                                 DescribePositions(ratioSizes),
                                 DescribeAction(analyzablePokerPlayer, street),
                                 street.ToString().ToLower(),
                                 DescribePot(analyzablePokerPlayer, street));
        }

        static string DescribePositions(ITuple<StrategicPositions, StrategicPositions> strategicPositions)
        {
            var fromPosition = StrategicPositionsUtility.NamePosition(strategicPositions.First);
            var toPostion = StrategicPositionsUtility.NamePosition(strategicPositions.Second);
            return fromPosition.Equals(toPostion)
                ? string.Format("in {0}", fromPosition)
                : string.Format("in between {0} and {1}", fromPosition, toPostion);
        }

        static string DescribePot(IAnalyzablePokerPlayer analyzablePokerPlayer, Streets street)
        {
            return ActionSequencesUtility.GetPreflopRaised.Contains(
                       analyzablePokerPlayer.ActionSequences[(int)street])
                       ? "a raised pot"
                       : "an unraised pot";
        }

        static string DescribeAction(IAnalyzablePokerPlayer analyzablePokerPlayer, Streets street)
        {
            return
                ActionSequencesUtility.NamePastTenseOfLastActionSequence(
                    analyzablePokerPlayer.ActionSequences[(int)street]).ToLower();
        }
    }
}