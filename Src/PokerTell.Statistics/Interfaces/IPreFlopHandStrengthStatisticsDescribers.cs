namespace PokerTell.Statistics.Interfaces
{
    using Infrastructure.Enumerations.PokerHand;

    public interface IPreFlopUnraisedPotCallingHandStrengthDescriber : IPreFlopHandStrengthDescriber
    {
    }

    public interface IPreFlopRaisedPotCallingHandStrengthDescriber : IPreFlopHandStrengthDescriber
    {
    }

    public interface IPreFlopRaisingHandStrengthDescriber : IPreFlopHandStrengthDescriber
    {
    }

    public interface IPreFlopHandStrengthDescriber
    {
        
        string Describe(string playerName, ActionSequences actionSequence);

        string Hint(string playerName);
    }
}