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

        public int Identity { get; set; }
    }
}