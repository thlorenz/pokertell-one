namespace PokerTell.Statistics.Detailed
{
    using System.Collections.Generic;
    using System.Linq;

    using Infrastructure.Enumerations.PokerHand;
    using Infrastructure.Interfaces.PokerHand;

    public class PostFlopActionSequenceStatistic : ActionSequenceStatistic
    {
        protected readonly bool _inPosition;

        public PostFlopActionSequenceStatistic(ActionSequences actionSequence, Streets street, bool inPosition, int indexesCount)
            : base(actionSequence, street, indexesCount)
        {
            _inPosition = inPosition;
        }

        protected override void ExtractMatchingPlayers(IEnumerable<IAnalyzablePokerPlayer> analyzablePokerPlayers)
        {
            for (int i = 0; i < MatchingPlayers.Length; i++)
            {
                int betSizeIndex = i;

                MatchingPlayers[betSizeIndex] =
                    (from player1 in analyzablePokerPlayers
                     where player1.ActionSequences[(int)_street] == _actionSequence
                           && player1.InPosition[(int)_street] == _inPosition
                           && player1.BetSizeIndexes[(int)_street] == betSizeIndex
                     select player1).ToList();
            }
        }

        public override string ToString()
        {
            return string.Format("{0} ({1} position)", base.ToString(), _inPosition ? "in" : "out of");
        }
    }
}