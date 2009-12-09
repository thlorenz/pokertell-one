namespace PokerTell.Statistics.Detailed
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Infrastructure.Enumerations.PokerHand;
    using Infrastructure.Interfaces.PokerHand;

    using Interfaces;

    public class PreFlopActionSequenceStatistic : ActionSequenceStatistic
    {
        public PreFlopActionSequenceStatistic(ActionSequences actionSequence)
            : base(actionSequence, Streets.PreFlop, (int)StrategicPositions.BU + 1)
        {
        }

        public override IActionSequenceStatistic UpdateWith(IEnumerable<IAnalyzablePokerPlayer> analyzablePokerPlayers)
        {
            for (var position = StrategicPositions.SB; position <= StrategicPositions.BU; position++)
            {
                StrategicPositions strategicPosition = position;
                MatchingPlayers[(int)strategicPosition] =
                    (from player in analyzablePokerPlayers
                     where player.ActionSequences[(int)_street] == ActionSequence
                     && player.StrategicPosition == strategicPosition
                     select player).ToList();
            }
            
            return this;
        }
    }
}