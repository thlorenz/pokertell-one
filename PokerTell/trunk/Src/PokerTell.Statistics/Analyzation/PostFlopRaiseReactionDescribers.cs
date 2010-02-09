namespace PokerTell.Statistics.Analyzation
{
    using Infrastructure.Enumerations.PokerHand;
    using Infrastructure.Interfaces.PokerHand;

    using Interfaces;

    using Tools.Interfaces;

    public class PostFlopHeroActsRaiseReactionDescriber : IPostFlopHeroActsRaiseReactionDescriber
    {
        /// <summary>
        ///   Used for the situations in which the user bet post flop.
        ///   Describes the situation defined by the passed parameters to
        ///   give the user feedback about what a statistics table depicts.
        /// </summary>
        /// <param name="playerName"></param>
        /// <param name="analyzablePokerPlayer">
        ///   A sample player from the statistics set
        /// </param>
        /// <param name="street">
        ///   On what street did the situation occur
        /// </param>
        /// <param name="ratioSizes">
        ///   Is used here to describe the range of betsizes
        /// </param>
        /// <returns>
        ///   A nice description of the situation depicted by the parameters
        /// </returns>
        public string Describe(
            string playerName,
            IAnalyzablePokerPlayer analyzablePokerPlayer,
            Streets street,
            ITuple<double, double> ratioSizes)
        {
            ActionSequences actionSequence = analyzablePokerPlayer.ActionSequences[(int)street];
            return string.Format(
                "{0} {1} {2} of the pot {3} on the {4} and was raised",
                playerName,
                ActionSequencesUtility.NameLastActionInSequence(actionSequence).ToLower(),
                 PostFlopRaiseReactionDescriberUtils.DescribeBetSizes(ratioSizes),
                PostFlopRaiseReactionDescriberUtils.DescribePosition(analyzablePokerPlayer, street),
                street.ToString().ToLower());
        }

    }

    public class PostFlopHeroReactsRaiseReactionDescriber : IPostFlopHeroReactsRaiseReactionDescriber
    {
        public string Describe(string playerName, IAnalyzablePokerPlayer analyzablePokerPlayer, Streets street, ITuple<double, double> ratioSizes)
        {
            ActionSequences actionSequence = analyzablePokerPlayer.ActionSequences[(int)street];
            return string.Format("{0} {1} ({2} of the pot), when {3} on the {4}, and was reraised",
                                 playerName,
                                 DescribeAction(actionSequence),
                                 PostFlopRaiseReactionDescriberUtils.DescribeBetSizes(ratioSizes),
                                 PostFlopRaiseReactionDescriberUtils.DescribePosition(analyzablePokerPlayer, street),
                                 street.ToString().ToLower());
        }

        static string DescribeAction(ActionSequences actionSequence)
        {
            string possibleCheck = ActionSequencesUtility.GetHeroChecksThenReacts.Contains(actionSequence)
                                       ? ActionTypesUtility.Name(ActionTypes.X).ToLower() + "-"
                                       : string.Empty;

            return possibleCheck + ActionSequencesUtility.NamePastTenseOfLastActionSequence(actionSequence) +
                   " a " + ActionTypesUtility.Name(ActionTypes.B).ToLower();
        }

    }

    internal static class PostFlopRaiseReactionDescriberUtils
    {
        internal static string DescribePosition(IAnalyzablePokerPlayer analyzablePokerPlayer, Streets street)
        {
            return (bool)analyzablePokerPlayer.InPosition[(int)street] ? "in position" : "out of position";
        }

        internal static string DescribeBetSizes(ITuple<double, double> ratioSizes)
        {
            return (ratioSizes.First == ratioSizes.Second)
                       ? ratioSizes.First.ToString()
                       : ratioSizes.First + " to " + ratioSizes.Second;
        }
    }
}