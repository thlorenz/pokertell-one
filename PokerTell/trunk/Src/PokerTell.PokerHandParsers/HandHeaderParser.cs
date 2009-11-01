namespace PokerTell.PokerHandParsers
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    public abstract class HandHeaderParser
    {
        public bool IsValid { get; protected set; }
        
        public bool IsTournament { get; protected set; }

        public ulong GameId { get; protected set; }

        public ulong TournamentId { get; protected set; }

        public abstract HandHeaderParser Parse(string handHistory);

        public abstract IList<HeaderMatchInformation> FindAllHeaders(string handHistories);

        public class HeaderMatchInformation
        {
            public readonly ulong GameId;

            public readonly Match HeaderMatch;

            public HeaderMatchInformation(ulong gameId, Match headerMatch)
            {
                HeaderMatch = headerMatch;
                GameId = gameId;
            }
        }
    }
}
