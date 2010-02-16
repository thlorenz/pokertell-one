namespace PokerTell.Statistics.Analyzation
{
    using System;
    using System.Text;

    using Infrastructure.Enumerations.PokerHand;

    using Interfaces;

    using Utilities;

    public class PreFlopUnraisedPotCallingHandStrengthDescriber : PreFlopHandStrengthDescriber, IPreFlopUnraisedPotCallingHandStrengthDescriber
    {
        public override string Describe(string playerName, ActionSequences actionSequence)
        {
            return string.Format("{0} called preflop in an unraised pot.", playerName);
        }
    }

    public class PreFlopRaisedPotCallingHandStrengthDescriber : PreFlopHandStrengthDescriber, IPreFlopRaisedPotCallingHandStrengthDescriber
    {
        public override string Describe(string playerName, ActionSequences actionSequence)
        {
            return string.Format("{0} called preflop in a raised pot.", playerName);
        }
    }

    public class PreFlopRaisingHandStrengthDescriber : PreFlopHandStrengthDescriber, IPreFlopRaisingHandStrengthDescriber
    {
        public override string Describe(string playerName, ActionSequences actionSequence)
        {
            return string.Format("{0} raised preflop in {1}.", playerName, StatisticsDescriberUtils.DescribePot(actionSequence));
        }
    }

    public abstract class PreFlopHandStrengthDescriber : IPreFlopHandStrengthDescriber
    {
        public abstract string Describe(string playerName, ActionSequences actionSequence);

        public virtual string Hint(string playerName)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Chen: avg starting hands value according to the Chen formula - AA = 20, 98s = 7.5");
            sb.AppendLine("S&M : avg starting hands Sklansky and Malmuth grouping       - AA =  1, 98s = 4  ");
            return sb.ToString();
        }
    }
}