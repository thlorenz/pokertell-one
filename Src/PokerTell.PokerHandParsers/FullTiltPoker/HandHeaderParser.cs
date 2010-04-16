namespace PokerTell.PokerHandParsers.FullTiltPoker
{
    using System.Text.RegularExpressions;

    using PokerTell.PokerHandParsers.Interfaces.Parsers;

    public class HandHeaderParser : Base.HandHeaderParser, IFullTiltPokerHandHeaderParser 
    {
        const string FullTiltHeaderPattern =
            @"((Full Tilt Poker Game)|(FullTiltPoker Game)) " +
            @"[#](?<GameId>[0-9]+)[:].+" +
            @"(No |Pot ){0,1}Limit (Hold'em|Holdem)";

        const string TournamentPattern = @"(\((?<TournamentId>[0-9]+)\),) +Table";

        protected override string HeaderPattern
        {
            get { return FullTiltHeaderPattern; }
        }

        public override IHandHeaderParser Parse(string handHistory)
        {
            Match header = MatchHeader(handHistory);
            IsValid = header.Success;

            if (IsValid)
            {
                GameId = ExtractGameId(header);
                ExtractTournamentId(handHistory);
            }

            return this;
        }

        void ExtractTournamentId(string handHistory)
        {
            Match tournament = Regex.Match(handHistory, TournamentPattern, RegexOptions.IgnoreCase);
            IsTournament = tournament.Success;
            TournamentId = IsTournament
                               ? ulong.Parse(tournament.Groups["TournamentId"].Value)
                               : 0;
        }
    }
}