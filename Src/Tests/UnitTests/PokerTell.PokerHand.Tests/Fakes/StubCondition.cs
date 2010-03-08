namespace PokerTell.PokerHand.Tests.Fakes
{
    using Infrastructure.Interfaces.PokerHand;

    using PokerTell.PokerHand.Conditions;

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
            return hand.Id == HandIdToMatch;
        }

        public int HandIdToMatch { get; set; }
    }
}