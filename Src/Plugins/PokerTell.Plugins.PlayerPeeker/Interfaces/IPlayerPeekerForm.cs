namespace PokerTell.Plugins.PlayerPeeker.Interfaces
{
    using System;
    using System.Collections.Generic;

    public interface IPlayerPeekerForm
    {
        event Action ResetRequested;

        void Close();

        void Dispose();

        void NewPlayersFound(IEnumerable<string> newPlayers);

        void Show();

        void ActivateAndRestore();
    }
}