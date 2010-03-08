namespace PokerTell.PokerHand.Conditions
{
    using Infrastructure.Interfaces.PokerHand;

    public abstract class PokerHandCondition : IPokerHandCondition
    {
        protected string PlayerName;

        public IPokerHandCondition AppliesTo(string playerName)
        {
            PlayerName = playerName;
            return this;
        }

        public abstract bool IsMetBy(IConvertedPokerHand hand);
        
    }
}