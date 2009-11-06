namespace PokerTell.PokerHand.Tests.Factories
{
    using Fakes;

    using Moq;

    internal static class ConvertedFactory
    {
        internal static DecapsulatedConvertedPlayer InitializeConvertedPokerPlayerWithSomeValidValues()
        {
            const string someCards = "As Ks";
            var stub = new StubBuilder();

            return
                (DecapsulatedConvertedPlayer)new DecapsulatedConvertedPlayer()
                                                 .InitializeWith(
                                                 "someName",
                                                 stub.Some(2.0),
                                                 stub.Some(1.0),
                                                 stub.Some(1),
                                                 stub.Some(4),
                                                 someCards);
        }
    }
}