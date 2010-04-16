namespace PokerTell.PokerHandParsers.Base
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    using Interfaces.Parsers;

    public abstract class HandHeaderParser : IHandHeaderParser
    {
        public ulong GameId { get; protected set; }

        public bool IsTournament { get; protected set; }

        public bool IsValid { get; protected set; }

        public ulong TournamentId { get; protected set; }

        protected abstract string HeaderPattern { get; }

        public virtual IList<HeaderMatchInformation> FindAllHeaders(string handHistories)
        {
            var allHeaders = Regex.Matches(handHistories, HeaderPattern, RegexOptions.IgnoreCase);

            return (from Match header in allHeaders
                    let gameId = ExtractGameId(header)
                    select new HeaderMatchInformation(gameId, header)).ToList();
        }

        public virtual IHandHeaderParser Parse(string handHistory)
        {
            var header = MatchHeader(handHistory);
            IsValid = header.Success;

            if (IsValid)
            {
                GameId = ExtractGameId(header);
                ExtractTournamentId(header);
            }

            return this;
        }

        protected virtual ulong ExtractGameId(Match header)
        {
            return ulong.Parse(header.Groups["GameId"].Value);
        }

        protected virtual void ExtractTournamentId(Match header)
        {
            IsTournament = header.Groups["TournamentId"].Value != string.Empty;
            TournamentId = IsTournament
                               ? ulong.Parse(header.Groups["TournamentId"].Value)
                               : 0;
        }

        protected virtual Match MatchHeader(string handHistory)
        {
            return Regex.Match(handHistory, HeaderPattern, RegexOptions.IgnoreCase);
        }

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