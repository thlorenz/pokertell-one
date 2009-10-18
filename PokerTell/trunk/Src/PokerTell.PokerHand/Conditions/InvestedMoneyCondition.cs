namespace PokerTell.PokerHand.Conditions
{
    using System.Linq;

    using Infrastructure.Enumerations.PokerHand;
    using Infrastructure.Interfaces.PokerHand;

    public class InvestedMoneyCondition : PokerHandCondition, IInvestedMoneyCondition
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
                              player.Rounds.Count > 0 &&
                              player[Streets.PreFlop].Count > 0 &&
                              (player[Streets.PreFlop][0].What == ActionTypes.C |
                               player[Streets.PreFlop][0].What == ActionTypes.R |
                               player[Streets.PreFlop][0].What == ActionTypes.X)
                          select player;

            return matches.Count() > 0;
        }
    }
}