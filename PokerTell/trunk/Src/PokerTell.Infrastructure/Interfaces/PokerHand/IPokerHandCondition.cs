namespace PokerTell.Infrastructure.Interfaces.PokerHand
{
    public interface IPokerHandCondition
    {
        IPokerHandCondition AppliesTo(string playerName);

        bool IsFullFilledBy(IConvertedPokerHand hand);
    }
}