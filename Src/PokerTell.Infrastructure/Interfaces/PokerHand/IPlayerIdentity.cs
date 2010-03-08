namespace PokerTell.Infrastructure.Interfaces.PokerHand
{
    using Iesi.Collections.Generic;

    using Tools.Interfaces;

    public interface IPlayerIdentity 
    {
        #region Properties

        ISet<IConvertedPokerPlayer> ConvertedPlayers { get; }

        int Id { get; }

        string Name { get; }

        string Site { get; }

        #endregion

        #region Public Methods

        IPlayerIdentity InitializeWith(string name, string site);

        string ToString();
        #endregion
    }
}