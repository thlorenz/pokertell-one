namespace PokerTell.Statistics.Analyzation
{
    using System;

    using Infrastructure.Enumerations.PokerHand;

    using Interfaces;


    public class PreFlopUnraisedPotCallingHandStrengthDescriber : PreFlopHandStrengthDescriber, IPreFlopUnraisedPotCallingHandStrengthDescriber
    {
    }

    public class PreFlopRaisedPotCallingHandStrengthDescriber : PreFlopHandStrengthDescriber, IPreFlopRaisedPotCallingHandStrengthDescriber
    {
    }

    public class PreFlopRaisingHandStrengthDescriber : PreFlopHandStrengthDescriber, IPreFlopRaisingHandStrengthDescriber
    {
    }

    public class PreFlopHandStrengthDescriber : IPreFlopHandStrengthDescriber
    {
        public string Describe(string playerName, ActionSequences actionSequence)
        {
            throw new NotImplementedException();
        }

        public string Hint(string playerName)
        {
            throw new NotImplementedException();
        }
    }
}