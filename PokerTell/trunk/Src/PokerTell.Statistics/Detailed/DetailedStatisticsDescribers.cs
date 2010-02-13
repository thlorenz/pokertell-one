namespace PokerTell.Statistics.Detailed
{
    using System;

    using Infrastructure.Enumerations.PokerHand;

    using Interfaces;

    using Utilities;

    public class DetailedPostFlopHeroActsStatisticsDescriber : IDetailedPostFlopHeroActsStatisticsDescriber
    {
        public string Describe(string playerName, ActionSequences actionSequence, Streets street, bool inPosition)
        {
            return string.Format("{0} bet on the {1} when {2}.", playerName, street.ToString().ToLower(), StatisticsDescriberUtils.DescribePosition(inPosition));
        }

        public string Hint(string playerName)
        {
            return string.Format("The table indicates how often {0} bet a certain fraction of the pot.", playerName);
        }
    }

    public class DetailedPostFlopHeroReactsStatisticsDescriber : IDetailedPostFlopHeroReactsStatisticsDescriber
    {
        public string Describe(string playerName, ActionSequences actionSequence, Streets street, bool inPosition)
        {
            return actionSequence == ActionSequences.OppB
                       ? string.Format("{0} reacted to a bet on the {1} when {2}.", playerName, street.ToString().ToLower(), StatisticsDescriberUtils.DescribePosition(inPosition))
                       : string.Format("{0} checked first and then reacted to a bet on the {1} when {2}.", playerName, street.ToString().ToLower(), StatisticsDescriberUtils.DescribePosition(inPosition));
        }

        public string Hint(string playerName)
        {
            return string.Format("The table shows how {0} reacted depending on the bet size of the opponent", playerName);
        }
    }

    public class DetailedPreFlopStatisticsDescriber : IDetailedPreFlopStatisticsDescriber
    {
        public string Describe(string playerName, ActionSequences actionSequence, Streets street, bool inPosition)
        {
            return string.Format("{0} acted preflop in {1}.", playerName, StatisticsDescriberUtils.DescribePot(actionSequence));
        }

        public string Hint(string playerName)
        {
            return string.Format("The table shows how {0} acted when sitting in a certain position at the table.", playerName);
        }

        public string Describe(string playerName, ActionSequences actionSequence)
        {
            return Describe(playerName, actionSequence, Streets.PreFlop, false);
        }
    }
}