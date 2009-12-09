namespace PokerTell.Statistics.Detailed
{
    using System.Collections.Generic;
    using System.Linq;

    using Infrastructure.Enumerations.PokerHand;
    using Infrastructure.Interfaces.PokerHand;

    using Interfaces;

    public class PostFlopActionSequenceWithoutIndexesStatistic : PostFlopActionSequenceStatistic
    {
        public PostFlopActionSequenceWithoutIndexesStatistic(ActionSequences actionSequence, Streets street, bool inPosition)
            : base(actionSequence, street, inPosition, 1)
        {
        }

        public override IActionSequenceStatistic UpdateWith(IEnumerable<IAnalyzablePokerPlayer> analyzablePokerPlayers)
        {
            MatchingPlayers[0] =
                (from player in analyzablePokerPlayers
                 where player.ActionSequences[(int)_street] == ActionSequence
                       && player.InPosition[(int)_street] == _inPosition
                 select player).ToList();

            return this;
        }
    }
}