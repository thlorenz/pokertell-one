namespace PokerTell.Statistics.Detailed
{
    using System.Collections.Generic;
    using System.Linq;

    using PokerTell.Infrastructure.Enumerations.PokerHand;
    using PokerTell.Infrastructure.Interfaces.PokerHand;

    public class PostFlopHeroXStatistic : PostFlopActionSequenceStatistic
    {
        #region Constructors and Destructors

        public PostFlopHeroXStatistic(Streets street, bool inPosition)
            : base(ActionSequences.HeroX, street, inPosition, 1)
        {
        }

        #endregion

        #region Methods

        protected override void ExtractMatchingPlayers(IEnumerable<IAnalyzablePokerPlayer> analyzablePokerPlayers)
        {
            MatchingPlayers[0] =
                (from player in analyzablePokerPlayers
                 where
                     player.InPosition[(int)_street] == _inPosition &&
                     (player.ActionSequences[(int)_street] == _actionSequence ||
                      player.ActionSequences[(int)_street] == ActionSequences.HeroXOppBHeroF ||
                      player.ActionSequences[(int)_street] == ActionSequences.HeroXOppBHeroC ||
                      player.ActionSequences[(int)_street] == ActionSequences.HeroXOppBHeroR)
                 select player).ToList();
        }

        #endregion
    }
}