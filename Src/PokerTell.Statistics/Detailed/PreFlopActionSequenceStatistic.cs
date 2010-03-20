namespace PokerTell.Statistics.Detailed
{
    using System.Collections.Generic;
    using System.Linq;

    using Infrastructure.Enumerations.PokerHand;
    using Infrastructure.Interfaces.PokerHand;

    using Interfaces;

    using Tools.FunctionalCSharp;

    public class PreFlopActionSequenceStatistic : ActionSequenceStatistic
    {
        public PreFlopActionSequenceStatistic(ActionSequences actionSequence)
            : base(actionSequence, Streets.PreFlop, (int)StrategicPositions.BU + 1)
        {
        }

        protected override void ExtractMatchingPlayers(IEnumerable<IAnalyzablePokerPlayer> analyzablePokerPlayers)
        {
            for (var position = StrategicPositions.SB; position <= StrategicPositions.BU; position++)
            {
                StrategicPositions strategicPosition = position;
                MatchingPlayers[(int)strategicPosition] =
                    (from player in analyzablePokerPlayers
                     where player.ActionSequences[(int)_street] == _actionSequence
                     && player.StrategicPosition == strategicPosition
                     select player).ToList();
            }

            AddChecksToFoldsForBigBlindInUnraisedPot(analyzablePokerPlayers);
        }

        void AddChecksToFoldsForBigBlindInUnraisedPot(IEnumerable<IAnalyzablePokerPlayer> analyzablePokerPlayers)
        {
            if (_actionSequence == ActionSequences.HeroF)
            {
                var checksFromBigBlind =
                    from player in analyzablePokerPlayers
                    where
                        player.StrategicPosition == StrategicPositions.BB &&
                        player.ActionSequences[(int)_street] == ActionSequences.HeroX
                    select player;

                checksFromBigBlind.ForEach(check => MatchingPlayers[(int)StrategicPositions.BB].Add(check));
            }
        }
    }
}