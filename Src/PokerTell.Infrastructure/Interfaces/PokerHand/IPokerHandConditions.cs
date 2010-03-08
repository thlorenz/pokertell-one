namespace PokerTell.Infrastructure.Interfaces.PokerHand
{
    public interface IPokerHandCondition
    {
        IPokerHandCondition AppliesTo(string playerName);

        bool IsMetBy(IConvertedPokerHand hand);
    }

    public interface IInvestedMoneyCondition : IPokerHandCondition
    {
    }

    public interface ISawFlopCondition : IPokerHandCondition
    {
    }

    public interface IAlwaysTrueCondition : IPokerHandCondition
    {
    }
}