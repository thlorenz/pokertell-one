namespace PokerTell.PokerHand.Tests.Fakes
{
    using PokerTell.PokerHand.Analyzation;

    internal class PlayerIdentityStub : PlayerIdentity
    {
        public PlayerIdentityStub(string name, string site, int id)
            : base(name, site)
        {
            Id = id;
        }
    }
}