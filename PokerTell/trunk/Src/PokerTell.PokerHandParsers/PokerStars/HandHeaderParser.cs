namespace PokerTell.PokerHandParsers.PokerStars
{
    using System;
    using System.Text.RegularExpressions;

    public class HandHeaderParser : PokerHandParsers.HandHeaderParser
    {
     const string HeaderPattern =
            @"PokerStars Game [#](?<GameId>[0-9]+)[:] (Tournament [#](?<TournamentId>[0-9]+)){0,1}.*(Hold'em|Holdem) (No |Pot )*Limit";
        
        public override void Parse(string handHistory)
        {
            var header = MatchHeader(handHistory);
            IsValid = header.Success;

            if (IsValid)
            {
                ExtractGameId(header);
                ExtractTournamentId(header);
            }
        }

        static Match MatchHeader(string handHistory)
        {
            return Regex.Match(handHistory, HeaderPattern, RegexOptions.IgnoreCase);
        }

        void ExtractGameId(Match header)
        {
            GameId = ulong.Parse(header.Groups["GameId"].Value);
        }

        void ExtractTournamentId(Match header)
        {
            IsTournament = header.Groups["TournamentId"].Value != string.Empty;
            TournamentId = IsTournament
                               ? ulong.Parse(header.Groups["TournamentId"].Value)
                               : 0;
        }
    }
}