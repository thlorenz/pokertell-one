namespace PokerTell.PokerHand.Conditions
{
    using System.Linq;

    using Infrastructure.Interfaces.PokerHand;

    public class SawFlopCondition : PokerHandCondition, ISawFlopCondition
    {
        public override bool IsMetBy(IConvertedPokerHand hand)
        {
            if (hand == null)
            {
                return false;
            }
           
            var matches = from player in hand.Players
                          where
                              player.Name.Equals(PlayerName) &&
                              player.Rounds.Count > 1
                          select player;

            return matches.Count() > 0;
        }
    }
}