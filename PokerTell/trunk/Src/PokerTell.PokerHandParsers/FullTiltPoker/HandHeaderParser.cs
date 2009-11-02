using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace PokerTell.PokerHandParsers.FullTiltPoker
{
    public class HandHeaderParser : PokerHandParsers.HandHeaderParser
    {
        const string HeaderPattern =
            @"((Full Tilt Poker Game)|(FullTiltPoker Game)) " + 
            @"[#](?<GameId>[0-9]+)[:].+" + 
            @"(No |Pot ){0,1}Limit (Hold'em|Holdem)";

        const string TournamentPattern = @"(\((?<TournamentId>[0-9]+)\),) +Table";
        
        public override PokerHandParsers.HandHeaderParser Parse(string handHistory)
        {
            var header = MatchHeader(handHistory);
            IsValid = header.Success;

            if (IsValid)
            {
                GameId = ExtractGameId(header);
                ExtractTournamentId(handHistory);
            }

            return this;
        }

        public override IList<HeaderMatchInformation> FindAllHeaders(string handHistories)
        {
            var allHeaders = Regex.Matches(handHistories, HeaderPattern, RegexOptions.IgnoreCase);

            return (from Match header in allHeaders
                    let gameId = ExtractGameId(header)
                    select new HeaderMatchInformation(gameId, header)).ToList();
        }

        static Match MatchHeader(string handHistory)
        {
            return Regex.Match(handHistory, HeaderPattern, RegexOptions.IgnoreCase);
        }

        static ulong ExtractGameId(Match header)
        {
            return ulong.Parse(header.Groups["GameId"].Value);
        }

        void ExtractTournamentId(string handHistory)
        {
            var tournament = Regex.Match(handHistory, TournamentPattern, RegexOptions.IgnoreCase);
            IsTournament = tournament.Success;
            TournamentId = IsTournament
                               ? ulong.Parse(tournament.Groups["TournamentId"].Value)
                               : 0;
        }

        
    }
}