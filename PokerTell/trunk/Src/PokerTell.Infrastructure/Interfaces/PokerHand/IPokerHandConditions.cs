namespace PokerTell.Infrastructure.Interfaces.PokerHand
{
    public interface IPokerHandCondition
    {
        IPokerHandCondition AppliesTo(string playerName);

        bool IsFullFilledBy(IConvertedPokerHand hand);
    }

    public interface IInvestedMoneyCondition : IPokerHandCondition
    {
    }

    public interface ISawFlopCondition : IPokerHandCondition
    {
    }
}