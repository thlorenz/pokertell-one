namespace PokerTell.PokerHandParsers
{
    using System;

    public abstract class HandHeaderParser
    {
        public bool IsValid { get; protected set; }
        
        public bool IsTournament { get; protected set; }

        public ulong GameId { get; protected set; }

        public ulong TournamentId { get; protected set; }

        public abstract void Parse(string handHistory);
    }
}
