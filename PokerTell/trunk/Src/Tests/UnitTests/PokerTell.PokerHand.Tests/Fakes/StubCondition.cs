namespace PokerTell.PokerHand.Tests.Fakes
{
    using Conditions;

    using Infrastructure.Interfaces.PokerHand;

    internal class StubCondition : PokerHandCondition
    {
        public override bool IsMetBy(IConvertedPokerHand hand)
        {
            return FullFill;
        }

        public bool FullFill { private get; set; }
    }

    internal class StubConditionWithHandId : PokerHandCondition
    {
        public override bool IsMetBy(IConvertedPokerHand hand)
        {
            return hand.HandId == HandIdToMatch;
        }

        public int HandIdToMatch { get; set; }
    }
}