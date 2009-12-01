namespace PokerTell.Infrastructure.Interfaces.PokerHand
{
    using Iesi.Collections.Generic;

    public interface IPlayerIdentity
    {
        string Name { get; }

        string Site { get; }

        int Id { get; }

        ISet<IConvertedPokerPlayer> ConvertedPlayers { get; }

        IPlayerIdentity InitializeWith(string name, string site);

        string ToString();
    }
}