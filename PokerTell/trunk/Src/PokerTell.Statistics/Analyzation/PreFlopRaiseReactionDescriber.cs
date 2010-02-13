namespace PokerTell.Statistics.Analyzation
{
    using PokerTell.Infrastructure.Enumerations.PokerHand;
    using PokerTell.Infrastructure.Interfaces.PokerHand;
    using PokerTell.Statistics.Interfaces;

    using Tools.Interfaces;

    using Utilities;

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
                                 StatisticsDescriberUtils.DescribePot(analyzablePokerPlayer, street));
        }

        /// <summary>
        /// Gives a hint about how the statistics presentation is to be interpreted, e.g. if the raise size refers to the opponent or the hero
        /// </summary>
        /// <param name="playerName"></param>
        /// <param name="analyzablePokerPlayer"></param>
        /// <returns></returns>
        public string Hint(string playerName, IAnalyzablePokerPlayer analyzablePokerPlayer)
        {
            return analyzablePokerPlayer.ActionSequences[(int)Streets.PreFlop] == ActionSequences.PreFlopFrontRaise
                       ? string.Format("Raise Size refers to the amount {0} reraised his opponent.", playerName)
                       : string.Format("Raise Size refers to the amount {0} raised before he was reraised.", playerName);
        }

        static string DescribePositions(ITuple<StrategicPositions, StrategicPositions> strategicPositions)
        {
            var fromPosition = StrategicPositionsUtility.NamePosition(strategicPositions.First);
            var toPostion = StrategicPositionsUtility.NamePosition(strategicPositions.Second);
            return fromPosition.Equals(toPostion)
                       ? string.Format("in {0}", fromPosition)
                       : string.Format("in between {0} and {1}", fromPosition, toPostion);
        }

        static string DescribeAction(IAnalyzablePokerPlayer analyzablePokerPlayer, Streets street)
        {
            return
                ActionSequencesUtility.NamePastTenseOfLastActionSequence(
                    analyzablePokerPlayer.ActionSequences[(int)street]).ToLower();
        }
    }
}