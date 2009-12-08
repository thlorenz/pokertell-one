namespace PokerTell.Statistics.Detailed
{
    using System.Collections.Generic;

    using Infrastructure.Enumerations.PokerHand;
    using Infrastructure.Interfaces.PokerHand;

    using Interfaces;

    public class PostFlopStatisticWithRatios : DetailedStatisticBase
    {
        public PostFlopStatisticWithRatios(ActionSequences actionSequence, Streets street, bool inPosition, int indexesCount)
            : base(actionSequence, street, inPosition, indexesCount)
        {
        }

        public override IDetailedStatistic UpdateWith(IEnumerable<IAnalyzablePokerPlayer> analyzablePokerPlayers)
        {
            base.UpdateWith(analyzablePokerPlayers);

            return this;
        }
    }
}