namespace PokerTell.PokerHand.Conditions
{
    using Infrastructure.Interfaces.PokerHand;

    public class AlwaysTrueCondition : PokerHandCondition, IAlwaysTrueCondition
    {
        public override bool IsMetBy(IConvertedPokerHand hand)
        {
            return true;
        }
    }
}